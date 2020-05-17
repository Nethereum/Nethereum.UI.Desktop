using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nethereum.UI.Desktop.Views.StandardToken
{
    public class StandardTokenUserControl : UserControl
    {
        public StandardTokenUserControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
