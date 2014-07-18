using System;
using System.Linq;
using GitHubSharp.Models;
using ReactiveUI;
using Xamarin.Utilities.Core.ViewModels;
using RepositoryStumble.Core.Services;
using RepositoryStumble.Core.ViewModels.Application;
using RepositoryStumble.Core.ViewModels.Repositories;
using Xamarin.Utilities.Core.Services;
using RepositoryStumble.Core.Data;
using System.Reactive.Linq;

namespace RepositoryStumble.Core.ViewModels.Profile
{
    public class ProfileViewModel : BaseViewModel, ILoadableViewModel
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

        public IReactiveCommand LoadCommand { get; private set; }

        public IReactiveCommand<object> GoToInterestsCommand { get; private set; }

        public IReactiveCommand<object> GoToLikesCommand { get; private set; }

        public IReactiveCommand<object> GoToDislikesCommand { get; private set; }

        public IReactiveCommand<object> GoToSettingsCommand { get; private set; }

        public IReactiveCommand<object> GoToRepositoryCommand { get; private set; }

        public IReactiveList<StumbledRepository> StumbleHistory { get; private set; }

        public ProfileViewModel(IApplicationService applicationService, INetworkActivityService networkActivity)
        {
            StumbleHistory = new ReactiveList<StumbledRepository>();
            GoToInterestsCommand = ReactiveCommand.Create();
            Username = applicationService.Account.Username;

            this.WhenActivated(d =>
            {
                if (applicationService.Account != null)
                {
                    Interests = applicationService.Account.Interests.Count();
                    Likes = applicationService.Account.StumbledRepositories.Count(x => x.Liked.HasValue && x.Liked.Value);
                    Dislikes = applicationService.Account.StumbledRepositories.Count(x => x.Liked.HasValue && !x.Liked.Value);
                    StumbleHistory.Reset(applicationService.Account.StumbledRepositories.OrderByDescending(x => x.CreatedAt));
                    d(applicationService.RepositoryAdded.Subscribe(x => Likes += 1));
                }
            });

            GoToRepositoryCommand = ReactiveCommand.Create();
            GoToRepositoryCommand.OfType<StumbledRepository>().Subscribe(x =>
            {
                var vm = CreateViewModel<RepositoryViewModel>();
                vm.RepositoryIdentifier = new BaseRepositoryViewModel.RepositoryIdentifierModel(x.Owner, x.Name);
                ShowViewModel(vm);
            });

            GoToLikesCommand = ReactiveCommand.Create();
            GoToLikesCommand.Subscribe(_ => CreateAndShowViewModel<LikedRepositoriesViewModel>());

            GoToDislikesCommand = ReactiveCommand.Create();
            GoToDislikesCommand.Subscribe(_ => CreateAndShowViewModel<DislikedRepositoriesViewModel>());

            GoToSettingsCommand = ReactiveCommand.Create();
            GoToSettingsCommand.Subscribe(_ => CreateAndShowViewModel<SettingsViewModel>());

   
            LoadCommand = ReactiveCommand.CreateAsyncTask(async t =>
            {
                var ret = await applicationService.Client.ExecuteAsync(applicationService.Client.Users[Username].Get());
                User = ret.Data;
            });

            LoadCommand.TriggerNetworkActivity(networkActivity);
        }
    }
}

