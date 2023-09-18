using Nethereum.Model;
using Nethereum.UI.ViewModels;

namespace Nethereum.UI.Desktop.Views.Designers
{
    public class SendTransactionViewModelDesigner : SendTransactionViewModel
    {
        public SendTransactionViewModelDesigner()
        {
            this.AddressTo = "0x13f022d72158410433cbd66f5dd8bf6d2d129924";
            this.AmountInEther = 100;
            this.Gas = (ulong)SignedLegacyTransaction.DEFAULT_GAS_LIMIT;
            //this.GasPrice = (ulong)Nethereum.Signer.Transaction.DEFAULT_GAS_PRICE;
            this.Nonce = 1;

        }
    }
}