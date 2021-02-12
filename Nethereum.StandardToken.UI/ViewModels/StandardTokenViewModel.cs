using Nethereum.StandardToken.UI.ViewModels;
using Nethereum.UI.Services;
using Nethereum.UI.ViewModels.Contracts;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Nethereum.UI.Desktop.ViewModels
{
    public class StandardTokenViewModel : ReactiveObject, IScreen
    {
        private readonly RoutingState routingState;

        [Reactive] public StandardTokenTransferViewModel StandardTokenTransferViewModel { get; set; }
        [Reactive] public ContractAdddressViewModel ContractAdddressViewModel { get; set; }
        [Reactive] public StandardTokenBalanceOfViewModel StandardTokenBalanceOfViewModel { get; set; }

        public StandardTokenViewModel(IEthereumHostProvider ethereumHostProvider)
        {
            this.routingState = new RoutingState();

            var provider = ethereumHostProvider ?? Locator.Current.GetService<IEthereumHostProvider>();
            var contractService =Locator.Current.GetService<IContractService>();
            
            ContractAdddressViewModel = new ContractAdddressViewModel(contractService);
            StandardTokenBalanceOfViewModel = new StandardTokenBalanceOfViewModel(provider, contractService);
            StandardTokenTransferViewModel = new StandardTokenTransferViewModel(provider, contractService, this);  
        }

        public RoutingState Router => this.routingState;
    }
}