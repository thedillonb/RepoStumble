using System;
using RepositoryStumble.Core.Data;
using UIKit;
using Xamarin.Utilities.DialogElements;

namespace RepositoryStumble.Elements
{
    public class InterestElement : StringElement
    {
        public Interest Interest { get; private set; }
        public InterestElement(Interest interest, Action action)
            : base(interest.Keyword, interest.Language, UITableViewCellStyle.Subtitle)
        {
            Accessory = UITableViewCellAccessory.DisclosureIndicator;
            Tapped += action;
            Interest = interest;
        }
    }
}

