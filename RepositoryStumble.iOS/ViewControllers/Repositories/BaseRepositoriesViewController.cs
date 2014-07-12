using System;
using RepositoryStumble.Core.ViewModels.Repositories;
using RepositoryStumble.Elements;
using System.Linq;
using Xamarin.Utilities.ViewControllers;
using Xamarin.Utilities.DialogElements;
using System.Reactive.Linq;
using System.Collections.Specialized;
using ReactiveUI;

namespace RepositoryStumble.ViewControllers.Repositories
{
    public abstract class BaseRepositoriesViewController<TViewModel> : ViewModelCollectionViewController<TViewModel> where TViewModel : BaseRepositoriesViewModel
    {
        protected BaseRepositoriesViewController()
            : base(unevenRows: true)
        {
            SearchTextChanging.Where(x => ViewModel != null).Subscribe(x => ViewModel.SearchKeyword = x);

            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(x => x.Repositories.Changed.StartWith(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)))
                .Switch()
                .Subscribe(_ =>
                {
                    var sec = new Section();
                    sec.AddAll(
                        from x in ViewModel.Repositories
                        select new RepositoryElement(x.Owner, x.Name, x.Description, x.ImageUrl, () => ViewModel.GoToRepositoryCommand.ExecuteIfCan(x)));
                    Root.Reset(sec);
                });
        }
    }
}

