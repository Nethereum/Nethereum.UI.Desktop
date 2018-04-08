using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nethereum.StandardToken.UI.ViewModels;
using ReactiveUI;

namespace Nethereum.UI.Desktop.Views.StandardToken
{
    public class StandardTokenTransferUserControl : UserControl, IViewFor<StandardTokenTransferViewModel>
    {
        public StandardTokenTransferUserControl()
        {
            this.InitializeComponent();
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            this.ViewModel = (StandardTokenTransferViewModel)DataContext;

            if (ViewModel != null)
            {
                this.ViewModel
                    .ConfirmTransfer
                    .RegisterHandler(
                        interaction =>
                        {
                            var dialogResult = true;
                            //TODO Confirm Dialog
                            https://github.com/AvaloniaUI/Avalonia/issues/670

                            if (dialogResult == true)
                            {
                                interaction.SetOutput(true);
                            }
                            else
                            {
                                interaction.SetOutput(false);
                            }
                        });
            }
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
