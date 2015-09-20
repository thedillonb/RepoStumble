using UIKit;

namespace RepositoryStumble
{
    public static class Images
    {
		public static UIImage ThumbUp { get { return UIImage.FromBundle("Images/thumb_up"); } }
		public static UIImage ThumbDown { get { return UIImage.FromBundle("Images/thumb_down"); } }
        public static UIImage ThumbUpFilled { get { return UIImage.FromBundle("Images/thumb_up_filled"); } }
        public static UIImage ThumbDownFilled { get { return UIImage.FromBundle("Images/thumb_down_filled"); } }

		public static UIImage Back { get { return UIImage.FromBundle("Images/back"); } }
		public static UIImage Forward { get { return UIImage.FromBundle("Images/forward"); } }
		public static UIImage Reload { get { return UIImage.FromBundle("Images/reload"); } }
		public static UIImage Gear { get { return UIImage.FromBundle("Images/gear"); } }

        public static UIImage CenterSearch { get { return UIImage.FromBundle("Images/center-search"); } }
		public static UIImage CenterSearchDisabled { get { return UIImage.FromBundle("Images/center-search_disabled"); } }

        public static UIImage User { get { return UIImage.FromBundle("Images/user"); } }
        public static UIImage UnknownUser { get { return UIImage.FromBundle("Images/unknown_user"); } }
        public static UIImage UserFilled { get { return UIImage.FromBundle("Images/user_filled"); } }

        public static UIImage Trending { get { return UIImage.FromBundle("Images/trending"); } }
        public static UIImage TrendingFilled { get { return UIImage.FromBundle("Images/trending_filled"); } }

        public static UIImage Spotlight { get { return UIImage.FromBundle("Images/spotlight"); } }

        public static UIImage Search { get { return UIImage.FromBundle("Images/search"); } }

        public static UIImage Heart { get { return UIImage.FromBundle("Images/heart"); } }
        public static UIImage HeartFilled { get { return UIImage.FromBundle("Images/heart_filled"); } }

        public static UIImage DownChevron { get { return UIImage.FromBundle("Images/down_chevron"); } }

        public static UIImage GreyButton { get { return UIImageHelper.FromFileAuto("Images/grey_button"); } }

        public static UIImage PurchaseIcon { get { return UIImageHelper.FromFileAuto("Images/purchase_icon"); } }

        public static UIImage BackChevron { get { return UIImageHelper.FromFileAuto("Images/back-chevron"); } }

        public static UIImage ForwardChevron { get { return UIImageHelper.FromFileAuto("Images/forward-chevron"); } }
    }

    public static class UIImageHelper
    {
        /// <summary>
        /// Load's an image via the FromFile.
        /// Also checks to make sure it's on the main thread.
        /// </summary>
        /// <returns>The file auto.</returns>
        /// <param name="filename">Filename.</param>
        /// <param name="extension">Extension.</param>
        public static UIImage FromFileAuto(string filename, string extension = "png")
        {
            UIImage img = null;
            if (Foundation.NSThread.Current.IsMainThread)
                img = LoadImageFromFile(filename, extension);
            else
            {
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    img = LoadImageFromFile(filename, extension);
                });
            }

            return img;
        }

        private static UIImage LoadImageFromFile(string filename, string extension = "png")
        {
            if (UIScreen.MainScreen.Scale > 1.0)
            {
                var file = filename + "@2x." + extension;
                return System.IO.File.Exists(file) ? UIImage.FromFile(file) : UIImage.FromFile(filename + "." + extension);
            }
            else
            {
                var file = filename + "." + extension;
                return System.IO.File.Exists(file) ? UIImage.FromFile(file) : UIImage.FromFile(filename + "@2x." + extension);
            }
        }
    }
}
