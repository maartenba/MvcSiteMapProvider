using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using StructureMap;
using MvcMusicStore.Code.IoC;

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
            var diContainer = new StructureMapContainer(container);

            //var resolver = new Code.IoC.StructureMapResolver(container);

            //// Setup the container in a static member so it can be used
            //// to inject dependencies later.
            //MvcSiteMapProvider.IoC.DI.Container = resolver;

            


            


            // Configure Dependencies
            //container.Configure(x => x
            //    .For<System.Web.HttpContext>()
            //    .Use(HttpContext.Current)
            //);

            container.Configure(x => x
                .For<MvcSiteMapProvider.Web.Mvc.IMvcContextFactory>()
                .Use<MvcSiteMapProvider.Web.Mvc.MvcContextFactory>()
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


            // Pass in the global ControllerBuilder reference
            container.Configure(x => x
                .For<System.Web.Mvc.ControllerBuilder>()
                .Use(y => ControllerBuilder.Current)
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
                .For<MvcSiteMapProvider.Web.Mvc.IMvcResolverFactory>()
                .Use<MvcSiteMapProvider.Web.Mvc.MvcResolverFactory>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Reflection.IObjectCopier>()
                .Singleton()
                .Use<MvcSiteMapProvider.Reflection.ObjectCopier>()
            );

#if !MVC2
            // Configure default filter provider with one that provides filters
            // from the global filter collection.
            container.Configure(x => x
                .For<System.Web.Mvc.IFilterProvider>()
                .Singleton()
                .Use<MvcSiteMapProvider.Web.Mvc.Filters.FilterProvider>()
            );
#endif

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


            // Configure security
            container.Configure(x => x
                .For<MvcSiteMapProvider.Security.IAclModule>().Use<MvcSiteMapProvider.Security.CompositeAclModule>()
                .EnumerableOf<MvcSiteMapProvider.Security.IAclModule>().Contains(y =>
                {
                    y.Type<MvcSiteMapProvider.Security.AuthorizeAttributeAclModule>();
                    y.Type<MvcSiteMapProvider.Security.XmlRolesAclModule>();   
                }
            ));




            container.Configure(x => x
                .For<MvcSiteMapProvider.ISiteMapNodeFactory>()
                .Use<MvcSiteMapProvider.SiteMapNodeFactory>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.IDynamicNodeProviderStrategy>()
                .Singleton()
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
                .Singleton()
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
                .Singleton()
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



            // Setup cache
            container.Configure(x => x
                .For<MvcSiteMapProvider.Caching.ISiteMapCache>()
                .Use<MvcSiteMapProvider.Caching.AspNetSiteMapCache>()
            );

            // Configure the SiteMap Builder Sets
            container.Configure(x =>
                {

                    var xmlSource = x.For<MvcSiteMapProvider.Xml.IXmlSource>().Use<MvcSiteMapProvider.Xml.FileXmlSource>()
                        .Ctor<string>("xmlFileName").Is("~/Mvc.sitemap");

                    var builder =  x.For<MvcSiteMapProvider.Builder.ISiteMapBuilder>().Use<MvcSiteMapProvider.Builder.CompositeSiteMapBuilder>()
                        .EnumerableOf<MvcSiteMapProvider.Builder.ISiteMapBuilder>().Contains(y =>
                        {
                            y.Type<MvcSiteMapProvider.Builder.XmlSiteMapBuilder>()
                                .Ctor<IEnumerable<string>>("attributesToIgnore").Is(new string[0])
                                .Ctor<MvcSiteMapProvider.Xml.IXmlSource>().Is(xmlSource);
                            y.Type<MvcSiteMapProvider.Builder.ReflectionSiteMapBuilder>()
                                .Ctor<IEnumerable<string>>("includeAssemblies").Is(new string[0])
                                .Ctor<IEnumerable<string>>("excludeAssemblies").Is(new string[0]);
                            y.Type<MvcSiteMapProvider.Builder.VisitingSiteMapBuilder>();
                        });

                    var cacheDependency = x.For<MvcSiteMapProvider.Caching.ICacheDependency>()
                        .Use<MvcSiteMapProvider.Caching.AspNetFileCacheDependency>()
                        .Ctor<string>("fileName")
                        .Is(System.Web.Hosting.HostingEnvironment.MapPath("~/Mvc.sitemap"));

                    var cacheDetails = x.For<MvcSiteMapProvider.Caching.ICacheDetails>().Use<MvcSiteMapProvider.Caching.CacheDetails>()
                        .Ctor<TimeSpan>("absoluteCacheExpiration").Is(TimeSpan.FromMinutes(5))
                        .Ctor<TimeSpan>("slidingCacheExpiration").Is(TimeSpan.MinValue)
                        .Ctor<MvcSiteMapProvider.Caching.ICacheDependency>().Is(cacheDependency);

                    x.For<MvcSiteMapProvider.Builder.ISiteMapBuilderSetStrategy>().Use<MvcSiteMapProvider.Builder.SiteMapBuilderSetStrategy>()
                        .EnumerableOf<MvcSiteMapProvider.Builder.ISiteMapBuilderSet>().Contains(y =>
                        {
                            y.Type<MvcSiteMapProvider.Builder.SiteMapBuilderSet>()
                                .Ctor<string>("instanceName").Is("default")
                                .Ctor<MvcSiteMapProvider.Builder.ISiteMapBuilder>().Is(builder)
                                .Ctor<MvcSiteMapProvider.Caching.ICacheDetails>().Is(cacheDetails);
                        });
                }
            );


            container.Configure(x => x
                .For<MvcSiteMapProvider.Xml.ISiteMapXmlValidator>()
                .Use<MvcSiteMapProvider.Xml.SiteMapXmlValidator>()
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




            // Fix for controllers - need to ensure they are transient scoped or
            // there will be problems.
            //http://code-inside.de/blog/2011/01/18/fix-a-single-instance-of-controller-foocontroller-cannot-be-used-to-handle-multiple-requests-mvc3/
            container.Configure(x => x
                .Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssemblyContainingType<MvcSiteMapProvider.Web.Mvc.XmlSiteMapController>();
                    scan.AddAllTypesOf<IController>();
                    scan.Include(t => typeof(IController).IsAssignableFrom(t));
                    scan.Convention<TransientConvention>();
                })
            );

            // Reconfigure MVC to use StructureMap for DI
            ControllerBuilder.Current.SetControllerFactory(
                new InjectableControllerFactory(diContainer)
            );


            // Configure the static instance of the SiteMapLoader

            // TODO: Evaluate whether we need a SiteMapLoaderFactory
            var loader = container.GetInstance<MvcSiteMapProvider.Loader.ISiteMapLoader>();

            MvcSiteMapProvider.SiteMaps.Loader = loader;

            var validator = container.GetInstance<MvcSiteMapProvider.Xml.ISiteMapXmlValidator>();
            // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider
            validator.ValidateXml(System.Web.Hosting.HostingEnvironment.MapPath("~/Mvc.sitemap"));
        }


        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var test = "";
        }


    }
}