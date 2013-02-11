using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Core;
using System.Web;
using System.Web.Routing;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapNode
        //: ICloneable
    {
        string Key { get; }
        bool IsDynamic { get; }
        bool IsReadOnly { get; }

        ISiteMapNode ParentNode { get; set; }
        ISiteMapNodeCollection ChildNodes { get; }
        bool IsDescendantOf(ISiteMapNode node);
        ISiteMapNode NextSibling { get; }
        ISiteMapNode PreviousSibling { get; }
        ISiteMapNode RootNode { get; }
        bool IsInCurrentPath();
        bool HasChildNodes { get; }
        int GetNodeLevel();
        ISiteMap SiteMap { get; }

        bool IsAccessibleToUser(HttpContext context);
        string HttpMethod { get; set; }
        string ResourceKey { get; }
        string Title { get; set; }
        string Description { get; set; }
        string TargetFrame { get; set; }
        string ImageUrl { get; set; }
        IAttributeCollection Attributes { get; }
        IList<string> Roles { get; }

        DateTime LastModifiedDate { get; set; }
        ChangeFrequency ChangeFrequency { get; set; }
        UpdatePriority UpdatePriority { get; set; }

        string VisibilityProvider { get; set; }
        bool IsVisible(HttpContext context, IDictionary<string, object> sourceMetadata);

        bool Clickable { get; set; }
        string UrlResolver { get; set; }
        string Url { get; set; }
        string UnresolvedUrl { get; }

        string DynamicNodeProvider { get; set; }
        IEnumerable<DynamicNode> GetDynamicNodeCollection();
        bool HasDynamicNodeProvider { get; }

        string Route { get; set; }
        IRouteValueCollection RouteValues { get; }
        IList<string> PreservedRouteParameters { get; }
        RouteData GetRouteData(HttpContextBase httpContext);
        bool MatchesRoute(IDictionary<string, object> routeValues);

        string Area { get; set; }
        string Controller { get; set; }
        string Action { get; set; }
    }
}
