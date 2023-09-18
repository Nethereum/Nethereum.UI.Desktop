using Genesis.Ensure;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Model;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.UI.HostProvider.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using ReactiveUI.Validation.States;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Nethereum.UI.ViewModels
{
    public class SendTransactionViewModel : ReactiveValidationObject, IRoutableViewModel
    {
        [Reactive] public string Address { get; set; }
        [Reactive] public string Url { get; set; }
        [Reactive] public string AddressTo { get; set; }
        [Reactive] public decimal? AmountInEther { get; set; } = 0;
        [Reactive] public ulong? Gas { get; set; }
        [Reactive] public ulong? Nonce { get; set; }
        [Reactive] public decimal GasPrice { get; set; }
        [Reactive] public string Data { get; set; }

        protected readonly Interaction<string, bool> _confirmTransfer;

        public Interaction<string, bool> ConfirmTransfer => this._confirmTransfer;

        protected ReactiveCommand<Unit, string> _executeTrasnactionCommand;
        public ReactiveCommand<Unit, string> ExecuteTransactionCommand => this._executeTrasnactionCommand;

        private readonly IEthereumHostProvider _ethereumHostProvider;
        private readonly CurrentAccountTransactionsService _currentAccountTransactionsService;

        public string UrlPathSegment => "SendTransaction";

        [Reactive] public IScreen HostScreen { get; protected set; }

        protected ReactiveCommand<Unit, Unit> _goBackCommand;
        public ReactiveCommand<Unit, Unit> GoBackCommand => this._goBackCommand;

        protected SendTransactionViewModel() { 
        
        }

        public async Task InitialiseAsync<T>(T functionMessage, string contractAddress) where T : FunctionMessage
        {
            var transactionInput = functionMessage.CreateTransactionInput(contractAddress);
            var web3 = await _ethereumHostProvider.GetWeb3Async();
            var gas = await web3.TransactionManager.EstimateGasAsync(transactionInput);
            transactionInput.Gas = gas;
            //TODO gas service this should be monitor etc..
            var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();
            transactionInput.GasPrice = gasPrice;
            Initialise(transactionInput);
        }

        public void Initialise(TransactionInput transactionInput)
        {
            this.AddressTo = transactionInput.To;
            this.AmountInEther = Web3.Web3.Convert.FromWei(transactionInput.Value.Value);
            this.Data = transactionInput.Data;
            this.Gas = (ulong)transactionInput.Gas.Value;
            this.GasPrice = Web3.Web3.Convert.FromWei(transactionInput.GasPrice.Value, Nethereum.Util.UnitConversion.EthUnit.Gwei);
        }

        public SendTransactionViewModel(IEthereumHostProvider ethereumHostProvider, IScreen hostScreen = null, CurrentAccountTransactionsService currentAccountTransactionsService = null) 
        {
            this.HostScreen = hostScreen;
            _ethereumHostProvider = ethereumHostProvider ?? Locator.Current.GetService<IEthereumHostProvider>();
            _currentAccountTransactionsService = currentAccountTransactionsService ?? Locator.Current.GetService<CurrentAccountTransactionsService>();

            _ethereumHostProvider.SelectedAccountCallback.Subscribe(x => Address = x);

            this._confirmTransfer = new Interaction<string, bool>();

            Gas = (ulong)SignedLegacyTransaction.DEFAULT_GAS_LIMIT;
           // GasPrice = Web3.Web3.Convert.FromWei(Signer.Transaction.DEFAULT_GAS_PRICE, Nethereum.Util.UnitConversion.EthUnit.Gwei);
            
            this.ValidationRule(x => x.AddressTo, address => Util.Utils.IsValidAddress(address), "Address is not valid");
            this.ValidationRule(x => x.AmountInEther, amount => amount >= 0, "Amount cannot be negative");

            IObservable<IValidationState> balanceValidated =
                this.WhenAnyValue(x => x.AmountInEther, x => x.Gas, x => x.GasPrice)
                    .Throttle(TimeSpan.FromSeconds(1), RxApp.TaskpoolScheduler)
                    .SelectMany(x => ValidateEnoughBalance(x.Item1, x.Item2, x.Item3))
                    .ObserveOn(RxApp.MainThreadScheduler);

            this.ValidationRule( vm => vm.AmountInEther, balanceValidated);

            var canExecuteTransaction = this.IsValid();

            var canExecuteTransactionAndEnabled = Observable.CombineLatest(canExecuteTransaction, _ethereumHostProvider.EnabledCallBack, (valid, enabled) => valid && enabled);

            this._executeTrasnactionCommand = ReactiveCommand.CreateFromTask(ExecuteAsync, canExecuteTransactionAndEnabled);
            
            this._goBackCommand = ReactiveCommand.CreateFromTask(GoBackAsync);

        }

        public async Task<IValidationState> ValidateEnoughBalance(decimal? amountInEther, ulong? gas, decimal gasPrice)
        {
            if (amountInEther == 0) return  ValidationState.Valid;
            var web3 = await _ethereumHostProvider.GetWeb3Async();
            var currentBalance = await web3.Eth.GetEtherTransferService().CalculateTotalAmountToTransferWholeBalanceInEtherAsync(Address,
                gasPrice, gas);
            if(currentBalance < amountInEther)
            {
                return new ValidationState(false, $"Not enough balance, to send {amountInEther}, you can transfer including gas costs {currentBalance}");
            }
            return ValidationState.Valid;
        }

        public async Task GoBackAsync()
        {
            Ensure.ArgumentNotNull(HostScreen, "Host should not be null");
            await HostScreen.Router.NavigateBack.Execute();
        }

        public async Task<string> ExecuteAsync()
        {
            Ensure.ArgumentNotNullOrEmpty(this.AddressTo, "Address To");
            Ensure.ArgumentNotNull(Address, "Account");
            Ensure.ArgumentNotNull(this.AmountInEther, "Amount in Ether");

            var confirmed = await _confirmTransfer.Handle(GetConfirmationMessage());

            if (confirmed)
            {
                var web3 = await _ethereumHostProvider.GetWeb3Async();

                var transactionInput =
                    new TransactionInput
                    {
                        Value = new HexBigInteger(Web3.Web3.Convert.ToWei(AmountInEther.Value)),
                        To = AddressTo,
                        From = web3.TransactionManager.Account.Address
                    };
                if (Gas != null) transactionInput.Gas = new HexBigInteger(Gas.Value);
                if (GasPrice > 0) transactionInput.GasPrice = new HexBigInteger(Web3.Web3.Convert.ToWei(GasPrice, Nethereum.Util.UnitConversion.EthUnit.Gwei));
                if (Nonce != null) transactionInput.Nonce = new HexBigInteger(Nonce.Value);
                if (!string.IsNullOrEmpty(Data)) transactionInput.Data = Data;

                var transactionHash =  await web3.TransactionManager.SendTransactionAsync(transactionInput);
                await _currentAccountTransactionsService.AddNewTransactionAsync(transactionHash);
                return transactionHash;
            }
            return null;
        }

     
        public virtual string GetConfirmationMessage()
        {
            return
                $"Are you sure you want to submit this transaction: \n\r To: {AddressTo} \n\r Amount: {AmountInEther}";
        }    
    }
}