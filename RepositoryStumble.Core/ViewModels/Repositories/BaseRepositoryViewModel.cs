using System;
using Xamarin.Utilities.Core.ViewModels;
using ReactiveUI;
using RepositoryStumble.Core.Services;
using GitHubSharp.Models;
using System.Diagnostics;
using RepositoryStumble.Core.Data;
using System.Reactive.Linq;
using Xamarin.Utilities.Core.Services;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public abstract class BaseRepositoryViewModel : BaseViewModel, ILoadableViewModel
    {
        public IReactiveCommand<object> LikeCommand { get; private set; }

        public IReactiveCommand<object> DislikeCommand { get; private set; }

        private RepositoryIdentifierModel _repositoryIdentifier;
        public RepositoryIdentifierModel RepositoryIdentifier
        {
            get { return _repositoryIdentifier; }
            set { this.RaiseAndSetIfChanged(ref _repositoryIdentifier, value); }
        }

        private string _readme;
        public string Readme
        {
            get { return _readme; }
            set { this.RaiseAndSetIfChanged(ref _readme, value); }
        }

        private RepositoryModel _repository;
        public RepositoryModel Repository
        {
            get { return _repository; }
            set { this.RaiseAndSetIfChanged(ref _repository, value); }
        }

        private StumbledRepository _stumbledRepository;
        public StumbledRepository StumbledRepository
        {
            get { return _stumbledRepository; }
            private set { this.RaiseAndSetIfChanged(ref _stumbledRepository, value); }
        }

        private int? _collaboratorCount;
        public int? CollaboratorCount
        {
            get { return _collaboratorCount; }
            set { this.RaiseAndSetIfChanged(ref _collaboratorCount, value); }
        }

        private bool? _liked;
        public bool? Liked
        {
            get { return _liked; }
            set { this.RaiseAndSetIfChanged(ref _liked, value); }
        }

        public IReactiveCommand<object> GoToGitHubCommand { get; private set; }

        public IReactiveCommand LoadCommand { get; private set; }

        protected BaseRepositoryViewModel(IApplicationService applicationService, INetworkActivityService networkActivity)
        {
            LikeCommand = ReactiveCommand.Create();
            LikeCommand.Subscribe(_ =>
            {
                if (StumbledRepository == null)
                {
                    var repo = CreateStumbledRepository();
                    repo.Liked = true;
                    applicationService.Account.StumbledRepositories.Insert(repo);
                    StumbledRepository = repo;
                }
                else
                {
                    StumbledRepository.Liked = true;
                    StumbledRepository.Description = Repository.Description;
                    StumbledRepository.Forks = Repository.Forks;
                    StumbledRepository.Stars = Repository.StargazersCount;
                    StumbledRepository.ImageUrl = Repository.Owner.AvatarUrl;
                    applicationService.Account.StumbledRepositories.Update(StumbledRepository);
                }

                applicationService.Client.ExecuteAsync(applicationService.Client.Users[Repository.Owner.Login].Repositories[Repository.Name].Star());

                Liked = true;
            });

            DislikeCommand = ReactiveCommand.Create();
            DislikeCommand.Subscribe(_ =>
            {
                if (StumbledRepository == null)
                {
                    var repo = CreateStumbledRepository();
                    repo.Liked = false;
                    applicationService.Account.StumbledRepositories.Insert(repo);
                    StumbledRepository = repo;
                }
                else
                {
                    StumbledRepository.Liked = false;
                    StumbledRepository.Description = Repository.Description;
                    StumbledRepository.Forks = Repository.Forks;
                    StumbledRepository.Stars = Repository.StargazersCount;
                    StumbledRepository.ImageUrl = Repository.Owner.AvatarUrl;
                    applicationService.Account.StumbledRepositories.Update(StumbledRepository);
                }

                applicationService.Client.ExecuteAsync(applicationService.Client.Users[Repository.Owner.Login].Repositories[Repository.Name].Unstar());

                Liked = false;
            });

            this.WhenAnyValue(x => x.RepositoryIdentifier)
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    StumbledRepository = applicationService.Account.StumbledRepositories.FindByFullname(x.Owner, x.Name);
                });

            this.WhenAnyValue(x => x.StumbledRepository)
                .Where(x => x != null)
                .Subscribe(x => Liked = x.Liked);

            GoToGitHubCommand = ReactiveCommand.Create(this.WhenAnyValue(x => x.Repository).Select(x => x != null));
            GoToGitHubCommand.Subscribe(_ => GoToUrlCommand.ExecuteIfCan(Repository.HtmlUrl));

            LoadCommand = ReactiveCommand.CreateAsyncTask(async t =>
            {
                // Cheap...
                if (RepositoryIdentifier == null)
                    return;

                var repo = applicationService.Client.Users[RepositoryIdentifier.Owner].Repositories[RepositoryIdentifier.Name];
                Repository = (await applicationService.Client.ExecuteAsync(repo.Get())).Data;
                CollaboratorCount = (await applicationService.Client.ExecuteAsync(repo.GetCollaborators())).Data.Count;

                try
                {
                    var readme = await applicationService.Client.ExecuteAsync(repo.GetReadme());
                    Readme = await applicationService.Client.Markdown.GetMarkdown(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(readme.Data.Content)));
                }
                catch (Exception e)
                {
                    Readme = "<center>There is no readme for this repository :(</center>";
                    Debug.WriteLine(e.Message + " for " + RepositoryIdentifier.Owner + "/" + RepositoryIdentifier.Name);
                }
            });

            LoadCommand.TriggerNetworkActivity(networkActivity);
        }

        private StumbledRepository CreateStumbledRepository()
        {
            return new StumbledRepository 
            {
                Description = Repository.Description,
                Forks = Repository.Forks,
                Fullname = Repository.FullName,
                ImageUrl = Repository.Owner.AvatarUrl,
                Name = Repository.Name,
                Owner = Repository.Owner.Login,
                Stars = Repository.StargazersCount
            };
        }

        public class RepositoryIdentifierModel
        {
            public string Owner { get; private set; }
            public string Name { get; private set; }

            public RepositoryIdentifierModel(string owner, string name)
            {
                Owner = owner;
                Name = name;
            }

            public override string ToString()
            {
                return string.Format("{0}/{1}", Owner, Name);
            }
        }
    }
}

