using System;
using RepositoryStumble.Core.Services;
using ReactiveUI;
using RepositoryStumble.Core.Data;
using System.Reactive.Linq;
using RepositoryStumble.Core.ViewModels.Stumble;
using RepositoryStumble.Core.ViewModels.Application;

namespace RepositoryStumble.Core.ViewModels.Interests
{
    public class InterestsViewModel : BaseViewModel
    {
        protected readonly IApplicationService ApplicationService;

        public IReadOnlyReactiveList<Interest> Interests { get; private set; }

        public IReactiveCommand<object> DeleteInterestCommand { get; private set; }

        public IReactiveCommand<object> GoToAddInterestCommand { get; private set; }

        public IReactiveCommand<object> GoToStumbleInterestCommand { get; private set; }

        public InterestsViewModel(IApplicationService applicationService, IFeaturesService featuresService)
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
                if (ApplicationService.Account.Interests.Count() >= 3 && !featuresService.ProEditionEnabled)
                {
                    var vm = CreateViewModel<PurchaseProViewModel>();
                    ShowViewModel(vm);
                }
                else
                {
                    var vm = CreateViewModel<AddInterestViewModel>();
                    ShowViewModel(vm);
                }
            });

            this.WhenActivated(d =>
            {
                interests.Reset(ApplicationService.Account.Interests.Query.OrderBy(x => x.Keyword));
            });
        }
    }
}

