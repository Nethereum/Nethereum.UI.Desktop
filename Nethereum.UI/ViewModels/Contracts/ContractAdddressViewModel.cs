using Nethereum.UI.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using System;
using System.Reactive.Linq;

namespace Nethereum.UI.ViewModels.Contracts
{
    public class ContractAdddressViewModel : ReactiveObject
    {
        private readonly IContractService contractService;
        [Reactive] public string ContractAddress { get; set; }
       
        private IObservable<bool> _isValidContractAddress;
        public extern bool ValidContractAddress { [ObservableAsProperty]get; }

        protected ContractAdddressViewModel() { }
        
        public ContractAdddressViewModel(IContractService contractService)
        {
            this.contractService = contractService;

            _isValidContractAddress = this.WhenAnyValue(
                x => x.ContractAddress,
                (contractAddress) => 
                        Util.Utils.IsValidAddress(contractAddress)
                    );

            _isValidContractAddress.ToPropertyEx(this, x => x.ValidContractAddress);

            _isValidContractAddress.Select(x => x = true).Subscribe(_ => contractService.SetContractAddress(ContractAddress));
        }

    }

}