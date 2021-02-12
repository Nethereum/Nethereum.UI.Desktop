using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Genesis.Ensure;
using Nethereum.UI.HostProvider;
using Nethereum.UI.HostProvider.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Nethereum.UI.ViewModels
{
    public class AccountViewModel : ReactiveObject
    {
        [Reactive] public string Address { get; set; }
        [Reactive] public decimal Balance { get; set; }

        public async Task<bool> RefreshBalanceAsync()
        {
            Balance = await GetBalanceAsync();
            return true;
        }

        private readonly ReactiveCommand<Unit, bool> _refreshBalanceCommand;
        
        private readonly NethereumHostProvider _ethereumHostProvider;
        public ReactiveCommand<Unit, bool> RefreshBalanceCommand => this._refreshBalanceCommand;

        public AccountViewModel()
        {
        }

        public AccountViewModel(NethereumHostProvider ethereumHostProvider)
        {
            _ethereumHostProvider = ethereumHostProvider;

            _ethereumHostProvider.SelectedAccountCallback.Subscribe(address => Address = address);

            var isValidAddress = this.WhenAnyValue(x => x.Address,
                (address) => Util.Utils.IsValidAddress(address));

            var isValidRefreshBalance = Observable.CombineLatest(_ethereumHostProvider.EnabledCallBack, isValidAddress, (valid, enabled) => valid && enabled);

            isValidRefreshBalance.Where(x => x == true)
                .Subscribe(async _ => await RefreshBalanceAsync());

           _refreshBalanceCommand =  ReactiveCommand.CreateFromTask(RefreshBalanceAsync, isValidRefreshBalance);
        }

       
        public async Task<decimal> GetBalanceAsync()
        {
            Ensure.ArgumentNotNull(this.Address, "Address");

            if (_ethereumHostProvider.Enabled)
            {
                var web3 = await _ethereumHostProvider.GetWeb3Async();
                var balance = await web3.Eth.GetBalance.SendRequestAsync(Address);
                var value = Web3.Web3.Convert.FromWeiToBigDecimal(balance.Value);
                return decimal.Parse(value.ToString());
            }

           return 0;
        }
    }
}