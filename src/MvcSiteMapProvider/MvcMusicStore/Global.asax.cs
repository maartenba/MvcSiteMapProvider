using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcMusicStore.App_Start;
using MvcSiteMapProvider.Bootstrapper;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using StructureMap;

namespace MvcMusicStore
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // MvcSiteMapProvider Configuration
#if NET35
            MvcSiteMapProvider.DI.Composer.Compose();
#endif
            XmlSiteMapController.RegisterRoutes(RouteTable.Routes);

            // NOTE: This check wouldn't have to be made in a real-world application - we do it
            // in the demo because we want to support both the internal and external DI containers.
            if (new MvcSiteMapProvider.DI.ConfigurationSettings().UseExternalDIContainer == true)
            {
                var container = MvcSiteMapProvider.Bootstrapper.StructureMap.DIConfig.Register();
#if DependencyResolver
                DependencyResolverConfig.Register(container);
#else
                ControllerFactoryConfig.Register(container);
#endif
                MvcSiteMapProviderConfig.Register(container);
            }
        }

        /// <summary>
        /// Used to fake roles in this demo application
        /// </summary>
        protected void Application_PostAuthenticateRequest(object sender, EventArgs eventArgs)
        {
            var user = HttpContext.Current.User;
            if (user.Identity.IsAuthenticated && user.Identity.Name.ToLowerInvariant() == "administrator")
            {
                HttpContext.Current.User = Thread.CurrentPrincipal = new AdministratorPrincipal(user.Identity);
            }
        }

        /// <summary>
        /// Used to fake roles in this demo application
        /// </summary>
        public class AdministratorPrincipal
            : IPrincipal
        {
            public AdministratorPrincipal(IIdentity identity)
            {
                Identity = identity;
            }

            public bool IsInRole(string role)
            {
                return true; // we satisfy *any* role
            }

            public IIdentity Identity { get; private set; }
        }
    }
}