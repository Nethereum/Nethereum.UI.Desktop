using System;
using System.Reactive.Subjects;

namespace Nethereum.UI.Services
{
    public class ContractService : IContractService
    {
        private readonly ISubject<string> _contractAddress = new ReplaySubject<string>();

        public ContractService()
        {
            _contractAddress.OnNext(string.Empty);
        }

        public IObservable<string> ContractAddress => _contractAddress;

        public void SetContractAddress(string contractAddress)
        {
            _contractAddress.OnNext(contractAddress);
        }
    }

}