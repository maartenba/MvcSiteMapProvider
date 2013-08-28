using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Hosting;
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
using Ninject;
using Ninject.Modules;


namespace DI.Ninject.Modules
{
    public class MvcSiteMapProviderModule
        : NinjectModule
    {
        public override void Load()
        {
            bool securityTrimmingEnabled = false;
            bool enableLocalization = true;
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
                (interfaceType, implementationType) => this.Kernel.Bind(interfaceType).To(implementationType).InSingletonScope(),
                new Assembly[] { siteMapProviderAssembly },
                allAssemblies,
                excludeTypes,
                string.Empty);

            // Multiple implementations of strategy based extension points
            CommonConventions.RegisterAllImplementationsOfInterface(
                (interfaceType, implementationType) => this.Kernel.Bind(interfaceType).To(implementationType).InSingletonScope(),
                multipleImplementationTypes,
                allAssemblies,
                excludeTypes,
                "^Composite");

            this.Kernel.Bind<ISiteMapNodeVisibilityProviderStrategy>().To<SiteMapNodeVisibilityProviderStrategy>()
                .WithConstructorArgument("defaultProviderName", string.Empty);

            this.Kernel.Bind<ControllerBuilder>().ToConstant(ControllerBuilder.Current);
            this.Kernel.Bind<IControllerBuilder>().To<ControllerBuilderAdaptor>();
            this.Kernel.Bind<IBuildManager>().To<BuildManagerAdaptor>();

            // Configure Security
            this.Kernel.Bind<AuthorizeAttributeAclModule>().ToSelf();
            this.Kernel.Bind<XmlRolesAclModule>().ToSelf();
            this.Kernel.Bind<IAclModule>().To<CompositeAclModule>()
                .WithConstructorArgument("aclModules", 
                    new IAclModule[] { 
                        this.Kernel.Get<AuthorizeAttributeAclModule>(), 
                        this.Kernel.Get<XmlRolesAclModule>() 
                    });


            // Setup cache
#if NET35
            this.Kernel.Bind(typeof(ICacheProvider<>)).To(typeof(AspNetCacheProvider<>));
            this.Kernel.Bind<ICacheDependency>().To<AspNetFileCacheDependency>().Named("cacheDependency1")
                .WithConstructorArgument("fileName", absoluteFileName);
#else
            this.Kernel.Bind<System.Runtime.Caching.ObjectCache>()
                .ToConstant<System.Runtime.Caching.ObjectCache>(System.Runtime.Caching.MemoryCache.Default);
            this.Kernel.Bind(typeof(ICacheProvider<>)).To(typeof(RuntimeCacheProvider<>));
            this.Kernel.Bind<ICacheDependency>().To<RuntimeFileCacheDependency>().Named("cacheDependency1")
                .WithConstructorArgument("fileName", absoluteFileName);
#endif
            this.Kernel.Bind<ICacheDetails>().To<CacheDetails>().Named("cacheDetails1")
                .WithConstructorArgument("absoluteCacheExpiration", absoluteCacheExpiration)
                .WithConstructorArgument("slidingCacheExpiration", TimeSpan.MinValue)
                .WithConstructorArgument("cacheDependency", this.Kernel.Get<ICacheDependency>("cacheDependency1"));


            // Configure the visitors
            this.Kernel.Bind<ISiteMapNodeVisitor>().To<UrlResolvingSiteMapNodeVisitor>();

            // Register the sitemap builder
            this.Kernel.Bind<IXmlSource>().To<FileXmlSource>().Named("xmlBuilderXmlSource")
                .WithConstructorArgument("fileName", absoluteFileName);
            this.Kernel.Bind<ISiteMapXmlReservedAttributeNameProvider>().To<SiteMapXmlReservedAttributeNameProvider>().Named("xmlBuilderReservedAttributeNameProvider")
                .WithConstructorArgument("attributesToIgnore", new string[0]);

            // Xml Builder
            this.Kernel.Bind<ISiteMapBuilder>().To<XmlSiteMapBuilder>().Named("xmlSiteMapBuilder")
                .WithConstructorArgument("reservedAttributeNameProvider", this.Kernel.Get<ISiteMapXmlReservedAttributeNameProvider>("xmlBuilderReservedAttributeNameProvider"))
                .WithConstructorArgument("xmlSource", this.Kernel.Get<IXmlSource>("xmlBuilderXmlSource"));

            

            // Reflection Builder
            this.Kernel.Bind<ISiteMapBuilder>().To<ReflectionSiteMapBuilder>().Named("reflectionSiteMapBuilder")
                .WithConstructorArgument("includeAssemblies", includeAssembliesForScan)
                .WithConstructorArgument("excludeAssemblies", new string[0]);

            // Visiting Builder
            this.Kernel.Bind<ISiteMapBuilder>().To<VisitingSiteMapBuilder>().Named("visitingSiteMapBuilder");


            // Composite builder
            this.Kernel.Bind<ISiteMapBuilder>().To<CompositeSiteMapBuilder>().Named("compositeSiteMapBuilder")
                .WithConstructorArgument("siteMapBuilders", 
                    new ISiteMapBuilder[] { 
                        this.Kernel.Get<ISiteMapBuilder>("xmlSiteMapBuilder"),
                        this.Kernel.Get<ISiteMapBuilder>("reflectionSiteMapBuilder"),
                        this.Kernel.Get<ISiteMapBuilder>("visitingSiteMapBuilder")
                    });


            // Configure the builder sets
            this.Kernel.Bind<ISiteMapBuilderSet>().To<SiteMapBuilderSet>().Named("siteMapBuilderSet1")
                .WithConstructorArgument("instanceName", "default")
                .WithConstructorArgument("securityTrimmingEnabled", securityTrimmingEnabled)
                .WithConstructorArgument("enableLocalization", enableLocalization)
                .WithConstructorArgument("siteMapBuilder", this.Kernel.Get<ISiteMapBuilder>("compositeSiteMapBuilder"))
                .WithConstructorArgument("cacheDetails", this.Kernel.Get<ICacheDetails>("cacheDetails1"));

            this.Kernel.Bind<ISiteMapBuilderSetStrategy>().To<SiteMapBuilderSetStrategy>()
                .WithConstructorArgument("siteMapBuilderSets",
                    new ISiteMapBuilderSet[] {
                        this.Kernel.Get<ISiteMapBuilderSet>("siteMapBuilderSet1")
                    });
        }
    }
}
