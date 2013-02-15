using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcSiteMapProvider.IoC
{
    /// <summary>
    /// Interface that can be implemented using a specific Dependency Injection
    /// container to provide property setter injection of dependencies.
    /// </summary>
    /// <remarks>
    /// This interface shields the developer from using the DI container as a
    /// service locator while still providing a way to inject dependencies in 
    /// classes that do not allow non-public constructors.
    /// </remarks>
    public interface IResolver
    {
        void Inject(object instance);
    }
}
