using Nethereum.UI.UIMessages;
using ReactiveUI;
using ReactiveUI.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Nethereum.UI.ViewModels
{
    public class TransactionsViewModel : ReactiveObject
    {
        public ReactiveList<TransactionViewModel> Transactions { get; set; } = new ReactiveList<TransactionViewModel>();
        private readonly TimeSpan updateInterval = TimeSpan.FromMilliseconds(2000);
        private readonly IDisposable timer;
        private readonly object receiptsCheckLock = new object();

        private string _url;
        public string Url
        {
            get => _url;
            set => this.RaiseAndSetIfChanged(ref _url, value);
        }

        public TransactionsViewModel()
        {
            Transactions.ChangeTrackingEnabled = true;

            MessageBus.Current.Listen<AccountLoaded>().Subscribe(x =>
                {
                    lock (receiptsCheckLock)
                    {
                        Transactions.Clear();
                    }
                }
            );

            MessageBus.Current.Listen<UrlChanged>().Subscribe(x =>
                {
                    lock (receiptsCheckLock)
                    {
                        Transactions.Clear();
                        Url = x.Url;
                    }
                }
            );

            MessageBus.Current.Listen<TransactionAdded>().Subscribe(async x =>
            {
                if (Util.Utils.IsValidUrl(Url))
                {
                    Web3.Web3 web3 = new Web3.Web3(Url);
                    TransactionViewModel transactionViewModel = new TransactionViewModel();
                    RPC.Eth.DTOs.Transaction transaction =
                        await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(x.TransactionHash);
                    transactionViewModel.Initialise(transaction);
                    transactionViewModel.Status = TransactionViewModel.STATUS_INPROGRESS;
                    lock (receiptsCheckLock)
                    {
                        Transactions.Add(transactionViewModel);
                    }
                }

            });

            timer = Observable.Timer(updateInterval, updateInterval, RxApp.MainThreadScheduler)
                .Subscribe(async _ => await CheckReceiptsAsync());
        }

        public async Task CheckReceiptsAsync()
        {
            IEnumerable<TransactionViewModel> transactionsInProgress = new List<TransactionViewModel>();

            lock (receiptsCheckLock)
            {
                transactionsInProgress =
                    Transactions.Where(x => x.Status == TransactionViewModel.STATUS_INPROGRESS);
            }

            if (Util.Utils.IsValidUrl(Url))
            {
                Web3.Web3 web3 = new Web3.Web3(Url);
                foreach (TransactionViewModel transaction in transactionsInProgress)
                {
                    RPC.Eth.DTOs.TransactionReceipt receipt = await web3.Eth.Transactions.GetTransactionReceipt
                        .SendRequestAsync(transaction.TransactionHash);

                    if (receipt != null)
                    {
                        transaction.BlockHash = receipt.BlockHash;
                        transaction.Status = TransactionViewModel.STATUS_COMPLETED;
                    }
                }
            }
        }
    }
}