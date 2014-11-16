using System;
using ReactiveUI;
using Xamarin.Utilities.Core.ViewModels;
using RepositoryStumble.Core.Services;
using RepositoryStumble.Core.ViewModels.Application;
using RepositoryStumble.Core.ViewModels.Repositories;
using Xamarin.Utilities.Core.Services;
using RepositoryStumble.Core.Data;
using System.Reactive.Linq;
using System.Threading;
using Octokit;

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

        private User _userModel;
        public User User
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

        private bool _canPurchase;
        public bool CanPurchase
        {
            get { return _canPurchase; }
            private set { this.RaiseAndSetIfChanged(ref _canPurchase, value); }
        }

        private bool _hasMoreHistory;
        public bool HasMoreHistory
        {
            get { return _hasMoreHistory; }
            private set { this.RaiseAndSetIfChanged(ref _hasMoreHistory, value); }
        }

        private int _stumbedRepositories;
        public int StumbledRepositories
        {
            get { return _stumbedRepositories; }
            private set { this.RaiseAndSetIfChanged(ref _stumbedRepositories, value); }
        }

        public IReactiveCommand LoadCommand { get; private set; }

        public IReactiveCommand<object> GoToInterestsCommand { get; private set; }

        public IReactiveCommand<object> GoToLikesCommand { get; private set; }

        public IReactiveCommand<object> GoToDislikesCommand { get; private set; }

        public IReactiveCommand<object> GoToSettingsCommand { get; private set; }

        public IReactiveCommand<object> GoToRepositoryCommand { get; private set; }

        public IReactiveCommand<object> GoToPurchaseCommand { get; private set; }

        public IReactiveCommand<object> GoToHistoryCommand { get; private set; }

        public IReactiveList<StumbledRepository> StumbleHistory { get; private set; }

        public ProfileViewModel(IApplicationService applicationService, INetworkActivityService networkActivity, IFeaturesService featuresService)
        {
            StumbleHistory = new ReactiveList<StumbledRepository>();
            GoToInterestsCommand = ReactiveCommand.Create();
            Username = applicationService.Account.Username;

            Action updateStumbled = () =>
            {
                var stumbledRepositories = applicationService.Account.StumbledRepositories.Count();
                Interests = applicationService.Account.Interests.Count();
                Likes = applicationService.Account.StumbledRepositories.LikedRepositories();
                Dislikes = applicationService.Account.StumbledRepositories.DislikedRepositories();
                HasMoreHistory = stumbledRepositories > 30;
                if (stumbledRepositories != StumbledRepositories)
                {
                    StumbledRepositories = stumbledRepositories;
                    StumbleHistory.Reset(applicationService.Account.StumbledRepositories.Query.OrderByDescending(x => x.CreatedAt).Take(30));
                }
            };

            this.WhenActivated(d =>
            {
                if (applicationService.Account != null)
                {
                    updateStumbled();

                    d(applicationService.RepositoryAdded
                        .Buffer(TimeSpan.FromSeconds(5))
                        .Where(x => x.Count > 0)
                        .ObserveOn(SynchronizationContext.Current)
                        .Subscribe(x => updateStumbled()));
                }

                CanPurchase = !featuresService.ProEditionEnabled;
            });

            GoToRepositoryCommand = ReactiveCommand.Create();
            GoToRepositoryCommand.OfType<StumbledRepository>().Subscribe(x =>
            {
                var vm = CreateViewModel<RepositoryViewModel>();
                vm.RepositoryIdentifier = new BaseRepositoryViewModel.RepositoryIdentifierModel(x.Owner, x.Name);
                ShowViewModel(vm);
            });

            GoToPurchaseCommand = ReactiveCommand.Create();
            GoToPurchaseCommand.Subscribe(_ => CreateAndShowViewModel<PurchaseProViewModel>());

            GoToHistoryCommand = ReactiveCommand.Create();
            GoToHistoryCommand.Subscribe(_ => CreateAndShowViewModel<HistoryViewModel>());

            GoToLikesCommand = ReactiveCommand.Create();
            GoToLikesCommand.Subscribe(_ => CreateAndShowViewModel<LikedRepositoriesViewModel>());

            GoToDislikesCommand = ReactiveCommand.Create();
            GoToDislikesCommand.Subscribe(_ => CreateAndShowViewModel<DislikedRepositoriesViewModel>());

            GoToSettingsCommand = ReactiveCommand.Create();
            GoToSettingsCommand.Subscribe(_ => CreateAndShowViewModel<SettingsViewModel>());

   
            LoadCommand = ReactiveCommand.CreateAsyncTask(async t =>
            {
                User = await applicationService.Client.User.Current();
            });

            LoadCommand.TriggerNetworkActivity(networkActivity);
        }
    }
}

