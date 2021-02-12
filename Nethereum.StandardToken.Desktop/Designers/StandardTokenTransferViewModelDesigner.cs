using Nethereum.StandardToken.UI.ViewModels;
using Nethereum.UI.ViewModels;

namespace Nethereum.StandardToken.Desktop.Designers
{
    public class StandardTokenTransferViewModelDesigner : StandardTokenTransferViewModel
    {
        public StandardTokenTransferViewModelDesigner()
        {
            this.AddressTo = "0x13f022d72158410433cbd66f5dd8bf6d2d129924";
            this.Amount = 100;
            
        }
    }
}