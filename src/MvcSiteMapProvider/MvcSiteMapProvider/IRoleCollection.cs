using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract of specialized string collection for providing business logic that manages
    /// the behavior of the roles.
    /// </summary>
    public interface IRoleCollection
        : IList<string>
    {
        void CopyTo(IList<string> destination);
    }
}
