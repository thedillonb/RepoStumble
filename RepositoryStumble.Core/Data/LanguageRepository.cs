using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Akavache;
using System.Reactive.Linq;
using System.Text;
using Octokit.Internal;

namespace RepositoryStumble.Core.Data
{
    public class LanguageRepository
    {
        private const string LanguagesUrl = "http://trending.codehub-app.com/languages";

        public async Task<List<Language>> GetLanguages()
        {
            var trendingData = await BlobCache.LocalMachine.DownloadUrl(LanguagesUrl, absoluteExpiration: DateTimeOffset.Now.AddDays(1));
            var serializer = new SimpleJsonSerializer();
            return serializer.Deserialize<List<Language>>(Encoding.UTF8.GetString(trendingData));
        }
    }
}

