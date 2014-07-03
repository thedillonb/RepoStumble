using RepositoryStumble.Core.Data;
using MonoTouch.UIKit;
using Xamarin.Utilities.DialogElements;
using System;

namespace RepositoryStumble.Elements
{
    public class StumbledRepositoryElement : StyledMultilineElement
    {
        public StumbledRepositoryElement(StumbledRepository repo, Action action)
            : base(repo.Owner + "/" + repo.Name, repo.Description, UITableViewCellStyle.Subtitle)
        {
            Accessory = UITableViewCellAccessory.DisclosureIndicator;
            Font = UIFont.BoldSystemFontOfSize(14f);
            SubtitleFont = UIFont.SystemFontOfSize(12f);
            Tapped += action;
        }
    }
}

