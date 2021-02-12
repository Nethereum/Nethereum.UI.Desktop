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
    public class AccountItemViewModel : ReactiveObject
    {
        [Reactive] public string Address { get; set; }
        [Reactive] public decimal Balance { get; set; }

        public async Task<bool> RefreshBalanceAsync()
        {
            Balance = await GetBalanceAsync();
            return true;
        }

        private readonly ReactiveCommand<Unit, bool> _refreshBalanceCommand;
        private readonly ReactiveCommand<Unit, Unit> _selectCommand;

        private readonly NethereumHostProvider _ethereumHostProvider;
        private readonly AccountsService _accountsService;

        public ReactiveCommand<Unit, bool> RefreshBalanceCommand => this._refreshBalanceCommand;
        public ReactiveCommand<Unit, Unit> SelectCommand => this._selectCommand;

        public AccountItemViewModel()
        {
        }

        public AccountItemViewModel(string address, NethereumHostProvider ethereumHostProvider, AccountsService accountsService)
        {
            _ethereumHostProvider = ethereumHostProvider;
            _accountsService = accountsService;
            Address = address;

            var isValidAddress = this.WhenAnyValue(x => x.Address,
                (ad) => Util.Utils.IsValidAddress(ad));

            var isValidRefreshBalance = Observable.CombineLatest(_ethereumHostProvider.EnabledCallBack, isValidAddress, (valid, enabled) => valid && enabled);

            isValidRefreshBalance.Where(x => x == true)
                .Subscribe(async _ => await RefreshBalanceAsync());

            _refreshBalanceCommand = ReactiveCommand.CreateFromTask(RefreshBalanceAsync, isValidRefreshBalance);
            _selectCommand = ReactiveCommand.Create(() => _accountsService.SetSelectedAccount(Address));
        }


        public async Task<decimal> GetBalanceAsync()
        {
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