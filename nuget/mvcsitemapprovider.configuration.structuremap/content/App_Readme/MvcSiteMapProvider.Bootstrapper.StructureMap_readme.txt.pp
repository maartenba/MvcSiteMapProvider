To use StructureMap as your dependency injection container, please follow the instructions below.

Add the following lines of code to the Application_Start() method of Global.asax.
Note that if you are using .NET 4, we already created the AppStart_MvcSiteMapProvider_Bootstrapper_StructureMap.cs file which registers the IControllerFactory.

If you are using IControllerFactory:

            // MvcSiteMapProvider Configuration
#if NET35
            MvcSiteMapProvider.DI.Composer.Compose();
#endif
            var container = $rootnamespace$.DI.MvcSiteMapProvider.DIConfig.Register();
            MvcSiteMapProvider.Bootstrapper.ControllerFactoryConfig.Register(container);
            MvcSiteMapProvider.Bootstrapper.MvcSiteMapProviderConfig.Register(container);


If you are using IDependencyResolver (if you don't know the difference, use IControllerFactory):

            // MvcSiteMapProvider Configuration
#if NET35
            MvcSiteMapProvider.DI.Composer.Compose();
#endif
            var container = $rootnamespace$.DI.MvcSiteMapProvider.DIConfig.Register();
            MvcSiteMapProvider.DI.Bootstrap.DependencyResolverConfig.Register(container);
            MvcSiteMapProvider.DI.Bootstrap.MvcSiteMapProviderConfig.Register(container);