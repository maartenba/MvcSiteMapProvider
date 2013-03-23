using System;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// IControllerTypeResolver contract
    /// </summary>
    public interface IControllerTypeResolver
    {
        /// <summary>
        /// Resolves the type of the controller.
        /// </summary>
        /// <param name="areaName">Name of the area.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns>Controller type</returns>
        Type ResolveControllerType(string areaName, string controllerName);
    }
}
