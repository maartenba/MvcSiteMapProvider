using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Contract for the specialized dictionary for dealing with custom site map attributes.
    /// </summary>
    public interface IAttributeDictionary
        : IDictionary<string, object>
    {
        void CopyTo(IDictionary<string, object> destination);
    }
}
