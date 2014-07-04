using System;
using Xamarin.Utilities.Core.ViewModels;
using RepositoryStumble.Core.Services;
using ReactiveUI;

namespace RepositoryStumble.Core.ViewModels.Application
{
    public class StartupViewModel : BaseViewModel
    {
        public IReactiveCommand StartupCommand { get; private set; }

        public StartupViewModel(IApplicationService applicationService)
        {
            StartupCommand = new ReactiveCommand();
            StartupCommand.Subscribe(_ =>
            {
                if (applicationService.Load())
                {
                    var vm = CreateViewModel<MainViewModel>();
                    ShowViewModel(vm);  
                }
                else
                {
                    var vm = CreateViewModel<LoginViewModel>();
                    ShowViewModel(vm);
                }
            });
        }
    }
}

