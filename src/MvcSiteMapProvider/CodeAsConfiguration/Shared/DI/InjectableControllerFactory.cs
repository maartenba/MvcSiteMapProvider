using System;
using System.Web.Routing;
using System.Web.Mvc;

namespace DI
{
    internal class InjectableControllerFactory
        : DefaultControllerFactory
    {
        private readonly IDependencyInjectionContainer container;

        public InjectableControllerFactory(IDependencyInjectionContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (requestContext.HttpContext.Request.Url.ToString().EndsWith("favicon.ico"))
                return null;

            return controllerType == null ?
                base.GetControllerInstance(requestContext, controllerType) :
                container.GetInstance(controllerType) as IController;
        }

        public override void ReleaseController(IController controller)
        {
            this.container.Release(controller);
        }
    }
}