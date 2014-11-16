using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Akavache;
using System.Reactive.Linq;
using Newtonsoft.Json;
using RepositoryStumble.Core.Utils;
using System.Text;

namespace RepositoryStumble.Core.Data
{
    public class LanguageRepository
    {
        private const string LanguagesUrl = "http://trending.codehub-app.com/v2/languages";

        public async Task<List<Language>> GetLanguages()
        {
            var trendingData = await BlobCache.LocalMachine.DownloadUrl(LanguagesUrl, absoluteExpiration: DateTimeOffset.Now.AddDays(1));
            return JsonConvert.DeserializeObject<List<Language>>(Encoding.UTF8.GetString(trendingData), new JsonSerializerSettings {
                ContractResolver = new UnderscoreContractResolver()
            });
        }
    }
}

