using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Hosting;
using System.Web.Routing;
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
        : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = typeof(MvcModule).Assembly;
            string absoluteFileName = HostingEnvironment.MapPath("~/Mvc.sitemap");
            TimeSpan absoluteCacheExpiration = TimeSpan.FromMinutes(5);
            string[] includeAssembliesForScan = new string[] { "$AssemblyName$" };

            builder.RegisterAssemblyTypes(currentAssembly, typeof(SiteMaps).Assembly)
                   .AsSelf()
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(currentAssembly, typeof(SiteMaps).Assembly)
                .Where(t =>                 
                       typeof(IMvcContextFactory).IsAssignableFrom(t)
                    || typeof(ISiteMapCacheKeyToBuilderSetMapper).IsAssignableFrom(t)
                    || typeof(IDynamicNodeProvider).IsAssignableFrom(t)
                    || typeof(ISiteMapNodeVisibilityProvider).IsAssignableFrom(t)
                    || typeof(ISiteMapNodeUrlResolver).IsAssignableFrom(t)
                    || typeof(IDynamicNodeProviderStrategy).IsAssignableFrom(t)
                    || typeof(ISiteMapNodeUrlResolverStrategy).IsAssignableFrom(t)
                    || typeof(ISiteMapNodeVisibilityProviderStrategy).IsAssignableFrom(t)
#if !MVC2
                    || typeof(IFilterProvider).IsAssignableFrom(t)
#endif
                    || typeof(IControllerDescriptorFactory).IsAssignableFrom(t)
                    || typeof(IObjectCopier).IsAssignableFrom(t)
                    || typeof(INodeKeyGenerator).IsAssignableFrom(t)
                    || typeof(IExplicitResourceKeyParser).IsAssignableFrom(t)
                    || typeof(IStringLocalizer).IsAssignableFrom(t)
                    || typeof(IDynamicNodeBuilder).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

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

#if !NET35
            builder.RegisterType<AspNetSiteMapCache>()
                   .As<ISiteMapCache>();

            builder.RegisterType<AspNetFileCacheDependency>()
                .Named<ICacheDependency>("cacheDependency")
                .WithParameter("fileName", absoluteFileName);
#else
            builder.RegisterInstance(System.Runtime.Caching.MemoryCache.Default)
                   .As<System.Runtime.Caching.ObjectCache>();

            builder.RegisterType<RuntimeSiteMapCache>()
                   .As<ISiteMapCache>();

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

