using System;
using System.Collections.Generic;
using System.Web.Mvc;

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
