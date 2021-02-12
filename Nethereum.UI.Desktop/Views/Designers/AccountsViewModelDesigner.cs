using DynamicData;
using Nethereum.UI.ViewModels;

namespace Nethereum.UI.Desktop.Views
{
    public class AccountsViewModelDesigner : AccountsViewModel
    {
        public AccountsViewModelDesigner()
        {
            Accounts.Add(new[] { new AccountItemViewModel()
            {
                Address = "0x13f022d72158410433cbd66f5dd8bf6d2d129924",
                Balance = 100
            },
            new AccountItemViewModel()
            {
                Address = "0x13f022d72158410433cbd66f5dd8bf6d2d129924",
                Balance = 10000
            } }); ;
        }
    }
}