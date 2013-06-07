[assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.AppStart_MvcSiteMapProvider_Bootstrapper_Autofac), "Start")]

namespace $rootnamespace$
{
    public static class AppStart_MvcSiteMapProvider_Bootstrapper_Autofac
    {
        public static void Start()
        {
            // MvcSiteMapProvider Configuration
#if NET35
            MvcSiteMapProvider.DI.Composer.Compose();
#endif
            var container = $rootnamespace$.DI.MvcSiteMapProvider.DIConfig.Register();
            MvcSiteMapProvider.DI.Bootstrap.ControllerFactoryConfig.Register(container);
            MvcSiteMapProvider.DI.Bootstrap.MvcSiteMapProviderConfig.Register(container);
        }
    }
}