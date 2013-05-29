[assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.AppStart_MvcSiteMapProvider_Bootstrapper_Unity), "Start")]

namespace $rootnamespace$
{
    public static class AppStart_MvcSiteMapProvider_Bootstrapper_Unity
    {
        public static void Start()
        {
            // MvcSiteMapProvider Configuration
#if NET35
            MvcSiteMapProvider.DI.Composer.Compose();
#endif
            var container = MvcSiteMapProvider.Bootstrapper.Unity.DIConfig.Register();
            MvcSiteMapProvider.Bootstrapper.ControllerFactoryConfig.Register(container);
            MvcSiteMapProvider.Bootstrapper.MvcSiteMapProviderConfig.Register(container);
        }
    }
}