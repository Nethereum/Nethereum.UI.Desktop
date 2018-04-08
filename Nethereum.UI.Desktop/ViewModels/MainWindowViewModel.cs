using Nethereum.UI.ViewModels;
using ReactiveUI;

namespace Nethereum.UI.Desktop.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {

        private PrivateKeyLoaderViewModel _privateKeyLoaderViewModel;
        public PrivateKeyLoaderViewModel PrivateKeyLoaderViewModel
        {
            get { return _privateKeyLoaderViewModel; }
            set
            {
                this.RaiseAndSetIfChanged(ref _privateKeyLoaderViewModel, value);
            }
        }


        private AccountViewModel _accountViewModel;
        public AccountViewModel AccountViewModel
        {
            get { return _accountViewModel; }
            set
            {
                this.RaiseAndSetIfChanged(ref _accountViewModel, value);
            }
        }

        private KeyStoreLoaderViewModel _keyStoreLoaderViewModel;
        public KeyStoreLoaderViewModel KeyStoreLoaderViewModel
        {
            get { return _keyStoreLoaderViewModel; }
            set
            {
                this.RaiseAndSetIfChanged(ref _keyStoreLoaderViewModel, value);
            }
        }

        private SendTransactionViewModel _sendTransactionViewModel;
        public SendTransactionViewModel SendTransactionViewModel
        {
            get { return _sendTransactionViewModel; }
            set
            {
                this.RaiseAndSetIfChanged(ref _sendTransactionViewModel, value);
            }
        }

        private TransactionsViewModel _transactionsViewModel;
        public TransactionsViewModel TransactionsViewModel
        {
            get { return _transactionsViewModel; }
            set
            {
                this.RaiseAndSetIfChanged(ref _transactionsViewModel, value);
            }
        }

        private StandardTokenViewModel _standardTokenViewModel;
        public StandardTokenViewModel StandardTokenViewModel
        {
            get { return _standardTokenViewModel; }
            set
            {
                this.RaiseAndSetIfChanged(ref _standardTokenViewModel, value);
            }
        }

        public MainWindowViewModel()
        {
           PrivateKeyLoaderViewModel = new PrivateKeyLoaderViewModel();
           AccountViewModel = new AccountViewModel();
           KeyStoreLoaderViewModel = new KeyStoreLoaderViewModel();
           SendTransactionViewModel = new SendTransactionViewModel();
           TransactionsViewModel =new TransactionsViewModel();
           StandardTokenViewModel = new StandardTokenViewModel();
           AccountViewModel.Url = "http://localhost:8545";
        }
    }
}
