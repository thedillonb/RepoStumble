using Splat;
using RepositoryStumble.Services;
using RepositoryStumble.Core.Services;
using RepositoryStumble.Core.Data;

namespace RepositoryStumble.Services
{
    public static class Registrations
    {
        public static void InitializeServices(this IMutableDependencyResolver resolverToUse)
        {
            resolverToUse.RegisterLazySingleton(() => new ApplicationService(), typeof(IApplicationService));
            resolverToUse.RegisterLazySingleton(() => DefaultValueService.Instance, typeof(IDefaultValueService));
            resolverToUse.RegisterLazySingleton(() => NetworkActivityService.Instance, typeof(INetworkActivityService));
            resolverToUse.RegisterLazySingleton(() => new EnvironmentalService(), typeof(IEnvironmentalService));
            resolverToUse.RegisterLazySingleton(() => new ViewModelViewService(resolverToUse.GetService<IServiceConstructor>()), typeof(IViewModelViewService));
            resolverToUse.RegisterLazySingleton(() => new AlertDialogService(), typeof(IAlertDialogService));
            resolverToUse.RegisterLazySingleton(() => new ServiceConstructor(), typeof(IServiceConstructor));
            resolverToUse.RegisterLazySingleton(() => new StatusIndicatorService(), typeof(IStatusIndicatorService));
            resolverToUse.RegisterLazySingleton(() => new TransitionOrchestrationService(), typeof(ITransitionOrchestrationService));
            resolverToUse.RegisterLazySingleton(() => new FeaturesService(resolverToUse.GetService<IDefaultValueService>()), typeof(IFeaturesService));
            resolverToUse.RegisterLazySingleton(() => new LanguageRepository(), typeof(LanguageRepository));
            resolverToUse.RegisterLazySingleton(() => new ShowcaseRepository(), typeof(ShowcaseRepository));
        }
    }
}

