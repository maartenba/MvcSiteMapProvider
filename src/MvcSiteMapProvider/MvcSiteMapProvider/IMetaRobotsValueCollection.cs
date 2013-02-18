using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMetaRobotsValueCollection
        : IList<string>
    {
        string GetMetaRobotsContentString();
        bool HasNoIndexAndNoFollow { get; }
        void CopyTo(IList<string> destination);
    }
}
