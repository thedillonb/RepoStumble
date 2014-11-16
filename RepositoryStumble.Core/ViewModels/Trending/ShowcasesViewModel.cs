using System;
using System.Reactive.Linq;
using Xamarin.Utilities.Core.ViewModels;
using RepositoryStumble.Core.Data;
using Xamarin.Utilities.Core.Services;
using ReactiveUI;

namespace RepositoryStumble.Core.ViewModels.Trending
{
    public class ShowcasesViewModel : BaseViewModel, ILoadableViewModel
    {
        public IReadOnlyReactiveList<Showcase> Showcases { get; private set; }

        public IReactiveCommand<object> GoToShowcaseCommand { get; private set; }

        public IReactiveCommand LoadCommand { get; private set; }

        public ShowcasesViewModel(INetworkActivityService networkActivity, ShowcaseRepository showcaseRepository)
        {
            GoToShowcaseCommand = ReactiveCommand.Create();
            GoToShowcaseCommand.OfType<Showcase>().Subscribe(x =>
            {
                var vm = CreateViewModel<ShowcaseViewModel>();
                vm.ShowcaseSlug = x.Slug;
                vm.Title = x.Name;
                ShowViewModel(vm);
            });

            var showcases = new ReactiveList<Showcase>();
            Showcases = showcases;

            LoadCommand = ReactiveCommand.CreateAsyncTask(async t =>
                showcases.Reset(await showcaseRepository.GetShowcases()));
            LoadCommand.TriggerNetworkActivity(networkActivity);
        }
    }
}

