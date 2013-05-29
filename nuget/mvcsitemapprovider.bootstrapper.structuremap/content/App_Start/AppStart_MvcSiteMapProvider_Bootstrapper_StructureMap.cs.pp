using MvcSiteMapProvider.Bootstrapper;
using MvcSiteMapProvider.Bootstrapper.StructureMap;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.AppStart_MvcSiteMapProvider_Bootstrapper_StructureMap), "Start")]

namespace $rootnamespace$
{
    public static class AppStart_MvcSiteMapProvider_Bootstrapper_StructureMap
    {
        public static void Start()
        {
            // MvcSiteMapProvider Configuration
#if NET35
            MvcSiteMapProvider.DI.Composer.Compose();
#endif
            var container = MvcSiteMapProvider.Bootstrapper.StructureMap.DIConfig.Register();
            MvcSiteMapProvider.Bootstrapper.ControllerFactoryConfig.Register(container);
            MvcSiteMapProvider.Bootstrapper.MvcSiteMapProviderConfig.Register(container);
        }
    }
}