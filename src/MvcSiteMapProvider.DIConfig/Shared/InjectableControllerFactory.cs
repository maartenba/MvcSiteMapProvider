using System;
using System.Web.Routing;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;

namespace DI
{
    internal class InjectableControllerFactory
        : DefaultControllerFactory
    {
        private readonly IServiceLocator container;

        public InjectableControllerFactory(IServiceLocator container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ?
                base.GetControllerInstance(requestContext, controllerType) :
                container.GetInstance(controllerType) as IController;
        }
    }
}
