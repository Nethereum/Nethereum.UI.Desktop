using Genesis.Ensure;
using Nethereum.UI.HostProvider.Services;
using Nethereum.Web3.Accounts;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;

namespace Nethereum.UI.ViewModels
{
    public class PrivateKeyLoaderViewModel : ReactiveObject
    {
        [Reactive] public string PrivateKey { get; set; }
        
        private readonly ReactiveCommand<Unit, Unit> _loadAccountFromPrivateKeyCommand;
        private readonly AccountsService accountsService;

        public ReactiveCommand<Unit, Unit> LoadAccountFromPrivateKeyCommand => this._loadAccountFromPrivateKeyCommand;


        protected PrivateKeyLoaderViewModel()
        {
            //Designer only..
        }

        public PrivateKeyLoaderViewModel(AccountsService accountsService)
        {
            var canExecuteLoadPrivateKey = this.WhenAnyValue(
                x => x.PrivateKey,
                (privateKey) => !string.IsNullOrEmpty(privateKey));

            this._loadAccountFromPrivateKeyCommand = ReactiveCommand.Create(this.LoadAccountFromPrivateKey, canExecuteLoadPrivateKey);
            this.accountsService = accountsService;
        }

        public void LoadAccountFromPrivateKey()
        {
            Ensure.ArgumentNotNull(this.PrivateKey, "Private Key");
            var account = new Account(PrivateKey);
            accountsService.AddAccount(account);
        }

    }
}