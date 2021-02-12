using DynamicData;
using System.Linq;
using Nethereum.RPC.Eth.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace Nethereum.UI.HostProvider.Services
{
    public class CurrentAccountTransactionsService
    {
        private readonly IEthereumHostProvider ethereumHostProvider;
        private readonly SourceCache<CurrentAccountTransaction, string> _transactions = new SourceCache<CurrentAccountTransaction, string>(x => x.Transaction.TransactionHash);
        private readonly TimeSpan updateInterval;
        private string currentAccount;
        private IDisposable timer;
        private readonly object receiptsCheckLock = new object();

        public IObservableCache<CurrentAccountTransaction, string> Transactions
        {
            get { return _transactions.AsObservableCache(); }
        }

        public CurrentAccountTransactionsService(IEthereumHostProvider ethereumHostProvider, int updateIntervalMilliseconds = 2000)
        {
            updateInterval = TimeSpan.FromMilliseconds(updateIntervalMilliseconds);
            this.ethereumHostProvider = ethereumHostProvider;
            this.ethereumHostProvider.SelectedAccountCallback.Subscribe(address =>
            {
                if (address != currentAccount)
                {
                    currentAccount = address;
                    _transactions.Clear();
                    //We should save ... reload
                }
            });

            timer = Observable.Timer(updateInterval, updateInterval, RxApp.MainThreadScheduler)
                .Subscribe(async _ => await CheckReceiptsAsync());
        }

        public async Task AddNewTransactionAsync(string transactionHash)
        {
            var web3 = await ethereumHostProvider.GetWeb3Async();
            var transaction = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transactionHash);
            var currentTransaction = new CurrentAccountTransaction();
            currentTransaction.Transaction = transaction;
            _transactions.AddOrUpdate(currentTransaction);
        }

        public async Task CheckReceiptsAsync()
        {
            IEnumerable<CurrentAccountTransaction> transactionsInProgress = new List<CurrentAccountTransaction>();

            lock (receiptsCheckLock)
            {
                transactionsInProgress =
                    Transactions.Items.Where(x => x.Status == CurrentAccountTransaction.TransactionStatus.InProgress);
            }

            if (ethereumHostProvider.Enabled)
            {
                var web3 = await ethereumHostProvider.GetWeb3Async();
                foreach (var currentAccountTransaction in transactionsInProgress)
                {
                    var receipt = await web3.Eth.Transactions.GetTransactionReceipt
                        .SendRequestAsync(currentAccountTransaction.Transaction.TransactionHash);

                    if (receipt != null)
                    {
                        currentAccountTransaction.Transaction.BlockHash = receipt.BlockHash;
                        currentAccountTransaction.Transaction.BlockNumber = receipt.BlockNumber;
                        currentAccountTransaction.TransactionReceipt = receipt;
                        if (receipt.Failed())
                        {
                            currentAccountTransaction.Status = CurrentAccountTransaction.TransactionStatus.Failed;
                        }
                        else
                        {
                            currentAccountTransaction.Status = CurrentAccountTransaction.TransactionStatus.Completed;
                        }
                        _transactions.AddOrUpdate(currentAccountTransaction);
                    }
                }
            }
            
        }
    }
}
