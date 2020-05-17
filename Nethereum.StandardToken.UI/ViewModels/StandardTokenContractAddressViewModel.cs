using Nethereum.StandardToken.UI.UIMessages;
using Nethereum.UI.Util;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace Nethereum.StandardToken.UI.ViewModels
{
    public class StandardTokenContractAddressViewModel : ReactiveObject
    {
        private string _contractAddress;

        public string ContractAddress
        {
            get => _contractAddress;
            set => this.RaiseAndSetIfChanged(ref _contractAddress, value);
        }

        public StandardTokenContractAddressViewModel()
        {
            IObservable<bool> isValidContractAddresss = this.WhenAnyValue(x => x.ContractAddress,
                (contractAddress) => Utils.IsValidAddress(contractAddress));

            isValidContractAddresss.Where(x => x == true)
                .Subscribe(_ => MessageBus.Current.SendMessage(new StandardTokenAddressChanged(ContractAddress)));
        }

    }
}