using Avalonia;
using Avalonia.ReactiveUI;
using Nethereum.UI.Desktop.Views;
using Nethereum.UI.HostProvider;
using Nethereum.UI.HostProvider.Services;
using Nethereum.UI.Services;
using Nethereum.UI.ViewModels;
using ReactiveUI;
using Splat;

namespace Nethereum.UI.Desktop
{
    class Program
    {
        public static void Main(string[] args) => BuildAvaloniaApp()
           .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            var nethereumHostProvider = new NethereumHostProvider();
            var currentAccountTransactionsService = new CurrentAccountTransactionsService(nethereumHostProvider);
            var accountsService = new AccountsService(nethereumHostProvider);

            Locator.CurrentMutable.RegisterConstant(accountsService);
            Locator.CurrentMutable.RegisterConstant(nethereumHostProvider);
            Locator.CurrentMutable.RegisterConstant(nethereumHostProvider, typeof(IEthereumHostProvider));
            Locator.CurrentMutable.RegisterConstant(new ContractService(), typeof(IContractService));
            Locator.CurrentMutable.RegisterConstant(currentAccountTransactionsService);
            Locator.CurrentMutable.Register(() => new SendTransactionUserControl(), typeof(IViewFor<SendTransactionViewModel>));

            return AppBuilder.Configure<App>()
                  .UsePlatformDetect()
                  .LogToTrace()
                  .UseReactiveUI();

        }
    }
}
