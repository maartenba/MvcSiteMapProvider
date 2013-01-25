#region Using directives

using System.Collections.Generic;

#endregion

namespace MvcSiteMapProvider.Core.Mvc
{
    /// <summary>
    /// IActionMethodParameterResolver contract
    /// </summary>
    public interface IActionMethodParameterResolver
    {
        /// <summary>
        /// Resolves the action method parameters.
        /// </summary>
        /// <param name="controllerTypeResolver">The controller type resolver.</param>
        /// <param name="areaName">Name of the area.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionMethodName">Name of the action method.</param>
        /// <returns>
        /// A action method parameters represented as a <see cref="string"/> instance 
        /// </returns>
        IEnumerable<string> ResolveActionMethodParameters(IControllerTypeResolver controllerTypeResolver, string areaName, string controllerName, string actionMethodName);
    }
}
