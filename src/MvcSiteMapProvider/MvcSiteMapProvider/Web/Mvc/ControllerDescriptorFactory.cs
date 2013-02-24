using System;
using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:System.Web.Mvc.ControllerDescriptor"/>
    /// at runtime. Will return a <see cref="T:System.Web.Mvc.Async.ReflectedAsyncControllerDescriptor"/> for asynchronous
    /// controller types.
    /// </summary>
    public class ControllerDescriptorFactory
        : IControllerDescriptorFactory
    {
        #region IControllerDescriptorFactory Members

        public ControllerDescriptor Create(Type controllerType)
        {
            ControllerDescriptor controllerDescriptor = null;
            if (typeof(IController).IsAssignableFrom(controllerType))
            {
                controllerDescriptor = new ReflectedControllerDescriptor(controllerType);
            }
            else if (typeof(IAsyncController).IsAssignableFrom(controllerType))
            {
                controllerDescriptor = new ReflectedAsyncControllerDescriptor(controllerType);
            }
            return controllerDescriptor;
        }

        #endregion
    }
}
