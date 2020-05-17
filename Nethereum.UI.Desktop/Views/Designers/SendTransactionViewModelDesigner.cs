using Nethereum.UI.ViewModels;

namespace Nethereum.UI.Desktop.Views.Designers
{
    public class SendTransactionViewModelDesigner : SendTransactionViewModel
    {
        public SendTransactionViewModelDesigner()
        {
            AddressTo = "0x13f022d72158410433cbd66f5dd8bf6d2d129924";
            AmountInEther = 100;
            Gas = (ulong)Nethereum.Signer.Transaction.DEFAULT_GAS_LIMIT;
            GasPrice = (ulong)Nethereum.Signer.Transaction.DEFAULT_GAS_PRICE;
            Nonce = 1;

        }
    }
}