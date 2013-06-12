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
code as configuration setup. 

Also, note that it is quite common to use DependencyResolver.Current as a 
service locator to resolve dependencies within classes. We recommend you 
do not follow this practice. But if you are stuck using 3rd party libraries 
that have done this (or you insist this is the right way to do DI), you have 
the option of configuring your dependency injection container for use with 
IDependencyResolver.

IMPORTANT: MVC doesn't allow the use of Controller Factory and Dependency
Resolver simultaneously. You must either configure one or the other. If you 
have code that depends on IDependencyResolver, you must configure all of your
DI configuration with IDependencyResolver including MvcSiteMapProvider.

CONFIGURING DEPENDENCY RESOLVER

If you need to use IDependencyResolver, add "DependencyResolver" 
(without the quotes) as a conditional compilation constant
to your MVC project. This can be done by going to the Build tab under 
project Properties, and adding the value to the end of the list. 
These symbols should be separated by a semicolon (;). 
If you are getting runtime errors, you should try this setting.

Alternatively, you can edit the App_Start/DIConfigBootstrapper.cs file 
to achieve the same result.

CONFIGURING OTHER OPTIONS

This package automatically sets a value in the appSettings section of 
web.config as follows:

<add key="MvcSiteMapProvider_UseExternalDIContainer" value="true" />

When this setting is set to true, ALL other appSettings keys prefixed 
by "MvcSiteMapProvider" are ignored. The reason for this is that we 
assume that all configuration will be done by the DI container rather 
than in web.config. If you still want to put settings in web.config 
you are free to do so, but you will need to add the functionality 
to your DI modules to do so.

This gives you complete control over the configuration of MvcSiteMapProvider
in the way that you see fit (with the exception of 1 appSettings value) and
you are free to add configuration settings that make the most sense for your
application instead of having a lot of clutter that doesn't apply to you.