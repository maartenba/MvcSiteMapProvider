using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Contract for the specialized collection for dealing with custom site map attributes.
    /// </summary>
    public interface IAttributeCollection
        : IDictionary<string, object>
    {
        void CopyTo(IDictionary<string, object> destination);
    }
}
