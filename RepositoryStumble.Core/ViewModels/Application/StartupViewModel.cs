using System;
using RepositoryStumble.Core.Services;
using ReactiveUI;

namespace RepositoryStumble.Core.ViewModels.Application
{
    public class StartupViewModel : BaseViewModel
    {
        public IReactiveCommand<object> StartupCommand { get; private set; }

        public StartupViewModel(IApplicationService applicationService)
        {
            StartupCommand = ReactiveCommand.Create();
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

