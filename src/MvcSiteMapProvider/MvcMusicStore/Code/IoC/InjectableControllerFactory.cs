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
            if (controllerType == null)
                return base.GetControllerInstance(requestContext, controllerType);
            try
            {
                return container.Resolve(controllerType) as IController;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                System.Diagnostics.Debug.WriteLine(message);
                throw new Exception(message);
            }
        }
    }
}