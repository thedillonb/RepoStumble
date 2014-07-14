using System;

using Xamarin.Utilities.Core.ViewModels;
using ReactiveUI;
using RepositoryStumble.Core.ViewModels.Stumble;

namespace RepositoryStumble.Core.ViewModels.Application
{
    public class MainViewModel : BaseViewModel
    {
        public IReactiveCommand<object> GoToStumbleCommand { get; private set; }

        public MainViewModel()
        {
            GoToStumbleCommand = ReactiveCommand.Create();
            GoToStumbleCommand.Subscribe(_ =>
            {
                var vm = CreateViewModel<StumbleViewModel>();
                ShowViewModel(vm);
            });
        }
    }
}