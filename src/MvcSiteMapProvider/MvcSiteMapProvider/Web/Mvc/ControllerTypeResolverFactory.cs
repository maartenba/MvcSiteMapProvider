using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web.Routing;
using MvcSiteMapProvider.DI;
using MvcSiteMapProvider.Web.Compilation;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.Web.Mvc.ControllerTypeResolver"/>
    /// at runtime.
    /// </summary>
    [ExcludeFromAutoRegistration]
    public class ControllerTypeResolverFactory
        : IControllerTypeResolverFactory
    {
        public ControllerTypeResolverFactory(
            IEnumerable<string> areaNamespacesToIgnore,
            IControllerBuilder controllerBuilder,
            IBuildManager buildManager
            )
        {
            if (areaNamespacesToIgnore == null)
                throw new ArgumentNullException("areaNamespacesToIgnore");
            if (controllerBuilder == null)
                throw new ArgumentNullException("controllerBuilder");
            if (buildManager == null)
                throw new ArgumentNullException("buildManager");

            this.areaNamespacesToIgnore = areaNamespacesToIgnore;
            this.controllerBuilder = controllerBuilder;
            this.buildManager = buildManager;
        }

        protected readonly IEnumerable<string> areaNamespacesToIgnore;
        protected readonly IControllerBuilder controllerBuilder;
        protected readonly IBuildManager buildManager;

        #region IControllerTypeResolverFactory Members

        public IControllerTypeResolver Create(RouteCollection routes)
        {
            return new ControllerTypeResolver(areaNamespacesToIgnore, routes, controllerBuilder, buildManager);
        }

        #endregion
    }
}
