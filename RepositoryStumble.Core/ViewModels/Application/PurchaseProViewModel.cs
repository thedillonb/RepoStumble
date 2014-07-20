using System;
using Xamarin.Utilities.Core.ViewModels;
using ReactiveUI;
using RepositoryStumble.Core.Services;
using System.Threading.Tasks;

namespace RepositoryStumble.Core.ViewModels.Application
{
    public class PurchaseProViewModel : BaseViewModel
    {
        public IReactiveCommand PurchaseCommand { get; private set; }

        private string _price;
        public string Price
        {
            get { return _price; }
            private set { this.RaiseAndSetIfChanged(ref _price, value); }
        }

        public PurchaseProViewModel(IFeaturesService featuresService)
        {
            var purchaseCommand = ReactiveCommand.CreateAsyncTask(x => featuresService.EnableProEdition());
            purchaseCommand.Subscribe(_ => DismissCommand.ExecuteIfCan());
            PurchaseCommand = purchaseCommand;

            GetPrice(featuresService);
        }

        private async Task GetPrice(IFeaturesService featuresService)
        {
            Price = await featuresService.GetProEditionPrice();
        }
    }
}

