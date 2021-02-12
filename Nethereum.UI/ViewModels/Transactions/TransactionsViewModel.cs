using DynamicData;
using DynamicData.Binding;
using Nethereum.UI.HostProvider.Services;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Nethereum.UI.ViewModels
{
    public class TransactionsViewModel : ReactiveObject
    {
        private ReadOnlyObservableCollection<TransactionViewModel> _transactions;
        public ReadOnlyObservableCollection<TransactionViewModel> Transactions => _transactions;
        private readonly CurrentAccountTransactionsService currentAccountTransactionsService;
     
        protected TransactionsViewModel()
        {

        }

        public TransactionsViewModel(IEthereumHostProvider ethereumHostProvider, CurrentAccountTransactionsService currentAccountTransactionsService)
        {
            this.currentAccountTransactionsService = currentAccountTransactionsService;
            this.currentAccountTransactionsService.Transactions.Connect()
                .Transform(transaction => new TransactionViewModel(transaction))
                .AutoRefresh()
                .Sort(SortExpressionComparer<TransactionViewModel>.Descending(t => t.Status).ThenByDescending(t=> t.BlockNumber))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _transactions)
                .DisposeMany()
                .Subscribe();   
        }
    }
}