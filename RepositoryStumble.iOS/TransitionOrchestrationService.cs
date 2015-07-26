using System;
using UIKit;
using ReactiveUI;
using RepositoryStumble.ViewControllers.Application;
using RepositoryStumble.ViewControllers.Interests;
using RepositoryStumble.ViewControllers.Repositories;
using RepositoryStumble.ViewControllers.Stumble;
using RepositoryStumble.ViewControllers.Languages;
using RepositoryStumble.ViewControllers.Trending;
using RepositoryStumble.Transitions;
using RepositoryStumble.Core.Services;
using RepositoryStumble.Core.ViewModels;

namespace RepositoryStumble
{
    class TransitionOrchestrationService : ITransitionOrchestrationService
    {
        public void Transition(IViewFor fromView, IViewFor toView)
        {
            var fromViewController = (UIViewController) fromView;
            var fromViewModel = (IBaseViewModel) fromView.ViewModel;
            var toViewController = (UIViewController) toView;
            var toViewModel = (IBaseViewModel) toView.ViewModel;

            fromViewController.InvokeOnMainThread(
                () => DoTransition(fromViewController, fromViewModel, toViewController, toViewModel));

        }

        private static void DoTransition(UIViewController fromViewController, IBaseViewModel fromViewModel,
            UIViewController toViewController, IBaseViewModel toViewModel)
        {
            var toViewDismissCommand = toViewModel.DismissCommand;

            if (toViewController is LoginViewController)
            {
                toViewDismissCommand.Subscribe(_ => fromViewController.DismissViewController(true, null));
                fromViewController.PresentViewController(new UINavigationController(toViewController), true, null);
            }
            else if (toViewController is MainViewController)
            {
                var nav = ((UINavigationController)UIApplication.SharedApplication.Delegate.GetWindow().RootViewController);
                UIView.Transition(nav.View, 0.6f,
                    UIViewAnimationOptions.BeginFromCurrentState | UIViewAnimationOptions.TransitionCrossDissolve,
                    () => nav.PushViewController(toViewController, false), null);
            }
            else if (toViewController is AddInterestViewController)
            {
                toViewDismissCommand.Subscribe(_ => fromViewController.DismissViewController(true, null));
                toViewController.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (s, e) => toViewDismissCommand.ExecuteIfCan());
                fromViewController.PresentViewController(new UINavigationController(toViewController), true, null);
            }
            else if (toViewController is StumbleViewController || toViewController is RepositoryViewController ||
                     toViewController is StumbledRepositoryViewController || toViewController is SettingsViewController)
            {
                toViewDismissCommand.Subscribe(_ => fromViewController.DismissViewController(true, null));
                toViewController.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => toViewDismissCommand.ExecuteIfCan());
                fromViewController.PresentViewController(new UINavigationController(toViewController), true, null);
            }
            else if (toViewController is LanguagesViewController && fromViewController is TrendingViewController)
            {
                toViewDismissCommand.Subscribe(_ => fromViewController.DismissViewController(true, null));
                toViewController.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => toViewDismissCommand.ExecuteIfCan());
                var ctrlToPresent = new UINavigationController(toViewController);
                ctrlToPresent.TransitioningDelegate = new SlideDownTransition();
                fromViewController.PresentViewController(ctrlToPresent, true, null);
            }
            else if (toViewController is PurchaseProViewController)
            {
                toViewDismissCommand.Subscribe(_ => fromViewController.DismissViewController(true, null));
                toViewController.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => toViewDismissCommand.ExecuteIfCan());
                fromViewController.PresentViewController(toViewController, true, null);
            }
            else
            {
                toViewDismissCommand.Subscribe(
                    _ => toViewController.NavigationController.PopToViewController(fromViewController, true));
                fromViewController.NavigationController.PushViewController(toViewController, true);
            }
        }
    }
}