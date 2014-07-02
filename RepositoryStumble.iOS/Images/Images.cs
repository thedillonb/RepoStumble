using System;
using MonoTouch.UIKit;

namespace RepositoryStumble
{
    public static class Images
    {
		public static UIImage ThumbUp { get { return UIImage.FromBundle("Images/thumb_up"); } }
		public static UIImage ThumbDown { get { return UIImage.FromBundle("Images/thumb_down"); } }

		public static UIImage Back { get { return UIImage.FromBundle("Images/back"); } }
		public static UIImage Forward { get { return UIImage.FromBundle("Images/forward"); } }
		public static UIImage Reload { get { return UIImage.FromBundle("Images/reload"); } }
		public static UIImage Gear { get { return UIImage.FromBundle("Images/gear"); } }

		public static UIImage CenterSearch { get { return UIImage.FromBundle("Images/center-search"); } }

        public static UIImage User { get { return UIImage.FromBundle("Images/user"); } }
        public static UIImage Trending { get { return UIImage.FromBundle("Images/trending"); } }
        public static UIImage Spotlight { get { return UIImage.FromBundle("Images/spotlight"); } }
        public static UIImage Search { get { return UIImage.FromBundle("Images/search"); } }
        public static UIImage Heart { get { return UIImage.FromBundle("Images/heart"); } }
    }
}

