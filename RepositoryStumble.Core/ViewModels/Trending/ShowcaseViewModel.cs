using System;
using System.Reactive.Linq;
using ReactiveUI;
using RepositoryStumble.Core.Data;
using Xamarin.Utilities.Core.Services;
using Xamarin.Utilities.Core.ViewModels;

namespace RepositoryStumble.Core.ViewModels.Trending
{
    public class ShowcaseViewModel : LoadableViewModel
    {
        public ReactiveList<ShowcaseRepository> Repositories { get; private set; }

        public IReactiveCommand GoToRepositoryCommand { get; private set; }

        public string ShowcaseSlug { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            private set { this.RaiseAndSetIfChanged(ref _title, value); }
        }

        private Showcase _showcase;
        public Showcase Showcase
        {
            get { return _showcase; }
            private set { this.RaiseAndSetIfChanged(ref _showcase, value); }
        }

        public ShowcaseViewModel(IJsonHttpClientService jsonHttpClientService)
        {
            GoToRepositoryCommand = new ReactiveCommand();
            GoToRepositoryCommand.OfType<ShowcaseRepository>().Subscribe(x =>
            {

            });

            Repositories = new ReactiveList<ShowcaseRepository>();
            LoadCommand.RegisterAsyncTask(async t =>
            {
                var url = string.Format("http://codehub-trending.herokuapp.com/showcase?name={0}", ShowcaseSlug);
                var data = await jsonHttpClientService.Get<ShowcaseRepositories>(url);
                Title = data.Name;
                Showcase = new Showcase {Slug = data.Slug, Description = data.Description, Name = data.Name};
                Repositories.Reset(data.Repositories);
            });
        }
    }
}