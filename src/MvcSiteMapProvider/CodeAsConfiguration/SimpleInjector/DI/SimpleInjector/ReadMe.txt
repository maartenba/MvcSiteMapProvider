Integrating MvcSiteMapProvider with SimpleInjector

To add MvcSiteMapProvider to your DI configuration,
simply add the following code to your composition root.

     // Create the DI container
    var container = new Container();

    // Setup configuration of DI
    MvcSiteMapProviderContainerInitializer.SetUp(container);

For more help consult the SimpleInjector documantation at
https://simpleinjector.codeplex.com/documentation