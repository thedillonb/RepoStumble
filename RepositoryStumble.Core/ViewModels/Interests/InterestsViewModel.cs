using System;
using RepositoryStumble.Core.Services;
using ReactiveUI;
using RepositoryStumble.Core.Data;
using System.Reactive.Linq;
using System.Linq;
using Xamarin.Utilities.Core.ViewModels;

namespace RepositoryStumble.Core.ViewModels.Interests
{
    public class InterestsViewModel : BaseViewModel
    {
        protected readonly IApplicationService ApplicationService;

        public ReactiveList<Interest> Interests { get; private set; }

        public IReactiveCommand DeleteInterestCommand { get; private set; }

        public InterestsViewModel(IApplicationService applicationService)
        {
            ApplicationService = applicationService;
            Interests = new ReactiveList<Interest>(ApplicationService.Account.Interests.OrderBy(x => x.Keyword));
            DeleteInterestCommand = new ReactiveCommand();

            DeleteInterestCommand.OfType<Interest>().Subscribe(x =>
            {
                ApplicationService.Account.Interests.Remove(x);
            });
        }
    }
}

