using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Hosting;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.Mvc.Filters;
using MvcSiteMapProvider.Web.Compilation;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Visitor;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Globalization;
using DI.StructureMap.Conventions;

namespace DI.StructureMap.Registries
{
    internal class MvcSiteMapProviderRegistry
        : Registry
    {
        public MvcSiteMapProviderRegistry()
        {
            string fileName = "~/Mvc.sitemap";
            TimeSpan absoluteCacheExpiration = TimeSpan.FromMinutes(5);

            this.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AssemblyContainingType<SiteMaps>();
                scan.WithDefaultConventions();
            });

            this.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AssemblyContainingType<SiteMaps>();
                scan.AssemblyContainingType<InjectableControllerFactory>();
                scan.WithDefaultConventions();
                scan.AddAllTypesOf<IMvcContextFactory>();
                scan.AddAllTypesOf<ISiteMapCacheKeyToBuilderSetMapper>();
                scan.AddAllTypesOf<IDynamicNodeProvider>();
                scan.AddAllTypesOf<ISiteMapNodeVisibilityProvider>();
                scan.AddAllTypesOf<ISiteMapNodeUrlResolver>();
                scan.AddAllTypesOf<IDynamicNodeProviderStrategy>();
                scan.AddAllTypesOf<ISiteMapNodeUrlResolverStrategy>();
                scan.AddAllTypesOf<ISiteMapNodeVisibilityProviderStrategy>();
                scan.AddAllTypesOf<IFilterProvider>();
                scan.AddAllTypesOf<IControllerDescriptorFactory>();
                scan.AddAllTypesOf<IObjectCopier>();
                scan.AddAllTypesOf<INodeKeyGenerator>();
                scan.AddAllTypesOf<IExplicitResourceKeyParser>();
                scan.AddAllTypesOf<IStringLocalizer>();
                scan.AddAllTypesOf<IDynamicNodeBuilder>();
                scan.Convention<SingletonConvention>();
            });

            // Pass in the global controllerBuilder reference
            this.For<ControllerBuilder>()
                .Use(x => ControllerBuilder.Current);

            this.For<IControllerBuilder>()
                .Use<ControllerBuilderAdaptor>();

            this.For<IBuildManager>()
                .Use<BuildManagerAdaptor>();

            // Pass in the global route collection
            this.For<System.Web.Routing.RouteCollection>()
                .Use(x => RouteTable.Routes);

#if !MVC2
            // Configure default filter provider with one that provides filters
            // from the global filter collection.
            this.For<System.Web.Mvc.IFilterProvider>()
                .Singleton()
                .Use<MvcSiteMapProvider.Web.Mvc.Filters.FilterProvider>();
#endif

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
        this.For<ISiteMapCache>()
            .Use<AspNetSiteMapCache>();

        var cacheDependency =
            this.For<ICacheDependency>().Use<AspNetFileCacheDependency>()
                .Ctor<string>("fileName").Is(HostingEnvironment.MapPath(fileName));

        cacheDetails =
            this.For<ICacheDetails>().Use<CacheDetails>()
                .Ctor<TimeSpan>("absoluteCacheExpiration").Is(absoluteCacheExpiration)
                .Ctor<TimeSpan>("slidingCacheExpiration").Is(TimeSpan.MinValue)
                .Ctor<ICacheDependency>().Is(cacheDependency);
#else
            this.For<System.Runtime.Caching.ObjectCache>()
                .Use(s => System.Runtime.Caching.MemoryCache.Default);

            this.For<ISiteMapCache>()
                .Use<RuntimeSiteMapCache>();

            var cacheDependency =
                this.For<ICacheDependency>().Use<RuntimeFileCacheDependency>()
                    .Ctor<string>("fileName").Is(HostingEnvironment.MapPath(fileName));

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
                           .Ctor<string>("xmlFileName").Is(fileName);

            var builder = this.For<ISiteMapBuilder>().Use<CompositeSiteMapBuilder>()
                .EnumerableOf<MvcSiteMapProvider.Builder.ISiteMapBuilder>().Contains(y =>
                {
                    y.Type<XmlSiteMapBuilder>()
                        .Ctor<IEnumerable<string>>("attributesToIgnore").Is(new string[0])
                        .Ctor<IXmlSource>().Is(xmlSource);
                    y.Type<ReflectionSiteMapBuilder>()
                        .Ctor<IEnumerable<string>>("includeAssemblies").Is(new string[0])
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
