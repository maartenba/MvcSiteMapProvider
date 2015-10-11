using MvcSiteMapProvider.Builder;
using System;

namespace MvcSiteMapProvider.Xml
{
    /// <summary>
    /// Contract for reserved XML attribute name provider for SiteMap nodes.
    /// </summary>
    [Obsolete("Use IReservedAttributeNameProvider instead. This interface will be removed in version 5.")]
    public interface ISiteMapXmlReservedAttributeNameProvider
        : IReservedAttributeNameProvider
    {
    }
}
