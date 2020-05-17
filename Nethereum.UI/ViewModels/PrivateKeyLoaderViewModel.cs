using Genesis.Ensure;
using Nethereum.UI.UIMessages;
using Nethereum.Web3.Accounts;
using ReactiveUI;
using System.Reactive;

namespace Nethereum.UI.ViewModels
{
    public class PrivateKeyLoaderViewModel : ReactiveObject
    {
        private string _privateKey = "private";
        public string PrivateKey
        {
            get => _privateKey;
            set => this.RaiseAndSetIfChanged(ref _privateKey, value);
        }

        private readonly ReactiveCommand<Unit, string> _loadAccountFromPrivateKeyCommand;
        public ReactiveCommand<Unit, string> LoadAccountFromPrivateKeyCommand => _loadAccountFromPrivateKeyCommand;

        public PrivateKeyLoaderViewModel()
        {
            System.IObservable<bool> canExecuteLoadPrivateKey = this.WhenAnyValue(
                x => x.PrivateKey,
                (privateKey) => !string.IsNullOrEmpty(privateKey));

            _loadAccountFromPrivateKeyCommand = ReactiveCommand.Create(LoadAccountFromPrivateKey, canExecuteLoadPrivateKey);
        }

        public string LoadAccountFromPrivateKey()
        {
            Ensure.ArgumentNotNull(PrivateKey, "Private Key");
            Account account = new Account(_privateKey);

            MessageBus.Current.SendMessage(new AccountLoaded(account));

            return _privateKey;
        }

    }
}