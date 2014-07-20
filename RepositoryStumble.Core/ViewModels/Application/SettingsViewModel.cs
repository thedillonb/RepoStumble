using System;
using ReactiveUI;
using RepositoryStumble.Core.Services;
using System.Reactive.Linq;
using Xamarin.Utilities.Core.ViewModels;
using RepositoryStumble.Core.Messages;
using Xamarin.Utilities.Core.Services;
using RepositoryStumble.Core.ViewModels.Repositories;

namespace RepositoryStumble.Core.ViewModels.Application
{
    public class SettingsViewModel : BaseViewModel
    {
        private bool _syncWithGitHub;
        public bool SyncWithGitHub
        {
            get { return _syncWithGitHub; }
            set { this.RaiseAndSetIfChanged(ref _syncWithGitHub, value); }
        }

        public string Version { get; private set; }

        public IReactiveCommand<object> LogoutCommand { get; private set; }

        public IReactiveCommand<object> GoToSourceCode { get; private set; }

        public SettingsViewModel(IApplicationService applicationService, IEnvironmentalService environmentalService)
        {
            SyncWithGitHub = applicationService.Account.SyncWithGitHub;

            LogoutCommand = ReactiveCommand.Create();
            LogoutCommand.Select(_ => new LogoutMessage()).Subscribe(x => applicationService.Logout());

            GoToSourceCode = ReactiveCommand.Create();
            GoToSourceCode.Subscribe(_ =>
            {
                var vm = CreateViewModel<RepositoryViewModel>();
                vm.RepositoryIdentifier = new BaseRepositoryViewModel.RepositoryIdentifierModel("thedillonb", "repostumble");
                ShowViewModel(vm);
            });

            Version = environmentalService.ApplicationVersion;

            this.WhenAnyValue(x => x.SyncWithGitHub).Skip(1).Subscribe(x =>
            {
                applicationService.Account.SyncWithGitHub = x;
                applicationService.Account.Save();
            });
        }
    }
}

