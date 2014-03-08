using System;

namespace MvcSiteMapProvider.Matching
{
    /// <summary>
    /// Contract for a class that can be used as a key for matching relative or absolute URLs.
    /// </summary>
    public interface IUrlKey
    {
        string HostName { get; }
        string RootRelativeUrl { get; }
    }
}
