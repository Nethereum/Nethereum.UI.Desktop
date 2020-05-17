using Genesis.Ensure;
using Nethereum.Hex.HexTypes;
using Nethereum.StandardToken.UI.SmartContractMessages;
using Nethereum.StandardToken.UI.UIMessages;
using Nethereum.UI.UIMessages;
using Nethereum.UI.Util;
using Nethereum.UI.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Nethereum.StandardToken.UI.ViewModels
{
    public class StandardTokenTransferViewModel : SendTransactionBaseViewModel
    {
        private string _contractAddress;
        public string ContractAddress
        {
            get => _contractAddress;
            set => this.RaiseAndSetIfChanged(ref _contractAddress, value);
        }

        private decimal? _amount;
        public decimal? Amount
        {
            get => _amount;
            set => this.RaiseAndSetIfChanged(ref _amount, value);
        }

        public StandardTokenTransferViewModel()
        {
            GasPrice = (ulong)Signer.Transaction.DEFAULT_GAS_PRICE;

            MessageBus.Current.Listen<StandardTokenAddressChanged>().Subscribe(x =>
                {
                    ContractAddress = x.Address;
                }
            );

            IObservable<bool> canExecuteTransaction = this.WhenAnyValue(
                x => x.AddressTo,
                x => x.Amount,
                x => x.Url,
                x => x.Account,
                (addressTo, amount, url, account) =>
                    Utils.IsValidAddress(addressTo) &&
                    amount != null &&
                    Utils.IsValidUrl(url) &&
                    account != null);

            _executeTrasnactionCommand = ReactiveCommand.CreateFromTask(ExecuteAsync, canExecuteTransaction);
        }


        public async Task<string> ExecuteAsync()
        {
            Ensure.ArgumentNotNullOrEmpty(AddressTo, "Address To");
            Ensure.ArgumentNotNullOrEmpty(Url, "Url");
            Ensure.ArgumentNotNull(Account, "Account");
            Ensure.ArgumentNotNull(Amount, "Token Amount");

            bool confirmed = await _confirmTransfer.Handle(GetConfirmationMessage());

            if (confirmed)
            {
                Web3.Web3 web3 = new Web3.Web3(Account, Url);

                TransferFunction transferfuction =
                    new TransferFunction
                    {
                        TokenAmount = Web3.Web3.Convert.ToWei(Amount.Value),
                        To = AddressTo,
                        FromAddress = web3.TransactionManager.Account.Address
                    };

                if (Gas != null)
                {
                    transferfuction.Gas = new HexBigInteger(Gas.Value);
                }

                if (GasPrice != null)
                {
                    transferfuction.GasPrice = new HexBigInteger(GasPrice.Value);
                }

                Contracts.ContractHandlers.ContractHandler handler = web3.Eth.GetContractHandler(ContractAddress);
                string transactionHash = await handler.SendRequestAsync(transferfuction);
                MessageBus.Current.SendMessage<TransactionAdded>(new TransactionAdded(transactionHash));
                return transactionHash;
            }
            return null;
        }

        public string GetConfirmationMessage()
        {
            return
                $"Are you sure you want to make this transfer: \n\r To: {AddressTo} \n\r Token Amount: {Amount}";
        }
    }
}