using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Core.Web.Mvc
{
    /// <summary>
    /// IActionMethodParameterResolver contract
    /// </summary>
    public interface IActionMethodParameterResolver
    {
        /// <summary>
        /// Resolves the action method parameters.
        /// </summary>
        /// <param name="areaName">Name of the area.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionMethodName">Name of the action method.</param>
        /// <returns>
        /// A action method parameters represented as a <see cref="string"/> instance 
        /// </returns>
        IEnumerable<string> ResolveActionMethodParameters(string areaName, string controllerName, string actionMethodName);
    }
}
