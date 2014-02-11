using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using MvcSiteMapProvider.Collections;

namespace MvcSiteMapProvider.Web.Mvc
{
    // TODO: Remove this type in version 5.

    /// <summary>
    /// ActionMethodParameterResolver class
    /// </summary>
    public class ActionMethodParameterResolver
        : IActionMethodParameterResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionMethodParameterResolver"/> class.
        /// </summary>
        public ActionMethodParameterResolver(
            IControllerDescriptorFactory controllerDescriptorFactory
            )
        {
            if (controllerDescriptorFactory == null)
                throw new ArgumentNullException("controllerDescriptorFactory");

            this.controllerDescriptorFactory = controllerDescriptorFactory;

            Cache = new ThreadSafeDictionary<string, IEnumerable<string>>();
        }

        protected readonly IControllerDescriptorFactory controllerDescriptorFactory;

        /// <summary>
        /// Gets or sets the cache.
        /// </summary>
        /// <value>The cache.</value>
        protected ThreadSafeDictionary<string, IEnumerable<string>> Cache { get; private set; }

        #region IActionMethodParameterResolver Members

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
        public IEnumerable<string> ResolveActionMethodParameters(IControllerTypeResolver controllerTypeResolver,
                                                                string areaName, string controllerName,
                                                                string actionMethodName)
        {
            // Is the request cached?
            string cacheKey = areaName + "_" + controllerName + "_" + actionMethodName;
            if (Cache.ContainsKey(cacheKey))
            {
                return Cache[cacheKey];
            }

            // Get controller type
            Type controllerType = controllerTypeResolver.ResolveControllerType(areaName, controllerName);

            // Get action method information
            var actionParameters = new List<string>();
            if (controllerType != null)
            {
                var controllerDescriptor = controllerDescriptorFactory.Create(controllerType);

                ActionDescriptor[] actionDescriptors = controllerDescriptor.GetCanonicalActions()
                    .Where(a => a.ActionName == actionMethodName).ToArray();

                if (actionDescriptors != null && actionDescriptors.Length > 0)
                {
                    foreach (ActionDescriptor actionDescriptor in actionDescriptors)
                    {
                        actionParameters.AddRange(actionDescriptor.GetParameters().Select(p => p.ParameterName));
                    }
                }
            }

            // Cache the result
            if (!Cache.ContainsKey(cacheKey))
            {
                try
                {
                    Cache.Add(cacheKey, actionParameters);
                }
                catch (ArgumentException)
                {
                    // Nomnomnom... We're intentionally eating it here
                }
            }

            // Return
            return actionParameters;
        }

        #endregion
    }
}