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
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;
using DI.StructureMap.Conventions;

namespace DI.StructureMap.Registries
{
    public class MvcSiteMapProviderRegistry
        : Registry
    {
        public MvcSiteMapProviderRegistry()
        {
            string absoluteFileName = HostingEnvironment.MapPath("~/Mvc.sitemap");
            TimeSpan absoluteCacheExpiration = TimeSpan.FromMinutes(5);
            string[] includeAssembliesForScan = new string[] { "$AssemblyName$" };

            var currentAssembly = this.GetType().Assembly;
            var siteMapProviderAssembly = typeof(SiteMaps).Assembly;
            var allAssemblies = new Assembly[] { currentAssembly, siteMapProviderAssembly };
            var excludeTypes = new Type[] { 
                typeof(SiteMapNodeVisibilityProviderStrategy),
                typeof(SiteMapXmlReservedAttributeNameProvider),
                typeof(SiteMapBuilderSetStrategy),
                typeof(ControllerTypeResolverFactory)
            };
            var multipleImplementationTypes = new Type[]  { 
                typeof(ISiteMapNodeUrlResolver), 
                typeof(ISiteMapNodeVisibilityProvider), 
                typeof(IDynamicNodeProvider) 
            };

            // Single implementations of interface with matching name (minus the "I").
            CommonConventions.RegisterDefaultConventions(
                (interfaceType, implementationType) => this.For(interfaceType).Singleton().Use(implementationType),
                new Assembly[] { siteMapProviderAssembly },
                allAssemblies,
                excludeTypes,
                string.Empty);

            // Multiple implementations of strategy based extension points
            CommonConventions.RegisterAllImplementationsOfInterface(
                (interfaceType, implementationType) => this.For(interfaceType).Singleton().Use(implementationType),
                multipleImplementationTypes,
                allAssemblies,
                excludeTypes,
                "^Composite");

            // Visibility Providers
            this.For<ISiteMapNodeVisibilityProviderStrategy>().Use<SiteMapNodeVisibilityProviderStrategy>()
                .Ctor<string>("defaultProviderName").Is(string.Empty);

            // Pass in the global controllerBuilder reference
            this.For<ControllerBuilder>()
                .Use(x => ControllerBuilder.Current);

            this.For<IControllerBuilder>()
                .Use<ControllerBuilderAdaptor>();

            this.For<IBuildManager>()
                .Use<BuildManagerAdaptor>();

            this.For<IControllerTypeResolverFactory>().Use<ControllerTypeResolverFactory>()
                .Ctor<string[]>("areaNamespacesToIgnore").Is(new string[0]);

            // Configure Security
            this.For<IAclModule>().Use<CompositeAclModule>()
                .EnumerableOf<IAclModule>().Contains(x =>
                {
                    x.Type<AuthorizeAttributeAclModule>();
                    x.Type<XmlRolesAclModule>();
                });

            // Setup cache
            SmartInstance<CacheDetails> cacheDetails;

#if NET35
        this.For<ICacheProvider<ISiteMap>>().Use<AspNetCacheProvider<ISiteMap>>();

        var cacheDependency =
            this.For<ICacheDependency>().Use<AspNetFileCacheDependency>()
                .Ctor<string>("fileName").Is(absoluteFileName);

        cacheDetails =
            this.For<ICacheDetails>().Use<CacheDetails>()
                .Ctor<TimeSpan>("absoluteCacheExpiration").Is(absoluteCacheExpiration)
                .Ctor<TimeSpan>("slidingCacheExpiration").Is(TimeSpan.MinValue)
                .Ctor<ICacheDependency>().Is(cacheDependency);
#else
            this.For<System.Runtime.Caching.ObjectCache>()
                .Use(s => System.Runtime.Caching.MemoryCache.Default);

            this.For<ICacheProvider<ISiteMap>>().Use<RuntimeCacheProvider<ISiteMap>>();

            var cacheDependency =
                this.For<ICacheDependency>().Use<RuntimeFileCacheDependency>()
                    .Ctor<string>("fileName").Is(absoluteFileName);

            cacheDetails =
                this.For<ICacheDetails>().Use<CacheDetails>()
                    .Ctor<TimeSpan>("absoluteCacheExpiration").Is(absoluteCacheExpiration)
                    .Ctor<TimeSpan>("slidingCacheExpiration").Is(TimeSpan.MinValue)
                    .Ctor<ICacheDependency>().Is(cacheDependency);
#endif
            // Configure the visitors
            this.For<ISiteMapNodeVisitor>()
                .Use<UrlResolvingSiteMapNodeVisitor>();


            // Register the sitemap builder
            var xmlSource = this.For<IXmlSource>().Use<FileXmlSource>()
                           .Ctor<string>("fileName").Is(absoluteFileName);

            var reservedAttributeNameProvider = this.For<ISiteMapXmlReservedAttributeNameProvider>()
                .Use<SiteMapXmlReservedAttributeNameProvider>()
                .Ctor<IEnumerable<string>>("attributesToIgnore").Is(new string[0]);
                
            var builder = this.For<ISiteMapBuilder>().Use<CompositeSiteMapBuilder>()
                .EnumerableOf<ISiteMapBuilder>().Contains(y =>
                {
                    y.Type<XmlSiteMapBuilder>()
                        .Ctor<ISiteMapXmlReservedAttributeNameProvider>().Is(reservedAttributeNameProvider)
                        .Ctor<IXmlSource>().Is(xmlSource);
                    y.Type<ReflectionSiteMapBuilder>()
                        .Ctor<IEnumerable<string>>("includeAssemblies").Is(includeAssembliesForScan)
                        .Ctor<IEnumerable<string>>("excludeAssemblies").Is(new string[0]);
                    y.Type<VisitingSiteMapBuilder>();
                });


            // Configure the builder sets
            this.For<ISiteMapBuilderSetStrategy>().Use<SiteMapBuilderSetStrategy>()
                .EnumerableOf<ISiteMapBuilderSet>().Contains(x =>
                {
                    x.Type<SiteMapBuilderSet>()
                        .Ctor<string>("instanceName").Is("default")
                        .Ctor<ISiteMapBuilder>().Is(builder)
                        .Ctor<ICacheDetails>().Is(cacheDetails);
                });
        }
    }
}
