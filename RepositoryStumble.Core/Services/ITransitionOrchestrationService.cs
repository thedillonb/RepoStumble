using ReactiveUI;

namespace RepositoryStumble.Core.Services
{
    public interface ITransitionOrchestrationService
    {
        void Transition(IViewFor fromView, IViewFor toView);
    }
}

