using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Contract for a provider that creates a list of <see cref="T:MvcSiteMapProvider.Web.IBinding"/>
    /// instances that can be used to determine port numbers based on host name and protocol.
    /// </summary>
    public interface IBindingProvider
    {
        IEnumerable<IBinding> GetBindings();
    }
}
