using System;
using RepositoryStumble.Core.Data;
using System.Threading.Tasks;
using System.Linq;
using ReactiveUI;
using RepositoryStumble.Core.Messages;
using RepositoryStumble.Core.Utils;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Subjects;

namespace RepositoryStumble.Core.Services
{
    public class ApplicationService : IApplicationService
    {
        private static Random _random = new Random();
        private readonly Subject<StumbledRepository> _stumbledRepositories = new Subject<StumbledRepository>();

        public GitHubSharp.Client Client { get; private set; }

        public Account Account { get; private set; }

        public ApplicationService()
        {

        }

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
            Account.Unload();
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
                                Stars = Convert.ToUInt32(x.StargazersCount),
                                Forks = Convert.ToUInt32(x.Forks),
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

        private async Task GetMoreRepositoriesForInterest(Interest interest)
        {
            var response = await Task.Run(() => {
                var req = Client.Repositories.SearchRepositories(new string[] { interest.Keyword, "in:name,description,readme" }, new string[] { interest.LanguageId }, sort: "stars", page: Convert.ToInt32(interest.NextPage));
                return Client.ExecuteAsync(req);
            });

            try
            {
                if (response.More == null)
                    throw new InterestExhaustedException();

                var regex = new System.Text.RegularExpressions.Regex(@"page=(\d+)");
                var match = regex.Match(response.More.Url);
                if (!match.Success)
                    throw new InterestExhaustedException();

                var page = uint.Parse(match.Groups[1].Value);
                interest.NextPage = page;
                Account.Interests.Update(interest);

            }
            catch (Exception e)
            {
                interest.Exhaused = true;
            }

            Account.Interests.Update(interest);

            foreach (var r in response.Data.Items)
            {
                Account.InterestedRepositories.Insert(new InterestedRepository 
                { 
                    Name = r.Name, 
                    Owner = r.Owner.Login, 
                    Description = r.Description, 
                    InterestId = interest.Id,
                    Stars = Convert.ToUInt32(r.StargazersCount),
                    Forks = Convert.ToUInt32(r.ForksCount)
                });
            }
        }


        public async Task<StumbleResult> StumbleRepository(Interest interest = null)
        {
            if (interest == null)
            {
                //Grab a random interest
                var interests = Account.Interests.Where(x => !x.Exhaused).ToList();
                if (interests.Count == 0)
                    throw new InterestExhaustedException();

                interest = interests[_random.Next(interests.Count)];
            }

            Console.WriteLine("Looking at interest: " + interest);

            goAgain:

            //Grab the next repo off the queue
            var interestedRepo = Account.InterestedRepositories.Where(x => x.InterestId == interest.Id).FirstOrDefault();
            if (interestedRepo == null)
            {
                await GetMoreRepositoriesForInterest(interest);
                goto goAgain;
            }

            if (Account.StumbledRepositories.Exists(interestedRepo.Owner, interestedRepo.Name))
            {
                Account.InterestedRepositories.Remove(interestedRepo);
                Console.WriteLine("I've seen this before: " + interestedRepo.Owner + "/" + interestedRepo.Name);
                goto goAgain;
            }

            var stumbledRepo = interestedRepo.CreateStumbledRepository();
            Account.StumbledRepositories.Insert(stumbledRepo);
            Account.InterestedRepositories.Remove(interestedRepo);
            return new StumbleResult { Repository = stumbledRepo, Interest =  interest };
        }

        public class StumbleResult
        {
            public StumbledRepository Repository { get; set; }
            public Interest Interest { get; set; }
        }
    }
}

