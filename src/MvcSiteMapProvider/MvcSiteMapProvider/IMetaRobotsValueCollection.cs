using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract of specialized string collection for providing business logic that manages
    /// the behavior of the meta robots content attribute.
    /// </summary>
    public interface IMetaRobotsValueCollection
        : IList<string>
    {
        string GetMetaRobotsContentString();
        bool HasNoIndexAndNoFollow { get; }
        void CopyTo(IList<string> destination);
    }
}
