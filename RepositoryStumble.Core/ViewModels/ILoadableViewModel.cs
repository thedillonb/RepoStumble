using ReactiveUI;

namespace RepositoryStumble.Core.ViewModels
{
    public interface ILoadableViewModel : IBaseViewModel
    {
        IReactiveCommand LoadCommand { get; }
    }
}

