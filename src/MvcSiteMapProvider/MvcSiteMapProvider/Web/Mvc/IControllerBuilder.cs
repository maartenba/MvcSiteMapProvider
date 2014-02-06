using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Web.Mvc
{
    public interface IControllerBuilder
    {
        HashSet<string> DefaultNamespaces { get; }
        IControllerFactory GetControllerFactory();
        void SetControllerFactory(Type controllerFactoryType);
        void SetControllerFactory(IControllerFactory controllerFactory);
    }
}
