using System.Web.Routing;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// Contract for abstract factory that can provide instances of <see cref="T:MvcSiteMapProvder.Web.Mvc.IControllerTypeResolver"/>
    /// at runtime.
    /// </summary>
    public interface IControllerTypeResolverFactory
    {
        IControllerTypeResolver Create(RouteCollection routes);
    }
}
