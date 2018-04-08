using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nethereum.UI.Desktop.Views.StandardToken
{
    public class StandardTokenBalanceOfUserControl : UserControl
    {
        public StandardTokenBalanceOfUserControl()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
