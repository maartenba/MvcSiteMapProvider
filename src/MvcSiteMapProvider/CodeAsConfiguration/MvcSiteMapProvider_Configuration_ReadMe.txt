Thank you for installing the MvcSiteMapProvider. This is a guide
to help you through the basics of configuring your application using 
dependency injection.

CONTROLLER FACTORY VS DEPENDENCY RESOLVER

Microsoft added the IDependencyResolver interface in MVC 3, however
although they recommend using it as the means to configure MVC, it is 
much simpler to use IControllerFactory. The reason is that you don't need 
to worry about what MVC does internally when using IControllerFactory.

However, due to the fact that instances of both of these interfaces can't 
be configured at the same time, we are providing both options in our 
CodeAsConfiguration setup. 

Also, note that it is quite common to use DependencyResolver.Current as a 
service locator to resolve dependencies within classes. We recommend you 
do not follow this practice. But if you are stuck using 3rd party libraries 
that have done this (or you insist this is the right way to do DI), you have 
the option of configuring your dependency injection container for use with 
IDependencyResolver.

CONFIGURING DEPENDENCY RESOLVER

To configure your application to use IDependencyResolver, add the 
following lines to DIConfigBootstrapper.cs:

    var dependencyResolver = new InjectableDependencyResolver(container);
    DependencyResolver.SetResolver(dependencyResolver);

Then comment out the following lines:

    //var controllerFactory = new InjectableControllerFactory(container);
    //ControllerBuilder.Current.SetControllerFactory(controllerFactory);
