using Genesis.Ensure;
using Nethereum.StandardToken.UI.SmartContractMessages;
using Nethereum.UI;
using Nethereum.UI.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Helpers;
using System;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Nethereum.StandardToken.UI.ViewModels
{
    public class StandardTokenBalanceOfViewModel : ReactiveValidationObject
    {
        [Reactive] public string ContractAddress { get; set; }
        [Reactive] public string AccountAddress { get; set; }
        [Reactive] public decimal Balance { get; set; }

        private readonly ReactiveCommand<Unit, bool> _refreshBalanceCommand;
        private readonly IEthereumHostProvider ethereumHostProvider;
        private readonly IContractService contractService;

        public ReactiveCommand<Unit, bool> RefreshBalanceCommand => this._refreshBalanceCommand;

        protected StandardTokenBalanceOfViewModel()
        {

        }

        public StandardTokenBalanceOfViewModel(IEthereumHostProvider ethereumHostProvider, IContractService contractService)
        {
            this.ethereumHostProvider = ethereumHostProvider;
            this.ethereumHostProvider.SelectedAccountCallback.Subscribe(address => AccountAddress = address);

            this.contractService = contractService;
            this.contractService.ContractAddress.Subscribe(x => ContractAddress = x);

            var hasValidAddresses = this.WhenAnyValue(x => x.AccountAddress, x => x.ContractAddress,
                (address, contractAddress) => Nethereum.UI.Util.Utils.IsValidAddress(address) && Nethereum.UI.Util.Utils.IsValidAddress(contractAddress));

            var isValidRefreshBalance = Observable.CombineLatest(hasValidAddresses, this.ethereumHostProvider.EnabledCallBack, (validAdress, enabled) => validAdress && enabled);

            isValidRefreshBalance.Where(x => x == true)
                .Subscribe(async _ => await RefreshBalanceAsync());

            _refreshBalanceCommand = ReactiveCommand.CreateFromTask(RefreshBalanceAsync, isValidRefreshBalance);
         
        }


        public async Task<bool> RefreshBalanceAsync()
        {
            Balance = await GetBalanceAsync();
            return true;
        }

        public async Task<decimal> GetBalanceAsync()
        {
            Ensure.ArgumentNotNull(this.AccountAddress, "Address");
            Ensure.ArgumentNotNull(this.ContractAddress, "ContractAddress");

            var web3 = await ethereumHostProvider.GetWeb3Async();
            var handler = web3.Eth.GetContractHandler(ContractAddress);
            var balanceMessage = new BalanceOfFunction(){Owner = AccountAddress};
            var balance = await handler.QueryAsync<BalanceOfFunction,BigInteger>(balanceMessage);
            
            //assuming all have 18 decimals
            var value = Web3.Web3.Convert.FromWeiToBigDecimal(balance);
            return decimal.Parse(value.ToString());
        }
    }
}