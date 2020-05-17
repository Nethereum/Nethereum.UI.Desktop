using Nethereum.UI.ViewModels;

namespace Nethereum.UI.Desktop.Views
{
    public class TransactionsViewModelDesigner : TransactionsViewModel
    {
        public TransactionsViewModelDesigner()
        {
            Transactions.Add(new TransactionViewModel()
            {
                Amount = 10,
                BlockHash = "0x13f022d72158410433cbd66f5dd8bf6d2d129924",
                TransactionHash = "0x13f022d72158410433cbd66f5dd8bf6d2d129924",
                To = "0x13f022d72158410433cbd66f5dd8bf6d2d129924",
                From = "0x13f022d72158410433cbd66f5dd8bf6d2d129924",
                Gas = 12345,
                Nonce = 2,
                Status = TransactionViewModel.STATUS_INPROGRESS

            });

            Transactions.Add(new TransactionViewModel()
            {
                Amount = 15,
                BlockHash = "0x13f022d72158410433cbd66f5dd8bf6d2d129924",
                TransactionHash = "0x13f022d72158410433cbd66f5dd8bf6d2d129924",
                To = "0x13f022d72158410433cbd66f5dd8bf6d2d129924",
                From = "0x13f022d72158410433cbd66f5dd8bf6d2d129924",
                Gas = 345678,
                Nonce = 1,
                Status = TransactionViewModel.STATUS_COMPLETED

            });
        }
    }
}