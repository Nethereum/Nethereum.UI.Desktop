using DynamicData;
using DynamicData.Binding;
using Nethereum.UI.HostProvider;
using Nethereum.UI.HostProvider.Services;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Nethereum.UI.ViewModels
{
    public class AccountsViewModel : ReactiveObject
    {
        private ReadOnlyObservableCollection<AccountItemViewModel> _accounts;
        public ReadOnlyObservableCollection<AccountItemViewModel> Accounts => _accounts;
        private readonly AccountsService _accountsService;
        private readonly NethereumHostProvider _nethereumHostProvider;
        protected AccountsViewModel()
        {

        }

        public AccountsViewModel(NethereumHostProvider nethereumHostProvider, AccountsService accountsService)
        {
            _accountsService = accountsService;
            _nethereumHostProvider = nethereumHostProvider;

            _accountsService.Accounts.Connect()
                .Transform(account => 
                        new AccountItemViewModel(account.Address, _nethereumHostProvider, accountsService)
                        )
                .AutoRefresh()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _accounts)
                .DisposeMany()
                .Subscribe();
        }
    }
}