using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Reflection
{
    /// <summary>
    /// Abstract factory that provides a IAttributeAssemblyProvider instance.
    /// </summary>
    public class AttributeAssemblyProviderFactory
        : IAttributeAssemblyProviderFactory
    {
        #region IAttributeAssemblyProviderFactory Members

        public IAttributeAssemblyProvider Create(IEnumerable<string> includeAssemblies, IEnumerable<string> excludeAssemblies)
        {
            return new AttributeAssemblyProvider(includeAssemblies, excludeAssemblies);
        }

        #endregion
    }
}
