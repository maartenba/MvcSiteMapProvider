using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Hosting;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Visitor;
using MvcSiteMapProvider.Web.Compilation;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Xml;

namespace DI.Windsor.Installers
{
    public class MvcSiteMapProviderInstaller
        : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            bool enableLocalization = true;
            string absoluteFileName = HostingEnvironment.MapPath("~/Mvc.sitemap");
            TimeSpan absoluteCacheExpiration = TimeSpan.FromMinutes(5);
            bool visibilityAffectsDescendants = true;
            bool useTitleIfDescriptionNotProvided = true;
#if Demo
            // Settings for MvcMusicStore demo: don't copy into your project
            bool securityTrimmingEnabled = true;
            string[] includeAssembliesForScan = new string[] { "Mvc Music Store" };
#else
            bool securityTrimmingEnabled = false;
            string[] includeAssembliesForScan = new string[] { "$AssemblyName$" };
#endif

            // Configure Windsor to resolve arrays in constructors
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel, true));

            var currentAssembly = this.GetType().Assembly;
            var siteMapProviderAssembly = typeof(SiteMaps).Assembly;
            var allAssemblies = new Assembly[] { currentAssembly, siteMapProviderAssembly };
            var excludeTypes = new Type[] { 
                // Use this array to add types you wish to explicitly exclude from convention-based  
                // auto-registration. By default all types that either match I[TypeName] = [TypeName] or 
                // I[TypeName] = [TypeName]Adapter will be automatically wired up as long as they don't 
                // have the [ExcludeFromAutoRegistrationAttribute].
                //
                // If you want to override a type that follows the convention, you should add the name 
                // of either the implementation name or the interface that it inherits to this list and 
                // add your manual registration code below. This will prevent duplicate registrations 
                // of the types from occurring. 

                // Example:
                // typeof(SiteMap),
                // typeof(SiteMapNodeVisibilityProviderStrategy)
                typeof(SiteMapNodeUrlResolver)
            };
            var multipleImplementationTypes = new Type[]  { 
                typeof(ISiteMapNodeUrlResolver), 
                typeof(ISiteMapNodeVisibilityProvider), 
                typeof(IDynamicNodeProvider) 
            };

            // Matching type name (I[TypeName] = [TypeName]) or matching type name + suffix Adapter (I[TypeName] = [TypeName]Adapter)
            // and not decorated with the [ExcludeFromAutoRegistrationAttribute].
            CommonConventions.RegisterDefaultConventions(
                (interfaceType, implementationType) => container.Register(Component.For(interfaceType).ImplementedBy(implementationType).LifestyleSingleton()),
                new Assembly[] { siteMapProviderAssembly },
                allAssemblies,
                excludeTypes,
                string.Empty);

            // Multiple implementations of strategy based extension points (and not decorated with [ExcludeFromAutoRegistrationAttribute]).
            CommonConventions.RegisterAllImplementationsOfInterface(
                (interfaceType, implementationType) => container.Register(Component.For(interfaceType).ImplementedBy(implementationType).LifestyleSingleton()),
                multipleImplementationTypes,
                allAssemblies,
                new Type[0],
                string.Empty);

            // Registration of internal controllers
            CommonConventions.RegisterAllImplementationsOfInterface(
                (interfaceType, implementationType) => container.Register(Component.For(implementationType).ImplementedBy(implementationType).LifestyleTransient()),
                new Type[] { typeof(IController) },
                new Assembly[] { siteMapProviderAssembly },
                new Type[0],
                string.Empty);

            // Visibility Providers
            container.Register(Component.For<ISiteMapNodeVisibilityProviderStrategy>().ImplementedBy<SiteMapNodeVisibilityProviderStrategy>()
                .DependsOn(Dependency.OnValue("defaultProviderName", string.Empty)));

            // Pass in the global controllerBuilder reference
            container.Register(Component.For<ControllerBuilder>().Instance(ControllerBuilder.Current));
            container.Register(Component.For<IControllerTypeResolverFactory>().ImplementedBy<ControllerTypeResolverFactory>()
                .DependsOn(Dependency.OnValue("areaNamespacesToIgnore", new string[0])));

            // Configure Security

            // First registration wins - we must do the outer one first
            container.Register(Component.For<IAclModule>().ImplementedBy<CompositeAclModule>()
                .DependsOn(Dependency.OnComponentCollection<IEnumerable<IAclModule>>(typeof(AuthorizeAttributeAclModule), typeof(XmlRolesAclModule))));

            container.Register(Component.For<IAclModule>().ImplementedBy<AuthorizeAttributeAclModule>());
            container.Register(Component.For<IAclModule>().ImplementedBy<XmlRolesAclModule>());


            // Setup cache
#if NET35
            container.Register(Component.For(typeof(ICacheProvider<>)).ImplementedBy(typeof(AspNetCacheProvider<>)));
            container.Register(Component.For<ICacheDependency>().ImplementedBy<AspNetFileCacheDependency>().Named("cacheDependency1")
                .DependsOn(Dependency.OnValue("fileName", absoluteFileName)));
#else
            container.Register(Component.For<System.Runtime.Caching.ObjectCache>().Instance(System.Runtime.Caching.MemoryCache.Default));
            container.Register(Component.For(typeof(ICacheProvider<>)).ImplementedBy(typeof(RuntimeCacheProvider<>)));
            container.Register(Component.For<ICacheDependency>().ImplementedBy<RuntimeFileCacheDependency>().Named("cacheDependency1")
                .DependsOn(Dependency.OnValue("fileName", absoluteFileName)));
#endif
            container.Register(Component.For<ICacheDetails>().ImplementedBy<CacheDetails>().Named("cacheDetails1")
                .DependsOn(Dependency.OnValue("absoluteCacheExpiration", absoluteCacheExpiration))
                .DependsOn(Dependency.OnValue("slidingCacheExpiration", TimeSpan.MinValue))
                .DependsOn(ServiceOverride.ForKey<ICacheDependency>().Eq("cacheDependency1"))
                );

            // Configure the visitors
            container.Register(Component.For<ISiteMapNodeVisitor>().ImplementedBy<UrlResolvingSiteMapNodeVisitor>());

            // Prepare for the sitemap node providers
            container.Register(Component.For<IXmlSource>().ImplementedBy<FileXmlSource>().Named("xmlSource1")
                .DependsOn(Dependency.OnValue("fileName", absoluteFileName)));
            container.Register(Component.For<IReservedAttributeNameProvider>().ImplementedBy<ReservedAttributeNameProvider>()
                .DependsOn(Dependency.OnValue("attributesToIgnore", new string[0])));

            // Register the sitemap node providers
            container.Register(Component.For<ISiteMapNodeProvider>().ImplementedBy<CompositeSiteMapNodeProvider>().Named("siteMapNodeProvider1")
                .DependsOn(Dependency.OnComponentCollection<ISiteMapNodeProvider[]>("xmlSiteMapNodeProvider1", "reflectionSiteMapNodeProvider1")));

            container.Register(Component.For<ISiteMapNodeProvider>().ImplementedBy<XmlSiteMapNodeProvider>().Named("xmlSiteMapNodeProvider1")
                .DependsOn(Dependency.OnValue("includeRootNode", true))
                .DependsOn(Dependency.OnValue("useNestedDynamicNodeRecursion", false))
                .DependsOn(ServiceOverride.ForKey<IXmlSource>().Eq("xmlSource1"))
                );

            container.Register(Component.For<ISiteMapNodeProvider>().ImplementedBy<ReflectionSiteMapNodeProvider>().Named("reflectionSiteMapNodeProvider1")
                .DependsOn(Dependency.OnValue("includeAssemblies", includeAssembliesForScan))
                .DependsOn(Dependency.OnValue("excludeAssemblies", new string[0]))
                );

            // Register the sitemap builders
            container.Register(Component.For<ISiteMapBuilder>().ImplementedBy<SiteMapBuilder>().Named("builder1")
                .DependsOn(ServiceOverride.ForKey<ISiteMapNodeProvider>().Eq("siteMapNodeProvider1"))
                );

            // Configure the builder sets
            container.Register(Component.For<ISiteMapBuilderSet>().ImplementedBy<SiteMapBuilderSet>().Named("siteMapBuilderSet1")
                .DependsOn(Dependency.OnValue("instanceName", "default"))
                .DependsOn(Dependency.OnValue("securityTrimmingEnabled", securityTrimmingEnabled))
                .DependsOn(Dependency.OnValue("enableLocalization", enableLocalization))
                .DependsOn(Dependency.OnValue("visibilityAffectsDescendants", visibilityAffectsDescendants))
                .DependsOn(Dependency.OnValue("useTitleIfDescriptionNotProvided", useTitleIfDescriptionNotProvided))
                .DependsOn(ServiceOverride.ForKey<ISiteMapBuilder>().Eq("builder1"))
                .DependsOn(ServiceOverride.ForKey<ICacheDetails>().Eq("cacheDetails1"))
                );

            container.Register(Component.For<ISiteMapBuilderSetStrategy>().ImplementedBy<SiteMapBuilderSetStrategy>()
                .DependsOn(Dependency.OnComponentCollection<ISiteMapBuilderSet[]>("siteMapBuilderSet1")));
        }

        #endregion
    }
}
