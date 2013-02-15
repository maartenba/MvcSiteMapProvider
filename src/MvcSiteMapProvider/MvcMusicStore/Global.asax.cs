using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Core.Mvc;
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

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Register XmlSiteMapController
            XmlSiteMapController.RegisterRoutes(RouteTable.Routes);

            RegisterRoutes(RouteTable.Routes);


            // Create the DI container (for structuremap)
            var container = new Container();
            //var resolver = new Code.IoC.StructureMapResolver(container);

            //// Setup the container in a static member so it can be used
            //// to inject dependencies later.
            //MvcSiteMapProvider.Core.IoC.DI.Container = resolver;


            // Configure Dependencies
            container.Configure(x => x
                .For<System.Web.HttpContext>()
                .Use(HttpContext.Current)
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Web.IHttpContextFactory>()
                .Use<MvcSiteMapProvider.Core.Web.HttpContextFactory>()
            );


            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.ISiteMap>()
                .Use<MvcSiteMapProvider.Core.RequestCacheableSiteMap>()
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
            //    .For<MvcSiteMapProvider.Core.Security.IAclModule>()
            //    .Use<MvcSiteMapProvider.Core.Security.CompositeAclModule>()
            //);


            



            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Mvc.IActionMethodParameterResolver>()
                .Use<MvcSiteMapProvider.Core.Mvc.ActionMethodParameterResolver>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Mvc.IControllerTypeResolver>()
                .Use<MvcSiteMapProvider.Core.Mvc.ControllerTypeResolver>()
            );

            var aclModules = new MvcSiteMapProvider.Core.Security.CompositeAclModule(
                container.GetInstance<MvcSiteMapProvider.Core.Security.AuthorizeAttributeAclModule>(),
                container.GetInstance<MvcSiteMapProvider.Core.Security.XmlRolesAclModule>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Security.IAclModule>()
                .Use(aclModules)
            );



            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.ISiteMapNodeFactory>()
                .Use<MvcSiteMapProvider.Core.SiteMapNodeFactory>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.IDynamicNodeProviderStrategy>()
                .Use<MvcSiteMapProvider.Core.DynamicNodeProviderStrategy>()
            );

            // Get all types that implement IDynamicNodeProvider in an array
            container.Configure(x => x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssemblyContainingType<MvcSiteMapProvider.Core.SiteMaps>();
                    scan.WithDefaultConventions();
                    scan.AddAllTypesOf<MvcSiteMapProvider.Core.IDynamicNodeProvider>();
                }
            ));

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Mvc.UrlResolver.ISiteMapNodeUrlResolverStrategy>()
                .Use<MvcSiteMapProvider.Core.Mvc.UrlResolver.SiteMapNodeUrlResolverStrategy>()
            );

            // Get all types that implement ISiteMapNodeVisibilityProvider in an array
            container.Configure(x => x.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AssemblyContainingType<MvcSiteMapProvider.Core.SiteMaps>();
                scan.WithDefaultConventions();
                scan.AddAllTypesOf<MvcSiteMapProvider.Core.ISiteMapNodeVisibilityProvider>();
            }
            ));

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.ISiteMapNodeVisibilityProviderStrategy>()
                .Use<MvcSiteMapProvider.Core.SiteMapNodeVisibilityProviderStrategy>()
            );

            // Get all types that implement ISiteMapNodeResolver in an array
            container.Configure(x => x.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AssemblyContainingType<MvcSiteMapProvider.Core.SiteMaps>();
                scan.WithDefaultConventions();
                scan.AddAllTypesOf<MvcSiteMapProvider.Core.Mvc.UrlResolver.ISiteMapNodeUrlResolver>();
            }
            ));
            

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Builder.INodeKeyGenerator>()
                .Singleton()
                .Use<MvcSiteMapProvider.Core.Builder.NodeKeyGenerator>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Globalization.IExplicitResourceKeyParser>()
                .Singleton()
                .Use<MvcSiteMapProvider.Core.Globalization.ExplicitResourceKeyParser>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Globalization.IStringLocalizer>()
                .Singleton()
                .Use<MvcSiteMapProvider.Core.Globalization.StringLocalizer>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Globalization.ILocalizationService>()
                .Use<MvcSiteMapProvider.Core.Globalization.LocalizationService>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Builder.IDynamicNodeBuilder>()
                .Singleton()
                .Use<MvcSiteMapProvider.Core.Builder.DynamicNodeBuilder>()
            );

            //container.Configure(x => x
            //    .For<MvcSiteMapProvider.Core.Builder.XmlSiteMapBuilder>()
            //    .Use<MvcSiteMapProvider.Core.Builder.XmlSiteMapBuilder>()
            //);

            //container.Configure(x => x
            //    .For<MvcSiteMapProvider.Core.Builder.ReflectionSiteMapBuilder>()
            //    .Use<MvcSiteMapProvider.Core.Builder.ReflectionSiteMapBuilder>()
            //);

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Builder.IXmlSiteMapBuilderFactory>()
                .Use<MvcSiteMapProvider.Core.Builder.XmlSiteMapBuilderFactory>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Builder.IReflectionSiteMapBuilderFactory>()
                .Use<MvcSiteMapProvider.Core.Builder.ReflectionSiteMapBuilderFactory>()
            );

            //container.Configure(x => x
            //    .For<System.Web.HttpContext>()
            //    .Use(HttpContext.Current)
            //);

            // Configure the SiteMap Builder Sets

            var xmlBuilderFactory = container.GetInstance<MvcSiteMapProvider.Core.Builder.IXmlSiteMapBuilderFactory>();
            var xmlBuilder = xmlBuilderFactory.Create("~/Mvc.sitemap", new string[] { "" });


            var reflectionBuilderFactory = container.GetInstance<MvcSiteMapProvider.Core.Builder.IReflectionSiteMapBuilderFactory>();
            var reflectionBuilder = reflectionBuilderFactory.Create(new string[] { "" }, new string[] { "" });


            //var builders = new MvcSiteMapProvider.Core.Builder.CompositeSiteMapBuilder(
            //    container.GetInstance<MvcSiteMapProvider.Core.Builder.XmlSiteMapBuilder>(),
            //    container.GetInstance<MvcSiteMapProvider.Core.Builder.ReflectionSiteMapBuilder>()
            //);

            var builders = new MvcSiteMapProvider.Core.Builder.CompositeSiteMapBuilder(
                xmlBuilder,
                reflectionBuilder
            );



            var builderSet = new MvcSiteMapProvider.Core.Builder.SiteMapBuilderSet("default", builders);
            var builderSets = new MvcSiteMapProvider.Core.Builder.ISiteMapBuilderSet[] { builderSet };

            //container.Configure(x => x
            //    .For<IEnumerable<MvcSiteMapProvider.Core.Builder.ISiteMapBuilderSet>>()
            //    .Use(builderSets)
            //);

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Builder.ISiteMapBuilderSetStrategy>()
                .Use<MvcSiteMapProvider.Core.Builder.SiteMapBuilderSetStrategy>()
                //.TheArrayOf<MvcSiteMapProvider.Core.Builder.ISiteMapBuilderSet>()
                //.Contains(y => { y.IsThis(builderSet); })
                .Ctor<MvcSiteMapProvider.Core.Builder.ISiteMapBuilderSet[]>()
                .Is(builderSets)
            );



            

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Cache.ISiteMapCache>()
                .Use<MvcSiteMapProvider.Core.Cache.SiteMapCache>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Cache.ISiteMapCacheKeyGenerator>()
                .Use<MvcSiteMapProvider.Core.Cache.SiteMapCacheKeyGenerator>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.ISiteMapFactory>()
                .Use<MvcSiteMapProvider.Core.SiteMapFactory>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Cache.ISiteMapCacheKeyToBuilderSetMapper>()
                .Use<MvcSiteMapProvider.Core.Cache.SiteMapCacheKeyToBuilderSetMapper>()
            );


            // Configure the static instance of the SiteMapLoader
            var loader = new MvcSiteMapProvider.Core.Loader.SiteMapLoader(
                TimeSpan.FromMinutes(5),
                container.GetInstance<MvcSiteMapProvider.Core.Cache.ISiteMapCache>(),
                container.GetInstance<MvcSiteMapProvider.Core.Cache.ISiteMapCacheKeyGenerator>(),
                container.GetInstance<MvcSiteMapProvider.Core.Builder.ISiteMapBuilderSetStrategy>(),
                container.GetInstance<MvcSiteMapProvider.Core.ISiteMapFactory>(),
                container.GetInstance<MvcSiteMapProvider.Core.Cache.ISiteMapCacheKeyToBuilderSetMapper>()
                );

            MvcSiteMapProvider.Core.SiteMaps.Loader = loader;
            

        }


        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var test = "";
        }


    }
}