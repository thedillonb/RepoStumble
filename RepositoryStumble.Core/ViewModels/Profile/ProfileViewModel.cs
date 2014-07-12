using System;
using System.Linq;
using GitHubSharp.Models;
using ReactiveUI;
using Xamarin.Utilities.Core.ViewModels;
using RepositoryStumble.Core.Services;
using RepositoryStumble.Core.ViewModels.Application;
using RepositoryStumble.Core.ViewModels.Repositories;
using System.Reactive.Linq;

namespace RepositoryStumble.Core.ViewModels.Profile
{
    public class ProfileViewModel : LoadableViewModel
    {
        private string _username;
        public string Username
        {
            get { return _username; }
            private set { this.RaiseAndSetIfChanged(ref _username, value); }
        }

        private UserModel _userModel;
        public UserModel User
        {
            get { return _userModel; }
            private set { this.RaiseAndSetIfChanged(ref _userModel, value); }
        }

        private int _interests;
        public int Interests
        {
            get { return _interests; }
            private set { this.RaiseAndSetIfChanged(ref _interests, value); }
        }

        private int _likes;
        public int Likes
        {
            get { return _likes; }
            private set { this.RaiseAndSetIfChanged(ref _likes, value); }
        }

        private int _dislikes;
        public int Dislikes
        {
            get { return _dislikes; }
            private set { this.RaiseAndSetIfChanged(ref _dislikes, value); }
        }

        public IReactiveCommand GoToInterestsCommand { get; private set; }

        public IReactiveCommand GoToLikesCommand { get; private set; }

        public IReactiveCommand GoToDislikesCommand { get; private set; }

        public IReactiveCommand GoToSettingsCommand { get; private set; }

        public ProfileViewModel(IApplicationService applicationService)
        {
            GoToInterestsCommand = new ReactiveCommand();

            Username = applicationService.Account.Username;

            this.WhenActivated(d =>
            {
                if (applicationService.Account != null)
                {
                    Interests = applicationService.Account.Interests.Count();
                    Likes = applicationService.Account.StumbledRepositories.Count(x => x.Liked.HasValue && x.Liked.Value);
                    Dislikes = applicationService.Account.StumbledRepositories.Count(x => x.Liked.HasValue && !x.Liked.Value);
                    d(applicationService.RepositoryAdded.Subscribe(x => Likes += 1));
                }
            });

            GoToLikesCommand = new ReactiveCommand();
            GoToLikesCommand.Subscribe(_ => CreateAndShowViewModel<LikedRepositoriesViewModel>());

            GoToDislikesCommand = new ReactiveCommand();
            GoToDislikesCommand.Subscribe(_ => CreateAndShowViewModel<DislikedRepositoriesViewModel>());

            GoToSettingsCommand = new ReactiveCommand();
            GoToSettingsCommand.Subscribe(_ => CreateAndShowViewModel<SettingsViewModel>());
   
            LoadCommand.RegisterAsyncTask(async t =>
            {
                var ret = await applicationService.Client.ExecuteAsync(applicationService.Client.Users[Username].Get());
                User = ret.Data;
            });
        }
    }
}

