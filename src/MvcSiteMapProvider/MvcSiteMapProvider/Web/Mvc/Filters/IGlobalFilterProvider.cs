using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcSiteMapProvider.Web.Mvc.Filters
{
    /// <summary>
    /// Contract to access the Global filters registered with MVC.
    /// </summary>
    /// <remarks>
    /// Using the built in IFilterProvider directly doesn't work in conjunction with IDependencyResolver because
    /// doing so makes it impossible to access the filters that are registered via the static method 
    /// FilterProviders.Providers.Add().
    /// </remarks>
    public interface IGlobalFilterProvider
    {
        IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor);
    }
}
