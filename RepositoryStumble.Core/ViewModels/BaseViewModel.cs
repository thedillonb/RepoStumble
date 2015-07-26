using System.Reactive.Linq;
using ReactiveUI;
using System;
using RepositoryStumble.Core.Services;
using Splat;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace RepositoryStumble.Core.ViewModels
{
    public interface IBaseViewModel
    {
        IReactiveCommand<object> DismissCommand { get; }

        IReactiveCommand<object> GoToUrlCommand { get; }

        IViewFor View { get; set; }
    }

    public abstract class BaseViewModel : ReactiveObject, IBaseViewModel, ISupportsActivation
    {
        private readonly ViewModelActivator _viewModelActivator;

        public IReactiveCommand<object> DismissCommand { get; private set; }

        public IReactiveCommand<object> GoToUrlCommand { get; private set; }

        public IViewFor View { get; set; }

        protected BaseViewModel()
        {
            _viewModelActivator = new ViewModelActivator();

            DismissCommand = ReactiveCommand.Create();

            GoToUrlCommand = ReactiveCommand.Create();
            GoToUrlCommand.OfType<string>().Subscribe(x =>
                {
                    var vm = CreateViewModel<WebBrowserViewModel>();
                    vm.Url = x;
                    ShowViewModel(vm);
                });
        }

        public TViewModel CreateViewModel<TViewModel>() where TViewModel : class
        {
            return (TViewModel)Construct(typeof(TViewModel));
        }

        public object Construct(Type type)
        {
            var constructor = type.GetConstructors().First();
            var parameters = constructor.GetParameters();
            var args = new List<object>(parameters.Length);
            foreach (var p in parameters)
            {
                var argument = Locator.Current.GetService(p.ParameterType);
                if (argument == null)
                    Debugger.Break();
                args.Add(argument);
            }
            return Activator.CreateInstance(type, args.ToArray(), null);
        }

        public void ShowViewModel<TViewModel>(TViewModel viewModel) where TViewModel : class, IBaseViewModel
        {
            var view = GetService<IViewModelViewService>().GetViewFor(viewModel);
            viewModel.View = view;
            view.ViewModel = viewModel;
            GetService<ITransitionOrchestrationService>().Transition(View, view);
        }

        public void CreateAndShowViewModel<TViewModel>() where TViewModel : class, IBaseViewModel
        {
            ShowViewModel(CreateViewModel<TViewModel>());
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>An instance of the service.</returns>
        protected TService GetService<TService>() where TService : class
        {
            return Locator.Current.GetService<TService>();
        }

        ViewModelActivator ISupportsActivation.Activator
        {
            get { return _viewModelActivator; }
        }
    }
}