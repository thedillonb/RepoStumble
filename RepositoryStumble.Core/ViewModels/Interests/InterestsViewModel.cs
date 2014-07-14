using System;
using RepositoryStumble.Core.Services;
using ReactiveUI;
using RepositoryStumble.Core.Data;
using System.Reactive.Linq;
using System.Linq;
using Xamarin.Utilities.Core.ViewModels;
using RepositoryStumble.Core.ViewModels.Stumble;

namespace RepositoryStumble.Core.ViewModels.Interests
{
    public class InterestsViewModel : BaseViewModel
    {
        protected readonly IApplicationService ApplicationService;

        public IReadOnlyReactiveList<Interest> Interests { get; private set; }

        public IReactiveCommand<object> DeleteInterestCommand { get; private set; }

        public IReactiveCommand<object> GoToAddInterestCommand { get; private set; }

        public IReactiveCommand<object> GoToStumbleInterestCommand { get; private set; }

        public InterestsViewModel(IApplicationService applicationService)
        {
            ApplicationService = applicationService;
            var interests = new ReactiveList<Interest>();
            Interests = interests;

            DeleteInterestCommand = ReactiveCommand.Create();
            DeleteInterestCommand.OfType<Interest>().Subscribe(ApplicationService.Account.Interests.Remove);

            GoToStumbleInterestCommand = ReactiveCommand.Create();
            GoToStumbleInterestCommand.OfType<Interest>().Subscribe(x =>
            {
                var vm = CreateViewModel<StumbleViewModel>();
                vm.Interest = x;
                ShowViewModel(vm);
            });

            GoToAddInterestCommand = ReactiveCommand.Create();
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

