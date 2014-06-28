using System;
using System.Threading.Tasks;
using MonoTouch.UIKit;
using RepositoryStumble.Core.Data;
using RepositoryStumble.Core.Services;

namespace RepositoryStumble.ViewControllers
{
	public abstract class RepositoryViewController : WebViewController
    {
        protected IApplicationService ApplicationService = IoC.Resolve<IApplicationService>();
		protected StumbledRepository CurrentRepo;
		protected readonly UIBarButtonItem _dislikeButton;
		protected readonly UIBarButtonItem _likeButton;

		protected static readonly UIColor SelectedColor = UIColor.FromRGB(0x4e, 0x4b, 0xbe);
		protected static readonly UIColor DeselectedColor = UIColor.FromRGB(50, 50, 50);

		protected RepositoryViewController()
		{
			NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Action, (s, e) => ShowMore());

			_dislikeButton = new UIBarButtonItem(Images.ThumbDown, UIBarButtonItemStyle.Plain, (s, e) => Dislike());
			_likeButton = new UIBarButtonItem(Images.ThumbUp, UIBarButtonItemStyle.Plain, (s, e) => Like());
		}

		protected void Clear()
		{
			Web.LoadHtmlString("<html><head></head><body></body></html>", null);
		}

		private string CreateUrl()
		{
			return string.Format("https://github.com/{0}/{1}", CurrentRepo.Owner, CurrentRepo.Name);
		}

		private void ShowMore()
		{
			if (CurrentRepo == null)
				return;
//
//			var a = MonoTouch.Utilities.GetSheet(null);
//			var show = a.AddButton("Show in GitHub");
//			var share = a.AddButton("Share");
//			a.CancelButtonIndex = a.AddButton("Cancel");
//
//			a.Clicked += (object sender, UIButtonEventArgs e) =>
//			{
//				if (e.ButtonIndex == show)
//				{
//					try { UIApplication.SharedApplication.OpenUrl(new MonoTouch.Foundation.NSUrl(CreateUrl())); } catch { }
//				}
//				else if (e.ButtonIndex == share)
//				{
//					var item = UIActivity.FromObject (CreateUrl());
//					var activityItems = new MonoTouch.Foundation.NSObject[] { item };
//					UIActivity[] applicationActivities = null;
//					var activityController = new UIActivityViewController (activityItems, applicationActivities);
//					PresentViewController (activityController, true, null);
//				}
//			};
//
//			a.ShowFrom(NavigationItem.RightBarButtonItem, true);
		}

		public async Task Load(StumbledRepository repo)
		{
			CurrentRepo = repo;
			Title = repo.Name;
			string data = null;

			try 
			{
				data = await GetRepositoryReadme(repo.Owner, repo.Name);
			}
			catch (Exception e)
			{
				Console.WriteLine("Unable to get Readme: " + e.Message);
			}

			var d = new Data
			{
				Owner = repo.Owner,
				Name = repo.Name,
				Description = repo.Description,
				Readme = data
			};

			var path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "repo.json");
//			var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(d);
//			System.IO.File.WriteAllText(path, serializedData, System.Text.Encoding.UTF8);
//			LoadFile(System.IO.Path.Combine(MonoTouch.Foundation.NSBundle.MainBundle.BundlePath, "readme.html"));
		}

		protected override bool ShouldStartLoad(MonoTouch.Foundation.NSUrlRequest request, UIWebViewNavigationType navigationType)
		{
			if (request.Url.AbsoluteString.StartsWith("http"))
			{
				var ctrl = new WebBrowserViewController();
				ctrl.Title = Title;
				ctrl.Load(request.Url);
				NavigationController.PushViewController(ctrl, true);
				return false;
			}

			return base.ShouldStartLoad(request, navigationType);
		}

		protected virtual void Like()
		{
//			CurrentRepo.Liked = true;
//			Application.Instance.Account.StumbledRepositories.Update(CurrentRepo);
//			BigTed.BTProgressHUD.ShowSuccessWithStatus("Liked!");
//			_likeButton.TintColor = SelectedColor;
//			_dislikeButton.TintColor = DeselectedColor;
//
//			if (Application.Instance.Account.SyncWithGitHub)
//			{
//				var req = Application.Instance.Client.Users[CurrentRepo.Owner].Repositories[CurrentRepo.Name].Star();
//				Application.Instance.Client.ExecuteAsync(req);
//			}
		}

		protected virtual void Dislike()
		{
//			CurrentRepo.Liked = false;
//			Application.Instance.Account.StumbledRepositories.Update(CurrentRepo);
//			BigTed.BTProgressHUD.ShowErrorWithStatus("Disliked!");
//			_dislikeButton.TintColor = SelectedColor;
//			_likeButton.TintColor = DeselectedColor;
//
//			if (Application.Instance.Account.SyncWithGitHub)
//			{
//				var req = Application.Instance.Client.Users[CurrentRepo.Owner].Repositories[CurrentRepo.Name].Unstar();
//				Application.Instance.Client.ExecuteAsync(req);
//			}
		}

		private async Task<string> GetRepositoryReadme(string username, string repository)
		{
            var req = ApplicationService.Client.Users[username].Repositories[repository].GetReadme();
            var readme = await ApplicationService.Client.ExecuteAsync(req);
			var data = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(readme.Data.Content));
			return data;
		}

		private class Data
		{
			public string Owner { get; set; }
			public string Name { get; set; }
			public string Description { get; set; }
			public string Readme { get; set; }
		}
    }
}

