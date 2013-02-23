using System;
using System.Web.Routing;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.Web.Mvc.ControllerTypeResolver"/>
    /// at runtime.
    /// </summary>
    public class ControllerTypeResolverFactory
        : IControllerTypeResolverFactory
    {
        #region IControllerTypeResolverFactory Members

        public IControllerTypeResolver Create(RouteCollection routes)
        {
            return new ControllerTypeResolver(routes);
        }

        #endregion
    }
}
