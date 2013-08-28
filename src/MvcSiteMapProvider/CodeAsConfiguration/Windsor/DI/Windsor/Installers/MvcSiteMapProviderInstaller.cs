using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Hosting;
using System.Reflection;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.Compilation;
using MvcSiteMapProvider.Web.Mvc.Filters;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Visitor;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Globalization;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;

namespace DI.Windsor.Installers
{
    public class MvcSiteMapProviderInstaller
        : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            bool securityTrimmingEnabled = false;
            bool enableLocalization = true;
            string absoluteFileName = HostingEnvironment.MapPath("~/Mvc.sitemap");
            TimeSpan absoluteCacheExpiration = TimeSpan.FromMinutes(5);
            string[] includeAssembliesForScan = new string[] { "$AssemblyName$" };

            // Configure Windsor to resolve arrays in constructors
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel, true));

            var currentAssembly = this.GetType().Assembly;
            var siteMapProviderAssembly = typeof(SiteMaps).Assembly;
            var allAssemblies = new Assembly[] { currentAssembly, siteMapProviderAssembly };
            var excludeTypes = new Type[] { 
                typeof(SiteMapNodeVisibilityProviderStrategy),
                typeof(SiteMapXmlReservedAttributeNameProvider),
                typeof(SiteMapBuilderSetStrategy),
                typeof(ControllerTypeResolverFactory),
                typeof(SiteMapNodeUrlResolver)
            };
            var multipleImplementationTypes = new Type[]  { 
                typeof(ISiteMapNodeUrlResolver), 
                typeof(ISiteMapNodeVisibilityProvider), 
                typeof(IDynamicNodeProvider) 
            };

            // Single implementations of interface with matching name (minus the "I").
            CommonConventions.RegisterDefaultConventions(
                (interfaceType, implementationType) => container.Register(Component.For(interfaceType).ImplementedBy(implementationType).LifestyleSingleton()),
                new Assembly[] { siteMapProviderAssembly },
                allAssemblies,
                excludeTypes,
                string.Empty);

            // Multiple implementations of strategy based extension points
            CommonConventions.RegisterAllImplementationsOfInterface(
                (interfaceType, implementationType) => container.Register(Component.For(interfaceType).ImplementedBy(implementationType).LifestyleSingleton()),
                multipleImplementationTypes,
                allAssemblies,
                new Type[0],
                "^Composite");

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
            container.Register(Component.For<IControllerBuilder>().ImplementedBy<ControllerBuilderAdaptor>());
            container.Register(Component.For<IBuildManager>().ImplementedBy<BuildManagerAdaptor>());
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

            // Register the sitemap builder
            container.Register(Component.For<IXmlSource>().ImplementedBy<FileXmlSource>().Named("xmlSource1")
                .DependsOn(Dependency.OnValue("fileName", absoluteFileName)));
            container.Register(Component.For<ISiteMapXmlReservedAttributeNameProvider>().ImplementedBy<SiteMapXmlReservedAttributeNameProvider>()
                .DependsOn(Dependency.OnValue("attributesToIgnore", new string[0])));

            container.Register(Component.For<ISiteMapBuilder>().ImplementedBy<CompositeSiteMapBuilder>().Named("builder1")
                .DependsOn(Dependency.OnComponentCollection<ISiteMapBuilder[]>("xmlSiteMapBuilder1", "reflectionSiteMapBuilder1", "visitingSiteMapBuilder1")));

            container.Register(Component.For<ISiteMapBuilder>().ImplementedBy<XmlSiteMapBuilder>().Named("xmlSiteMapBuilder1")
                .DependsOn(Dependency.OnValue<ISiteMapXmlReservedAttributeNameProvider>(container.Resolve<ISiteMapXmlReservedAttributeNameProvider>()))
                .DependsOn(ServiceOverride.ForKey<IXmlSource>().Eq("xmlSource1"))
                );

            container.Register(Component.For<ISiteMapBuilder>().ImplementedBy<ReflectionSiteMapBuilder>().Named("reflectionSiteMapBuilder1")
                .DependsOn(Dependency.OnValue("includeAssemblies", includeAssembliesForScan))
                .DependsOn(Dependency.OnValue("excludeAssemblies", new string[0]))
                );

            container.Register(Component.For<ISiteMapBuilder>().ImplementedBy<VisitingSiteMapBuilder>().Named("visitingSiteMapBuilder1"));
                
            // Configure the builder sets
            container.Register(Component.For<ISiteMapBuilderSet>().ImplementedBy<SiteMapBuilderSet>().Named("siteMapBuilderSet1")
                .DependsOn(Dependency.OnValue("instanceName", "default"))
                .DependsOn(Dependency.OnValue("securityTrimmingEnabled", securityTrimmingEnabled))
                .DependsOn(Dependency.OnValue("enableLocalization", enableLocalization))
                .DependsOn(ServiceOverride.ForKey<ISiteMapBuilder>().Eq("builder1"))
                .DependsOn(ServiceOverride.ForKey<ICacheDetails>().Eq("cacheDetails1"))
                );

            container.Register(Component.For<ISiteMapBuilderSetStrategy>().ImplementedBy<SiteMapBuilderSetStrategy>()
                .DependsOn(Dependency.OnComponentCollection<ISiteMapBuilderSet[]>("siteMapBuilderSet1")));
        }

        #endregion

    }
}
