using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;
using SimpleInjector;
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
#if Demo
            // Settings for MvcMusicStore demo: don't copy into your project
            bool securityTrimmingEnabled = true;
            string[] includeAssembliesForScan = new string[] { "Mvc Music Store" };
#else
            bool securityTrimmingEnabled = false;
            string[] includeAssembliesForScan = new string[] { "$AssemblyName$" };
#endif

            // Extension to allow resolution of arrays by GetAllInstances (natively based on IEnumerable).
            // source from: https://simpleinjector.codeplex.com/wikipage?title=CollectionRegistrationExtensions
            container.AllowToResolveArraysAndLists();

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
                typeof(IMvcContextFactory)
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
                (interfaceType, implementationType) => container.Register(interfaceType, implementationType),
                new Assembly[] { siteMapProviderAssembly },
                allAssemblies,
                excludeTypes,
                string.Empty);

            // Multiple implementations of strategy based extension points (and not decorated with [ExcludeFromAutoRegistrationAttribute]).
            CommonConventions.RegisterAllImplementationsOfInterfaceSingle(
                (interfaceType, implementationTypes) => container.RegisterCollection(interfaceType, implementationTypes),
                multipleImplementationTypes,
                allAssemblies,
                excludeTypes,
                string.Empty);

            container.RegisterMvcController<XmlSiteMapController>();

            container.Register<IMvcContextFactory, MvcContextFactory>(Lifestyle.Singleton);

            // Visibility Providers
            container.Register<ISiteMapNodeVisibilityProviderStrategy>(() =>
                new SiteMapNodeVisibilityProviderStrategy(
                    container.GetAllInstances<ISiteMapNodeVisibilityProvider>().ToArray(), string.Empty), 
                Lifestyle.Singleton);

            // Pass in the global controllerBuilder reference
            container.Register<ControllerBuilder>(() => ControllerBuilder.Current, Lifestyle.Singleton);

            container.Register<IControllerTypeResolverFactory>(() =>
                new ControllerTypeResolverFactory(
                    new string[0],
                    container.GetInstance<IControllerBuilder>(),
                    container.GetInstance<IBuildManager>()),
                Lifestyle.Singleton);

            // Configure Security
            container.RegisterCollection(typeof(IAclModule), new Type[] { typeof(AuthorizeAttributeAclModule), typeof(XmlRolesAclModule) });
            container.Register<IAclModule>(() => new CompositeAclModule(container.GetAllInstances<IAclModule>().ToArray()), Lifestyle.Singleton);

            // Setup cache
#if NET35
            container.RegisterOpenGeneric(typeof(ICacheProvider<>), typeof(AspNetCacheProvider<>), Lifestyle.Singleton);
            container.Register<ICacheDependency>(() => new AspNetFileCacheDependency(absoluteFileName), Lifestyle.Singleton);
#else
            container.Register<System.Runtime.Caching.ObjectCache>(() => System.Runtime.Caching.MemoryCache.Default, Lifestyle.Singleton);
            container.RegisterOpenGeneric(typeof(ICacheProvider<>), typeof(RuntimeCacheProvider<>), Lifestyle.Singleton);
            container.Register<ICacheDependency>(() => new RuntimeFileCacheDependency(absoluteFileName), Lifestyle.Singleton);
#endif
            container.Register<ICacheDetails>(() => new CacheDetails(absoluteCacheExpiration, TimeSpan.MinValue, container.GetInstance<ICacheDependency>()), Lifestyle.Singleton);

            // Configure the visitors
            container.Register<ISiteMapNodeVisitor, UrlResolvingSiteMapNodeVisitor>(Lifestyle.Singleton);

            // Prepare for the sitemap node providers
            container.Register<IReservedAttributeNameProvider>(() => new ReservedAttributeNameProvider(new string[0]), Lifestyle.Singleton);
            container.Register<IXmlSource>(() => new FileXmlSource(absoluteFileName), Lifestyle.Singleton);

            // Register the sitemap node providers
            container.Register<XmlSiteMapNodeProvider>(() => container.GetInstance<XmlSiteMapNodeProviderFactory>()
                .Create(container.GetInstance<IXmlSource>()), 
                Lifestyle.Singleton);
            container.Register<ReflectionSiteMapNodeProvider>(() => container.GetInstance<ReflectionSiteMapNodeProviderFactory>()
                .Create(includeAssembliesForScan), 
                Lifestyle.Singleton);

            // Register the sitemap builders
            container.Register<ISiteMapBuilder>(() => container.GetInstance<SiteMapBuilderFactory>()
                .Create(new CompositeSiteMapNodeProvider(container.GetInstance<XmlSiteMapNodeProvider>(), container.GetInstance<ReflectionSiteMapNodeProvider>())), 
                Lifestyle.Singleton);

            container.RegisterCollection<ISiteMapBuilderSet>(
                ResolveISiteMapBuilderSets(container, securityTrimmingEnabled, enableLocalization, visibilityAffectsDescendants, useTitleIfDescriptionNotProvided));
            container.Register<ISiteMapBuilderSetStrategy>(() => new SiteMapBuilderSetStrategy(container.GetAllInstances<ISiteMapBuilderSet>().ToArray()), 
                Lifestyle.Singleton);
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
    }

    public static class ContainerExtensions
    {
        public static void AllowToResolveArraysAndLists(this Container container)
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

        // Extension methods for cross-version support of Simple Injector 2.x and 3.x.

        // This will succeed on 2.x and will be bypassed on 3.x
        public static void RegisterCollection(this Container container, Type serviceType, IEnumerable<Type> serviceTypes)
        {
            // container.RegisterAll(serviceType, serviceTypes);
            var method = container.GetType().GetMethod(
                "RegisterAll",
                BindingFlags.Instance | BindingFlags.Public,
                null,
                new Type[] { typeof(Type), typeof(IEnumerable<Type>) },
                null);

            if (method != null)
            {
                method.Invoke(container, new object[] { serviceType, serviceTypes });
            }
        }

        // This will succeed on 2.x and will be bypassed on 3.x
        public static void RegisterCollection<TService>(this Container container, IEnumerable<TService> containerUncontrolledCollection) where TService : class
        {
            // container.RegisterAll(containerUncontrolledCollection);
            var method = container.GetType().GetMethods()
                .Where(mi => mi.Name == "RegisterAll")
                .Select(mi => new { M = mi, P = mi.GetParameters(), A = mi.GetGenericArguments() })
                .Where(x => x.A.Length == 1
                    && x.P.Length == 1
                    && x.P[0].Name == "collection")
                .Select(x => x.M)
                .FirstOrDefault();

            if (method != null)
            {
                var genericMethod = method.MakeGenericMethod(new Type[] { typeof(TService) });
                genericMethod.Invoke(container, new object[] { containerUncontrolledCollection });
            }
        }

        // This will work on both 2.x and 3.x. On 2.x this will override the default implementation because it is in the same namespace as the caller.
        public static void RegisterOpenGeneric(this Container container, Type openGenericServiceType, Type openGenericImplementation, Lifestyle lifestyle)
        {
            bool isSimpleInjector2 = false;
            var openGenericExtensionType = container.GetType().Assembly.GetType("SimpleInjector.Extensions.OpenGenericRegistrationExtensions");
            if (openGenericExtensionType != null)
            {
                // Attempt to find the method to invoke
                // OpenGenericRegistrationExtensions.RegisterOpenGeneric(container, openGenericServiceType, openGenericImplementation, lifestyle);
                var method = openGenericExtensionType.GetMethod(
                    "RegisterOpenGeneric",
                    BindingFlags.Static | BindingFlags.Public,
                    null,
                    new Type[] { typeof(Container), typeof(Type), typeof(Type), typeof(Lifestyle) },
                    null);

                if (method != null && !Attribute.IsDefined(method, typeof(ObsoleteAttribute)))
                {
                    // This is SimpleInjector 2 - Invoke the method
                    isSimpleInjector2 = true;
                    method.Invoke(null, new object[] { container, openGenericServiceType, openGenericImplementation, lifestyle });
                }
            }

            if (!isSimpleInjector2)
            {
                container.Register(openGenericServiceType, openGenericImplementation, lifestyle);
            }
        }

        public static void RegisterMvcController<TService>(this Container container) where TService: IController
        {
            var registration = Lifestyle.Transient.CreateRegistration(typeof(IController), typeof(TService), container);
            container.AddRegistration(typeof(TService), registration);

            // This will run if using SimpleInjector 3.x
            Type diagnosticTypeType = container.GetType().Assembly.GetType("SimpleInjector.Diagnostics.DiagnosticType");
            if (diagnosticTypeType != null)
            {
                var method = registration.GetType().GetMethod(
                    "SuppressDiagnosticWarning",
                    BindingFlags.Instance | BindingFlags.Public,
                    null,
                    new Type[] { diagnosticTypeType, typeof(string) },
                    null);

                if (method != null)
                {
                    object disposableTransientComponent = Enum.Parse(diagnosticTypeType, "DisposableTransientComponent", true);

                    method.Invoke(registration, new object[] { 
                    disposableTransientComponent, 
                    "MVC's DefaultControllerFactory disposes the controller when the web request ends." });
                }
            }
        }
    }
}
