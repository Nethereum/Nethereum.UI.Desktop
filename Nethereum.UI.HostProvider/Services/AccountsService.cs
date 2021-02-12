using DynamicData;
using Nethereum.Web3.Accounts;
using System.Linq;
using System.Reactive.Linq;

namespace Nethereum.UI.HostProvider.Services
{
    public class AccountsService
    {
        private readonly NethereumHostProvider nethereumHostProvider;
        private readonly SourceCache<Account, string> _accounts = new SourceCache<Account, string>(x => x.Address);

        public IObservableCache<Account, string> Accounts
        {
            get { return _accounts.AsObservableCache(); }
        }

        public AccountsService(NethereumHostProvider nethereumHostProvider)
        {
            this.nethereumHostProvider = nethereumHostProvider;
        }

        public void AddAccount(Account account)
        {
            _accounts.AddOrUpdate(account);
            nethereumHostProvider.SetSelectedAccount(account);
        }

        public void SetSelectedAccount(string address)
        {
            var account = _accounts.Items.FirstOrDefault(x => x.Address == address);
            nethereumHostProvider.SetSelectedAccount(account);
        }

        public void RemoveAccount(Account account)
        {
            _accounts.RemoveKey(account.Address);
            if(nethereumHostProvider.SelectedAccount == account.Address)
            {
                var firstAccount = Accounts.KeyValues.FirstOrDefault().Value;
                nethereumHostProvider.SetSelectedAccount(firstAccount);
            }
        }
    }
}
