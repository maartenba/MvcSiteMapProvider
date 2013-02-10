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
        string Action { get; set; }
        string Area { get; set; }
        IDictionary<string, string> Attributes { get; }
        ChangeFrequency ChangeFrequency { get; set; }
        ISiteMapNodeCollection ChildNodes { get; }
        bool Clickable { get; set; }
        string Controller { get; set; }
        string Description { get; set; }
        string DynamicNodeProvider { get; set; }
        int GetNodeLevel();
        RouteData GetRouteData(HttpContextBase httpContext);
        bool HasChildNodes { get; }
        bool HasDynamicNodeProvider { get; }
        string HttpMethod { get; set; }
        string ImageUrl { get; set; }
        bool IsAccessibleToUser(HttpContext context);
        bool IsDescendantOf(ISiteMapNode node);
        bool IsDynamic { get; }
        bool IsInCurrentPath();
        bool IsVisible(HttpContext context, IDictionary<string, object> sourceMetadata);
        string Key { get; }
        DateTime LastModifiedDate { get; set; }
        ISiteMapNode NextSibling { get; }
        ISiteMapNode ParentNode { get; set; }
        IList<string> PreservedRouteParameters { get; }
        ISiteMapNode PreviousSibling { get; }
        string ResourceKey { get; }
        IList<string> Roles { get; }
        ISiteMapNode RootNode { get; }
        string Route { get; set; }
        IRouteValueCollection RouteValues { get; }
        ISiteMap SiteMap { get; }
        string TargetFrame { get; set; }
        string Title { get; set; }
        string UnresolvedUrl { get; }
        UpdatePriority UpdatePriority { get; set; }
        string Url { get; set; }
        string UrlResolver { get; set; }
        string VisibilityProvider { get; set; }

        // from IDynamicNodeProvider (possibly inherit?)
        IEnumerable<DynamicNode> GetDynamicNodeCollection();
    }
}
