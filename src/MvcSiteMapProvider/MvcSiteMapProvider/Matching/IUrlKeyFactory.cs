using System;

namespace MvcSiteMapProvider.Matching
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of 
    /// <see cref="T:MvcSiteMapProvider.Matching.IUrlKey"/> at runtime.
    /// </summary>
    public interface IUrlKeyFactory
    {
        IUrlKey Create(ISiteMapNode node);
        IUrlKey Create(string relativeOrAbsoluteUrl, string hostName);
    }
}
