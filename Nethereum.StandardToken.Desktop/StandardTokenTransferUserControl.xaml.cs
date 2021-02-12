using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nethereum.StandardToken.UI.ViewModels;
using ReactiveUI.Validation.Extensions;
using ReactiveUI;
using System.Reactive.Disposables;

namespace Nethereum.StandardToken.Desktop
{
    public class StandardTokenTransferUserControl : UserControl, IViewFor<StandardTokenTransferViewModel>
    {
        private TextBlock ToValidation => this.FindControl<TextBlock>("ToValidation");
        private TextBlock AmountValidation => this.FindControl<TextBlock>("AmountValidation");

        public StandardTokenTransferUserControl()
        {
            this.InitializeComponent();
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            this.ViewModel = (StandardTokenTransferViewModel)DataContext;

            this.WhenActivated(disposables =>
            {
                this.BindValidation(ViewModel, x => x.AddressTo, x => x.ToValidation.Text)
                    .DisposeWith(disposables);

                this.BindValidation(ViewModel, x => x.Amount, x => x.AmountValidation.Text)
                    .DisposeWith(disposables);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        Window GetWindow() => (Window)this.VisualRoot;

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (StandardTokenTransferViewModel) value; }
        }

        public StandardTokenTransferViewModel ViewModel { get; set; }
    }
}
