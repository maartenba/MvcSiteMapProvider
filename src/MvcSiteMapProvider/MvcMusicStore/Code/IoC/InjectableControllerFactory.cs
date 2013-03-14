using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace MvcMusicStore.Code.IoC
{
    public class InjectableControllerFactory 
        : DefaultControllerFactory
    {
        private readonly IDependencyInjectionContainer container;

        public InjectableControllerFactory(IDependencyInjectionContainer container)
        {
            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? 
                base.GetControllerInstance(requestContext, controllerType) :
                container.Resolve(controllerType) as IController;
        }
    }
}