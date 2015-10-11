using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.DI;
using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Xml
{
    /// <summary>
    /// Provides information whether a given XML attribute name is a reserved name or it is
    /// safe to use the attribute in a SiteMapNode.
    /// </summary>
    [ExcludeFromAutoRegistration]
    [Obsolete("Use the ReservedAttributeNameProvider instead. This class will be removed in version 5.")]
    public class SiteMapXmlReservedAttributeNameProvider
        : ReservedAttributeNameProvider, ISiteMapXmlReservedAttributeNameProvider
    {
        public SiteMapXmlReservedAttributeNameProvider(
            IEnumerable<string> attributesToIgnore
            ) 
            : base(attributesToIgnore)
        {
        }
    }
}
