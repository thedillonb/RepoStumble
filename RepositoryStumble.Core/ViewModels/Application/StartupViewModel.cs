using System;
using Xamarin.Utilities.Core.ViewModels;
using System.Threading.Tasks;
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
            GoToMainCommand = new ReactiveCommand();
            StartupCommand = new ReactiveCommand();
            GoToLoginCommand = new ReactiveCommand();

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

