using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for the specialized class that manages the state and business logic of each 
    /// node in the hierarchy.
    /// </summary>
    public interface ISiteMapNode
        : ISortable, IEquatable<ISiteMapNode>
    {
        string Key { get; }
        bool IsDynamic { get; }
        bool IsReadOnly { get; }

        ISiteMapNode ParentNode { get; }
        ISiteMapNodeCollection ChildNodes { get; }
        ISiteMapNodeCollection Descendants { get; }
        ISiteMapNodeCollection Ancestors { get; }
        bool IsDescendantOf(ISiteMapNode node);
        ISiteMapNode NextSibling { get; }
        ISiteMapNode PreviousSibling { get; }
        ISiteMapNode RootNode { get; }
        bool IsInCurrentPath();
        bool HasChildNodes { get; }
        int GetNodeLevel();
        ISiteMap SiteMap { get; }
        new int Order { get; set; }

        bool IsAccessibleToUser();
        string HttpMethod { get; set; }
        string ResourceKey { get; }
        string Title { get; set; }
        string Description { get; set; }
        string TargetFrame { get; set; }
        string ImageUrl { get; set; }
        string ImageUrlProtocol { get; set; }
        string ImageUrlHostName { get; set; }
        IAttributeDictionary Attributes { get; }
        IRoleCollection Roles { get; }

        DateTime LastModifiedDate { get; set; }
        ChangeFrequency ChangeFrequency { get; set; }
        UpdatePriority UpdatePriority { get; set; }

        string VisibilityProvider { get; set; }
        bool IsVisible(IDictionary<string, object> sourceMetadata);

        string DynamicNodeProvider { get; set; }
        IEnumerable<DynamicNode> GetDynamicNodeCollection();
        bool HasDynamicNodeProvider { get; }

        bool Clickable { get; set; }
        string UrlResolver { get; set; }
        string Url { get; set; }
        string UnresolvedUrl { get; }
        string ResolvedUrl { get; }
        bool CacheResolvedUrl { get; set; }
        void ResolveUrl();
        bool IncludeAmbientValuesInUrl { get; set; }
        string Protocol { get; set; }
        string HostName { get; set; }
        bool HasAbsoluteUrl();
        bool HasExternalUrl(HttpContextBase httpContext);

        string CanonicalKey { get; set; }
        string CanonicalUrl { get; set; }
        string CanonicalUrlProtocol { get; set; }
        string CanonicalUrlHostName { get; set; }

        IMetaRobotsValueCollection MetaRobotsValues { get; }
        string GetMetaRobotsContentString();
        bool HasNoIndexAndNoFollow { get; }

        string Route { get; set; }
        IRouteValueDictionary RouteValues { get; }
        IPreservedRouteParameterCollection PreservedRouteParameters { get; }
        RouteData GetRouteData(HttpContextBase httpContext);
        bool MatchesRoute(IDictionary<string, object> routeValues);
        
        string Area { get; set; }
        string Controller { get; set; }
        string Action { get; set; }

        void CopyTo(ISiteMapNode node);
    }
}
