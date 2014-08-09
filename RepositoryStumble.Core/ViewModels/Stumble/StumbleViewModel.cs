using RepositoryStumble.Core.ViewModels.Repositories;
using RepositoryStumble.Core.Services;
using Xamarin.Utilities.Core.Services;
using ReactiveUI;
using RepositoryStumble.Core.Data;
using System.Threading.Tasks;
using System.Linq;
using RepositoryStumble.Core.Utils;
using System;
using System.Reactive.Linq;
using System.Diagnostics;
using RepositoryStumble.Core.ViewModels.Application;
using System.Collections.Generic;
using System.Globalization;

namespace RepositoryStumble.Core.ViewModels.Stumble
{
    public class StumbleViewModel : BaseRepositoryViewModel
    {
        private const string StumbleKey = "stumble.times";
        private static readonly Random _random = new Random();
        private readonly IApplicationService _applicationService;

        public IReactiveCommand<StumbleResult> StumbleCommand { get; private set; }

        public IReactiveCommand<object> GoToPurchaseCommand { get; private set; }

        public Interest Interest { get; set; }

        public StumbleViewModel(IApplicationService applicationService, INetworkActivityService networkActivity, 
                                IFeaturesService featuresService, IDefaultValueService defaultValues)
            : base(applicationService, networkActivity)
        {
            this._applicationService = applicationService;

            var localStumbleCount = 0;

            GoToPurchaseCommand = ReactiveCommand.Create();
            GoToPurchaseCommand.Subscribe(_ => CreateAndShowViewModel<PurchaseProViewModel>());

            StumbleCommand = ReactiveCommand.CreateAsyncTask(LoadCommand.IsExecuting.Select(x => !x), x => StumbleRepository());
            StumbleCommand.Subscribe(x =>
            {
                if (!featuresService.ProEditionEnabled)
                {
                    var stumbleTimes = defaultValues.Get<int>(StumbleKey) + 1;
                    defaultValues.Set(StumbleKey, stumbleTimes);

                    if (localStumbleCount > 0 && stumbleTimes % 50 == 0)
                    {
                        GoToPurchaseCommand.ExecuteIfCan();
                    }
                }

                localStumbleCount++;

                Reset();
                RepositoryIdentifier = new RepositoryIdentifierModel(x.Repository.Owner, x.Repository.Name);
                LoadCommand.ExecuteIfCan();
            });
            StumbleCommand.TriggerNetworkActivity(networkActivity);

            DislikeCommand.Subscribe(_ => StumbleCommand.ExecuteIfCan());
            LikeCommand.Subscribe(_ => StumbleCommand.ExecuteIfCan());
        }

        private async Task GetMoreRepositoriesForInterest(Interest interest)
        {
            try
            {
                var search = new SearchRequest(interest.Keyword);
                search.In = new List<Octokit.InQualifier>() { Octokit.InQualifier.Description, Octokit.InQualifier.Name, Octokit.InQualifier.Readme };
                search.Language = interest.LanguageId;
                search.Sort = Octokit.RepoSearchSort.Stars;
                search.Page = Convert.ToInt32(interest.NextPage) + 1;
                var apiConnection = new Octokit.ApiConnection(_applicationService.Client.Connection);
                var response = await apiConnection.Get<Octokit.SearchRepositoryResult>(Octokit.ApiUrls.SearchRepositories(), search.Parameters);

                if (response.Items.Count == 0)
                    throw new InterestExhaustedException();

                interest.NextPage++;
                _applicationService.Account.Interests.Update(interest);

                foreach (var r in response.Items)
                {
                    _applicationService.Account.InterestedRepositories.Insert(new InterestedRepository 
                    { 
                        Name = r.Name, 
                        Owner = r.Owner.Login, 
                        Description = r.Description, 
                        InterestId = interest.Id,
                        Stars = r.WatchersCount,
                        Forks = r.ForksCount,
                        ImageUrl = r.Owner.AvatarUrl
                    });
                }
            }
            catch (Octokit.ApiException e)
            {
                Debug.WriteLine(e.Message);
                interest.Exhaused = true;
                _applicationService.Account.Interests.Update(interest);
            }
            catch (InterestExhaustedException)
            {
                interest.Exhaused = true;
                _applicationService.Account.Interests.Update(interest);
            }
        }


        private async Task<StumbleResult> StumbleRepository()
        {
            var interest = Interest;
            if (interest == null)
            {
                //Grab a random interest
                var interests = _applicationService.Account.Interests.Query.Where(x => x.Exhaused == false).ToList();
                if (interests.Count == 0)
                    throw new InterestExhaustedException();
                interest = interests[_random.Next(interests.Count)];
            }

            Debug.WriteLine("Looking at interest: " + interest);

            goAgain:

            //Grab the next repo off the queue
            var interestedRepo = _applicationService.Account.InterestedRepositories.Where(x => x.InterestId == interest.Id).Take(1).FirstOrDefault();
            if (interestedRepo == null)
            {
                await GetMoreRepositoriesForInterest(interest);
                goto goAgain;
            }

            if (_applicationService.Account.StumbledRepositories.Exists(interestedRepo.Owner, interestedRepo.Name))
            {
                _applicationService.Account.InterestedRepositories.Remove(interestedRepo);
                Debug.WriteLine("I've seen this before: " + interestedRepo.Owner + "/" + interestedRepo.Name);
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
                Fullname = string.Format("{0}/{1}", interest.Owner, interest.Name).ToLower(),
                ImageUrl = interest.ImageUrl
            };
        }

        public class StumbleResult
        {
            public StumbledRepository Repository { get; set; }
            public Interest Interest { get; set; }
        }

        public class SearchRequest : Octokit.SearchRepositoriesRequest
        {
            public SearchRequest(string term) : base(term)
            {
            }
             
            public string Language { get; set; }

            public new IDictionary<string, string> Parameters
            {
                get
                {
                    var parameters = MergeParameters();
                    if (Language != null)
                        parameters += (String.Format(CultureInfo.InvariantCulture, "+language:{0}", Language));

                    var d = new Dictionary<string, string>();
                    d.Add("page", Page.ToString());
                    d.Add("per_page", PerPage.ToString());
                    d.Add("sort", Sort.ToString());
                    d.Add("q", Term + " " + parameters); //add qualifiers onto the search term
                    return d;
                }
            }

        }
    }
}

