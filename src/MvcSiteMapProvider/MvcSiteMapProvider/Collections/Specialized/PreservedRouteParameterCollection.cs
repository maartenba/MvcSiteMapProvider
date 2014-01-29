using System;
using MvcSiteMapProvider.DI;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Specialized string collection for providing business logic that manages
    /// the behavior of the preserved route parameters.
    /// </summary>
    [ExcludeFromAutoRegistration]
    public class PreservedRouteParameterCollection
        : LockableList<string>, IPreservedRouteParameterCollection
    {
        public PreservedRouteParameterCollection(
            ISiteMap siteMap
            )
            : base(siteMap)
        {
        }
    }
}
