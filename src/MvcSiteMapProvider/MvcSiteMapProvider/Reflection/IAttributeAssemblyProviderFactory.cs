using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Reflection
{
    /// <summary>
    /// Contract for attribute assembly provider factory.
    /// </summary>
    public interface IAttributeAssemblyProviderFactory
    {
        IAttributeAssemblyProvider Create(IEnumerable<string> includeAssemblies, IEnumerable<string> excludeAssemblies);
    }
}
