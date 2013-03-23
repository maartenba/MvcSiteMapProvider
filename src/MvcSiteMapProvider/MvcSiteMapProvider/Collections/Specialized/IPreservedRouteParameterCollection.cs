using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Contract of specialized string collection for providing business logic that manages
    /// the behavior of the preserved route parameters.
    /// </summary>
    public interface IPreservedRouteParameterCollection
        : IList<string>
    {
        void CopyTo(IList<string> destination);
    }
}
