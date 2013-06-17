using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Hosting;
using System.Web.Routing;
using System.Reflection;
using Autofac;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Loader;
using MvcSiteMapProvider.Visitor;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.Compilation;
using MvcSiteMapProvider.Web.Mvc.Filters;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Globalization;

namespace DI.Autofac.Modules
{
    public class MvcSiteMapProviderModule
        : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
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
                typeof(SiteMapBuilderSetStrategy)
            };
            var multipleImplementationTypes = new Type[]  { 
                typeof(ISiteMapNodeUrlResolver), 
                typeof(ISiteMapNodeVisibilityProvider), 
                typeof(IDynamicNodeProvider) 
            };

            // Single implementations of interface with matching name (minus the "I").
            CommonConventions.RegisterDefaultConventions(
                (interfaceType, implementationType) => builder.RegisterType(implementationType).As(interfaceType).SingleInstance(),
                new Assembly[] { siteMapProviderAssembly },
                allAssemblies,
                excludeTypes,
                string.Empty);

            // Multiple implementations of strategy based extension points
            CommonConventions.RegisterAllImplementationsOfInterface(
                (interfaceType, implementationType) => builder.RegisterType(implementationType).As(interfaceType).SingleInstance(),
                multipleImplementationTypes,
                allAssemblies,
                excludeTypes,
                "^Composite");

            // Registration of internal controllers
            CommonConventions.RegisterAllImplementationsOfInterface(
                (interfaceType, implementationType) => builder.RegisterType(implementationType).As(interfaceType).AsSelf().InstancePerDependency(),
                new Type[] { typeof(IController) },
                new Assembly[] { siteMapProviderAssembly },
                new Type[0],
                string.Empty);

            // Visibility Providers
            builder.RegisterType<SiteMapNodeVisibilityProviderStrategy>()
                .As<ISiteMapNodeVisibilityProviderStrategy>()
                .WithParameter("defaultProviderName", string.Empty);

            // Pass in the global controllerBuilder reference
            builder.RegisterInstance(ControllerBuilder.Current)
                   .As<ControllerBuilder>();

            builder.RegisterType<BuildManagerAdaptor>()
                   .As<IBuildManager>();

            builder.RegisterType<ControllerBuilderAdaptor>()
                   .As<IControllerBuilder>();

            builder.RegisterType<ControllerTypeResolverFactory>()
                .As<IControllerTypeResolverFactory>()
                .WithParameter("areaNamespacesToIgnore", new string[0]);

#if !MVC2
            // Configure default filter provider with one that provides filters
            // from the global filter collection.
            builder.RegisterType<FilterProvider>()
                   .As<IFilterProvider>()
                   .SingleInstance();
#endif

            // Configure Security
            builder.RegisterType<AuthorizeAttributeAclModule>()
                   .AsSelf();
            builder.RegisterType<XmlRolesAclModule>()
                   .AsSelf();
            builder.Register(ctx => new CompositeAclModule(
                                        ctx.Resolve<AuthorizeAttributeAclModule>(),
                                        ctx.Resolve<XmlRolesAclModule>()
                                    ))
                    .As<IAclModule>();

#if NET35
            builder.RegisterType<RuntimeCacheProvider<ISiteMap>>()
                   .As<ICacheProvider<ISiteMap>>();

            builder.RegisterType<AspNetFileCacheDependency>()
                .Named<ICacheDependency>("cacheDependency")
                .WithParameter("fileName", absoluteFileName);
#else
            builder.RegisterInstance(System.Runtime.Caching.MemoryCache.Default)
                   .As<System.Runtime.Caching.ObjectCache>();

            builder.RegisterType<RuntimeCacheProvider<ISiteMap>>()
                   .As<ICacheProvider<ISiteMap>>();

            builder.RegisterType<RuntimeFileCacheDependency>()
                .Named<ICacheDependency>("cacheDependency")
                .WithParameter("fileName", absoluteFileName);
#endif
            builder.RegisterType<CacheDetails>()
                .Named<ICacheDetails>("cacheDetails")
                .WithParameter("absoluteCacheExpiration", absoluteCacheExpiration)
                .WithParameter("slidingCacheExpiration", TimeSpan.MinValue)
                .WithParameter(
                    (p, c) => p.Name == "cacheDependency",
                    (p, c) => c.ResolveNamed<ICacheDependency>("cacheDependency"));

            // Configure the visitors
            builder.RegisterType<UrlResolvingSiteMapNodeVisitor>()
                   .As<ISiteMapNodeVisitor>();

            // Prepare for our builders
            builder.Register(ctx => new FileXmlSource(absoluteFileName))
                   .Named<IXmlSource>("xmlSource");

            builder.RegisterType<SiteMapXmlReservedAttributeNameProvider>()
                .As<ISiteMapXmlReservedAttributeNameProvider>()
                .WithParameter("attributesToIgnore", new string[0]);

            // Register the sitemap builders
            builder.RegisterType<XmlSiteMapBuilder>()
                .AsSelf()
                .WithParameter(
                    (p, c) => p.Name == "xmlSource",
                    (p, c) => c.ResolveNamed<IXmlSource>("xmlSource"));

            builder.RegisterType<ReflectionSiteMapBuilder>()
                .AsSelf()
                .WithParameter("includeAssemblies", includeAssembliesForScan)
                .WithParameter("excludeAssemblies", new string[0]);


            builder.RegisterType<VisitingSiteMapBuilder>()
                   .AsSelf();

            builder.Register(ctx => new CompositeSiteMapBuilder(
                                        ctx.Resolve<XmlSiteMapBuilder>(),
                                        ctx.Resolve<ReflectionSiteMapBuilder>(),
                                        ctx.Resolve<VisitingSiteMapBuilder>()
                                    ))
                   .Named<ISiteMapBuilder>("siteMapBuilder");

            // Configure the builder sets
            builder.RegisterType<SiteMapBuilderSet>()
                   .Named<ISiteMapBuilderSet>("builderSet")
                   .WithParameter("instanceName", "default")
                   .WithParameter(
                        (p, c) => p.Name == "siteMapBuilder",
                        (p, c) => c.ResolveNamed<ISiteMapBuilder>("siteMapBuilder"))
                   .WithParameter(
                        (p, c) => p.Name == "cacheDetails",
                        (p, c) => c.ResolveNamed<ICacheDetails>("cacheDetails"));

            builder.RegisterType<SiteMapBuilderSetStrategy>()
                .As<ISiteMapBuilderSetStrategy>()
                .WithParameter(
                    (p, c) => p.Name == "siteMapBuilderSets",
                    (p, c) => c.ResolveNamed<IEnumerable<ISiteMapBuilderSet>>("builderSet"));
        }
    }
}

