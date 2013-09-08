using System;
using System.Web.Hosting;
using System.Collections.Generic;
using MvcSiteMapProvider.Loader;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Visitor;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Reflection;

namespace MvcSiteMapProvider.DI
{
    /// <summary>
    /// A specialized dependency injection container for resolving a <see cref="T:MvcSiteMapProvider.Loader.SiteMapLoader"/> instance.
    /// </summary>
    internal class SiteMapLoaderContainer
    {
        public SiteMapLoaderContainer(ConfigurationSettings settings)
        {
            // Singleton instances
            if (settings.EnableSiteMapFile)
            {
                this.absoluteFileName = HostingEnvironment.MapPath(settings.SiteMapFileName);
            }
            this.mvcContextFactory = new MvcContextFactory();
#if NET35
            this.siteMapCache = new SiteMapCache(new AspNetCacheProvider<ISiteMap>(this.mvcContextFactory));
#else
            this.siteMapCache = new SiteMapCache(new RuntimeCacheProvider<ISiteMap>(System.Runtime.Caching.MemoryCache.Default));
#endif
            this.cacheDependency = this.ResolveCacheDependency(settings);
            this.requestCache = this.mvcContextFactory.GetRequestCache();
            this.urlPath = new UrlPath(this.mvcContextFactory);
            this.siteMapCacheKeyGenerator = new SiteMapCacheKeyGenerator(this.mvcContextFactory);
            this.siteMapCacheKeyToBuilderSetMapper = new SiteMapCacheKeyToBuilderSetMapper();
            var siteMapNodeFactoryContainer = new SiteMapNodeFactoryContainer(settings, this.mvcContextFactory, this.urlPath);
            this.siteMapNodeParentMapFactory = new SiteMapNodeParentMapFactory();
            this.nodeKeyGenerator = new NodeKeyGenerator();
            this.siteMapNodeFactory = siteMapNodeFactoryContainer.ResolveSiteMapNodeFactory();
            this.siteMapNodeCreationService = this.ResolveSiteMapNodeCreationService();
            this.dynamicNodeParentMapBuilder = new DynamicNodeParentMapBuilder(this.siteMapNodeCreationService);
            this.siteMapHierarchyBuilder = new SiteMapHierarchyBuilder();
            this.siteMapNodeBuilderService = this.ResolveSiteMapNodeBuilderService();
            this.siteMapAssemblyService = this.ResolveSiteMapAssemblyService();
            this.attributeAssemblyProviderFactory = new AttributeAssemblyProviderFactory();
            this.mvcSiteMapNodeAttributeDefinitionProvider = new MvcSiteMapNodeAttributeDefinitionProvider();
            this.siteMapXmlNameProvider = new SiteMapXmlNameProvider();
            this.siteMapBuiderSetStrategy = this.ResolveSiteMapBuilderSetStrategy(settings);
            var siteMapFactoryContainer = new SiteMapFactoryContainer(settings, this.mvcContextFactory, this.urlPath);
            this.siteMapFactory = siteMapFactoryContainer.ResolveSiteMapFactory();
            this.siteMapCreator = new SiteMapCreator(this.siteMapCacheKeyToBuilderSetMapper, this.siteMapBuiderSetStrategy, this.siteMapFactory);
        }

        private readonly string absoluteFileName;
        private readonly IMvcContextFactory mvcContextFactory;
        private readonly ISiteMapCache siteMapCache;
        private readonly ICacheDependency cacheDependency;
        private readonly IRequestCache requestCache;
        private readonly IUrlPath urlPath;
        private readonly ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator;
        private readonly ISiteMapCacheKeyToBuilderSetMapper siteMapCacheKeyToBuilderSetMapper;
        private readonly ISiteMapBuilderSetStrategy siteMapBuiderSetStrategy;
        private readonly INodeKeyGenerator nodeKeyGenerator;
        private readonly ISiteMapNodeParentMapFactory siteMapNodeParentMapFactory;
        private readonly ISiteMapNodeFactory siteMapNodeFactory;
        private readonly ISiteMapNodeCreationService siteMapNodeCreationService;
        private readonly ISiteMapNodeBuilderService siteMapNodeBuilderService;
        private readonly ISiteMapAssemblyService siteMapAssemblyService;
        private readonly ISiteMapHierarchyBuilder siteMapHierarchyBuilder;
        private readonly IAttributeAssemblyProviderFactory attributeAssemblyProviderFactory;
        private readonly IMvcSiteMapNodeAttributeDefinitionProvider mvcSiteMapNodeAttributeDefinitionProvider;
        private readonly ISiteMapXmlNameProvider siteMapXmlNameProvider;
        private readonly IDynamicNodeParentMapBuilder dynamicNodeParentMapBuilder;
        private readonly ISiteMapFactory siteMapFactory;
        private readonly ISiteMapCreator siteMapCreator;
        
        public ISiteMapLoader ResolveSiteMapLoader()
        {
            return new SiteMapLoader(
                this.siteMapCache,
                this.siteMapCacheKeyGenerator,
                this.siteMapCreator);
        }

        private ISiteMapBuilderSetStrategy ResolveSiteMapBuilderSetStrategy(ConfigurationSettings settings)
        {
            return new SiteMapBuilderSetStrategy(
                new ISiteMapBuilderSet[] {
                    new SiteMapBuilderSet(
                        "default", 
                        settings.SecurityTrimmingEnabled, 
                        settings.EnableLocalization,
                        this.ResolveSiteMapBuilder(settings),
                        this.ResolveCacheDetails(settings)
                        )
                    }
                );
        }

        private ISiteMapBuilder ResolveSiteMapBuilder(ConfigurationSettings settings)
        {
            var builders = new List<ISiteMapBuilder>();
            if (settings.EnableSiteMapFile)
            {
                builders.Add(this.ResolveXmlSiteMapBuilder(settings.SiteMapFileName, settings.AttributesToIgnore));
            }
            if (settings.ScanAssembliesForSiteMapNodes)
            {
                builders.Add(this.ResolveReflectionSiteMapBuilder(settings.IncludeAssembliesForScan, settings.ExcludeAssembliesForScan, settings.AttributesToIgnore));
            }
            if (settings.EnableResolvedUrlCaching)
            {
                builders.Add(this.ResolveVisitingSiteMapBuilder());
            }
            return new CompositeSiteMapBuilder(builders.ToArray());
        }

        private ISiteMapBuilder ResolveXmlSiteMapBuilder(string siteMapFile, IEnumerable<string> attributesToIgnore)
        {
            return new XmlSiteMapBuilder(
                new FileXmlSource(this.absoluteFileName),
                new SiteMapXmlReservedAttributeNameProvider(attributesToIgnore),
                this.siteMapXmlNameProvider,
                this.siteMapAssemblyService);
        }

        private ISiteMapBuilder ResolveReflectionSiteMapBuilder(IEnumerable<string> includeAssemblies, IEnumerable<string> excludeAssemblies, IEnumerable<string> attributesToIgnore)
        {
            return new ReflectionSiteMapBuilder(
                includeAssemblies,
                excludeAssemblies,
                new SiteMapXmlReservedAttributeNameProvider(attributesToIgnore),
                this.siteMapAssemblyService,
                this.attributeAssemblyProviderFactory,
                this.mvcSiteMapNodeAttributeDefinitionProvider);
        }

        private ISiteMapBuilder ResolveVisitingSiteMapBuilder()
        {
            return new VisitingSiteMapBuilder(
                new UrlResolvingSiteMapNodeVisitor());
        }

        private ISiteMapNodeCreationService ResolveSiteMapNodeCreationService()
        {
            return new SiteMapNodeCreationService(
                this.siteMapNodeFactory, 
                this.nodeKeyGenerator, 
                this.siteMapNodeParentMapFactory);
        }
        
        private ISiteMapNodeBuilderService ResolveSiteMapNodeBuilderService()
        {
            return new SiteMapNodeBuilderService(
                this.siteMapNodeCreationService,
                this.dynamicNodeParentMapBuilder);
        }

        private ISiteMapAssemblyService ResolveSiteMapAssemblyService()
        {
            return new SiteMapAssemblyService(
                this.siteMapNodeBuilderService,
                this.siteMapHierarchyBuilder,
                this.siteMapCacheKeyGenerator);
        }

        private ICacheDetails ResolveCacheDetails(ConfigurationSettings settings)
        {
            return new CacheDetails(
                TimeSpan.FromMinutes(settings.CacheDuration),
                TimeSpan.MinValue,
                this.cacheDependency
                );
        }

        private ICacheDependency ResolveCacheDependency(ConfigurationSettings settings)
        {
            if (settings.EnableSiteMapFile)
            {
#if NET35
                return new AspNetFileCacheDependency(absoluteFileName);
#else
                return new RuntimeFileCacheDependency(absoluteFileName);
#endif
            }
            else
            {
                return new NullCacheDependency();
            }
        }
    }
}
