using RepositoryStumble.Core.ViewModels.Repositories;
using RepositoryStumble.Core.Services;
using Xamarin.Utilities.Core.Services;
using ReactiveUI;
using RepositoryStumble.Core.Data;
using System.Threading.Tasks;
using System.Linq;
using RepositoryStumble.Core.Utils;
using System;

namespace RepositoryStumble.Core.ViewModels.Stumble
{
    public class StumbleViewModel : BaseRepositoryViewModel
    {
        private static readonly Random _random = new Random();
        private readonly IApplicationService _applicationService;

        public IReactiveCommand<StumbleResult> StumbleCommand { get; private set; }

        public Interest Interest { get; set; }

        public StumbleViewModel(IApplicationService applicationService, INetworkActivityService networkActivity)
            : base(applicationService, networkActivity)
        {
            this._applicationService = applicationService;

            StumbleCommand = ReactiveCommand.CreateAsyncTask(LoadCommand.CanExecuteObservable, x => StumbleRepository());
            StumbleCommand.Subscribe(x =>
            {
                RepositoryIdentifier = new RepositoryIdentifierModel(x.Repository.Owner, x.Repository.Name);
                LoadCommand.ExecuteIfCan();
            });
            StumbleCommand.TriggerNetworkActivity(networkActivity);

            DislikeCommand.Subscribe(_ => StumbleCommand.ExecuteIfCan());
        }

        private async Task GetMoreRepositoriesForInterest(Interest interest)
        {
            var response = await Task.Run(() => {
                var req = _applicationService.Client.Repositories.SearchRepositories(
                    new [] { interest.Keyword, "in:name,description,readme" }, 
                    new [] { interest.LanguageId }, 
                    sort: "stars", page: Convert.ToInt32(interest.NextPage));
                return _applicationService.Client.ExecuteAsync(req);
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
                _applicationService.Account.Interests.Update(interest);

            }
            catch (Exception e)
            {
                interest.Exhaused = true;
            }

            _applicationService.Account.Interests.Update(interest);

            foreach (var r in response.Data.Items)
            {
                _applicationService.Account.InterestedRepositories.Insert(new InterestedRepository 
                { 
                    Name = r.Name, 
                    Owner = r.Owner.Login, 
                    Description = r.Description, 
                    InterestId = interest.Id,
                    Stars = r.StargazersCount,
                    Forks = r.ForksCount,
                    ImageUrl = r.Owner.AvatarUrl
                });
            }
        }


        private async Task<StumbleResult> StumbleRepository()
        {
            var interest = Interest;
            if (interest == null)
            {
                //Grab a random interest
                var interests = _applicationService.Account.Interests.Where(x => !x.Exhaused).ToList();
                if (interests.Count == 0)
                    throw new InterestExhaustedException();
                interest = interests[_random.Next(interests.Count)];
            }

            Console.WriteLine("Looking at interest: " + interest);

            goAgain:

            //Grab the next repo off the queue
            var interestedRepo = _applicationService.Account.InterestedRepositories.FirstOrDefault(x => x.InterestId == interest.Id);
            if (interestedRepo == null)
            {
                await GetMoreRepositoriesForInterest(interest);
                goto goAgain;
            }

            if (_applicationService.Account.StumbledRepositories.Exists(interestedRepo.Owner, interestedRepo.Name))
            {
                _applicationService.Account.InterestedRepositories.Remove(interestedRepo);
                Console.WriteLine("I've seen this before: " + interestedRepo.Owner + "/" + interestedRepo.Name);
                goto goAgain;
            }

            var stumbledRepo = CreateStumbledFromInterested(interestedRepo);
            _applicationService.Account.StumbledRepositories.Insert(stumbledRepo);
            _applicationService.Account.InterestedRepositories.Remove(interestedRepo);
            return new StumbleResult { Repository = stumbledRepo, Interest =  interest };
        }

        private static StumbledRepository CreateStumbledFromInterested(InterestedRepository interest)
        {
            return new StumbledRepository 
            { 
                Name = interest.Name, 
                Description = interest.Description, 
                Owner = interest.Owner, 
                Stars = interest.Stars, 
                Forks = interest.Forks, 
                Fullname = string.Format("{0}/{1}", interest.Owner, interest.Name),
                ImageUrl = interest.ImageUrl
            };
        }

        public class StumbleResult
        {
            public StumbledRepository Repository { get; set; }
            public Interest Interest { get; set; }
        }
    }
}

