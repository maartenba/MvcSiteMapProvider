namespace VideoSitemap
{
    using System.Collections.Generic;
    using MvcSiteMapProvider;

    public interface IExtensibleSiteMapNode : ISiteMapNode
    {
        IDictionary<string, object> DataByExtensionKey { get; }
    }
}