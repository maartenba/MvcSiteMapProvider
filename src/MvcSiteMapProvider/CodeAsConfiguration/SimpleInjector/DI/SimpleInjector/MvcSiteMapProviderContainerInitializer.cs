using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Extensions;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Visitor;
using MvcSiteMapProvider.Web.Compilation;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Xml;

namespace DI.SimpleInjector
{
    public static class MvcSiteMapProviderContainerInitializer
    {
        public static void SetUp(Container container)
        {
            bool enableLocalization = true;
            string absoluteFileName = HostingEnvironment.MapPath("~/Mvc.sitemap");
            TimeSpan absoluteCacheExpiration = TimeSpan.FromMinutes(5);
            bool visibilityAffectsDescendants = true;
            bool useTitleIfDescriptionNotProvided = true;
#if Demo // Settings for MvcMusicStore demo: don't copy into your project
            bool securityTrimmingEnabled = true;
            string[] includeAssembliesForScan = new string[] { "Mvc Music Store" };
#else
            bool securityTrimmingEnabled = false;
            string[] includeAssembliesForScan = new string[] { "$AssemblyName$" };
#endif

            // Extension to allow resolution of arrays by GetAllInstances (natively based on IEnumerable).
            // source from: https://simpleinjector.codeplex.com/wikipage?title=CollectionRegistrationExtensions
            AllowToResolveArraysAndLists(container);

            var currentAssembly = typeof(MvcSiteMapProviderContainerInitializer).Assembly;
            var siteMapProviderAssembly = typeof(SiteMaps).Assembly;
            var allAssemblies = new Assembly[] { currentAssembly, siteMapProviderAssembly };
            var excludeTypes = new Type[]
            {
                // Use this array to add types you wish to explicitly exclude from convention-based  
                // auto-registration. By default all types that either match I[TypeName] = [TypeName] or 
                // I[TypeName] = [TypeName]Adapter will be automatically wired up as long as they don't 
                // have the [ExcludeFromAutoRegistrationAttribute].
                //
                // If you want to override a type that follows the convention, you should add the name 
                // of either the implementation name or the interface that it inherits to this list and 
                // add your manual registration code below. This will prevent duplicate registrations 
                // of the types from occurring. 

                // Example:
                // typeof(SiteMap),
                // typeof(SiteMapNodeVisibilityProviderStrategy)
            };
            var multipleImplementationTypes = new Type[]
            {
                typeof(ISiteMapNodeUrlResolver),
                typeof(ISiteMapNodeVisibilityProvider),
                typeof(IDynamicNodeProvider)
            };

            // Matching type name (I[TypeName] = [TypeName]) or matching type name + suffix Adapter (I[TypeName] = [TypeName]Adapter)
            // and not decorated with the [ExcludeFromAutoRegistrationAttribute].
            CommonConventions.RegisterDefaultConventions(
                (interfaceType, implementationType) => container.RegisterSingle(interfaceType, implementationType),
                new Assembly[] { siteMapProviderAssembly },
                allAssemblies,
                excludeTypes,
                string.Empty);

            // Multiple implementations of strategy based extension points (and not decorated with [ExcludeFromAutoRegistrationAttribute]).
            CommonConventions.RegisterAllImplementationsOfInterfaceSingle(
                (interfaceType, implementationTypes) => container.RegisterAll(interfaceType, implementationTypes),
                multipleImplementationTypes,
                allAssemblies,
                excludeTypes,
                string.Empty);

            container.Register<XmlSiteMapController>();

            // Visibility Providers
            container.RegisterSingle<ISiteMapNodeVisibilityProviderStrategy>(() =>
                new SiteMapNodeVisibilityProviderStrategy(
                    container.GetAllInstances<ISiteMapNodeVisibilityProvider>().ToArray(), string.Empty));

            // Pass in the global controllerBuilder reference
            container.RegisterSingle<ControllerBuilder>(() => ControllerBuilder.Current);

            container.RegisterSingle<IControllerTypeResolverFactory>(() =>
                new ControllerTypeResolverFactory(
                    new string[0],
                    container.GetInstance<IControllerBuilder>(),
                    container.GetInstance<IBuildManager>()));

            // Configure Security
            container.RegisterAll<IAclModule>(typeof(AuthorizeAttributeAclModule), typeof(XmlRolesAclModule));
            container.RegisterSingle<IAclModule>(() => new CompositeAclModule(container.GetAllInstances<IAclModule>().ToArray()));

            // Setup cache
#if NET35
            container.RegisterSingleOpenGeneric(typeof(ICacheProvider<>), typeof(AspNetCacheProvider<>));
            container.RegisterSingle<ICacheDependency>(() => new AspNetFileCacheDependency(absoluteFileName));
#else
            container.RegisterSingle<System.Runtime.Caching.ObjectCache>(() => System.Runtime.Caching.MemoryCache.Default);
            container.RegisterSingleOpenGeneric(typeof(ICacheProvider<>), typeof(RuntimeCacheProvider<>));
            container.RegisterSingle<ICacheDependency>(() => new RuntimeFileCacheDependency(absoluteFileName));
#endif
            container.RegisterSingle<ICacheDetails>(() => new CacheDetails(absoluteCacheExpiration, TimeSpan.MinValue, container.GetInstance<ICacheDependency>()));

            // Configure the visitors
            container.RegisterSingle<ISiteMapNodeVisitor, UrlResolvingSiteMapNodeVisitor>();

            // Prepare for the sitemap node providers
            container.RegisterSingle<IReservedAttributeNameProvider>(() => new ReservedAttributeNameProvider(new string[0]));
            container.RegisterSingle<IXmlSource>(() => new FileXmlSource(absoluteFileName));

            // Register the sitemap node providers
            container.RegisterSingle<XmlSiteMapNodeProvider>(() => container.GetInstance<XmlSiteMapNodeProviderFactory>()
                .Create(container.GetInstance<IXmlSource>()));
            container.RegisterSingle<ReflectionSiteMapNodeProvider>(() => container.GetInstance<ReflectionSiteMapNodeProviderFactory>()
                .Create(includeAssembliesForScan));

            // Register the sitemap builders
            container.RegisterSingle<ISiteMapBuilder>(() => container.GetInstance<SiteMapBuilderFactory>()
                .Create(new CompositeSiteMapNodeProvider(container.GetInstance<XmlSiteMapNodeProvider>(), container.GetInstance<ReflectionSiteMapNodeProvider>())));

            container.RegisterAll<ISiteMapBuilderSet>(
                ResolveISiteMapBuilderSets(container, securityTrimmingEnabled, enableLocalization, visibilityAffectsDescendants, useTitleIfDescriptionNotProvided));
            container.RegisterSingle<ISiteMapBuilderSetStrategy>(() => new SiteMapBuilderSetStrategy(container.GetAllInstances<ISiteMapBuilderSet>().ToArray()));
        }

        private static IEnumerable<ISiteMapBuilderSet> ResolveISiteMapBuilderSets(
            Container container, bool securityTrimmingEnabled, bool enableLocalization, bool visibilityAffectsDescendants, bool useTitleIfDescriptionNotProvided)
        {
            yield return new SiteMapBuilderSet(
                "default",
                securityTrimmingEnabled,
                enableLocalization,
                visibilityAffectsDescendants,
                useTitleIfDescriptionNotProvided,
                container.GetInstance<ISiteMapBuilder>(),
                container.GetInstance<ICacheDetails>());
        }

        private static void AllowToResolveArraysAndLists(Container container)
        {
            container.ResolveUnregisteredType += (sender, e) =>
            {
                var serviceType = e.UnregisteredServiceType;

                if (serviceType.IsArray)
                {
                    RegisterArrayResolver(e, container,
                        serviceType.GetElementType());
                }
                else if (serviceType.IsGenericType &&
                    serviceType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    RegisterArrayResolver(e, container,
                        serviceType.GetGenericArguments()[0]);
                }
            };
        }

        private static void RegisterArrayResolver(UnregisteredTypeEventArgs e, Container container, Type elementType)
        {
            var producer = container.GetRegistration(typeof(IEnumerable<>)
                .MakeGenericType(elementType));
            var enumerableExpression = producer.BuildExpression();
            var arrayMethod = typeof(Enumerable).GetMethod("ToArray")
                .MakeGenericMethod(elementType);
            var arrayExpression = Expression.Call(arrayMethod, enumerableExpression);
            e.Register(arrayExpression);
        }
    }
}
