using Nethereum.Util;
using System;

namespace Nethereum.UI.Util
{
    public class Utils
    {
        private static readonly AddressUtil _addressUtil = new AddressUtil();
        public static bool IsValidUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }

        public static bool IsValidAddress(string address)
        {
            return !string.IsNullOrEmpty(address) && _addressUtil.IsValidAddressLength(address);
        }
    }
}
