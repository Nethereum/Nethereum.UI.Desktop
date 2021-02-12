using Genesis.Ensure;
using Nethereum.RPC.Accounts;
using Nethereum.Signer;
using Nethereum.Web3.Accounts;
using Org.BouncyCastle.Cms;
using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Nethereum.UI.HostProvider
{
    public class NethereumHostProvider : IEthereumHostProvider
    {
        private readonly ISubject<string> _selectedAccountCallback = new ReplaySubject<string>();
        private readonly ISubject<bool> _enabledCallback = new ReplaySubject<bool>();
        public string Name => "Nethereum Host Provider";

        public bool Available => true;

        protected IAccount Account { get; private set; }

        protected string Url { get; set; }

        public string SelectedAccount { get { return Account == null ? null : Account.Address; } }

        public int SelectedNetwork { get; private set; }

        public bool Enabled => Account != null && !string.IsNullOrEmpty(Url);

        public IObservable<string> SelectedAccountCallback => _selectedAccountCallback;
        public IObservable<bool> EnabledCallBack => _enabledCallback;

        public event Func<string, Task> SelectedAccountChanged;
        public event Func<int, Task> NetworkChanged;
        public event Func<bool, Task> AvailabilityChanged;
        public event Func<bool, Task> EnabledChanged;

        public Task<bool> CheckProviderAvailabilityAsync()
        {
            return Task.FromResult(true);
        }

        public Task<string> EnableProviderAsync()
        {
            return Task.FromResult(SelectedAccount);
        }

        public Task<string> GetProviderSelectedAccountAsync()
        {
            return Task.FromResult(SelectedAccount);
        }

        public Task<int> GetProviderSelectedNetworkAsync()
        {
            return Task.FromResult(SelectedNetwork);
        }

        public Task<Web3.Web3> GetWeb3Async()
        {
            
            return Task.FromResult(new Web3.Web3(Account, Url));
        }

        public Task<string> SignMessageAsync(string message)
        {
            Ensure.ArgumentCondition(!string.IsNullOrEmpty(message), "message cannot be null", nameof(message));
            var signer = new EthereumMessageSigner();
            var signedMessage = signer.EncodeUTF8AndSign(message, new EthECKey(((Web3.Accounts.Account)Account).PrivateKey));
            return Task.FromResult(signedMessage);
        }

        public void SetSelectedAccount(string privateKey)
        {
            SetSelectedAccount(new Account(privateKey));
        }

        public void SetSelectedAccount(Account account)
        {
            Account = account;

            if (SelectedAccountChanged != null)
            {
                if (Account != null) {
                    
                    SelectedAccountChanged(Account.Address);
                }
                else
                {
                    SelectedAccountChanged(null);
                }
            }

            if (Account != null)
            {

                _selectedAccountCallback.OnNext(Account.Address);
            }
            else
            {
                _selectedAccountCallback.OnNext(null);
            }

            _enabledCallback.OnNext(Enabled);
            
        }

        public async Task<bool> SetUrl(string url, int networkId = 1)
        {
            Ensure.ArgumentCondition(Nethereum.UI.Util.Utils.IsValidUrl(url), "Invalid url", "Url");
            var web3 = new Web3.Web3(url);
            try
            {
                var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                Url = url;
                SelectedNetwork = networkId;

                if (NetworkChanged != null)
                {
                    await NetworkChanged(networkId);
                }

                _enabledCallback.OnNext(Enabled);
                return true;
            }
            catch // toasts and other stuff
            {
                Url = null;
                _enabledCallback.OnNext(Enabled);
                return false;
            }
        }
    }
}
