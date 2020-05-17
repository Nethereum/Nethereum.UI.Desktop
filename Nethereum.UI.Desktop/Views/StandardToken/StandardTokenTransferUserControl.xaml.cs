using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nethereum.StandardToken.UI.ViewModels;
using ReactiveUI;
using System;

namespace Nethereum.UI.Desktop.Views.StandardToken
{
    public class StandardTokenTransferUserControl : UserControl, IViewFor<StandardTokenTransferViewModel>
    {
        public StandardTokenTransferUserControl()
        {
            InitializeComponent();
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            ViewModel = (StandardTokenTransferViewModel)DataContext;

            if (ViewModel != null)
            {
                ViewModel
                    .ConfirmTransfer
                    .RegisterHandler(
                        interaction =>
                        {
                            bool dialogResult = true;
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

        private Window GetWindow()
        {
            return (Window)VisualRoot;
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (StandardTokenTransferViewModel)value;
        }

        public StandardTokenTransferViewModel ViewModel { get; set; }
    }
}
