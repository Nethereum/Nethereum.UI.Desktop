using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nethereum.StandardToken.Desktop
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
