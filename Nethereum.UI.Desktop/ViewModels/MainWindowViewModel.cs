using Nethereum.UI.HostProvider;
using Nethereum.UI.HostProvider.Services;
using Nethereum.UI.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Nethereum.UI.Desktop.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        [Reactive] public PrivateKeyLoaderViewModel PrivateKeyLoaderViewModel { get; set; }
        [Reactive] public AccountViewModel AccountViewModel { get; set; }

        [Reactive] public AccountsViewModel AccountsViewModel { get; set; }
        [Reactive] public UrlSettingViewModel UrlSettingViewModel { get; set; }

        [Reactive] public KeyStoreLoaderViewModel KeyStoreLoaderViewModel { get; set; }

        [Reactive] public SendTransactionViewModel SendTransactionViewModel { get; set; }

        [Reactive] public TransactionsViewModel TransactionsViewModel { get; set; }
        
        [Reactive] public StandardTokenViewModel StandardTokenViewModel { get; set; }
        
        [Reactive] public HdWalletAccountLoaderViewModel HdWalletAccountLoaderViewModel { get; set; }


        public MainWindowViewModel()
        {
           var nethereumHostProvider = Locator.Current.GetService<NethereumHostProvider>();
           var accountService = Locator.Current.GetService<AccountsService>();
           var currentAccountsTransactionsService = Locator.Current.GetService<CurrentAccountTransactionsService>();
           
            //Add an account to the account service so we have one already
           accountService.AddAccount(new Web3.Accounts.Account("0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7"));

           PrivateKeyLoaderViewModel = new PrivateKeyLoaderViewModel(accountService);
           PrivateKeyLoaderViewModel.PrivateKey =  "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
           //Default the private key    

           AccountViewModel = new AccountViewModel(nethereumHostProvider);
           AccountsViewModel = new AccountsViewModel(nethereumHostProvider, accountService);
           KeyStoreLoaderViewModel = new KeyStoreLoaderViewModel(accountService);
           SendTransactionViewModel = new SendTransactionViewModel(nethereumHostProvider);
           TransactionsViewModel =new TransactionsViewModel(nethereumHostProvider, currentAccountsTransactionsService);
           StandardTokenViewModel = new StandardTokenViewModel(nethereumHostProvider);
           UrlSettingViewModel = new UrlSettingViewModel(nethereumHostProvider);
           UrlSettingViewModel.Url = "http://localhost:8545";
           //Default simple url

           //Default someone to send something
           SendTransactionViewModel.AddressTo = "0x243e72b69141f6af525a9a5fd939668ee9f2b354";
           
           HdWalletAccountLoaderViewModel = new HdWalletAccountLoaderViewModel(accountService);
           //Default some words
           HdWalletAccountLoaderViewModel.Words = "ripple scissors kick mammal hire column oak again sun offer wealth tomorrow wagon turn fatal";
        }
    }
}
