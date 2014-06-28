using System;
using System.Reactive.Linq;
using Xamarin.Utilities.Core.ViewModels;
using System.Collections.Generic;
using RepositoryStumble.Core.Data;
using Xamarin.Utilities.Core.Services;
using ReactiveUI;

namespace RepositoryStumble.Core.ViewModels.Trending
{
    public class ShowcasesViewModel : LoadableViewModel
    {
        public ReactiveList<Showcase> Showcases { get; private set; }

        public IReactiveCommand GoToShowcaseCommand { get; private set; }

        public ShowcasesViewModel(IJsonHttpClientService jsonHttpClientService)
        {
            GoToShowcaseCommand = new ReactiveCommand();
            GoToShowcaseCommand.OfType<Showcase>().Subscribe(x =>
            {
                var vm = CreateViewModel<ShowcaseViewModel>();
                vm.ShowcaseSlug = x.Slug;
                ShowViewModel(vm);
            });

            Showcases = new ReactiveList<Showcase>();
            LoadCommand.RegisterAsyncTask(async t => Showcases.Reset(
                (await jsonHttpClientService.Get<List<Showcase>>("http://codehub-trending.herokuapp.com/showcases"))));
        }
    }
}

