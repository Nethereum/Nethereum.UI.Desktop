using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nethereum.UI.ViewModels;

namespace Nethereum.UI.Desktop.Views
{
    public class HdWalletUserControl : UserControl
    {
        public HdWalletUserControl()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        Window GetWindow() => (Window)this.VisualRoot;

    }
}
