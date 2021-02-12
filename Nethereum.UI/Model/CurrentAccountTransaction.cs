using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.UI.HostProvider.Services
{
    public class CurrentAccountTransaction
    {
        public enum TransactionStatus
        {
            InProgress,
            Completed,
            Failed
        }

        public Transaction Transaction { get; set; }
        public TransactionReceipt TransactionReceipt { get; set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.InProgress;
    }
}
