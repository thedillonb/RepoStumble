using System;
using System.Reactive.Linq;
using Xamarin.Utilities.Core.ViewModels;
using System.Collections.Generic;
using RepositoryStumble.Core.Data;
using Xamarin.Utilities.Core.Services;
using ReactiveUI;
using Akavache;

namespace RepositoryStumble.Core.ViewModels.Trending
{
    public class ShowcasesViewModel : BaseViewModel, ILoadableViewModel
    {
        private const string ShowcaseUrl = "http://trending.codehub-app.com/showcases";

        public IReadOnlyReactiveList<Showcase> Showcases { get; private set; }

        public IReactiveCommand<object> GoToShowcaseCommand { get; private set; }

        public IReactiveCommand LoadCommand { get; private set; }

        public ShowcasesViewModel(IJsonSerializationService jsonSerializationService, INetworkActivityService networkActivity)
        {
            GoToShowcaseCommand = ReactiveCommand.Create();
            GoToShowcaseCommand.OfType<Showcase>().Subscribe(x =>
            {
                var vm = CreateViewModel<ShowcaseViewModel>();
                vm.ShowcaseSlug = x.Slug;
                vm.Title = x.Name;
                ShowViewModel(vm);
            });

            var showcases = new ReactiveList<Showcase>();
            Showcases = showcases;

            LoadCommand = ReactiveCommand.CreateAsyncTask(async t =>
            {
                var data = await BlobCache.LocalMachine.DownloadUrl(ShowcaseUrl, absoluteExpiration: DateTimeOffset.Now.AddDays(1));
                var showcaseRepos = jsonSerializationService.Deserialize<List<Showcase>>(System.Text.Encoding.UTF8.GetString(data));
                showcases.Reset(showcaseRepos);
            });
            LoadCommand.TriggerNetworkActivity(networkActivity);
        }
    }
}

