Thank you for installing the MvcSiteMapProvider. This is a guide
to help you through the basics of configuring your application using 
dependency injection.

IMPORTANT: KEEPING YOUR DI CONFIGURATION UP TO DATE
=========================================================================

Making MvcSiteMapProvider depend on DI is a bit of a double-edged sword. 
While this makes MvcSiteMapProvider extremely easy to extend, it is 
possible that new features added to MvcSiteMapProvider will cause 
your existing DI configuration to break when doing an upgrade.

Unfortunately, NuGet doesn't have a way to automatically merge changes 
into your DI modules - if you have changed your configuration in any
way, the module will be skipped when you upgrade. But then, the purpose
of giving you this code is so you can change it. For this reason, 
when you upgrade your MvcSiteMapProvider version, you should also compare 
your DI module to the corresponding module in the master branch to see if 
there are any changes that need to be made to your configuration. The best 
way to do this is to use some kind of diff tool (such as Beyond Compare)
to highlight the differences and assist with bringing the changes into 
your configuration without overwriting your customizations.

Note that you don't need to merge in #if, #else, and #endif blocks inside 
of the module, but only the code between them that applies to your specific 
.NET and/or MVC version.

Here is a list of the locations for each DI container where you can view 
the latest version of the MvcSiteMapProvider modules:

Autofac: https://github.com/maartenba/MvcSiteMapProvider/blob/master/src/MvcSiteMapProvider/CodeAsConfiguration/Autofac/DI/Autofac/Modules/MvcSiteMapProviderModule.cs
Grace: https://github.com/maartenba/MvcSiteMapProvider/blob/master/src/MvcSiteMapProvider/CodeAsConfiguration/Grace/DI/Grace/Modules/MvcSiteMapProviderModule.cs
Ninject: https://github.com/maartenba/MvcSiteMapProvider/blob/master/src/MvcSiteMapProvider/CodeAsConfiguration/Ninject/DI/Ninject/Modules/MvcSiteMapProviderModule.cs
SimpleInjector: https://github.com/maartenba/MvcSiteMapProvider/blob/master/src/MvcSiteMapProvider/CodeAsConfiguration/SimpleInjector/DI/SimpleInjector/MvcSiteMapProviderContainerInitializer.cs
StructureMap: https://github.com/maartenba/MvcSiteMapProvider/blob/master/src/MvcSiteMapProvider/CodeAsConfiguration/StructureMap/DI/StructureMap/Registries/MvcSiteMapProviderRegistry.cs
Unity: https://github.com/maartenba/MvcSiteMapProvider/blob/master/src/MvcSiteMapProvider/CodeAsConfiguration/Unity/DI/Unity/ContainerExtensions/MvcSiteMapProviderContainerExtension.cs
Windsor: https://github.com/maartenba/MvcSiteMapProvider/blob/master/src/MvcSiteMapProvider/CodeAsConfiguration/Windsor/DI/Windsor/Installers/MvcSiteMapProviderInstaller.cs


CONTROLLER FACTORY VS DEPENDENCY RESOLVER
=========================================================================

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
=========================================================================

If you need to use IDependencyResolver, add "DependencyResolver" 
(without the quotes) as a conditional compilation constant
to your MVC project. This can be done by going to the Build tab under 
project Properties, and adding the value to the end of the list. 
These symbols should be separated by a semicolon (;). 
If you are getting runtime errors, you should try this setting.

Alternatively, you can edit the App_Start/DIConfig.cs file 
to achieve the same result.


CONFIGURING OTHER OPTIONS
=========================================================================

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
