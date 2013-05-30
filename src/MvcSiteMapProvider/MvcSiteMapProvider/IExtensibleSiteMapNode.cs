namespace MvcSiteMapProvider
{
    using System.Collections.Generic;

    public interface IExtensibleSiteMapNode : ISiteMapNode
    {
        IDictionary<string, object> DataByExtensionKey { get; }
    }
}