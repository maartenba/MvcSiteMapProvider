using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcSiteMapProvider.Web.Mvc.Filters
{
    /// <summary>
    /// Provides access to the Global filters registered with MVC.
    /// </summary>
    /// <remarks>
    /// Using the built in IFilterProvider directly doesn't work in conjunction with IDependencyResolver because
    /// doing so makes it impossible to access the filters that are registered via the static method 
    /// FilterProviders.Providers.Add().
    /// </remarks>
    public class GlobalFilterProvider
        : IGlobalFilterProvider
    {
        #region IGlobalFilterProvider Members

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
#if !MVC2
            return FilterProviders.Providers.GetFilters(controllerContext, actionDescriptor);
#else
            return new List<Filter>();
#endif
        }

        #endregion
    }
}
