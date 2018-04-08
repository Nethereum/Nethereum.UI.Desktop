using Avalonia;
using Avalonia.Logging.Serilog;
using Nethereum.UI.Desktop.ViewModels;
using Nethereum.UI.Desktop.Views;

namespace Nethereum.UI.Desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
