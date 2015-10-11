using System;
using System.Web.Mvc;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// Contract for abstract factory that can provide instances of <see cref="T:System.Web.Mvc.ControllerDescriptor"/>
    /// at runtime.
    /// </summary>
    public interface IControllerDescriptorFactory
    {
        ControllerDescriptor Create(Type controllerType);
    }
}
