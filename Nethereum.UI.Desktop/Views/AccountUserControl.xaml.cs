using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nethereum.UI.Desktop.Views
{
    public class AccountUserControl : UserControl
    {
        public AccountUserControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
