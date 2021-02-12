using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Nethereum.UI.HostProvider;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using ReactiveUI.Validation.States;

namespace Nethereum.UI.ViewModels
{
    public class UrlSettingViewModel : ReactiveValidationObject
    {
        [Reactive] public string Url { get; set; }
        private readonly NethereumHostProvider _ethereumHostProvider;
        public extern bool ValidUrl { [ObservableAsProperty]get; }
        public UrlSettingViewModel() { }

        public UrlSettingViewModel(NethereumHostProvider ethereumHostProvider)
        {
            _ethereumHostProvider = ethereumHostProvider;

            var validUrlAndConnection =
               this.WhenAnyValue(x => x.Url)
                   .Throttle(TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                   .SelectMany(x => ValidateAndSetUrl(x))
                   .ObserveOn(RxApp.MainThreadScheduler).ToPropertyEx(this, x => x.ValidUrl);
        }

        public async Task<bool> ValidateAndSetUrl(string url)
        {
            if (Util.Utils.IsValidUrl(url))
            {
                return  await _ethereumHostProvider.SetUrl(url);
            }
            else
            {
                return false;
            }
        }
    }
}