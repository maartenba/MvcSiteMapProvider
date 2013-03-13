using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// XmlSiteMapController class
    /// </summary>
#if MVC4    
    [AllowAnonymous]
#endif
    public class XmlSiteMapController
        : Controller
    {
        public XmlSiteMapController(
            IXmlSiteMapResultFactory xmlSiteMapResultFactory
            )
        {
            if (xmlSiteMapResultFactory == null)
                throw new ArgumentNullException("xmlSiteMapResultFactory");
            this.xmlSiteMapResultFactory = xmlSiteMapResultFactory;
        }

        private readonly IXmlSiteMapResultFactory xmlSiteMapResultFactory;

        /// <summary>
        /// GET: /XmlSiteMap/Index
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>XmlSiteMapResult instance</returns>
        public ActionResult Index(int page)
        {
            return xmlSiteMapResultFactory.Create(page);
        }

        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routeCollection">The route collection.</param>
        public static void RegisterRoutes(RouteCollection routeCollection)
        {
            List<RouteBase> routes = new List<RouteBase> {
                new Route("sitemap.xml",
                    new RouteValueDictionary(
                        new { controller = "XmlSiteMap", action = "Index", page = 0 }),
                            new MvcRouteHandler()),

                new Route("sitemap-{page}.xml",
                    new RouteValueDictionary(
                        new { controller = "XmlSiteMap", action = "Index", page = 0 }),
                            new MvcRouteHandler())
            };

            if (routeCollection.Count == 0)
            {
                foreach (var route in routes)
                {
                    routeCollection.Add(route);
                }
            }
            else
            {
                foreach (var route in routes)
                {
                    routeCollection.Insert(routeCollection.Count - 1, route);
                }
            }
        }
    }
}
