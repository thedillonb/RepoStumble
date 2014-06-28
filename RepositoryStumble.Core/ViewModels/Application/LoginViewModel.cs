using System;
using ReactiveUI;
using RepositoryStumble.Core.Data;
using RepositoryStumble.Core.Services;

namespace RepositoryStumble.Core.ViewModels.Application
{
    public class LoginViewModel : ReactiveObject
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

        public LoginViewModel(IApplicationService applicationService)
        {
            LoginCommand = new ReactiveCommand();
            LoginCommand.RegisterAsyncTask(async t =>
            {
                var account = new Account();

                var token = await GitHubSharp.Client.RequestAccessToken(ClientId, ClientSecret, LoginCode, null);
                var client = GitHubSharp.Client.BasicOAuth(token.AccessToken);
                var info = await client.ExecuteAsync(client.AuthenticatedUser.GetInfo());
                account.AvatarUrl = info.Data.AvatarUrl;
                account.Username = info.Data.Login;
                account.Fullname = info.Data.Name;
                account.OAuth = token.AccessToken;

                account.Save();
                applicationService.Load();

//                try
//                {
//                    await Application.Instance.LoadLikesFromStars();
//                }
//                catch
//                {
//                }
            });
        }
    }
}

