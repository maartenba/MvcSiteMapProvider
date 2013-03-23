using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.Web.Mvc.MvcResolver"/>
    /// at runtime.
    /// </summary>
    public class MvcResolverFactory
        : IMvcResolverFactory
    {
        #region IMvcResolverFactory Members

        public IMvcResolver Create(
            IControllerTypeResolver controllerTypeResolver, 
            IActionMethodParameterResolver actionMethodParameterResolver)
        {
            return new MvcResolver(controllerTypeResolver, actionMethodParameterResolver);
        }

        #endregion
    }
}
