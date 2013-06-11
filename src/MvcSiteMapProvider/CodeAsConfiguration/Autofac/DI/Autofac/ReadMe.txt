Integrating MvcSiteMapProvider with Autofac

To add MvcSiteMapProvider to your DI configuration,
simply add the following code to your composition root.

	// Create a container builder
	var builder = new ContainerBuilder();
	builder.RegisterModule(new MvcSiteMapProviderModule());
	builder.RegisterModule(new MvcModule());

	// Create the DI container
	var container = builder.Build();

For more help consult the Autofac documantation at
http://code.google.com/p/autofac/wiki/StructuringWithModules