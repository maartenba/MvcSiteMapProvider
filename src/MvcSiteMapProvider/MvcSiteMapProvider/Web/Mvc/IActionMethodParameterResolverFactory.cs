using System;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// Contract for abstract factory that can provide instances of <see cref="T:MvcSiteMapProvder.Web.Mvc.IActionMethodParameterResolver"/>
    /// at runtime.
    /// </summary>
    public interface IActionMethodParameterResolverFactory
    {
        IActionMethodParameterResolver Create();
    }
}
