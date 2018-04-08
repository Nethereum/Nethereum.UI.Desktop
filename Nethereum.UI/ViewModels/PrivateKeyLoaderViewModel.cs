using System.Reactive;
using Genesis.Ensure;
using Nethereum.UI.UIMessages;
using Nethereum.Web3.Accounts;
using ReactiveUI;

namespace Nethereum.UI.ViewModels
{
    public class PrivateKeyLoaderViewModel : ReactiveObject
    {
        private string _privateKey;
        public string PrivateKey
        {
            get => _privateKey;
            set => this.RaiseAndSetIfChanged(ref _privateKey, value);
        }

        private readonly ReactiveCommand<Unit, string> _loadAccountFromPrivateKeyCommand;
        public ReactiveCommand<Unit, string> LoadAccountFromPrivateKeyCommand => this._loadAccountFromPrivateKeyCommand;

        public PrivateKeyLoaderViewModel()
        {
            var canExecuteLoadPrivateKey = this.WhenAnyValue(
                x => x.PrivateKey,
                (privateKey) => !string.IsNullOrEmpty(privateKey));

            this._loadAccountFromPrivateKeyCommand = ReactiveCommand.Create(this.LoadAccountFromPrivateKey, canExecuteLoadPrivateKey);
        }

        public string LoadAccountFromPrivateKey()
        {
            Ensure.ArgumentNotNull(this.PrivateKey, "Private Key");
            var account = new Account(_privateKey);

            MessageBus.Current.SendMessage(new AccountLoaded(account));

            return _privateKey;
        }

    }
}