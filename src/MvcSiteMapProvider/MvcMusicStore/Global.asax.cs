using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using StructureMap;

namespace MvcMusicStore
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Browse",                                                // Route name
                "Store/Browse/{genre}",                                  // URL with parameters
                new { controller = "Store", action = "Browse", id = UrlParameter.Optional }, // Parameter defaults
                new string[] { "MvcMusicStore.Controllers" }
            );

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },  // Parameter defaults
                new string[] { "MvcMusicStore.Controllers" }
            );
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new AuthorizeAttribute());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Register XmlSiteMapController
            XmlSiteMapController.RegisterRoutes(RouteTable.Routes);

            RegisterRoutes(RouteTable.Routes);
            RegisterGlobalFilters(System.Web.Mvc.GlobalFilters.Filters);


            // Create the DI container (for structuremap)
            var container = new Container();
            //var resolver = new Code.IoC.StructureMapResolver(container);

            //// Setup the container in a static member so it can be used
            //// to inject dependencies later.
            //MvcSiteMapProvider.IoC.DI.Container = resolver;


            // Configure Dependencies
            container.Configure(x => x
                .For<System.Web.HttpContext>()
                .Use(HttpContext.Current)
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Web.IHttpContextFactory>()
                .Use<MvcSiteMapProvider.Web.HttpContextFactory>()
            );


            container.Configure(x => x
                .For<MvcSiteMapProvider.ISiteMap>()
                .Use<MvcSiteMapProvider.RequestCacheableSiteMap>()
            );

            //// We create a new Setter Injection Policy that
            //// forces StructureMap to inject all public properties
            //// where the Property Type name equals 'ISiteMap'
            //container.Configure(
            //    x => x.SetAllProperties(p =>
            //        {
            //            p.TypeMatches(t => t.Name == "ISiteMap");
            //        }
            //    )
            //);

            //container.Configure(x => x
            //    .For<MvcSiteMapProvider.Security.IAclModule>()
            //    .Use<MvcSiteMapProvider.Security.CompositeAclModule>()
            //);




            // Pass in the global ControllerBuilder reference
            container.Configure(x => x
                .For<System.Web.Mvc.ControllerBuilder>()
                .Use(ControllerBuilder.Current)
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Web.Mvc.IControllerBuilder>()
                .Use<MvcSiteMapProvider.Web.Mvc.ControllerBuilderAdaptor>()
            );

            
            
            container.Configure(x => x
                .For<MvcSiteMapProvider.Web.Compilation.IBuildManager>()
                .Use<MvcSiteMapProvider.Web.Compilation.BuildManagerAdaptor>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Web.Mvc.IActionMethodParameterResolverFactory>()
                .Use<MvcSiteMapProvider.Web.Mvc.ActionMethodParameterResolverFactory>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Web.Mvc.IControllerTypeResolverFactory>()
                .Use<MvcSiteMapProvider.Web.Mvc.ControllerTypeResolverFactory>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Reflection.IObjectCopier>()
                .Singleton()
                .Use<MvcSiteMapProvider.Reflection.ObjectCopier>()
            );

            // Configure default filter provider with one that provides filters
            // from the global filter collection.
            container.Configure(x => x
                .For<System.Web.Mvc.IFilterProvider>()
                .Singleton()
                .Use<MvcSiteMapProvider.Web.Mvc.Filters.FilterProvider>()
            );

            // Pass in the global route collection
            container.Configure(x => x
                .For<System.Web.Routing.RouteCollection>()
                .Use(RouteTable.Routes)
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Web.Mvc.IControllerDescriptorFactory>()
                .Singleton()
                .Use<MvcSiteMapProvider.Web.Mvc.ControllerDescriptorFactory>()
            );



            var aclModules = new MvcSiteMapProvider.Security.CompositeAclModule(
                container.GetInstance<MvcSiteMapProvider.Security.AuthorizeAttributeAclModule>(),
                container.GetInstance<MvcSiteMapProvider.Security.XmlRolesAclModule>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Security.IAclModule>()
                .Use(aclModules)
            );



            container.Configure(x => x
                .For<MvcSiteMapProvider.ISiteMapNodeFactory>()
                .Use<MvcSiteMapProvider.SiteMapNodeFactory>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.IDynamicNodeProviderStrategy>()
                .Use<MvcSiteMapProvider.DynamicNodeProviderStrategy>()
            );

            // Get all types that implement IDynamicNodeProvider in an array
            container.Configure(x => x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssemblyContainingType<MvcSiteMapProvider.SiteMaps>();
                    scan.WithDefaultConventions();
                    scan.AddAllTypesOf<MvcSiteMapProvider.IDynamicNodeProvider>();
                }
            ));

            container.Configure(x => x
                .For<MvcSiteMapProvider.Web.UrlResolver.ISiteMapNodeUrlResolverStrategy>()
                .Use<MvcSiteMapProvider.Web.UrlResolver.SiteMapNodeUrlResolverStrategy>()
            );

            // Get all types that implement ISiteMapNodeVisibilityProvider in an array
            container.Configure(x => x.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AssemblyContainingType<MvcSiteMapProvider.SiteMaps>();
                scan.WithDefaultConventions();
                scan.AddAllTypesOf<MvcSiteMapProvider.ISiteMapNodeVisibilityProvider>();
            }
            ));

            container.Configure(x => x
                .For<MvcSiteMapProvider.ISiteMapNodeVisibilityProviderStrategy>()
                .Use<MvcSiteMapProvider.SiteMapNodeVisibilityProviderStrategy>()
            );

            // Get all types that implement ISiteMapNodeResolver in an array
            container.Configure(x => x.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AssemblyContainingType<MvcSiteMapProvider.SiteMaps>();
                scan.WithDefaultConventions();
                scan.AddAllTypesOf<MvcSiteMapProvider.Web.UrlResolver.ISiteMapNodeUrlResolver>();
            }
            ));
            

            container.Configure(x => x
                .For<MvcSiteMapProvider.Builder.INodeKeyGenerator>()
                .Singleton()
                .Use<MvcSiteMapProvider.Builder.NodeKeyGenerator>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Globalization.IExplicitResourceKeyParser>()
                .Singleton()
                .Use<MvcSiteMapProvider.Globalization.ExplicitResourceKeyParser>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Globalization.IStringLocalizer>()
                .Singleton()
                .Use<MvcSiteMapProvider.Globalization.StringLocalizer>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Globalization.ILocalizationService>()
                .Use<MvcSiteMapProvider.Globalization.LocalizationService>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Builder.IDynamicNodeBuilder>()
                .Singleton()
                .Use<MvcSiteMapProvider.Builder.DynamicNodeBuilder>()
            );

            container.Configure(x => x
               .For<MvcSiteMapProvider.Visitor.ISiteMapNodeVisitor>()
               .Use<MvcSiteMapProvider.Visitor.UrlResolvingSiteMapNodeVisitor>()
            );

            //container.Configure(x => x
            //    .For<MvcSiteMapProvider.Builder.XmlSiteMapBuilder>()
            //    .Use<MvcSiteMapProvider.Builder.XmlSiteMapBuilder>()
            //);

            //container.Configure(x => x
            //    .For<MvcSiteMapProvider.Builder.ReflectionSiteMapBuilder>()
            //    .Use<MvcSiteMapProvider.Builder.ReflectionSiteMapBuilder>()
            //);

            container.Configure(x => x
                .For<MvcSiteMapProvider.Builder.IXmlSiteMapBuilderFactory>()
                .Use<MvcSiteMapProvider.Builder.XmlSiteMapBuilderFactory>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Builder.IReflectionSiteMapBuilderFactory>()
                .Use<MvcSiteMapProvider.Builder.ReflectionSiteMapBuilderFactory>()
            );

            container.Configure(x => x
               .For<MvcSiteMapProvider.Builder.IVisitingSiteMapBuilderFactory>()
               .Use<MvcSiteMapProvider.Builder.VisitingSiteMapBuilderFactory>()
            );

            //container.Configure(x => x
            //    .For<System.Web.HttpContext>()
            //    .Use(HttpContext.Current)
            //);

            // Configure the SiteMap Builder Sets

            var xmlBuilderFactory = container.GetInstance<MvcSiteMapProvider.Builder.IXmlSiteMapBuilderFactory>();
            var xmlBuilder = xmlBuilderFactory.Create("~/Mvc.sitemap", new string[] { "" });


            var reflectionBuilderFactory = container.GetInstance<MvcSiteMapProvider.Builder.IReflectionSiteMapBuilderFactory>();
            var reflectionBuilder = reflectionBuilderFactory.Create(new string[] { "" }, new string[] { "" });


            var visitingBuilderFactory = container.GetInstance<MvcSiteMapProvider.Builder.IVisitingSiteMapBuilderFactory>();
            var visitingBuilder = visitingBuilderFactory.Create();


            //var builders = new MvcSiteMapProvider.Builder.CompositeSiteMapBuilder(
            //    container.GetInstance<MvcSiteMapProvider.Builder.XmlSiteMapBuilder>(),
            //    container.GetInstance<MvcSiteMapProvider.Builder.ReflectionSiteMapBuilder>()
            //);

            var builders = new MvcSiteMapProvider.Builder.CompositeSiteMapBuilder(
                xmlBuilder,
                reflectionBuilder,
                visitingBuilder
            );

            //var dependency = new System.Web.Caching.CacheDependency(System.Web.Hosting.HostingEnvironment.MapPath("~/Mvc.sitemap"));
            //var cacheDependency = new MvcSiteMapProvider.Caching.AspNetCacheDependency(dependency);

            // Specify caching details for the default builder set.
            container.Configure(x => x
               .For<MvcSiteMapProvider.Caching.ICacheDependencyFactory>()
               .Use<MvcSiteMapProvider.Caching.AspNetCacheDependencyFactory>()
               .Ctor<IEnumerable<string>>()
                .Is(new string[] { System.Web.Hosting.HostingEnvironment.MapPath("~/Mvc.sitemap") })
            );
            
            var cacheDependencyFactory = container.GetInstance<MvcSiteMapProvider.Caching.ICacheDependencyFactory>();
            var cacheDetails = new MvcSiteMapProvider.Caching.CacheDetails(TimeSpan.FromMinutes(5), TimeSpan.Zero, cacheDependencyFactory);

            var builderSet = new MvcSiteMapProvider.Builder.SiteMapBuilderSet("default", builders, cacheDetails);
            var builderSets = new MvcSiteMapProvider.Builder.ISiteMapBuilderSet[] { builderSet };

            //container.Configure(x => x
            //    .For<IEnumerable<MvcSiteMapProvider.Builder.ISiteMapBuilderSet>>()
            //    .Use(builderSets)
            //);

            container.Configure(x => x
                .For<MvcSiteMapProvider.Builder.ISiteMapBuilderSetStrategy>()
                .Use<MvcSiteMapProvider.Builder.SiteMapBuilderSetStrategy>()
                //.TheArrayOf<MvcSiteMapProvider.Builder.ISiteMapBuilderSet>()
                //.Contains(y => { y.IsThis(builderSet); })
                .Ctor<MvcSiteMapProvider.Builder.ISiteMapBuilderSet[]>()
                .Is(builderSets)
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Xml.ISiteMapXmlValidator>()
                .Use<MvcSiteMapProvider.Xml.SiteMapXmlValidator>()
            );

            var validator = container.GetInstance<MvcSiteMapProvider.Xml.ISiteMapXmlValidator>();
            // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider
            validator.ValidateXml(System.Web.Hosting.HostingEnvironment.MapPath("~/Mvc.sitemap"));
            

            container.Configure(x => x
                .For<MvcSiteMapProvider.Caching.ISiteMapCache>()
                .Use<MvcSiteMapProvider.Caching.AspNetSiteMapCache>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Caching.ISiteMapCacheKeyGenerator>()
                .Use<MvcSiteMapProvider.Caching.SiteMapCacheKeyGenerator>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.ISiteMapFactory>()
                .Use<MvcSiteMapProvider.SiteMapFactory>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Caching.ISiteMapCacheKeyToBuilderSetMapper>()
                .Use<MvcSiteMapProvider.Caching.SiteMapCacheKeyToBuilderSetMapper>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Loader.ISiteMapLoaderFactory>()
                .Use<MvcSiteMapProvider.Loader.SiteMapLoaderFactory>()
            );


            // Configure the static instance of the SiteMapLoader
            //var loaderFactory = container.GetInstance<MvcSiteMapProvider.Loader.ISiteMapLoaderFactory>();
            //var loader = loaderFactory.Create();

            // TODO: Evaluate whether we need a SiteMapLoaderFactory
            var loader = container.GetInstance<MvcSiteMapProvider.Loader.ISiteMapLoader>();

            MvcSiteMapProvider.SiteMaps.Loader = loader;
        }


        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var test = "";
        }


    }
}