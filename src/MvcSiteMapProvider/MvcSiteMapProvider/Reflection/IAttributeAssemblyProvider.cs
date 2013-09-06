using System;
using System.Collections.Generic;
using System.Reflection;

namespace MvcSiteMapProvider.Reflection
{
    public interface IAttributeAssemblyProvider
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}
