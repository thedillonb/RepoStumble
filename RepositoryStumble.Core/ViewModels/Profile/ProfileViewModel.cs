using Xamarin.Utilities.Core.ViewModels;
using ReactiveUI;
using GitHubSharp.Models;
using RepositoryStumble.Core.Services;
using System.Linq;

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

        public ProfileViewModel(IApplicationService applicationService)
        {
            GoToInterestsCommand = new ReactiveCommand();
            GoToLikesCommand = new ReactiveCommand();
            GoToDislikesCommand = new ReactiveCommand();

            Username = applicationService.Account.Username;

            this.WhenActivated(d =>
            {
                Interests = applicationService.Account.Interests.Count();
                Likes = applicationService.Account.StumbledRepositories.Count(x => x.Liked.HasValue && x.Liked.Value);
                Dislikes = applicationService.Account.StumbledRepositories.Count(x => x.Liked.HasValue && !x.Liked.Value);
            });
   
            LoadCommand.RegisterAsyncTask(async t =>
            {
                var ret = await applicationService.Client.ExecuteAsync(applicationService.Client.Users[Username].Get());
                User = ret.Data;
            });
        }
    }
}

