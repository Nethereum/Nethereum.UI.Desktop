using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using Nethereum.UI.ViewModels;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;

namespace Nethereum.UI.Desktop.Views
{
    public class SendTransactionUserControl : UserControl, IViewFor<SendTransactionViewModel>
    {
        public SendTransactionUserControl()
        {
            this.InitializeComponent();
        }

        private TextBlock ToValidation => this.FindControl<TextBlock>("ToValidation");
        private TextBlock AmountValidation => this.FindControl<TextBlock>("AmountValidation");

        protected override void OnDataContextChanged(EventArgs e)
        {
            this.ViewModel = (SendTransactionViewModel)DataContext;

            this.WhenActivated(disposables =>
            {
                this.BindValidation(ViewModel, x => x.AddressTo, x => x.ToValidation.Text)
                    .DisposeWith(disposables);

                this.BindValidation(ViewModel, x => x.AmountInEther, x => x.AmountValidation.Text)
                    .DisposeWith(disposables);
            });


            if (ViewModel != null)
            {
                this.ViewModel
                    .ConfirmTransfer
                    .RegisterHandler( async
                        interaction =>
                        {
                            //TODO Confirm Dialog
                            https://github.com/AvaloniaUI/Avalonia/issues/670

                            var messageBox = MessageBox.Avalonia.MessageBoxManager
                                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                                    {
                                        ButtonDefinitions = ButtonEnum.OkCancel,
                                        ContentTitle = "Confirm Transaction",
                                        ContentMessage = interaction.Input,
                                        Icon = Icon.Warning,
                                        Style = Style.DarkMode
                                    });

                            var result = await messageBox.ShowDialog(GetWindow());

                            if (result == ButtonResult.Ok)
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
            set { ViewModel = (SendTransactionViewModel) value; }
        }

        public SendTransactionViewModel ViewModel { get; set; }
    }
}
