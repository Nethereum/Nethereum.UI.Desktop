namespace Nethereum.UI.UIMessages
{
    public class TransactionAdded
    {
        public TransactionAdded(string transactionHash)
        {
            TransactionHash = transactionHash;
        }

        public string TransactionHash { get; }
    }
}