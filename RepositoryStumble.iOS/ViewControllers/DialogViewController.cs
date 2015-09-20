using System;
using System.Reactive.Subjects;
using ReactiveUI;
using Xamarin.Utilities.DialogElements;
using RepositoryStumble.Core.Services;
using Splat;
using UIKit;
using Foundation;
using CoreGraphics;

namespace RepositoryStumble.ViewControllers
{
    public class DialogViewController : ReactiveTableViewController
    {
        protected readonly INetworkActivityService NetworkActivityService = Locator.Current.GetService<INetworkActivityService>();
        private readonly RootElement _root;
        private readonly UITableView _tableView;
        private Source _tableSource;
        private readonly bool _unevenRows;
        private readonly Subject<CGPoint> _scrolledSubject = new Subject<CGPoint>();

        public IObservable<CGPoint> Scrolled { get { return _scrolledSubject; } }

        public RootElement Root
        {
            get { return _root; }
        }

        public DialogViewController(bool unevenRows = false, UITableViewStyle style = UITableViewStyle.Grouped)
        {
            _unevenRows = unevenRows;
            _tableView = new UITableView(UIScreen.MainScreen.Bounds, style);
            _root = new RootElement(_tableView);
        }

        public class Source : UITableViewSource
        {
            private readonly DialogViewController _dvc;

            public RootElement Root
            {
                get { return _dvc.Root; }
            }

            public Source(DialogViewController dvc)
            {
                _dvc = dvc;
                dvc.TableView.RowHeight = UITableView.AutomaticDimension;
            }

            public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
            {
                var section = Root[indexPath.Section];
                var element = (section[indexPath.Row] as StringElement);
                if (element != null)
                    element.AccessoryTap();
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return Root[(int)section].Count;
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                return Root.Count;
            }

            public override string TitleForHeader(UITableView tableView, nint section)
            {
                return Root[(int)section].Header;
            }

            public override string TitleForFooter(UITableView tableView, nint section)
            {
                return Root[(int)section].Footer;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var section = Root[indexPath.Section];
                var element = section[indexPath.Row];
                var cell = element.GetCell(tableView);
                cell.Hidden = element.Hidden;
                return cell;
            }

            public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
            {
                var section = Root[indexPath.Section];
                var element = section[indexPath.Row];
                element.Deselected(tableView, indexPath);
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var section = Root[indexPath.Section];
                var element = section[indexPath.Row];
                element.Selected(tableView, indexPath);
            }

            public override UIView GetViewForHeader(UITableView tableView, nint sectionIdx)
            {
                var section = Root[(int)sectionIdx];
                return section.HeaderView;
            }

            public override nfloat GetHeightForHeader(UITableView tableView, nint sectionIdx)
            {
                var section = Root[(int)sectionIdx];
                return section.HeaderView == null ? -1 : section.HeaderView.Frame.Height;
            }

            public override UIView GetViewForFooter(UITableView tableView, nint sectionIdx)
            {
                var section = Root[(int)sectionIdx];
                return section.FooterView;
            }

            public override nfloat GetHeightForFooter(UITableView tableView, nint sectionIdx)
            {
                var section = Root[(int)sectionIdx];
                return section.FooterView == null ? -1 : section.FooterView.Frame.Height;
            }

            public override void Scrolled(UIScrollView scrollView)
            {
                _dvc._scrolledSubject.OnNext(Root.TableView.ContentOffset);
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                var section = Root[indexPath.Section];
                var element = section[indexPath.Row];
                return element.Hidden ? 0f : tableView.RowHeight;
            }
        }

        public override void LoadView()
        {
            base.LoadView();

            _tableSource = CreateSizingSource(_unevenRows);

            _tableView.Bounds = View.Bounds;
            _tableView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            _tableView.AutosizesSubviews = true;
            _tableView.Source = _tableSource;
            TableView = _tableView;
        }

        public virtual Source CreateSizingSource(bool unevenRows)
        {
            return new Source(this);
        }

        public void ReloadData()
        {
            _tableView.ReloadData();
        }
    }

}