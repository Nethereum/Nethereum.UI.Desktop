using Nethereum.RPC.Eth.DTOs;
using ReactiveUI;

namespace Nethereum.UI.ViewModels
{
    public class TransactionViewModel : ReactiveObject
    {

        public const string STATUS_INPROGRESS = "Pending";
        public const string STATUS_COMPLETED = "Completed";


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
            
            if (transaction.Value != null) this.Amount = Web3.Web3.Convert.FromWei(transaction.Value.Value);
        }

        private string _blockHash;
        public string BlockHash
        {
            get => _blockHash;
            set => this.RaiseAndSetIfChanged(ref _blockHash, value);
        }

        private string _transactionHash;
        public string TransactionHash
        {
            get => _transactionHash;
            set => this.RaiseAndSetIfChanged(ref _transactionHash, value);
        }

        private string _from;
        public string From
        {
            get => _from;
            set => this.RaiseAndSetIfChanged(ref _from, value);
        }

        private string _to;
        public string To
        {
            get => _to;
            set => this.RaiseAndSetIfChanged(ref _to, value);
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set => this.RaiseAndSetIfChanged(ref _amount, value);
        }

        private ulong? _gas;
        public ulong? Gas
        {
            get => _gas;
            set => this.RaiseAndSetIfChanged(ref _gas, value);
        }

        private string _data;
        public string Data
        {
            get => _data;
            set => this.RaiseAndSetIfChanged(ref _data, value);
        }

        private ulong? _gasPrice;
        public ulong? GasPrice
        {
            get => _gasPrice;
            set => this.RaiseAndSetIfChanged(ref _gasPrice, value);
        }

        private ulong _nonce;
        public ulong Nonce
        {
            get => _nonce;
            set => this.RaiseAndSetIfChanged(ref _nonce, value);
        }

        private string _status;

        public string Status
        {
            get => _status;
            set => this.RaiseAndSetIfChanged(ref _status, value);
        }
    }
}