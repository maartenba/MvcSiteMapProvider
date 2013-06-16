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
using Ninject.Extensions.Conventions;


namespace DI.Ninject.Modules
{
    public class MvcSiteMapProviderModule
        : NinjectModule
    {
        public override void Load()
        {
            var currentAssembly = typeof(MvcSiteMapProviderModule).Assembly;
            var allAssemblies = new Assembly[] { currentAssembly, typeof(SiteMaps).Assembly };

            string absoluteFileName = HostingEnvironment.MapPath("~/Mvc.sitemap");
            TimeSpan absoluteCacheExpiration = TimeSpan.FromMinutes(5);
            string[] includeAssembliesForScan = new string[] { "$AssemblyName$" };

            this.Kernel.Bind(scanner => scanner.From(allAssemblies)
                .Select(IsServiceType)
#if !MVC2
                .Excluding<FilterProvider>()
#endif
                .Excluding<SiteMapXmlReservedAttributeNameProvider>()
                .Excluding<SiteMapBuilderSetStrategy>()
                .Excluding<AuthorizeAttributeAclModule>()
                .Excluding<XmlRolesAclModule>()
                .Excluding<CompositeAclModule>()
                .Excluding<SiteMapNodeUrlResolver>()
                .Excluding<SiteMapNodeVisibilityProviderStrategy>()
                .BindDefaultInterface()
                .Configure(binding => binding.InSingletonScope())
                );

            // Register strategy classes

            // Url Resolvers
            this.BindAllImplementationsOf(typeof(ISiteMapNodeUrlResolver), allAssemblies);
            // Visibility Providers
            this.BindAllImplementationsOf(typeof(ISiteMapNodeVisibilityProvider), allAssemblies, typeof(CompositeSiteMapNodeVisibilityProvider));
            this.Kernel.Bind<ISiteMapNodeVisibilityProviderStrategy>().To<SiteMapNodeVisibilityProviderStrategy>()
                .WithConstructorArgument("defaultProviderName", string.Empty);
            // Dynamic Node Providers
            this.BindAllImplementationsOf(typeof(IDynamicNodeProvider), allAssemblies);
                
            this.Kernel.Bind<ControllerBuilder>().ToConstant(ControllerBuilder.Current);
            this.Kernel.Bind<IControllerBuilder>().To<ControllerBuilderAdaptor>();
            this.Kernel.Bind<IBuildManager>().To<BuildManagerAdaptor>();

#if !MVC2
            // Configure default filter provider with one that provides filters
            // from the global filter collection.
            this.Kernel.Bind<IFilterProvider>().To<FilterProvider>().InSingletonScope();
#endif

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
            this.Kernel.Bind<ISiteMapCache>().To<AspNetSiteMapCache>();
            this.Kernel.Bind<ICacheDependency>().To<AspNetFileCacheDependency>().Named("cacheDependency1")
                .WithConstructorArgument("fileName", absoluteFileName);
#else
            this.Kernel.Bind<System.Runtime.Caching.ObjectCache>().ToConstant<System.Runtime.Caching.ObjectCache>(System.Runtime.Caching.MemoryCache.Default);
            this.Kernel.Bind<ISiteMapCache>().To<RuntimeSiteMapCache>();
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
                .WithConstructorArgument("siteMapBuilder", this.Kernel.Get<ISiteMapBuilder>("compositeSiteMapBuilder"))
                .WithConstructorArgument("cacheDetails", this.Kernel.Get<ICacheDetails>("cacheDetails1"));

            this.Kernel.Bind<ISiteMapBuilderSetStrategy>().To<SiteMapBuilderSetStrategy>()
                .WithConstructorArgument("siteMapBuilderSets",
                    new ISiteMapBuilderSet[] {
                        this.Kernel.Get<ISiteMapBuilderSet>("siteMapBuilderSet1")
                    });
        }

        private bool IsServiceType(Type type)
        {
            return type.IsClass && type.GetInterfaces().Any(intface => intface.Name == "I" + type.Name);
        }

        private void BindAllImplementationsOf(Type type, Assembly[] assemblies, params Type[] excludingTypes)
        {
            List<Type> implementations = new List<Type>();

            foreach (var assembly in assemblies)
                implementations.AddRange(assembly.GetImplementationsOfInterface(type));

            foreach (var implementation in implementations)
            {
                if (!excludingTypes.Contains(implementation))
                {
                    this.Kernel.Bind(type).To(implementation);
                }
            }
        }

    }
}
