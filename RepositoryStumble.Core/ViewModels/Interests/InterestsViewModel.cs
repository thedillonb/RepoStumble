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

        public IReadOnlyReactiveList<Interest> Interests { get; private set; }

        public IReactiveCommand DeleteInterestCommand { get; private set; }

        public IReactiveCommand GoToAddInterestCommand { get; private set; }

        public InterestsViewModel(IApplicationService applicationService)
        {
            ApplicationService = applicationService;
            var interests = new ReactiveList<Interest>();
            Interests = interests;

            DeleteInterestCommand = new ReactiveCommand();
            DeleteInterestCommand.OfType<Interest>().Subscribe(ApplicationService.Account.Interests.Remove);

            GoToAddInterestCommand = new ReactiveCommand();
            GoToAddInterestCommand.Subscribe(_ =>
            {
                var vm = CreateViewModel<AddInterestViewModel>();
                ShowViewModel(vm);
            });

            this.WhenActivated(d =>
            {
                interests.Reset(ApplicationService.Account.Interests.OrderBy(x => x.Keyword));
            });
        }
    }
}

