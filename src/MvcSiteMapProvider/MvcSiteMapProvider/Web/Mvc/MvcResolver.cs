using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// Facade service that resolves MVC dependencies.
    /// </summary>
    public class MvcResolver
        : IMvcResolver
    {
        public MvcResolver(
            IControllerTypeResolver controllerTypeResolver,
            IActionMethodParameterResolver actionMethodParameterResolver
            )
        {
            if (controllerTypeResolver == null)
                throw new ArgumentNullException("controllerTypeResolver");
            if (actionMethodParameterResolver == null)
                throw new ArgumentNullException("actionMethodParameterResolver");

            this.controllerTypeResolver = controllerTypeResolver;
            this.actionMethodParameterResolver = actionMethodParameterResolver;
        }

        protected readonly IControllerTypeResolver controllerTypeResolver;
        protected readonly IActionMethodParameterResolver actionMethodParameterResolver;

        #region IMvcResolver Members

        public Type ResolveControllerType(string areaName, string controllerName)
        {
            return controllerTypeResolver.ResolveControllerType(areaName, controllerName);
        }

        public IEnumerable<string> ResolveActionMethodParameters(string areaName, string controllerName, string actionMethodName)
        {
            return actionMethodParameterResolver.ResolveActionMethodParameters(controllerTypeResolver, areaName, controllerName, actionMethodName);
        }

        #endregion
    }
}
