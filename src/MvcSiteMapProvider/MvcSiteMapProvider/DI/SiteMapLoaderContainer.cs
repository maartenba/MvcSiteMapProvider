﻿using System;
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
            this.siteMapNodeToParentRelationFactory = new SiteMapNodeToParentRelationFactory();
            this.nodeKeyGenerator = new NodeKeyGenerator();
            this.siteMapNodeFactory = siteMapNodeFactoryContainer.ResolveSiteMapNodeFactory();
            this.siteMapNodeCreatorFactory = this.ResolveSiteMapNodeCreatorFactory();
            this.dynamicSiteMapNodeBuilderFactory = new DynamicSiteMapNodeBuilderFactory(this.siteMapNodeCreatorFactory);
            this.siteMapHierarchyBuilder = new SiteMapHierarchyBuilder();
            this.siteMapXmlReservedAttributeNameProvider = new SiteMapXmlReservedAttributeNameProvider(settings.AttributesToIgnore);
            this.siteMapNodeHelperFactory = this.ResolveSiteMapNodeHelperFactory();
            this.siteMapNodeVisitor = this.ResolveSiteMapNodeVisitor(settings);
            this.siteMapXmlNameProvider = new SiteMapXmlNameProvider();
            this.attributeAssemblyProviderFactory = new AttributeAssemblyProviderFactory();
            this.mvcSiteMapNodeAttributeDefinitionProvider = new MvcSiteMapNodeAttributeDefinitionProvider();
            this.siteMapNodeProvider = this.ResolveSiteMapNodeProvider(settings);
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
        private readonly ISiteMapNodeToParentRelationFactory siteMapNodeToParentRelationFactory;
        private readonly ISiteMapNodeFactory siteMapNodeFactory;
        private readonly ISiteMapNodeCreatorFactory siteMapNodeCreatorFactory;
        private readonly ISiteMapNodeHelperFactory siteMapNodeHelperFactory;
        private readonly ISiteMapNodeVisitor siteMapNodeVisitor;
        private readonly ISiteMapNodeProvider siteMapNodeProvider;
        private readonly ISiteMapHierarchyBuilder siteMapHierarchyBuilder;
        private readonly IAttributeAssemblyProviderFactory attributeAssemblyProviderFactory;
        private readonly IMvcSiteMapNodeAttributeDefinitionProvider mvcSiteMapNodeAttributeDefinitionProvider;
        private readonly ISiteMapXmlNameProvider siteMapXmlNameProvider;
        private readonly ISiteMapXmlReservedAttributeNameProvider siteMapXmlReservedAttributeNameProvider;
        private readonly IDynamicSiteMapNodeBuilderFactory dynamicSiteMapNodeBuilderFactory;
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
                        settings.VisibilityAffectsDescendants,
                        this.ResolveSiteMapBuilder(settings),
                        this.ResolveCacheDetails(settings)
                        )
                    }
                );
        }

        private ISiteMapBuilder ResolveSiteMapBuilder(ConfigurationSettings settings)
        {
            return new SiteMapBuilder(
                this.siteMapNodeProvider,
                this.siteMapNodeVisitor,
                this.siteMapHierarchyBuilder,
                this.siteMapCacheKeyGenerator,
                this.siteMapNodeHelperFactory);
        }

        private ISiteMapNodeProvider ResolveSiteMapNodeProvider(ConfigurationSettings settings)
        {
            var providers = new List<ISiteMapNodeProvider>();
            if (settings.EnableSiteMapFile)
            {
                providers.Add(this.ResolveXmlSiteMapNodeProvider(settings.IncludeRootNodeFromSiteMapFile, settings.EnableSiteMapFileNestedDynamicNodeRecursion));
            }
            if (settings.ScanAssembliesForSiteMapNodes)
            {
                providers.Add(this.ResolveReflectionSiteMapNodeProvider(settings.IncludeAssembliesForScan, settings.ExcludeAssembliesForScan));
            }
            return new CompositeSiteMapNodeProvider(providers.ToArray());
        }

        private ISiteMapNodeProvider ResolveXmlSiteMapNodeProvider(bool includeRootNode, bool useNestedDynamicNodeRecursion)
        {
            return new XmlSiteMapNodeProvider(
                includeRootNode,
                useNestedDynamicNodeRecursion,
                new FileXmlSource(this.absoluteFileName),
                this.siteMapXmlNameProvider);
        }

        private ISiteMapNodeProvider ResolveReflectionSiteMapNodeProvider(IEnumerable<string> includeAssemblies, IEnumerable<string> excludeAssemblies)
        {
            return new ReflectionSiteMapNodeProvider(
                includeAssemblies,
                excludeAssemblies,
                this.attributeAssemblyProviderFactory,
                this.mvcSiteMapNodeAttributeDefinitionProvider);
        }

        private ISiteMapNodeVisitor ResolveSiteMapNodeVisitor(ConfigurationSettings settings)
        {
            if (settings.EnableResolvedUrlCaching)
            {
                return new UrlResolvingSiteMapNodeVisitor();
            }
            else
            {
                return new NullSiteMapNodeVisitor();
            }
        }

        private ISiteMapNodeCreatorFactory ResolveSiteMapNodeCreatorFactory()
        {
            return new SiteMapNodeCreatorFactory(
                this.siteMapNodeFactory, 
                this.nodeKeyGenerator, 
                this.siteMapNodeToParentRelationFactory);
        }
        
        private ISiteMapNodeHelperFactory ResolveSiteMapNodeHelperFactory()
        {
            return new SiteMapNodeHelperFactory(
                this.siteMapNodeCreatorFactory,
                this.dynamicSiteMapNodeBuilderFactory,
                this.siteMapXmlReservedAttributeNameProvider);
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
