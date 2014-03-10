using System;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.Web.IBinding"/> at runtime.
    /// </summary>
    public interface IBindingFactory
    {
        IBinding Create(string hostName, string protocol, int port);
    }
}
