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

            builder.RegisterAssemblyTypes(currentAssembly)
                   .AsSelf()
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof (SiteMaps).Assembly)
                   .AsSelf()
                   .AsImplementedInterfaces();
            
            builder.RegisterAssemblyTypes(currentAssembly)
                .Where(t =>                 
                       typeof(IMvcContextFactory).IsAssignableFrom(t)
                    || typeof(ISiteMapCacheKeyToBuilderSetMapper).IsAssignableFrom(t)
                    || typeof(IDynamicNodeProvider).IsAssignableFrom(t)
                    || typeof(ISiteMapNodeVisibilityProvider).IsAssignableFrom(t)
                    || typeof(ISiteMapNodeUrlResolver).IsAssignableFrom(t)
                    || typeof(IDynamicNodeProviderStrategy).IsAssignableFrom(t)
                    || typeof(ISiteMapNodeUrlResolverStrategy).IsAssignableFrom(t)
                    || typeof(ISiteMapNodeVisibilityProviderStrategy).IsAssignableFrom(t)
                    || typeof(IFilterProvider).IsAssignableFrom(t)
                    || typeof(IControllerDescriptorFactory).IsAssignableFrom(t)
                    || typeof(IObjectCopier).IsAssignableFrom(t)
                    || typeof(INodeKeyGenerator).IsAssignableFrom(t)
                    || typeof(IExplicitResourceKeyParser).IsAssignableFrom(t)
                    || typeof(IStringLocalizer).IsAssignableFrom(t)
                    || typeof(IDynamicNodeBuilder).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(SiteMaps).Assembly)
                .Where(t =>                 
                       typeof(IMvcContextFactory).IsAssignableFrom(t)
                    || typeof(ISiteMapCacheKeyToBuilderSetMapper).IsAssignableFrom(t)
                    || typeof(IDynamicNodeProvider).IsAssignableFrom(t)
                    || typeof(ISiteMapNodeVisibilityProvider).IsAssignableFrom(t)
                    || typeof(ISiteMapNodeUrlResolver).IsAssignableFrom(t)
                    || typeof(IDynamicNodeProviderStrategy).IsAssignableFrom(t)
                    || typeof(ISiteMapNodeUrlResolverStrategy).IsAssignableFrom(t)
                    || typeof(ISiteMapNodeVisibilityProviderStrategy).IsAssignableFrom(t)
                    || typeof(IFilterProvider).IsAssignableFrom(t)
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

            // Register routes
            builder.RegisterInstance(RouteTable.Routes)
                   .AsSelf();

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
            builder.RegisterType<AspNetSiteMapCache>()
                   .As<ISiteMapCache>();

            builder.Register(ctx => new AspNetFileCacheDependency(absoluteFileName))
                   .As<ICacheDependency>();

            builder.Register(ctx => new CacheDetails(absoluteCacheExpiration, TimeSpan.MinValue, ctx.Resolve<ICacheDependency>()))
                   .As<ICacheDetails>();
#else
            builder.RegisterInstance(System.Runtime.Caching.MemoryCache.Default)
                   .As<System.Runtime.Caching.ObjectCache>();

            builder.RegisterType<RuntimeSiteMapCache>()
                   .As<ISiteMapCache>();

            builder.Register(ctx => new RuntimeFileCacheDependency(absoluteFileName))
                   .As<ICacheDependency>();

            builder.Register(ctx => new CacheDetails(absoluteCacheExpiration, TimeSpan.MinValue, ctx.Resolve<ICacheDependency>()))
                   .As<ICacheDetails>();
#endif
            // Configure the visitors
            builder.RegisterType<UrlResolvingSiteMapNodeVisitor>()
                   .As<ISiteMapNodeVisitor>();

            // Register the sitemap builder
            builder.Register(ctx => new FileXmlSource(absoluteFileName))
                   .As<IXmlSource>();

            builder.Register(ctx => new SiteMapXmlReservedAttributeNameProvider(new string[0]))
                   .As<ISiteMapXmlReservedAttributeNameProvider>();

            builder.RegisterType<XmlSiteMapBuilder>()
                   .AsSelf();

            builder.Register(ctx => new ReflectionSiteMapBuilder(new string[0], new string[0], ctx.Resolve<ISiteMapXmlReservedAttributeNameProvider>(), ctx.Resolve<INodeKeyGenerator>(), ctx.Resolve<IDynamicNodeBuilder>(), ctx.Resolve<ISiteMapNodeFactory>(), ctx.Resolve<ISiteMapCacheKeyGenerator>()))
                   .AsSelf();

            builder.RegisterType<VisitingSiteMapBuilder>()
                   .AsSelf();

            builder.Register(ctx => new CompositeSiteMapBuilder(
                                        ctx.Resolve<XmlSiteMapBuilder>(),
                                        ctx.Resolve<ReflectionSiteMapBuilder>(),
                                        ctx.Resolve<VisitingSiteMapBuilder>()
                                    ))
                   .As<ISiteMapBuilder>();

            // Configure the builder sets
            builder.Register(ctx => new SiteMapBuilderSetStrategy(
                new ISiteMapBuilderSet[] { new SiteMapBuilderSet("default", ctx.Resolve<ISiteMapBuilder>(), ctx.Resolve<ICacheDetails>()) }))
                   .As<ISiteMapBuilderSetStrategy>();
        }
    }
}

