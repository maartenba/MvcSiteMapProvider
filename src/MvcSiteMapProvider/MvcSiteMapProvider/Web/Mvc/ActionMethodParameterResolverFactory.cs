using System;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.Web.Mvc.ActionMethodParameterResolver"/>
    /// at runtime.
    /// </summary>
    public class ActionMethodParameterResolverFactory
        : IActionMethodParameterResolverFactory
    {
        #region IActionMethodParameterResolverFactory Members

        public IActionMethodParameterResolver Create()
        {
            return new ActionMethodParameterResolver();
        }

        #endregion
    }
}
