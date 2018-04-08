using System.Reactive;
using Genesis.Ensure;
using ReactiveUI;

namespace Nethereum.UI.ViewModels
{
    public class HdWalletAccountLoaderViewModel : ReactiveObject
    {
        public ReactiveList<HdWalletAccountViewModel> Accounts { get; set; } = new ReactiveList<HdWalletAccountViewModel>();

        private string _words;
        private string _seedPassword;
        private string _path;

        public string Words
        {
            get { return _words; }
            set { this.RaiseAndSetIfChanged(ref _words, value); }
        }

        public string SeedPassword
        {
            get => _seedPassword;
            set => this.RaiseAndSetIfChanged(ref _seedPassword, value);
        }

        public string Path
        {
            get => _path;
            set => this.RaiseAndSetIfChanged(ref _path, value);
        }

        private readonly ReactiveCommand<Unit, Unit> _loadAccountsCommand;
        public ReactiveCommand<Unit, Unit> LoadAccountsCommand => this._loadAccountsCommand;

        public HdWalletAccountLoaderViewModel()
        {

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
                var hdWalletAccount = new HdWalletAccountViewModel
                {
                    Index = i,
                    Address = account.Address,
                    PrivateKey = account.PrivateKey
                };
                Accounts.Add(hdWalletAccount);
            }
        } 
    }


    public class HdWalletAccountViewModel : PrivateKeyLoaderViewModel
    {
        private int _index;
        public int Index
        {
            get => _index;
            set => this.RaiseAndSetIfChanged(ref _index, value);
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => this.RaiseAndSetIfChanged(ref _address, value);
        }
    }
}