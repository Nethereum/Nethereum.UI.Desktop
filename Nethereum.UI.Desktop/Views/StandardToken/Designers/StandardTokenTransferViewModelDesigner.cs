using Nethereum.StandardToken.UI.ViewModels;

namespace Nethereum.UI.Desktop.Views.StandardToken.Designers
{
    public class StandardTokenTransferViewModelDesigner : StandardTokenTransferViewModel
    {
        public StandardTokenTransferViewModelDesigner()
        {
            AddressTo = "0x13f022d72158410433cbd66f5dd8bf6d2d129924";
            Amount = 100;
            Gas = (ulong)Nethereum.Signer.Transaction.DEFAULT_GAS_LIMIT;
            GasPrice = (ulong)Nethereum.Signer.Transaction.DEFAULT_GAS_PRICE;
        }
    }
}