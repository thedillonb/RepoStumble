using System;
using System.Reactive.Linq;
using ReactiveUI;
using RepositoryStumble.Core.Data;
using RepositoryStumble.Core.ViewModels.Repositories;
using RepositoryStumble.Core.Services;

namespace RepositoryStumble.Core.ViewModels.Trending
{
    public class ShowcaseViewModel : BaseViewModel, ILoadableViewModel
    {
        public IReadOnlyReactiveList<Octokit.Repository> Repositories { get; private set; }

        public IReactiveCommand<object> GoToRepositoryCommand { get; private set; }

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

        public IReactiveCommand LoadCommand { get; private set; }

        public ShowcaseViewModel(INetworkActivityService networkActivity, ShowcaseRepository showcaseRepository)
        {
            Title = "Showcase";

            GoToRepositoryCommand = ReactiveCommand.Create();
            GoToRepositoryCommand.OfType<Octokit.Repository>().Subscribe(x =>
            {
                var vm = CreateViewModel<RepositoryViewModel>();
                vm.RepositoryIdentifier = new BaseRepositoryViewModel.RepositoryIdentifierModel(x.Owner.Login, x.Name);
                ShowViewModel(vm);
            });

            var repositories = new ReactiveList<Octokit.Repository>();
            Repositories = repositories;
            LoadCommand = ReactiveCommand.CreateAsyncTask(async t =>
            {
                var showcaseRepos = await showcaseRepository.GetShowcaseRepositories(ShowcaseSlug);
                Title = showcaseRepos.Name;
                Showcase = new Showcase {Slug = showcaseRepos.Slug, Description = showcaseRepos.Description, Name = showcaseRepos.Name};
                repositories.Reset(showcaseRepos.Repositories);
            });
            LoadCommand.TriggerNetworkActivity(networkActivity);
        }
    }
}