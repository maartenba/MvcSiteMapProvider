using System;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// Contract for abstract factory that can provide instances of <see cref="T:System.Web.Mvc.IMvcResolver"/>
    /// at runtime.
    /// </summary>
    public interface IMvcResolverFactory
    {
        IMvcResolver Create(
            IControllerTypeResolver controllerTypeResolver, 
            IActionMethodParameterResolver actionMethodParameterResolver);
    }
}
