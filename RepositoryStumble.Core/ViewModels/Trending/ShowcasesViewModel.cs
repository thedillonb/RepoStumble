using System;
using Xamarin.Utilities.Core.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using RepositoryStumble.Core.Data;
using Xamarin.Utilities.Core.Services;
using ReactiveUI;

namespace RepositoryStumble.Core.ViewModels.Trending
{
    public class ShowcasesViewModel : LoadableViewModel
    {
        protected readonly IJsonHttpClientService JsonHttpClientService;

        public ReactiveList<Showcase> Showcases { get; private set; }

        public ShowcasesViewModel(IJsonHttpClientService jsonHttpClientService)
        {
            JsonHttpClientService = jsonHttpClientService;
            Showcases = new ReactiveList<Showcase>();
        }

        protected override async Task Load()
        {
            Showcases.Reset((await JsonHttpClientService.Get<List<Showcase>>("http://codehub-trending.herokuapp.com/showcases")));
        }
    }
}

