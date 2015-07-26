using System;
using System.Drawing;
using System.Reactive.Subjects;
using RepositoryStumble.Core.ViewModels;
using UIKit;
using ReactiveUI;
using Xamarin.Utilities.DialogElements;
using System.Reactive.Linq;
using System.Linq;

namespace RepositoryStumble.ViewControllers
{
    public abstract class ViewModelCollectionViewController<TViewModel> : ViewModelDialogViewController<TViewModel> where TViewModel : class, IBaseViewModel
    {
        private readonly UISearchBar _searchBar;
        private Subject<string> _searchTextChanging = new Subject<string>();
        private Subject<string> _searchTextChanged = new Subject<string>();

        /// <summary>
        /// An observable when the search text is changing (incremental)
        /// </summary>
        public IObservable<string> SearchTextChanging { get { return _searchTextChanging; } }

        /// <summary>
        /// An observable when the search text has finally changed (button press)
        /// </summary>
        /// <value>The search text changed.</value>
        public IObservable<string> SearchTextChanged { get { return _searchTextChanged; } }

        public bool AutoHideSearch { get; set; }

        public string SearchPlaceholder { get; set; }

        public UISearchBar SearchBar { get { return _searchBar; } }

        protected ViewModelCollectionViewController(bool unevenRows = false, bool searchbarEnabled = true)
            : base(unevenRows, UITableViewStyle.Plain)
        {
            if (searchbarEnabled)
            {
                _searchBar = new UISearchBar(new RectangleF(0f, 0f, 320f, 44f));
                _searchBar.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
                _searchBar.Delegate = new SearchDelegate(this);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (_searchBar != null)
            {
                if (!string.IsNullOrEmpty(SearchPlaceholder))
                    _searchBar.Placeholder = SearchPlaceholder;
                TableView.TableHeaderView = _searchBar;

                if (AutoHideSearch)
                {
                    if (TableView.ContentOffset.Y < 44)
                        TableView.ContentOffset = new PointF(0, 44);
                }
            }
        }

        class SearchDelegate : UISearchBarDelegate 
        {
            readonly ViewModelCollectionViewController<TViewModel> container;

            public SearchDelegate (ViewModelCollectionViewController<TViewModel> container)
            {
                this.container = container;
            }

            public override void OnEditingStarted (UISearchBar searchBar)
            {
                searchBar.ShowsCancelButton = true;
            }

            public override void OnEditingStopped (UISearchBar searchBar)
            {
                searchBar.ShowsCancelButton = false;
            }

            public override void TextChanged (UISearchBar searchBar, string searchText)
            {
                container._searchTextChanging.OnNext(searchText);
            }

            public override void CancelButtonClicked (UISearchBar searchBar)
            {
                searchBar.ShowsCancelButton = false;
                searchBar.ResignFirstResponder ();
                searchBar.Text = string.Empty;
                container._searchTextChanging.OnNext(searchBar.Text);
            }

            public override void SearchButtonClicked (UISearchBar searchBar)
            {
                searchBar.ResignFirstResponder();
                container._searchTextChanged.OnNext(searchBar.Text);
            }
        }
    }

    public static class ViewModelCollectionViewControllerExtensions
    {
        public static void BindList<T, TItem>(this ViewModelCollectionViewController<T> @this, IReadOnlyReactiveList<TItem> list, Func<TItem, Element> selector) where T : class, IBaseViewModel
        {
            list.Changed
                .Select(_ => list)
                .Subscribe(languages => {
                    var sec = new Section();
                    sec.AddAll(languages.Select(selector));
                    @this.Root.Reset(sec);
                });
        }
    }
}
