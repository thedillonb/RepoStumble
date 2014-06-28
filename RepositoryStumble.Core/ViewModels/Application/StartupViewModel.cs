using System;
using Xamarin.Utilities.Core.ViewModels;
using RepositoryStumble.Core.Services;
using ReactiveUI;

namespace RepositoryStumble.Core.ViewModels.Application
{
    public class StartupViewModel : BaseViewModel
    {
        public IReactiveCommand GoToMainCommand { get; private set; }

        public IReactiveCommand StartupCommand { get; private set; }

        public IReactiveCommand GoToLoginCommand { get; private set; }

        public StartupViewModel(IApplicationService applicationService)
        {

            GoToLoginCommand = new ReactiveCommand();
            GoToLoginCommand.Subscribe(x =>
            {
                var vm = CreateViewModel<LoginViewModel>();
                ShowViewModel(vm);
            });

            GoToMainCommand = new ReactiveCommand();
            GoToMainCommand.Subscribe(x =>
            {
                var vm = CreateViewModel<MainViewModel>();
                ShowViewModel(vm);
            });

            StartupCommand = new ReactiveCommand();
            StartupCommand.Subscribe(_ =>
            {
                if (applicationService.Load())
                    GoToMainCommand.ExecuteIfCan();
                else
                    GoToLoginCommand.ExecuteIfCan();
            });
        }
    }
}

