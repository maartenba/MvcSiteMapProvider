using System;

namespace MvcSiteMapProvider
{
    public interface ISiteMapSettings
    {
        bool SecurityTrimmingEnabled { get; }
        bool EnableLocalization { get; }
    }
}
