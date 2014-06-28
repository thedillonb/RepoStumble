using System;
using MonoTouch.Dialog;
using RepositoryStumble.Core.Data;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace RepositoryStumble.Elements
{
    public class StumbledRepositoryElement : StyledMultilineElement
    {
        public StumbledRepositoryElement(StumbledRepository repo, NSAction action)
            : base(repo.Owner + "/" + repo.Name, repo.Description, UITableViewCellStyle.Subtitle)
        {
            Accessory = UITableViewCellAccessory.DisclosureIndicator;
            Font = UIFont.BoldSystemFontOfSize(14f);
            SubtitleFont = UIFont.SystemFontOfSize(12f);
            Tapped += action;
        }
    }
}

