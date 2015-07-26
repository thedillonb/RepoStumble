
using CoreGraphics;
using System;
using UIKit;
using RepositoryStumble.Core.ViewModels.Application;
using System.Reactive.Linq;
using ReactiveUI;
using RepositoryStumble.Core.Services;

namespace RepositoryStumble.ViewControllers.Application
{
    public partial class PurchaseProViewController : ViewModelViewController<PurchaseProViewModel>
    {
        private readonly IStatusIndicatorService _statusIndicatorService;

        public PurchaseProViewController(IStatusIndicatorService statusIndicatorService)
            : base("PurchaseProViewController", null)
        {
            _statusIndicatorService = statusIndicatorService;
        }
            
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            MainImageView.Image = Images.PurchaseIcon;
            MainImageView.Layer.CornerRadius = 32f;
            MainImageView.Layer.MasksToBounds = true;

            PurchaseButton.SetBackgroundImage(Images.GreyButton.CreateResizableImage(new UIEdgeInsets(18, 18, 18, 18)), UIControlState.Normal);
            PurchaseButton.Layer.ShadowColor = UIColor.Black.CGColor;
            PurchaseButton.Layer.ShadowOffset = new CGSize(0, 1);
            PurchaseButton.Layer.ShadowOpacity = 0.3f;
            PurchaseButton.TouchUpInside += (sender, e) => ViewModel.PurchaseCommand.ExecuteIfCan();

            CancelButton.TouchUpInside += (sender, e) => ViewModel.DismissCommand.ExecuteIfCan();
            RestoreButton.TouchUpInside += (sender, e) => ViewModel.RestoreCommand.ExecuteIfCan();

            Observable.Merge(ViewModel.PurchaseCommand.IsExecuting, ViewModel.RestoreCommand.IsExecuting).Skip(1).Subscribe(x =>
            {
                if (x)
                    _statusIndicatorService.Show("Enabling...");
                else
                    _statusIndicatorService.Hide();
            });

            ViewModel.WhenAnyValue(x => x.Price)
                .Where(x => x != null)
                .Subscribe(x => PurchaseButton.SetTitle("Purchase! (" + x + ")", UIControlState.Normal));
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            UIApplication.SharedApplication.StatusBarHidden = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            UIApplication.SharedApplication.StatusBarHidden = false;
        }
    }
}

