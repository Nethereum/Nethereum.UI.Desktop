using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nethereum.UI.Desktop.Views
{
    public class TransactionsUserControl : UserControl
    {
        public TransactionsUserControl()
        {
            this.InitializeComponent();
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

       

    }
}
