using System;

namespace Nethereum.UI.Services
{
    public interface IContractService
    {
        IObservable<string> ContractAddress { get; }
        void SetContractAddress(string contractAddress);
    }
}