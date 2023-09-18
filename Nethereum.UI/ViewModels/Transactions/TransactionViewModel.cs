using Nethereum.RPC.Eth.DTOs;
using Nethereum.UI.HostProvider.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Nethereum.UI.ViewModels
{
    public class TransactionViewModel : ReactiveObject
    {
        public const string STATUS_INPROGRESS = "Pending";
        public const string STATUS_COMPLETED = "Completed";
        public const string STATUS_FAILED = "Failed";

        public TransactionViewModel()
        {

        }

        public TransactionViewModel(CurrentAccountTransaction currentAccountTransaction)
        {
            this.Initialise(currentAccountTransaction.Transaction);
            switch (currentAccountTransaction.Status)
            {
                case CurrentAccountTransaction.TransactionStatus.InProgress:
                    this.Status = STATUS_INPROGRESS;
                    break;
                case CurrentAccountTransaction.TransactionStatus.Completed:
                    this.Status = STATUS_COMPLETED;
                    break;
                case CurrentAccountTransaction.TransactionStatus.Failed:
                    this.Status = STATUS_FAILED;
                    break;
                default:
                    break;
            }   
        }

        public void Initialise(Transaction transaction)
        {
            this.TransactionHash = transaction.TransactionHash;
            this.BlockHash = transaction.BlockHash;
            this.Nonce = (ulong)transaction.Nonce.Value;
            this.From = transaction.From;
            this.To = transaction.To;
            this.Gas = (ulong)transaction.Gas.Value;
            this.GasPrice = (ulong)transaction.GasPrice.Value;
            this.Data = transaction.Input;
            if (transaction.BlockNumber != null)
            {
                this.BlockNumber = (ulong)transaction.BlockNumber.Value;
            }
            
            if (transaction.Value != null) this.Amount = Web3.Web3.Convert.FromWei(transaction.Value.Value);
        }

        [Reactive] public string BlockHash { get; set; }
        [Reactive] public ulong BlockNumber { get; set; }
        [Reactive] public string TransactionHash { get; set; }
        [Reactive] public string From { get; set; }
        [Reactive] public string To { get; set; }
        [Reactive] public decimal Amount { get; set; }
        [Reactive] public ulong? Gas { get; set; }
        [Reactive] public string Data { get; set; }
        [Reactive] public ulong? GasPrice { get; set; }
        [Reactive] public ulong Nonce { get; set; }
        [Reactive] public string Status { get; set; }
       
    }
}