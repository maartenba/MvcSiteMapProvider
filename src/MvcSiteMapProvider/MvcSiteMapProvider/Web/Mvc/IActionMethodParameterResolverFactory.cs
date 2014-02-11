using System;

namespace MvcSiteMapProvider.Web.Mvc
{
    // TODO: Remove this type in version 5.

    /// <summary>
    /// Contract for abstract factory that can provide instances of <see cref="T:MvcSiteMapProvder.Web.Mvc.IActionMethodParameterResolver"/>
    /// at runtime.
    /// </summary>
    public interface IActionMethodParameterResolverFactory
    {
        IActionMethodParameterResolver Create();
    }
}
