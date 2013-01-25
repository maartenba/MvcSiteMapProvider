using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Core;
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
            var resolver = new Code.IoC.StructureMapResolver(container);

            // Setup the container in a static member so it can be used
            // to inject dependencies later.
            MvcSiteMapProvider.Core.IoC.DI.Container = resolver;


            // Configure Dependencies
            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.SiteMap.ISiteMap>()
                .Use<MvcSiteMapProvider.Core.SiteMap.SiteMap>()
            );

            // We create a new Setter Injection Policy that
            // forces StructureMap to inject all public properties
            // where the Property Type name equals 'ISiteMap'
            container.Configure(
                x => x.SetAllProperties(p =>
                    {
                        p.TypeMatches(t => t.Name == "ISiteMap");
                    }
                )
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Security.IAclModule>()
                .Use<MvcSiteMapProvider.Core.Security.DefaultAclModule>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Mvc.IActionMethodParameterResolver>()
                .Use<MvcSiteMapProvider.Core.Mvc.DefaultActionMethodParameterResolver>()
            );

            container.Configure(x => x
                .For<MvcSiteMapProvider.Core.Mvc.IControllerTypeResolver>()
                .Use<MvcSiteMapProvider.Core.Mvc.DefaultControllerTypeResolver>()
            );




        }


        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var test = "";
        }


    }
}