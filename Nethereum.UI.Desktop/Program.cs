using Avalonia;
using Avalonia.ReactiveUI;
using Nethereum.UI.Desktop.ViewModels;
using Nethereum.UI.Desktop.Views;

namespace Nethereum.UI.Desktop
{
    class Program
    {
        public static void Main(string[] args) => BuildAvaloniaApp()
           .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}
