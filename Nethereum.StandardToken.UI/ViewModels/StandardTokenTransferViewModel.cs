using Genesis.Ensure;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.StandardToken.UI.SmartContractMessages;
using Nethereum.UI;
using Nethereum.UI.Services;
using Nethereum.UI.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Nethereum.StandardToken.UI.ViewModels
{
    public class StandardTokenTransferViewModel : ReactiveValidationObject
    {
        [Reactive] public string ContractAddress { get; set; }
        [Reactive] public string AddressTo { get; set; }
        [Reactive] public decimal? Amount { get; set; }
        
        private readonly IEthereumHostProvider _ethereumHostProvider;
        private readonly IContractService _contractService;
        private readonly IScreen _screenHost;

        protected ReactiveCommand<Unit, Unit> _executeTransactionCommand;
        public ReactiveCommand<Unit, Unit> ExecuteTransactionCommand => this._executeTransactionCommand;


        protected StandardTokenTransferViewModel()
        {

        }

        public StandardTokenTransferViewModel(IEthereumHostProvider ethereumHostProvider, IContractService contractService, IScreen screenHost) 
        {
            this._ethereumHostProvider = ethereumHostProvider;
            this._contractService = contractService;
            this._screenHost = screenHost;
            this._contractService.ContractAddress.Subscribe(x => ContractAddress = x);

            this.ValidationRule(x => x.AddressTo, address => Nethereum.UI.Util.Utils.IsValidAddress(address), "Address is not valid");
            this.ValidationRule(x => x.Amount, amount => amount >= 0, "Amount cannot be negative");

           

            var canExecuteTransaction = this.WhenAnyValue(
                x => x.AddressTo,
                x => x.Amount,
                x => x.ContractAddress,
                (addressTo, amount, contractAddress) =>
                    Nethereum.UI.Util.Utils.IsValidAddress(addressTo) &&
                    amount != null && amount > 0 &&
                    contractAddress != null);

            var canExecuteAndEnabled = Observable.CombineLatest(canExecuteTransaction, _ethereumHostProvider.EnabledCallBack, (valid, enabled) => valid && enabled);

            this._executeTransactionCommand = ReactiveCommand.CreateFromTask(ExecuteAsync, canExecuteAndEnabled);
   
        }

        public async Task ExecuteAsync()
        {
            Ensure.ArgumentNotNullOrEmpty(this.AddressTo, "Address To");
            Ensure.ArgumentNotNull(this.Amount, "Token Amount");

            var transferfuction =
                                    new TransferFunction
                                    {
                                        TokenAmount = Web3.Web3.Convert.ToWei(Amount.Value),
                                        To = AddressTo
                                    };

            var transactionViewModel = new SendTransactionViewModel(_ethereumHostProvider, _screenHost);
            await transactionViewModel.InitialiseAsync(transferfuction, ContractAddress);
            await _screenHost.Router.Navigate.Execute(transactionViewModel);

        }

    }
}