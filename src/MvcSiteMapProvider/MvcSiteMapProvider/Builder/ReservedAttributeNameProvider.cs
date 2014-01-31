using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.DI;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Provides information whether a given attribute name is a reserved name or it is
    /// safe to use the attribute in a SiteMapNode's Attributes or RouteValues dictionaries.
    /// </summary>
    [ExcludeFromAutoRegistration]
    public class ReservedAttributeNameProvider
        : IReservedAttributeNameProvider
    {
        public ReservedAttributeNameProvider(
            IEnumerable<string> attributesToIgnore
            )
        {
            if (attributesToIgnore == null)
                throw new ArgumentNullException("attributesToIgnore");

            this.attributesToIgnore = attributesToIgnore;
        }

        protected readonly IEnumerable<string> attributesToIgnore;

        #region IReservedAttributeNameProvider Members

        public virtual bool IsRegularAttribute(string attributeName)
        {
            return !this.IsKnownAttribute(attributeName)
                && attributeName != "controller"
                && attributeName != "action"
                && attributeName != "area";
        }

        public virtual bool IsRouteAttribute(string attributeName)
        {
            return !this.IsKnownAttribute(attributeName)
                && attributeName != "visibility"
                && !attributesToIgnore.Contains(attributeName)
                && !attributeName.StartsWith("data-");
        }

        #endregion

        protected virtual bool IsKnownAttribute(string attributeName)
        {
            return attributeName == "title"
                || attributeName == "description"
                || attributeName == "resourceKey"
                || attributeName == "key"
                || attributeName == "roles"
                || attributeName == "route"
                || attributeName == "url"
                || attributeName == "cacheResolvedUrl"
                || attributeName == "clickable"
                || attributeName == "httpMethod"
                || attributeName == "dynamicNodeProvider"
                || attributeName == "urlResolver"
                || attributeName == "visibilityProvider"
                || attributeName == "lastModifiedDate"
                || attributeName == "changeFrequency"
                || attributeName == "updatePriority"
                || attributeName == "targetFrame"
                || attributeName == "imageUrl"
                || attributeName == "inheritedRouteParameters"
                || attributeName == "preservedRouteParameters"
                || attributeName == "canonicalUrl"
                || attributeName == "canonicalKey"
                || attributeName == "metaRobotsValues";
        }
    }
}
