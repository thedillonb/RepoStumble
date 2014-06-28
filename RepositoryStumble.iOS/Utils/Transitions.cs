using System;
using MonoTouch.UIKit;

namespace RepositoryStumble.Utils
{
    /// <summary>
    /// Just some silly transition code that I didn't want to write a hundred times over and over again.
    /// </summary>
    public static class Transitions
    {
        public static void TransitionToController(UIViewController controller, Action doneCallback = null)
        {
            Transition(controller, UIViewAnimationOptions.TransitionCrossDissolve, 1.0, doneCallback);
        }

        public static void Transition(UIViewController controller, UIViewAnimationOptions options, double duration = 0.6, Action doneCallback = null)
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            UIView.Transition(window, duration, options, () => {
                var oldState = UIView.AnimationsEnabled;
                UIView.AnimationsEnabled = false;
                window.RootViewController = controller;
                UIView.AnimationsEnabled = oldState;
            }, () => {
                if (doneCallback != null)
                    doneCallback();
            });
        }
    }
}

