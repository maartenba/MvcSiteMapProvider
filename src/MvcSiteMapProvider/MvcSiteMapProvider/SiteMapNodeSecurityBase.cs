using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// An abstract class that provides business logic for managing security. 
    /// </summary>
    public abstract class SiteMapNodeSecurityBase
        : ISiteMapNode
    {
        #region Security

        /// <summary>
        /// Determines whether the current node is accessible to the current user based on context.
        /// </summary>
        /// <value>
        /// True if the current node is accessible.
        /// </value>
        public virtual bool IsAccessibleToUser()
        {
            return this.SiteMap.IsAccessibleToUser(this);
        }

        #endregion


        #region Abstract Members

        public abstract string Key { get; }
        public abstract bool IsDynamic { get; }
        public abstract bool IsReadOnly { get; }
        public abstract ISiteMapNode ParentNode { get; }
        public abstract ISiteMapNodeCollection ChildNodes { get; }
        public abstract ISiteMapNodeCollection Descendants { get; }
        public abstract ISiteMapNodeCollection Ancestors { get; }
        public abstract bool IsDescendantOf(ISiteMapNode node);
        public abstract ISiteMapNode NextSibling { get; }
        public abstract ISiteMapNode PreviousSibling { get; }
        public abstract ISiteMapNode RootNode { get; }
        public abstract bool IsInCurrentPath();
        public abstract bool HasChildNodes { get; }
        public abstract int GetNodeLevel();
        public abstract ISiteMap SiteMap { get; }
        public abstract int Order { get; set; }
        public abstract string HttpMethod { get; set; }
        public abstract string ResourceKey { get; }
        public abstract string Title { get; set; }
        public abstract string Description { get; set; }
        public abstract string TargetFrame { get; set; }
        public abstract string ImageUrl { get; set; }
        public abstract string ImageUrlProtocol { get; set; }
        public abstract string ImageUrlHostName { get; set; }
        public abstract IAttributeDictionary Attributes { get; }
        public abstract IRoleCollection Roles { get; }
        public abstract DateTime LastModifiedDate { get; set; }
        public abstract ChangeFrequency ChangeFrequency { get; set; }
        public abstract UpdatePriority UpdatePriority { get; set; }
        public abstract string VisibilityProvider { get; set; }
        public abstract bool IsVisible(IDictionary<string, object> sourceMetadata);
        public abstract string DynamicNodeProvider { get; set; }
        public abstract IEnumerable<DynamicNode> GetDynamicNodeCollection();
        public abstract bool HasDynamicNodeProvider { get; }
        public abstract bool Clickable { get; set; }
        public abstract string UrlResolver { get; set; }
        public abstract string Url { get; set; }
        public abstract string UnresolvedUrl { get; }
        public abstract string ResolvedUrl { get; }
        public abstract bool CacheResolvedUrl { get; set; }
        public abstract void ResolveUrl();
        public abstract bool IncludeAmbientValuesInUrl { get; set; }
        public abstract string Protocol { get; set; }
        public abstract string HostName { get; set; }
        public abstract bool HasAbsoluteUrl();
        public abstract bool HasExternalUrl(HttpContextBase httpContext);
        public abstract string CanonicalKey { get; set; }
        public abstract string CanonicalUrl { get; set; }
        public abstract string CanonicalUrlProtocol { get; set; }
        public abstract string CanonicalUrlHostName { get; set; }
        public abstract IMetaRobotsValueCollection MetaRobotsValues { get; }
        public abstract string GetMetaRobotsContentString();
        public abstract bool HasNoIndexAndNoFollow { get; }
        public abstract string Route { get; set; }
        public abstract IRouteValueDictionary RouteValues { get; }
        public abstract IPreservedRouteParameterCollection PreservedRouteParameters { get; }
        public abstract RouteData GetRouteData(HttpContextBase httpContext);
        public abstract bool MatchesRoute(IDictionary<string, object> routeValues);
        public abstract string Area { get; set; }
        public abstract string Controller { get; set; }
        public abstract string Action { get; set; }
        public abstract void CopyTo(ISiteMapNode node);
        public abstract bool Equals(ISiteMapNode node);

        #endregion
    }
}
