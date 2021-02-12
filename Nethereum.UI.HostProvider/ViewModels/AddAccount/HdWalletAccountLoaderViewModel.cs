using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Genesis.Ensure;
using Nethereum.UI.HostProvider;
using Nethereum.UI.HostProvider.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Nethereum.UI.ViewModels
{
    public class HdWalletAccountLoaderViewModel : ReactiveObject
    {
        public ObservableCollection<HdWalletAccountViewModel> Accounts { get; set; }

        [Reactive] public string Words { get; set; }
        [Reactive] public string SeedPassword { get; set; }
        [Reactive] public string Path { get; set; }
        [Reactive] public HdWalletAccountViewModel SelectedAccount { get; set; }


        private readonly ReactiveCommand<Unit, Unit> _loadAccountsCommand;
        private readonly AccountsService accountsService;

        public ReactiveCommand<Unit, Unit> LoadAccountsCommand => this._loadAccountsCommand;

        public NethereumHostProvider NethereumHostProvider { get; }

        protected HdWalletAccountLoaderViewModel()
        {
            //Designer only..
        }

        public HdWalletAccountLoaderViewModel(AccountsService accountsService)
        {
            this.accountsService = accountsService;
            Accounts = new ObservableCollection<HdWalletAccountViewModel>();

            var canExecuteLoadAccounts= this.WhenAnyValue(
                x => x.Words, x => x.Path,
                (words, path) => !string.IsNullOrEmpty(words) && !string.IsNullOrEmpty(path));

            this._loadAccountsCommand = ReactiveCommand.Create(this.LoadAccounts, canExecuteLoadAccounts);
            Path = HdWallet.Wallet.DEFAULT_PATH;
           

          
        }


        public void LoadAccounts()
        {
            Ensure.ArgumentNotNullOrEmpty(Words, "No words are set");
            Ensure.ArgumentNotNullOrEmpty(Path, "No path set");

            Accounts.Clear();
            var wallet = string.IsNullOrEmpty(SeedPassword) ? 
                new HdWallet.Wallet(Words, SeedPassword, Path) : 
                new HdWallet.Wallet(Words, Path);

            for (int i = 0; i < 20; i++)
            {
                var account = wallet.GetAccount(i);
                var hdWalletAccount = new HdWalletAccountViewModel(accountsService)
                {
                    Index = i,
                    Address = account.Address,
                    PrivateKey = account.PrivateKey
                };
                Accounts.Add(hdWalletAccount);
            }
        } 
    }
}