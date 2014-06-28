using Xamarin.Utilities.Core.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RepositoryStumble.Core.Data
{
    public class ExternalResources
    {
        private readonly IJsonHttpClientService _jsonHttpClientService;

        public ExternalResources(IJsonHttpClientService jsonHttpClientService)
        {
            _jsonHttpClientService = jsonHttpClientService;
        }

        public Task<List<Language>> GetLanguages()
        {
            return _jsonHttpClientService.Get<List<Language>>("http://codehub-trending.herokuapp.com/languages");
        }
    }
}

