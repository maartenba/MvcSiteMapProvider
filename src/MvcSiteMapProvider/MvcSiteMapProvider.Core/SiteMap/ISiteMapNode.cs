// -----------------------------------------------------------------------
// <copyright file="ISiteMapNode.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap
{
    using System;
    using System.Collections.Generic;
    using MvcSiteMapProvider.Core;
    using System.Web;
    using System.Web.Routing;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapNode
        //: ICloneable
    {
        string Action { get; set; }
        string Area { get; set; }
        IDictionary<string, string> Attributes { get; set; }
        ChangeFrequency ChangeFrequency { get; set; }
        SiteMapNodeCollection ChildNodes { get; set; }
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
        IList<string> PreservedRouteParameters { get; set; }
        ISiteMapNode PreviousSibling { get; }
        string ResourceKey { get; }
        IList<string> Roles { get; set; }
        ISiteMapNode RootNode { get; }
        string Route { get; set; }
        IDictionary<string, object> RouteValues { get; set; }
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
