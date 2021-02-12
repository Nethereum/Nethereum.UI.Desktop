using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Nethereum.UI.HostProvider.Configuration
{
    [DataContract]
    public class EthereumConnection
    {
        [DataMember]
        public string Url { get; set; }
    }
}
