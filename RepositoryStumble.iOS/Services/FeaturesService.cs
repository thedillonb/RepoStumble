using System;
using RepositoryStumble.Core.Services;
using Xamarin.Utilities.Core.Services;
using Xamarin.Utilities.Purchases;

namespace RepositoryStumble.Services
{
    public class FeaturesService : IFeaturesService
    {
        private const string ProEditionKey = "repostumble.pro";
        private readonly IDefaultValueService _defaultValueService;

        public FeaturesService(IDefaultValueService defaultValueService)
        {
            _defaultValueService = defaultValueService;
        }

        public async System.Threading.Tasks.Task EnableProEdition()
        {
            var productData = await InAppPurchases.Instance.RequestProductData(ProEditionKey);
            if (productData.Products.Length == 0)
                throw new Exception("No Such Product!");
            await InAppPurchases.Instance.PurchaseProduct(productData.Products[0]);
            _defaultValueService.Set(ProEditionKey, true);
        }

        public bool ProEditionEnabled
        {
            get { return _defaultValueService.Get<bool>(ProEditionKey); }
        }

        public async System.Threading.Tasks.Task<string> GetProEditionPrice()
        {
            var productData = await InAppPurchases.Instance.RequestProductData(ProEditionKey);
            if (productData.Products.Length == 0)
                throw new Exception("No Such Product!");
            return productData.Products[0].LocalizedPrice();
        }

        public System.Threading.Tasks.Task RestorePurchase()
        {
            return InAppPurchases.Instance.Restore();
        }
    }
}

