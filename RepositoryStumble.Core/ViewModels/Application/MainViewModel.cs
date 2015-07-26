using System;

using ReactiveUI;
using RepositoryStumble.Core.ViewModels.Stumble;
using RepositoryStumble.Core.Services;
using System.Linq;

namespace RepositoryStumble.Core.ViewModels.Application
{
    public class MainViewModel : BaseViewModel
    {
        public IReactiveCommand<object> GoToStumbleCommand { get; private set; }

        public IReactiveCommand<object> GoToInterestsCommand { get; private set; }

        public MainViewModel(IApplicationService applicationService)
        {
            GoToStumbleCommand = ReactiveCommand.Create();
            GoToStumbleCommand.Subscribe(_ =>
            {
                var vm = CreateViewModel<StumbleViewModel>();
                ShowViewModel(vm);
            });

            GoToInterestsCommand = ReactiveCommand.Create();
        }
    }
}