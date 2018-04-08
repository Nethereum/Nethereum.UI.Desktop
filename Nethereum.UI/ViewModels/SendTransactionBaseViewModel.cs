using System;
using System.Reactive;
using Nethereum.UI.UIMessages;
using Nethereum.Web3.Accounts;
using ReactiveUI;

namespace Nethereum.UI.ViewModels
{
    public class SendTransactionBaseViewModel : ReactiveObject
    {
        private Account _account;
        public Account Account
        {
            get => _account;
            set => this.RaiseAndSetIfChanged(ref _account, value);
        }

        private string _url;
        public string Url
        {
            get => _url;
            set => this.RaiseAndSetIfChanged(ref _url, value);
        }

        private string _addressTo;
        public string AddressTo
        {
            get => _addressTo;
            set => this.RaiseAndSetIfChanged(ref _addressTo, value);
        }

        private decimal? _amountInEther;
        public decimal? AmountInEther
        {
            get => _amountInEther;
            set => this.RaiseAndSetIfChanged(ref _amountInEther, value);
        }

        private ulong? _gas;
        public ulong? Gas
        {
            get => _gas;
            set => this.RaiseAndSetIfChanged(ref _gas, value);
        }

        private ulong? _nonce;
        public ulong? Nonce
        {
            get => _nonce;
            set => this.RaiseAndSetIfChanged(ref _nonce, value);
        }

        private string _data;
        public string Data
        {
            get => _data;
            set => this.RaiseAndSetIfChanged(ref _data, value);
        }

        private ulong? _gasPrice;
        public ulong? GasPrice
        {
            get => _gasPrice;
            set => this.RaiseAndSetIfChanged(ref _gasPrice, value);
        }

        protected readonly Interaction<string, bool> _confirmTransfer;
        public Interaction<string, bool> ConfirmTransfer => this._confirmTransfer;

        protected ReactiveCommand<Unit, string> _executeTrasnactionCommand;
        public ReactiveCommand<Unit, string> ExecuteTransactionCommand => this._executeTrasnactionCommand;

        public SendTransactionBaseViewModel()
        {
            this._confirmTransfer = new Interaction<string, bool>();
            GasPrice = (ulong)Signer.Transaction.DEFAULT_GAS_PRICE;

            MessageBus.Current.Listen<AccountLoaded>().Subscribe(x =>
                {
                    Account = x.Account;
                }
            );

            MessageBus.Current.Listen<UrlChanged>().Subscribe(x =>
                {
                    Url = x.Url;
                }
            );
        }
    }
}