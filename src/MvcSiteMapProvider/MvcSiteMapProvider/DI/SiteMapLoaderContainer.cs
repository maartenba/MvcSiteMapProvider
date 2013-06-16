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
            this.absoluteFileName = HostingEnvironment.MapPath(settings.SiteMapFileName);
            this.mvcContextFactory = new MvcContextFactory();
#if NET35
            this.siteMapCache = new SiteMapCache(new AspNetCacheProvider<ISiteMap>(this.mvcContextFactory));
#else
            this.siteMapCache = new SiteMapCache(new RuntimeCacheProvider<ISiteMap>(System.Runtime.Caching.MemoryCache.Default));
#endif
            this.requestCache = this.mvcContextFactory.GetRequestCache();
            this.urlPath = new UrlPath(this.mvcContextFactory);
            this.siteMapCacheKeyGenerator = new SiteMapCacheKeyGenerator(this.mvcContextFactory);
            this.siteMapCacheKeyToBuilderSetMapper = new SiteMapCacheKeyToBuilderSetMapper();
            this.nodeKeyGenerator = new NodeKeyGenerator();
            var siteMapNodeFactoryContainer = new SiteMapNodeFactoryContainer(settings, this.mvcContextFactory, this.urlPath);
            this.siteMapNodeFactory = siteMapNodeFactoryContainer.ResolveSiteMapNodeFactory();
            this.siteMapXmlNameProvider = new SiteMapXmlNameProvider();
            this.dynamicNodeBuilder = new DynamicNodeBuilder(this.nodeKeyGenerator, this.siteMapNodeFactory);
            this.siteMapBuiderSetStrategy = this.ResolveSiteMapBuilderSetStrategy(settings);
            var siteMapFactoryContainer = new SiteMapFactoryContainer(settings, this.mvcContextFactory, this.urlPath);
            this.siteMapFactory = siteMapFactoryContainer.ResolveSiteMapFactory();
            this.siteMapCreator = new SiteMapCreator(this.siteMapCacheKeyToBuilderSetMapper, this.siteMapBuiderSetStrategy, this.siteMapFactory);
        }

        private readonly string absoluteFileName;
        private readonly IMvcContextFactory mvcContextFactory;
        private readonly ISiteMapCache siteMapCache;
        private readonly IRequestCache requestCache;
        private readonly IUrlPath urlPath;
        private readonly ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator;
        private readonly ISiteMapCacheKeyToBuilderSetMapper siteMapCacheKeyToBuilderSetMapper;
        private readonly ISiteMapBuilderSetStrategy siteMapBuiderSetStrategy;
        private readonly INodeKeyGenerator nodeKeyGenerator;
        private readonly ISiteMapNodeFactory siteMapNodeFactory;
        private readonly ISiteMapXmlNameProvider siteMapXmlNameProvider;
        private readonly IDynamicNodeBuilder dynamicNodeBuilder;
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
                        this.ResolveSiteMapBuilder(settings),
                        this.ResolveCacheDetails(settings)
                        )
                    }
                );
        }

        private ISiteMapBuilder ResolveSiteMapBuilder(ConfigurationSettings settings)
        {
            var xmlSiteMapBuilder = this.ResolveXmlSiteMapBuilder(settings.SiteMapFileName, settings.AttributesToIgnore);
            var visitingSiteMapBuilder = this.ResolveVisitingSiteMapBuilder();

            if (settings.ScanAssembliesForSiteMapNodes)
            {
                var reflectionSiteMapBuilder = this.ResolveReflectionSiteMapBuilder(settings.IncludeAssembliesForScan, settings.ExcludeAssembliesForScan, settings.AttributesToIgnore);
                return new CompositeSiteMapBuilder(xmlSiteMapBuilder, reflectionSiteMapBuilder, visitingSiteMapBuilder);
            }
            else
            {
                return new CompositeSiteMapBuilder(xmlSiteMapBuilder, visitingSiteMapBuilder);
            }
        }

        private ISiteMapBuilder ResolveXmlSiteMapBuilder(string siteMapFile, IEnumerable<string> attributesToIgnore)
        {
            return new XmlSiteMapBuilder(
                new FileXmlSource(this.absoluteFileName),
                new SiteMapXmlReservedAttributeNameProvider(attributesToIgnore), 
                this.nodeKeyGenerator, 
                this.dynamicNodeBuilder, 
                this.siteMapNodeFactory,
                this.siteMapXmlNameProvider);
        }

        private ISiteMapBuilder ResolveReflectionSiteMapBuilder(IEnumerable<string> includeAssemblies, IEnumerable<string> excludeAssemblies, IEnumerable<string> attributesToIgnore)
        {
            return new ReflectionSiteMapBuilder(
                includeAssemblies,
                excludeAssemblies,
                new SiteMapXmlReservedAttributeNameProvider(attributesToIgnore),
                this.nodeKeyGenerator,
                this.dynamicNodeBuilder,
                this.siteMapNodeFactory,
                this.siteMapCacheKeyGenerator);
        }

        private ISiteMapBuilder ResolveVisitingSiteMapBuilder()
        {
            return new VisitingSiteMapBuilder(
                new UrlResolvingSiteMapNodeVisitor());
        }

        private ICacheDetails ResolveCacheDetails(ConfigurationSettings settings)
        {
            
            return new CacheDetails(
                TimeSpan.FromMinutes(settings.CacheDuration),
                TimeSpan.MinValue,
#if NET35
                new AspNetFileCacheDependency(absoluteFileName)
#else
                new RuntimeFileCacheDependency(absoluteFileName)
#endif
                );
        }
    }
}
