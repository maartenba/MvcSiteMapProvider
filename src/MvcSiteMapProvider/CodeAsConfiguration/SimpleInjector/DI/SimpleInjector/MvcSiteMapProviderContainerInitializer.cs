﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Visitor;
using MvcSiteMapProvider.Web.Compilation;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.Mvc.Filters;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Xml;
using SimpleInjector;

namespace DI.SimpleInjector
{
    public static class MvcSiteMapProviderContainerInitializer
    {
        public static void SetUp(Container container)
        {
            string absoluteFileName = HostingEnvironment.MapPath("~/Mvc.sitemap");
            TimeSpan absoluteCacheExpiration = TimeSpan.FromMinutes(5);
            string[] includeAssembliesForScan = new string[] { "$AssemblyName$" }; 

#if false // not used
            // Extension to allow resolution of arrays by GetAllInstances (natively based on IEnumerable).
            // source from: https://simpleinjector.codeplex.com/wikipage?title=CollectionRegistrationExtensions
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
#endif

            var currentAssembly = typeof(MvcSiteMapProviderContainerInitializer).Assembly;
            var siteMapProviderAssembly = typeof(SiteMaps).Assembly;
            var allAssemblies = new Assembly[] { currentAssembly, siteMapProviderAssembly };
            var excludeTypes = new Type[]
                {
                    typeof (SiteMapNodeVisibilityProviderStrategy),
                    typeof (SiteMapXmlReservedAttributeNameProvider),
                    typeof (SiteMapBuilderSetStrategy),
                    typeof (ControllerTypeResolverFactory),

                    // Added 2013-06-28 by eric-b to avoid default singleton registration:
                    typeof(XmlSiteMapController),

                    // Added 2013-06-28 by eric-b for SimpleInjector.Verify method:
                    typeof(PreservedRouteParameterCollection),
                    typeof(MvcResolver), 
                    typeof(MvcSiteMapProvider.SiteMap), 
                    typeof(MetaRobotsValueCollection), 
                    typeof(RoleCollection), 
                    typeof(SiteMapPluginProvider), 
                    typeof(ControllerTypeResolver),
                    typeof(RouteValueDictionary), 
                    typeof(AttributeDictionary)
                };
            var multipleImplementationTypes = new Type[]
                {
                    typeof (ISiteMapNodeUrlResolver),
                    typeof (ISiteMapNodeVisibilityProvider),
                    typeof (IDynamicNodeProvider)
                };

            Dictionary<Type, List<Type>> implementations = new Dictionary<Type, List<Type>>();


            HashSet<Type> autoRegistered = new HashSet<Type>();
            foreach (var type in allAssemblies
                .SelectMany(t => t.GetExportedTypes())
                .Where(t => !t.IsInterface && !t.IsAbstract && !t.IsGenericType && !excludeTypes.Contains(t)))
            {
                var typeName = type.Name;
                var typeInterface = type.GetInterfaces().FirstOrDefault(t => t.Assembly == siteMapProviderAssembly && !t.IsGenericType);
                if (typeInterface != null
                    && type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                    .Any(t => t.GetParameters().Select(p => p.ParameterType).All(p => (p.IsInterface || p.IsClass) && !p.IsArray && p != typeof(string)))) // ignores ctors with value/string parameters or arrays (for composite services)
                {
                    List<Type> impl;
                    if (!implementations.TryGetValue(typeInterface, out impl))
                    {
                        impl = new List<Type>();
                        implementations.Add(typeInterface, impl);
                    }
                    impl.Add(type);
                }
            }


            foreach (var implementation in implementations)
            {
                var typeInterface = implementation.Key;
                var typeImpl = implementation.Value.First();
                if (autoRegistered.Contains(typeInterface))
                    continue;
                if (implementation.Value.Count == 1 
                    && typeInterface.Name == "I" + typeImpl.Name 
                    && !multipleImplementationTypes.Contains(typeInterface))
                {
                    // Single implementations of interface with matching name (minus the "I").
                    System.Diagnostics.Debug.WriteLine("Auto registration of {1} : {0}", typeInterface.Name, typeImpl.Name);
                    container.RegisterSingle(typeInterface, typeImpl);
                    autoRegistered.Add(typeInterface);
                }
                else if (multipleImplementationTypes.Contains(typeInterface))
                {
                    // Multiple implementations of strategy/composite based extension points
                    System.Diagnostics.Debug.WriteLine("Auto multiple registration of {1} : {0}", typeInterface.Name, string.Join(", ", implementation.Value.Select(t => t.Name)));
                    foreach (var t in implementation.Value)
                        container.RegisterSingle(t, t);
                    container.RegisterAll(typeInterface, implementation.Value);
                }
            }

            container.Register<XmlSiteMapController>();

            // Visibility Providers
            container.RegisterSingle<ISiteMapNodeVisibilityProviderStrategy>(() =>
                                                                       new SiteMapNodeVisibilityProviderStrategy(
                                                                           container.GetAllInstances
                                                                               <ISiteMapNodeVisibilityProvider>().
                                                                               ToArray(), string.Empty));

            // Pass in the global controllerBuilder reference
            container.RegisterSingle<ControllerBuilder>(() => ControllerBuilder.Current);

            container.RegisterSingle<IControllerBuilder, ControllerBuilderAdaptor>();

            container.RegisterSingle<IBuildManager, BuildManagerAdaptor>();

            container.RegisterSingle<IControllerTypeResolverFactory>(() =>
                                                               new ControllerTypeResolverFactory(new string[0],
                                                                                                 container.GetInstance
                                                                                                     <IControllerBuilder
                                                                                                     >(),
                                                                                                 container.GetInstance
                                                                                                     <IBuildManager>()));

            // Configure Security

            container.RegisterSingle<AuthorizeAttributeAclModule>(() => new AuthorizeAttributeAclModule(
                                                                      container.GetInstance<IMvcContextFactory>(),
                                                                      container.GetInstance<IObjectCopier>(),
                                                                      container.GetInstance<IControllerDescriptorFactory>(),
                                                                      container.GetInstance<IControllerBuilder>(),
                                                                      container.GetInstance<IAuthorizeAttributeBuilder>(),
                                                                      container.GetInstance<IGlobalFilterProvider>()));

            container.RegisterAll<IAclModule>(typeof(AuthorizeAttributeAclModule), typeof(XmlRolesAclModule));
            container.RegisterSingle<IAclModule>(() => new CompositeAclModule(container.GetAllInstances<IAclModule>().ToArray()));

            // Setup cache


#if NET35
            container.RegisterSingle<ICacheProvider<ISiteMap>, AspNetCacheProvider<ISiteMap>>();
            container.RegisterSingle<ICacheDependency>(() => new AspNetFileCacheDependency(absoluteFileName));
#else
            container.RegisterSingle<System.Runtime.Caching.ObjectCache>(() => System.Runtime.Caching.MemoryCache.Default);
            container.RegisterSingle<ICacheProvider<ISiteMap>, RuntimeCacheProvider<ISiteMap>>();
            container.RegisterSingle<ICacheDependency>(() => new RuntimeFileCacheDependency(absoluteFileName));
#endif
            container.RegisterSingle<ICacheDetails>(() => new CacheDetails(absoluteCacheExpiration, TimeSpan.MinValue, container.GetInstance<ICacheDependency>()));

            // Configure the visitors
            container.RegisterSingle<ISiteMapNodeVisitor, UrlResolvingSiteMapNodeVisitor>();


            // Register the sitemap builder
            container.RegisterSingle<ISiteMapXmlReservedAttributeNameProvider>(
                () => new SiteMapXmlReservedAttributeNameProvider(new string[0]));

            container.RegisterSingle<IXmlSource>(() => new FileXmlSource(absoluteFileName));



            container.RegisterSingle<IDynamicNodeProviderStrategy>(() => new DynamicNodeProviderStrategy(container.GetAllInstances<IDynamicNodeProvider>().ToArray()));  // IDynamicNodeProvider est typiquement implémenté par l'application finale.
            container.RegisterSingle<ISiteMapNodeUrlResolverStrategy>(() => new SiteMapNodeUrlResolverStrategy(container.GetAllInstances<ISiteMapNodeUrlResolver>().ToArray()));


            container.RegisterSingle<XmlSiteMapBuilder>(() =>
                                                  new XmlSiteMapBuilder(
                                                      container.GetInstance<IXmlSource>(),
                                                      container.GetInstance<ISiteMapXmlReservedAttributeNameProvider>(),
                                                      container.GetInstance<INodeKeyGenerator>(),
                                                      container.GetInstance<IDynamicNodeBuilder>(),
                                                      container.GetInstance<ISiteMapNodeFactory>(),
                                                      container.GetInstance<ISiteMapXmlNameProvider>()
                                                      ));

            container.RegisterSingle<ReflectionSiteMapBuilder>(() =>
                                                         new ReflectionSiteMapBuilder(
                                                             includeAssembliesForScan,
                                                             new string[0],
                                                             container.GetInstance
                                                                 <ISiteMapXmlReservedAttributeNameProvider>(),
                                                             container.GetInstance<INodeKeyGenerator>(),
                                                             container.GetInstance<IDynamicNodeBuilder>(),
                                                             container.GetInstance<ISiteMapNodeFactory>(),
                                                             container.GetInstance<ISiteMapCacheKeyGenerator>()
                                                             ));

            container.RegisterAll<ISiteMapBuilderSet>(ResolveISiteMapBuilderSets(container));
            container.RegisterSingle<ISiteMapBuilderSetStrategy>(() => new SiteMapBuilderSetStrategy(container.GetAllInstances<ISiteMapBuilderSet>().ToArray()));


            container.RegisterSingle<VisitingSiteMapBuilder>();

        }

        private static IEnumerable<ISiteMapBuilderSet> ResolveISiteMapBuilderSets(Container container)
        {
            yield return new SiteMapBuilderSet(
                "default",
                new CompositeSiteMapBuilder(
                    container.GetInstance<XmlSiteMapBuilder>(),
                    container.GetInstance<ReflectionSiteMapBuilder>(),
                    container.GetInstance<VisitingSiteMapBuilder>()),
                container.GetInstance<ICacheDetails>());
        }

#if false // not used
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
#endif
    }
}
