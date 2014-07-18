using System;
using RepositoryStumble.Core.Data;
using System.Threading.Tasks;
using System.Linq;
using ReactiveUI;
using RepositoryStumble.Core.Messages;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace RepositoryStumble.Core.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly Subject<StumbledRepository> _stumbledRepositories = new Subject<StumbledRepository>();

        public GitHubSharp.Client Client { get; private set; }

        public Account Account { get; private set; }

        public bool Load()
        {
            Account = Account.Load();
            if (Account == null)
                return false;

            Client = GitHubSharp.Client.BasicOAuth(Account.OAuth);
            LoadLikesFromStars();
            return true;
        }

        public void Logout()
        {
            Account.Dispose();
            Account = null;
            Client = null;
            MessageBus.Current.SendMessage(new LogoutMessage());
        }

        private async Task LoadLikesFromStars()
        {
            try
            {
                var d = new Dictionary<string, StumbledRepository>(Account.StumbledRepositories.Count());
                foreach (var r in Account.StumbledRepositories)
                    d.Add(r.Fullname.ToLower(), r);

                var req = Client.AuthenticatedUser.Repositories.GetStarred();
                while (req != null)
                {
                    var repos = await Client.ExecuteAsync(req);
                    foreach (var x in repos.Data)
                    {
                        StumbledRepository repository;
                        if (!d.TryGetValue(x.FullName.ToLower(), out repository))
                        {
                            var newRepo = new StumbledRepository
                            { 
                                Name = x.Name, 
                                Owner = x.Owner.Login,
                                Fullname = x.FullName, 
                                Description = x.Description,
                                Stars = x.StargazersCount,
                                Forks = x.Forks,
                                ImageUrl = x.Owner.AvatarUrl,
                                Liked = true,
                                ShowInHistory = false
                            };

                            Account.StumbledRepositories.Insert(newRepo);
                            _stumbledRepositories.OnNext(newRepo);
                        }
                        else
                        {
                            if (!string.Equals(repository.ImageUrl, x.Owner.AvatarUrl, StringComparison.InvariantCultureIgnoreCase))
                            {
                                repository.ImageUrl = x.Owner.AvatarUrl;
                                Account.StumbledRepositories.Update(repository);
                            }
                        }
                    }

                    req = repos.More;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to get likes from stars: " + e.Message);
            }
        }

        public IObservable<StumbledRepository> RepositoryAdded { get { return _stumbledRepositories; } }
    }
}

