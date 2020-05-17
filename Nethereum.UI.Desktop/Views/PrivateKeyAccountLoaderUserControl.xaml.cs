using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nethereum.UI.Desktop.Views
{
    public class PrivateKeyAccountLoaderUserControl : UserControl
    {
        public PrivateKeyAccountLoaderUserControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
