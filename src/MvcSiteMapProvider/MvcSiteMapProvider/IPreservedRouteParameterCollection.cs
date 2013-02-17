using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPreservedRouteParameterCollection
        : IList<string>
    {
        void CopyTo(IList<string> destination);
    }
}
