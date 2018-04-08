namespace Nethereum.StandardToken.UI.UIMessages
{
    public class StandardTokenAddressChanged
    {
        public StandardTokenAddressChanged(string address)
        {
            Address = address;
        }
        public string Address { get; }
    }
}