﻿using System;
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
            return controllerType == null ?
                base.GetControllerInstance(requestContext, controllerType) :
                container.Resolve(controllerType) as IController;
        }
    }
}
