using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Hosting;
using System.Reflection;
using Grace.DependencyInjection;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Visitor;
using MvcSiteMapProvider.Web.Compilation;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Xml;

namespace DI.Grace.Modules
{
    public class MvcSiteMapProviderModule 
        : IConfigurationModule
    {
        public void Configure(IExportRegistrationBlock container)
        {
            bool enableLocalization = true;
            string absoluteFileName = HostingEnvironment.MapPath("~/Mvc.sitemap");
            TimeSpan absoluteCacheExpiration = TimeSpan.FromMinutes(5);
            TimeSpan slidingCacheExpiration = TimeSpan.MinValue;
            bool includeRootNode = true;
            bool useNestedDynamicNodeRecursion = false;
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
                 (interfaceType, implementationType) => container.Export(implementationType).As(interfaceType).AndSingleton(),
                 new Assembly[] { siteMapProviderAssembly },
                 allAssemblies,
                 excludeTypes,
                 string.Empty);

            // Multiple implementations of strategy based extension points (and not decorated with [ExcludeFromAutoRegistrationAttribute]).
            CommonConventions.RegisterAllImplementationsOfInterface(
                 (interfaceType, implementationType) => container.Export(implementationType).As(interfaceType).AndSingleton(),
                 multipleImplementationTypes,
                 allAssemblies,
                 new Type[0],
                 string.Empty);

            // Registration of internal controllers
            CommonConventions.RegisterAllImplementationsOfInterface(
                 (interfaceType, implementationType) => container.Export(implementationType).As(interfaceType).ExternallyOwned(),
                 new Type[] { typeof(IController) },
                 new Assembly[] { siteMapProviderAssembly },
                 new Type[0],
                 string.Empty);

            // Visibility Providers
            container.Export<SiteMapNodeVisibilityProviderStrategy>()
                .As<ISiteMapNodeVisibilityProviderStrategy>()
                .WithCtorParam(() => string.Empty).Named("defaultProviderName");

            // Pass in the global controllerBuilder reference
            container.ExportInstance((scope, context) => ControllerBuilder.Current);

            container.Export<ControllerTypeResolverFactory>().As<IControllerTypeResolverFactory>()
                .WithCtorParam(() => new string[0]).Named("areaNamespacesToIgnore");

            // Configure Security
            string attributeModuleKey = typeof(AuthorizeAttributeAclModule).Name;
            container.Export<AuthorizeAttributeAclModule>()
                .As<IAclModule>()
                .WithKey(attributeModuleKey);

            string xmlModuleKey = typeof(XmlRolesAclModule).Name;
            container.Export<XmlRolesAclModule>()
                .As<IAclModule>()
                .WithKey(xmlModuleKey);
            
            container.Export<CompositeAclModule>()
                .As<IAclModule>()
                .WithCtorParam<IAclModule[]>().Named("aclModules").LocateWithKey(new[] { attributeModuleKey, xmlModuleKey });

            // Configure cache
            container.ExportInstance<System.Runtime.Caching.ObjectCache>(
                (scope, context) => System.Runtime.Caching.MemoryCache.Default);

            container.Export(typeof(RuntimeCacheProvider<>)).As(typeof(ICacheProvider<>));

            container.Export<RuntimeFileCacheDependency>()
                .As<ICacheDependency>()
                .WithKey("cacheDependency1")
                .WithCtorParam(() => absoluteFileName).Named("fileName");

            container.Export<CacheDetails>()
                .As<ICacheDetails>()
                .WithKey("cacheDetails1")
                .WithCtorParam<ICacheDependency>().LocateWithKey("cacheDependency1")
                .WithNamedCtorValue(() => absoluteCacheExpiration)
                .WithNamedCtorValue(() => slidingCacheExpiration);

            // Configure the visitors
            container.Export<UrlResolvingSiteMapNodeVisitor>().As<ISiteMapNodeVisitor>();

            // Prepare for our node providers
            container.Export<FileXmlSource>()
                .As<IXmlSource>()
                .WithKey("xmlSource1")
                .WithCtorParam(() => absoluteFileName);

            container.Export<ReservedAttributeNameProvider>().As<IReservedAttributeNameProvider>()
                .WithCtorParam(() => new string[0]).Named("attributesToIgnore");

            // Register the sitemap node providers
            container.Export<XmlSiteMapNodeProvider>()
                .As<ISiteMapNodeProvider>()
                .WithKey("xmlSiteMapNodeProvider1")
                .WithCtorParam<IXmlSource>().LocateWithKey("xmlSource1")
                .WithNamedCtorValue(() => includeRootNode)
                .WithNamedCtorValue(() => useNestedDynamicNodeRecursion);

            container.Export<ReflectionSiteMapNodeProvider>()
                .As<ISiteMapNodeProvider>()
                .WithKey("reflectionSiteMapNodeProvider1")
                .WithCtorParam(() => includeAssembliesForScan).Named("includeAssemblies")
                .WithCtorParam(() => new string[0]).Named("excludeAssemblies");

            container.Export<CompositeSiteMapNodeProvider>()
                .As<ISiteMapNodeProvider>()
                .WithKey("siteMapNodeProvider1")
                .WithCtorParam<ISiteMapNodeProvider[]>().LocateWithKey(new[]
                    {
                        "xmlSiteMapNodeProvider1",
                        "reflectionSiteMapNodeProvider1"
                    });

            // Register the sitemap builders
            container.Export<SiteMapBuilder>()
                .As<ISiteMapBuilder>()
                .WithKey("siteMapBuilder1")
                .WithCtorParam<ISiteMapNodeProvider>().Named("siteMapNodeProvider").LocateWithKey("siteMapNodeProvider1");

            // Configure the builder sets
            container.Export<SiteMapBuilderSet>()
                .As<ISiteMapBuilderSet>()
                .WithKey("builderSet1")
                .WithCtorParam(() => "default").Named("instanceName")
                .WithCtorParam<ISiteMapBuilder>().Named("siteMapBuilder").LocateWithKey("siteMapBuilder1")
                .WithCtorParam<ICacheDetails>().Named("cacheDetails").LocateWithKey("cacheDetails1")
                .WithNamedCtorValue(() => securityTrimmingEnabled)
                .WithNamedCtorValue(() => enableLocalization)
                .WithNamedCtorValue(() => visibilityAffectsDescendants)
                .WithNamedCtorValue(() => useTitleIfDescriptionNotProvided);

            container.Export<SiteMapBuilderSetStrategy>()
                .As<ISiteMapBuilderSetStrategy>()
                .WithCtorParam<ISiteMapBuilderSet[]>().Named("siteMapBuilderSets").LocateWithKey(new[] { "builderSet1" });
        }
    }
}
