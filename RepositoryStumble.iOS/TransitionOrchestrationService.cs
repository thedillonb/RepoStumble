using System;
using MonoTouch.UIKit;
using ReactiveUI;
using RepositoryStumble.ViewControllers.Application;
using Xamarin.Utilities.Core.Services;
using Xamarin.Utilities.Core.ViewModels;

namespace RepositoryStumble
{
    internal class TransitionOrchestrationService : ITransitionOrchestrationService
    {
        public void Transition(IViewFor fromView, IViewFor toView)
        {
            var fromViewController = (UIViewController) fromView;
            var fromViewModel = (IBaseViewModel) fromView.ViewModel;
            var toViewController = (UIViewController) toView;
            var toViewModel = (IBaseViewModel) toView.ViewModel;

            fromViewController.BeginInvokeOnMainThread(
                () => DoTransition(fromViewController, fromViewModel, toViewController, toViewModel));
        }

        private static void DoTransition(UIViewController fromViewController, IBaseViewModel fromViewModel,
            UIViewController toViewController, IBaseViewModel toViewModel)
        {
            var toViewDismissCommand = toViewModel.DismissCommand;

            if (toViewController is LoginViewController)
            {
                toViewDismissCommand.Subscribe(_ => fromViewController.DismissViewController(true, null));
                fromViewController.PresentViewController(toViewController, true, null);
            }
            else if (toViewController is MainViewController)
            {
                var nav = ((UINavigationController) UIApplication.SharedApplication.Delegate.Window.RootViewController);
                UIView.Transition(nav.View, 0.1,
                    UIViewAnimationOptions.BeginFromCurrentState | UIViewAnimationOptions.TransitionCrossDissolve,
                    () => nav.PushViewController(toViewController, false), null);
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