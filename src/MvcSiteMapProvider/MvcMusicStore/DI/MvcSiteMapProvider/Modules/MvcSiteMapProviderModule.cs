using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Hosting;
using Autofac;
using MvcMusicStore.DI.MvcSiteMapProvider.Conventions;
using MvcSiteMapProvider;
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

namespace MvcMusicStore.DI.MvcSiteMapProvider.Registries
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
                   .AsSelf();

            builder.RegisterAssemblyTypes(typeof(SiteMaps).Assembly)
                   .AsSelf();
            
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

#if !MVC2
            // Configure default filter provider with one that provides filters
            // from the global filter collection.
            builder.RegisterType<FilterProvider>()
                   .As<IFilterProvider>()
                   .SingleInstance();

            this.For<System.Web.Mvc.IFilterProvider>()
                .Singleton()
                .Use<>();
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
                .Ctor<string>("fileName").Is(absoluteFileName);

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
                
            var buxilder = this.For<ISiteMapBuilder>().Use<CompositeSiteMapBuilder>()
                .EnumerableOf<ISiteMapBuilder>().Contains(y =>
                {
                    y.Type<XmlSiteMapBuilder>()
                        .Ctor<ISiteMapXmlReservedAttributeNameProvider>().Is(reservedAttributeNameProvider)
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
