using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcSiteMapProvider.Web.Mvc.Filters
{
#if !MVC2
    /// <summary>
    /// This is the default implementation of <see cref="T:System.Web.Mvc.IFilterProvider"/> that provides globally registered
    /// provider instances. Implement the <see cref="T:System.Web.Mvc.IFilterProvider"/> interface and replace this type in
    /// your DI configuration to inject your own filters.
    /// </summary>
    public class FilterProvider
        : IFilterProvider
    {
        #region IFilterProvider Members

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return FilterProviders.Providers.GetFilters(controllerContext, actionDescriptor);
        }

        #endregion
    }
#endif
}
