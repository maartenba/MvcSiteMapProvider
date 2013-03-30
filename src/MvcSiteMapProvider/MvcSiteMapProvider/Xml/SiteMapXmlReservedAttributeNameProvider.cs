using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSiteMapProvider.Xml
{
    /// <summary>
    /// Provides information whether a given XML attribute name is a reserved name or it is
    /// safe to use the attribute in a SiteMapNode.
    /// </summary>
    public class SiteMapXmlReservedAttributeNameProvider
        : ISiteMapXmlReservedAttributeNameProvider
    {
        public SiteMapXmlReservedAttributeNameProvider(
            IEnumerable<string> attributesToIgnore
            )
        {
            if (attributesToIgnore == null)
                throw new ArgumentNullException("attributesToIgnore");

            this.attributesToIgnore = attributesToIgnore;
        }

        protected readonly IEnumerable<string> attributesToIgnore;

        #region ISiteMapXmlReservedAttributeNameProvider Members

        public virtual bool IsRegularAttribute(string attributeName)
        {
            return attributeName != "title"
                   && attributeName != "description";
        }

        public virtual bool IsRouteAttribute(string attributeName)
        {
            return attributeName != "title"
               && attributeName != "description"
               && attributeName != "resourceKey"
               && attributeName != "key"
               && attributeName != "roles"
               && attributeName != "route"
               && attributeName != "url"
               && attributeName != "cacheResolvedUrl"
               && attributeName != "clickable"
               && attributeName != "httpMethod"
               && attributeName != "dynamicNodeProvider"
               && attributeName != "urlResolver"
               && attributeName != "visibilityProvider"
               && attributeName != "visibility"
               && attributeName != "lastModifiedDate"
               && attributeName != "changeFrequency"
               && attributeName != "updatePriority"
               && attributeName != "targetFrame"
               && attributeName != "imageUrl"
               && attributeName != "inheritedRouteParameters"
               && attributeName != "preservedRouteParameters"
               && attributeName != "canonicalUrl"
               && attributeName != "canonicalKey"
               && attributeName != "metaRobotsValues"
               && !attributesToIgnore.Contains(attributeName)
               && !attributeName.StartsWith("data-");
        }

        #endregion
    }
}
