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

            RegisterRoutes(RouteTable.Routes);
            RegisterGlobalFilters(System.Web.Mvc.GlobalFilters.Filters);


            // MvcSiteMapProvider Configuration
#if NET35
    MvcSiteMapProvider.DI.Composer.Compose();
#endif
            XmlSiteMapController.RegisterRoutes(RouteTable.Routes);
            var container = DIConfig.Register();
            MvcSiteMapProviderConfig.Register(container);
        }
    }
}