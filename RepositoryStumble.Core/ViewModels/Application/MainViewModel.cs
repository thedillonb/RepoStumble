using System;

using Xamarin.Utilities.Core.ViewModels;
using ReactiveUI;
using RepositoryStumble.Core.ViewModels.Stumble;

namespace RepositoryStumble.Core.ViewModels.Application
{
    public class MainViewModel : BaseViewModel
    {
        public IReactiveCommand GoToStumbleCommand { get; private set; }

        public MainViewModel()
        {
            GoToStumbleCommand = new ReactiveCommand();
            GoToStumbleCommand.Subscribe(_ =>
            {
                var vm = CreateViewModel<StumbleViewModel>();
                ShowViewModel(vm);
            });
        }
    }
}