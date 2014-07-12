using System;
using Xamarin.Utilities.Core.ViewModels;
using ReactiveUI;
using RepositoryStumble.Core.Services;
using GitHubSharp.Models;
using System.Diagnostics;
using RepositoryStumble.Core.Data;
using System.Reactive.Linq;

namespace RepositoryStumble.Core.ViewModels.Repositories
{
    public abstract class BaseRepositoryViewModel : LoadableViewModel
    {
        public IReactiveCommand LikeCommand { get; private set; }

        public IReactiveCommand DislikeCommand { get; private set; }

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
        public StumbledRepository StumbedRepository
        {
            get { return _stumbledRepository; }
            private set { this.RaiseAndSetIfChanged(ref _stumbledRepository, value); }
        }

        private int _collaboratorCount;
        public int CollaboratorCount
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

        protected BaseRepositoryViewModel(IApplicationService applicationService)
        {
            LikeCommand = new ReactiveCommand();
            DislikeCommand = new ReactiveCommand();

            this.WhenAnyValue(x => x.RepositoryIdentifier)
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    StumbedRepository = applicationService.Account.StumbledRepositories.FindByFullname(x.Owner, x.Name);
                });

            this.WhenAnyValue(x => x.StumbedRepository)
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    Liked = x.Liked;
                });

            LoadCommand.RegisterAsyncTask(async t =>
            {
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
        }
    }
}

