using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Genesis.Ensure;
using Nethereum.UI.UIMessages;
using Nethereum.Web3.Accounts;
using ReactiveUI;

namespace Nethereum.UI.ViewModels
{
    public class AccountViewModel : ReactiveObject
    {
        private Account account;
        private string _privateKey;
        public string PrivateKey
        {
            get { return _privateKey; }
            set
            {
                this.RaiseAndSetIfChanged(ref _privateKey, value);
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => this.RaiseAndSetIfChanged(ref _address, value);
        }

        private string _url;
        public string Url
        {
            get => _url;
            set => this.RaiseAndSetIfChanged(ref _url, value);
        }

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set => this.RaiseAndSetIfChanged(ref _balance, value);
        }

        public async Task<bool> RefreshBalanceAsync()
        {
            Balance = await GetBalanceAsync();
            return true;
        }

        private readonly ReactiveCommand<Unit, bool> _refreshBalanceCommand;
        public ReactiveCommand<Unit, bool> RefreshBalanceCommand => this._refreshBalanceCommand;

        public AccountViewModel()
        {
           
            this.WhenAnyValue(x => x.Url, (url) => Util.Utils.IsValidUrl(url)).Subscribe(_ =>
                MessageBus.Current.SendMessage(
                    new UrlChanged(_url)));
            
            MessageBus.Current.Listen<AccountLoaded>().Subscribe(x =>
                {
                    account = x.Account;
                    PrivateKey = x.Account.PrivateKey;
                    Address = x.Account.Address;
                }
            );

            var isValidRefreshBalance = this.WhenAnyValue(x => x.Address, x => x.Url,
                (address, url) => Util.Utils.IsValidAddress(address) && Util.Utils.IsValidUrl(url));

            isValidRefreshBalance.Where(x => x == true)
                .Subscribe(async _ => await RefreshBalanceAsync());

           _refreshBalanceCommand =  ReactiveCommand.CreateFromTask(RefreshBalanceAsync, isValidRefreshBalance);

       
        }

        
        public async Task<decimal> GetBalanceAsync()
        {
            Ensure.ArgumentNotNull(this.Address, "Address");
            Ensure.ArgumentNotNull(this.Url, "Url");

            var web3 = new Web3.Web3(Url);
            var balance = await web3.Eth.GetBalance.SendRequestAsync(Address);
            var value = Web3.Web3.Convert.FromWeiToBigDecimal(balance.Value);
            return decimal.Parse(value.ToString());
        }
    }
}