using Genesis.Ensure;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.UI.UIMessages;
using ReactiveUI;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Nethereum.UI.ViewModels
{
    public class SendTransactionViewModel : SendTransactionBaseViewModel
    {

        public SendTransactionViewModel()
        {
            Gas = (ulong)Signer.Transaction.DEFAULT_GAS_LIMIT;
            GasPrice = (ulong)Signer.Transaction.DEFAULT_GAS_PRICE;

            System.IObservable<bool> canExecuteTransaction = this.WhenAnyValue(
                x => x.AddressTo,
                x => x.AmountInEther,
                x => x.Url,
                x => x.Account,
                (addressTo, amountInEther, url, account) =>
                        Util.Utils.IsValidAddress(addressTo) &&
                        amountInEther != null &&
                        Util.Utils.IsValidUrl(url) &&
                        account != null);

            _executeTrasnactionCommand = ReactiveCommand.CreateFromTask(ExecuteAsync, canExecuteTransaction);
        }

        public async Task<string> ExecuteAsync()
        {
            Ensure.ArgumentNotNullOrEmpty(AddressTo, "Address To");
            Ensure.ArgumentNotNullOrEmpty(Url, "Url");
            Ensure.ArgumentNotNull(Account, "Account");
            Ensure.ArgumentNotNull(AmountInEther, "Amount in Ether");

            bool confirmed = await _confirmTransfer.Handle(GetConfirmationMessage());

            if (confirmed)
            {
                Web3.Web3 web3 = new Web3.Web3(Account, Url);

                TransactionInput transactionInput =
                    new TransactionInput
                    {
                        Value = new HexBigInteger(Web3.Web3.Convert.ToWei(AmountInEther.Value)),
                        To = AddressTo,
                        From = web3.TransactionManager.Account.Address
                    };
                if (Gas != null)
                {
                    transactionInput.Gas = new HexBigInteger(Gas.Value);
                }

                if (GasPrice != null)
                {
                    transactionInput.GasPrice = new HexBigInteger(GasPrice.Value);
                }

                if (Nonce != null)
                {
                    transactionInput.Nonce = new HexBigInteger(Nonce.Value);
                }

                if (!string.IsNullOrEmpty(Data))
                {
                    transactionInput.Data = Data;
                }

                string transactionHash = await web3.TransactionManager.SendTransactionAsync(transactionInput);
                MessageBus.Current.SendMessage<TransactionAdded>(new TransactionAdded(transactionHash));
                return transactionHash;
            }
            return null;
        }



        public string GetConfirmationMessage()
        {
            return
                $"Are you sure you want to submit this transaction: \n\r To: {AddressTo} \n\r Amount: {AmountInEther}";
        }
    }
}