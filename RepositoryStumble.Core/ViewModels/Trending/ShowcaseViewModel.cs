using System;
using System.Reactive.Linq;
using ReactiveUI;
using RepositoryStumble.Core.Data;
using Xamarin.Utilities.Core.Services;
using Xamarin.Utilities.Core.ViewModels;
using RepositoryStumble.Core.ViewModels.Repositories;

namespace RepositoryStumble.Core.ViewModels.Trending
{
    public class ShowcaseViewModel : LoadableViewModel
    {
        public IReadOnlyReactiveList<ShowcaseRepository> Repositories { get; private set; }

        public IReactiveCommand GoToRepositoryCommand { get; private set; }

        public string ShowcaseSlug { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }

        private Showcase _showcase;
        public Showcase Showcase
        {
            get { return _showcase; }
            private set { this.RaiseAndSetIfChanged(ref _showcase, value); }
        }

        public ShowcaseViewModel(IJsonHttpClientService jsonHttpClientService)
        {
            Title = "Showcase";

            GoToRepositoryCommand = new ReactiveCommand();
            GoToRepositoryCommand.OfType<ShowcaseRepository>().Subscribe(x =>
            {
                var vm = CreateViewModel<RepositoryViewModel>();
                vm.RepositoryIdentifier = new BaseRepositoryViewModel.RepositoryIdentifierModel(x.Owner, x.Name);
                ShowViewModel(vm);
            });

            var repositories = new ReactiveList<ShowcaseRepository>();
            Repositories = repositories;
            LoadCommand.RegisterAsyncTask(async t =>
            {
                var url = string.Format("http://trending.codehub-app.com/showcases/{0}", ShowcaseSlug);
                var data = await jsonHttpClientService.Get<ShowcaseRepositories>(url);
                Title = data.Name;
                Showcase = new Showcase {Slug = data.Slug, Description = data.Description, Name = data.Name};
                repositories.Reset(data.Repositories);
            });
        }
    }
}