using System;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Genesis.Ensure;
using Nethereum.StandardToken.UI.SmartContractMessages;
using Nethereum.StandardToken.UI.UIMessages;
using Nethereum.UI.UIMessages;
using Nethereum.UI.Util;
using ReactiveUI;

namespace Nethereum.StandardToken.UI.ViewModels
{
    public class StandardTokenBalanceOfViewModel : ReactiveObject
    {
        private string _url;
        public string Url
        {
            get => _url;
            set => this.RaiseAndSetIfChanged(ref _url, value);
        }

        private string _contractAddress;
        public string ContractAddress
        {
            get => _contractAddress;
            set => this.RaiseAndSetIfChanged(ref _contractAddress, value);
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => this.RaiseAndSetIfChanged(ref _address, value);
        }

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set => this.RaiseAndSetIfChanged(ref _balance, value);
        }

        private readonly ReactiveCommand<Unit, bool> _refreshBalanceCommand;
        public ReactiveCommand<Unit, bool> RefreshBalanceCommand => this._refreshBalanceCommand;

        public StandardTokenBalanceOfViewModel()
        {
            MessageBus.Current.Listen<UrlChanged>().Subscribe(x =>
                {
                    Url = x.Url;
                }
            );

            MessageBus.Current.Listen<StandardTokenAddressChanged>().Subscribe(x =>
                {
                    ContractAddress = x.Address;
                }
            );

            MessageBus.Current.Listen<AccountLoaded>().Subscribe(x =>
                {
                    Address = x.Account.Address;
                }
            );

            var isValidRefreshBalance = this.WhenAnyValue(x => x.Address, x => x.Url, x => x.ContractAddress,
                (address, url, contractAddress) => Utils.IsValidAddress(address) && Utils.IsValidAddress(contractAddress) && Utils.IsValidUrl(url));

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
            Ensure.ArgumentNotNull(this.Address, "Address");
            Ensure.ArgumentNotNull(this.Url, "Url");
            Ensure.ArgumentNotNull(this.ContractAddress, "ContractAddress");

            var web3 = new Web3.Web3(Url);
            var handler = web3.Eth.GetContractHandler(ContractAddress);
            var balanceMessage = new BalanceOfFunction(){Owner = Address};
            var balance = await handler.QueryAsync<BalanceOfFunction,BigInteger>(balanceMessage);
            
            //assuming all have 18 decimals
            var value = Web3.Web3.Convert.FromWeiToBigDecimal(balance);
            return decimal.Parse(value.ToString());
        }
    }
}