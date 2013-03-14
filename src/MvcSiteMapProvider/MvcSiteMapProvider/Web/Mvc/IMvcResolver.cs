using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// Contract for facade service that resolves MVC dependencies.
    /// </summary>
    public interface IMvcResolver
    {
        Type ResolveControllerType(string areaName, string controllerName);
        IEnumerable<string> ResolveActionMethodParameters(string areaName, string controllerName, string actionMethodName);
    }
}
