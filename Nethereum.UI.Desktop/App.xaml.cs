using Avalonia;
using Avalonia.Markup.Xaml;

namespace Nethereum.UI.Desktop
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
