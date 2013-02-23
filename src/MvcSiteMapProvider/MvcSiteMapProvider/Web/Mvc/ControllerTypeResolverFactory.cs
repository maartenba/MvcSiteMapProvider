using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Web.Compilation;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.Web.Mvc.ControllerTypeResolver"/>
    /// at runtime.
    /// </summary>
    public class ControllerTypeResolverFactory
        : IControllerTypeResolverFactory
    {
        public ControllerTypeResolverFactory(
            IControllerBuilder controllerBuilder,
            IBuildManager buildManager
            )
        {
            if (controllerBuilder == null)
                throw new ArgumentNullException("controllerBuilder");
            if (buildManager == null)
                throw new ArgumentNullException("buildManager");

            this.controllerBuilder = controllerBuilder;
            this.buildManager = buildManager;
        }

        protected readonly IControllerBuilder controllerBuilder;
        protected readonly IBuildManager buildManager;

        #region IControllerTypeResolverFactory Members

        public IControllerTypeResolver Create(RouteCollection routes)
        {
            return new ControllerTypeResolver(routes, controllerBuilder, buildManager);
        }

        #endregion
    }
}
