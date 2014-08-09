using System;
using ReactiveUI;
using RepositoryStumble.Core.Data;
using Xamarin.Utilities.Core.ViewModels;
using Xamarin.Utilities.Core.Services;
using System.Reactive.Linq;

namespace RepositoryStumble.Core.ViewModels.Application
{
    public class LoginViewModel : BaseViewModel
    {
        private const string ClientId = "28cfa26e6ceee13c226e";
        private const string ClientSecret = "87d1e0747ba4a7965b27e4cc5cc7553f59390cea";

        private string _loginCode;
        public string LoginCode
        {
            get { return _loginCode; }
            set { this.RaiseAndSetIfChanged(ref _loginCode, value); }
        }

        public IReactiveCommand LoginCommand { get; private set; }

        public string LoginUrl
        {
            get
            {
                return string.Format("https://github.com/login/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}", 
                    ClientId, Uri.EscapeUriString("http://dillonbuchanan.com/"), Uri.EscapeUriString("user,public_repo"));
            }
        }

        public LoginViewModel(INetworkActivityService networkActivity)
        {
            LoginCommand = ReactiveCommand.CreateAsyncTask(async t =>
            {
                var account = new Account();

                var connection = new Octokit.Connection(new Octokit.ProductHeaderValue("RepoStumble"));
                var client = new Octokit.OauthClient(connection);
                var token = await client.CreateAccessToken(new Octokit.OauthTokenRequest(ClientId, ClientSecret, LoginCode));

                connection.Credentials = new Octokit.Credentials(token.AccessToken);
                var githubClient = new Octokit.GitHubClient(connection);
                var info = await githubClient.User.Current();
                account.AvatarUrl = info.AvatarUrl;
                account.Username = info.Login;
                account.Fullname = info.Name;
                account.OAuth = token.AccessToken;
                account.Save();

                DismissCommand.ExecuteIfCan();
            });

            LoginCommand.IsExecuting.Skip(1).Where(x => x).Subscribe(x => networkActivity.PushNetworkActive());
            LoginCommand.IsExecuting.Skip(1).Where(x => !x).Subscribe(x => networkActivity.PopNetworkActive());
        }
    }
}

