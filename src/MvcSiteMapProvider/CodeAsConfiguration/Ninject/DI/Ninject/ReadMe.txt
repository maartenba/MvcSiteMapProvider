Integrating MvcSiteMapProvider with Ninject

To add MvcSiteMapProvider to your DI configuration,
simply add the following code to your composition root.

    // Create the DI container
    var container = new StandardKernel();

    // Setup configuration of DI
    container.Load(new MvcModule());
    container.Load(new MvcSiteMapProviderModule());

For more help consult the Ninject documantation at
https://github.com/ninject/ninject/wiki/Modules-and-the-Kernel