using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using Splat;

namespace RepositoryStumble.Core.Services
{
    public interface IViewModelViewService
    {
        void Register(Type viewModelType, Type viewType);

        IViewFor GetViewFor<TViewModel>(TViewModel viewModel) where TViewModel : class;

        void RegisterViewModels(System.Reflection.Assembly asm);
    }

    public class ViewModelViewService : IViewModelViewService
    {
        private readonly Dictionary<Type, Type> _viewModelViewDictionary = new Dictionary<Type, Type>();
        private readonly IServiceConstructor _constructor;

        public ViewModelViewService(IServiceConstructor constructor)
        {
            _constructor = constructor;
        }

        public void RegisterViewModels(System.Reflection.Assembly asm)
        {
            foreach (var type in asm.DefinedTypes.Where(x => !x.IsAbstract && x.ImplementedInterfaces.Any(y => y == typeof(IViewFor))))
            {
                var viewForType = type.ImplementedInterfaces.FirstOrDefault(
                    x => x.IsConstructedGenericType && x.GetGenericTypeDefinition() == typeof(IViewFor<>));
                Register(viewForType.GenericTypeArguments[0], type.AsType());
            }
        }

        public void Register(Type viewModelType, Type viewType)
        {
            _viewModelViewDictionary.Add(viewModelType, viewType);
        }

        public IViewFor GetViewFor<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            var viewType = _viewModelViewDictionary[viewModel.GetType()];
            return viewType == null ? null : (IViewFor)_constructor.Construct(viewType);

        }
    }
}

