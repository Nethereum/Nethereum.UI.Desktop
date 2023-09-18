using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nethereum.UI.Desktop.ViewModels;
using ReactiveUI;
using System;

namespace Nethereum.StandardToken.Desktop
{
    public partial class StandardTokenUserControl : UserControl, IViewFor<StandardTokenViewModel>
    {
        public StandardTokenUserControl()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            this.ViewModel = (StandardTokenViewModel)DataContext;
        }

        Window GetWindow() => (Window)this.VisualRoot;

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (StandardTokenViewModel)value; }
        }

        public StandardTokenViewModel ViewModel { get; set; }
    }
}
