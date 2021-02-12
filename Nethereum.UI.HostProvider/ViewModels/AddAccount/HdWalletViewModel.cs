using DynamicData;
using Nethereum.UI.HostProvider.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Nethereum.UI.ViewModels
{
    public class HdWalletAccountViewModel : PrivateKeyLoaderViewModel
    {
        public HdWalletAccountViewModel(AccountsService accountsService):base(accountsService)
        {
        }

        [Reactive] public int Index { get; set; }
        [Reactive] public string Address { get; set; }
    }
}