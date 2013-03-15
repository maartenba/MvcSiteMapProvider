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
            this.mvcContextFactory = new MvcContextFactory();
            this.siteMapCache = new AspNetSiteMapCache(this.mvcContextFactory);
            this.requestCache = this.mvcContextFactory.GetRequestCache();
            this.urlPath = new UrlPath(this.mvcContextFactory);
            this.siteMapCacheKeyGenerator = new SiteMapCacheKeyGenerator(this.mvcContextFactory);
            this.siteMapCacheKeyToBuilderSetMapper = new SiteMapCacheKeyToBuilderSetMapper();
            this.nodeKeyGenerator = new NodeKeyGenerator();
            var siteMapNodeFactoryContainer = new SiteMapNodeFactoryContainer(settings, this.mvcContextFactory, this.urlPath);
            this.siteMapNodeFactory = siteMapNodeFactoryContainer.ResolveSiteMapNodeFactory();
            this.dynamicNodeBuilder = new DynamicNodeBuilder(this.nodeKeyGenerator, this.siteMapNodeFactory);
            this.siteMapBuiderSetStrategy = this.ResolveSiteMapBuilderSetStrategy(settings);
            var siteMapFactoryContainer = new SiteMapFactoryContainer(this.mvcContextFactory, this.urlPath);
            this.siteMapFactory = siteMapFactoryContainer.ResolveSiteMapFactory();
            this.siteMapCreator = new SiteMapCreator(this.siteMapCacheKeyToBuilderSetMapper, this.siteMapBuiderSetStrategy, this.siteMapFactory);
        }

        private readonly IMvcContextFactory mvcContextFactory;
        private readonly ISiteMapCache siteMapCache;
        private readonly IRequestCache requestCache;
        private readonly IUrlPath urlPath;
        private readonly ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator;
        private readonly ISiteMapCacheKeyToBuilderSetMapper siteMapCacheKeyToBuilderSetMapper;
        private readonly ISiteMapBuilderSetStrategy siteMapBuiderSetStrategy;
        private readonly INodeKeyGenerator nodeKeyGenerator;
        private readonly ISiteMapNodeFactory siteMapNodeFactory;
        private readonly IDynamicNodeBuilder dynamicNodeBuilder;
        private readonly ISiteMapFactory siteMapFactory;
        private readonly ISiteMapCreator siteMapCreator;
        
        // TODO: Take into consideration multi-threading and rework singleton lifestyle types in all
        // specialized DI containers.

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
                var reflectionSiteMapBuilder = this.ResolveReflectionSiteMapBuilder(settings.IncludeAssembliesForScan, settings.ExcludeAssembliesForScan);
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
                attributesToIgnore, 
                new FileXmlSource(siteMapFile), 
                this.nodeKeyGenerator, 
                this.dynamicNodeBuilder, 
                this.siteMapNodeFactory);
        }

        private ISiteMapBuilder ResolveReflectionSiteMapBuilder(IEnumerable<string> includeAssemblies, IEnumerable<string> excludeAssemblies)
        {
            return new ReflectionSiteMapBuilder(
                includeAssemblies,
                excludeAssemblies,
                this.nodeKeyGenerator,
                this.dynamicNodeBuilder,
                this.siteMapNodeFactory);
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
                new AspNetFileCacheDependency(HostingEnvironment.MapPath(settings.SiteMapFileName))
                );
        }
    }
}
