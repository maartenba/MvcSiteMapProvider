using System;

namespace MvcSiteMapProvider.Web.Mvc
{
    // TODO: Remove this type in version 5.

    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.Web.Mvc.ActionMethodParameterResolver"/>
    /// at runtime.
    /// </summary>
    public class ActionMethodParameterResolverFactory
        : IActionMethodParameterResolverFactory
    {
        public ActionMethodParameterResolverFactory(
            IControllerDescriptorFactory controllerDescriptorFactory
            )
        {
            if (controllerDescriptorFactory == null)
                throw new ArgumentNullException("controllerDescriptorFactory");

            this.controllerDescriptorFactory = controllerDescriptorFactory;
        }

        protected readonly IControllerDescriptorFactory controllerDescriptorFactory;


        #region IActionMethodParameterResolverFactory Members

        public IActionMethodParameterResolver Create()
        {
            return new ActionMethodParameterResolver(controllerDescriptorFactory);
        }

        #endregion
    }
}
