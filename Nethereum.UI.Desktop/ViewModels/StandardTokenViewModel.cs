using Nethereum.StandardToken.UI.ViewModels;
using ReactiveUI;

namespace Nethereum.UI.Desktop.ViewModels
{
    public class StandardTokenViewModel : ReactiveObject
    {
        private StandardTokenTransferViewModel _standardTokenTransferViewModel;
        public StandardTokenTransferViewModel StandardTokenTransferViewModel
        {
            get => _standardTokenTransferViewModel;
            set => this.RaiseAndSetIfChanged(ref _standardTokenTransferViewModel, value);
        }

        private StandardTokenContractAddressViewModel _standardTokenContractAddressViewModel;
        public StandardTokenContractAddressViewModel StandardTokenContractAddressViewModel
        {
            get => _standardTokenContractAddressViewModel;
            set => this.RaiseAndSetIfChanged(ref _standardTokenContractAddressViewModel, value);
        }

        private StandardTokenBalanceOfViewModel _standardTokenBalanceOfViewModel;
        public StandardTokenBalanceOfViewModel StandardTokenBalanceOfViewModel
        {
            get => _standardTokenBalanceOfViewModel;
            set => this.RaiseAndSetIfChanged(ref _standardTokenBalanceOfViewModel, value);
        }

        public StandardTokenViewModel()
        {
            StandardTokenContractAddressViewModel = new StandardTokenContractAddressViewModel();
            StandardTokenBalanceOfViewModel = new StandardTokenBalanceOfViewModel();
            StandardTokenTransferViewModel = new StandardTokenTransferViewModel();
        }
    }
}