using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nethereum.UI.ViewModels.Contracts;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using System;
using System.Reactive.Disposables;

namespace Nethereum.UI.Desktop.Common
{
    public class ContractAddressUserControl : UserControl, IViewFor<ContractAdddressViewModel>
    {
        public ContractAddressUserControl()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            
            this.ViewModel = (ContractAdddressViewModel)DataContext;
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ContractAdddressViewModel)value; }
        }

        public ContractAdddressViewModel ViewModel { get; set; }
    }
}
