// -----------------------------------------------------------------------
// <copyright file="ISiteMap.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMap
    {
        // ISiteMapProvider members
        System.Web.SiteMapNode CurrentNode { get; }
        bool EnableLocalization { get; set; }
        System.Web.SiteMapNode FindSiteMapNode(string rawUrl);
        System.Web.SiteMapNode FindSiteMapNode(System.Web.HttpContext context);
        System.Web.SiteMapNode FindSiteMapNodeFromKey(string key);
        System.Web.SiteMapNodeCollection GetChildNodes(System.Web.SiteMapNode node);
        System.Web.SiteMapNode GetCurrentNodeAndHintAncestorNodes(int upLevel);
        System.Web.SiteMapNode GetCurrentNodeAndHintNeighborhoodNodes(int upLevel, int downLevel);
        System.Web.SiteMapNode GetParentNode(System.Web.SiteMapNode node);
        System.Web.SiteMapNode GetParentNodeRelativeToCurrentNodeAndHintDownFromParent(int walkupLevels, int relativeDepthFromWalkup);
        System.Web.SiteMapNode GetParentNodeRelativeToNodeAndHintDownFromParent(System.Web.SiteMapNode node, int walkupLevels, int relativeDepthFromWalkup);
        void HintAncestorNodes(System.Web.SiteMapNode node, int upLevel);
        void HintNeighborhoodNodes(System.Web.SiteMapNode node, int upLevel, int downLevel);
        void Initialize(string name, System.Collections.Specialized.NameValueCollection attributes);
        bool IsAccessibleToUser(System.Web.HttpContext context, System.Web.SiteMapNode node);
        SiteMap ParentProvider { get; set; }
        string ResourceKey { get; set; }
        System.Web.SiteMapNode RootNode { get; }
        //System.Web.SiteMapProvider RootProvider { get; }
        bool SecurityTrimmingEnabled { get; }
        //event System.Web.SiteMapResolveEventHandler SiteMapResolve;

        // IStaticSiteMapProvider members
        System.Web.SiteMapNode BuildSiteMap();
        //System.Web.SiteMapNode FindSiteMapNode(string rawUrl);
        //System.Web.SiteMapNode FindSiteMapNodeFromKey(string key);
        //System.Web.SiteMapNodeCollection GetChildNodes(System.Web.SiteMapNode node);
        //System.Web.SiteMapNode GetParentNode(System.Web.SiteMapNode node);

        // IDefaultSiteMapProvider members
        //MvcSiteMapProvider.Extensibility.IAclModule AclModule { get; set; }
        //MvcSiteMapProvider.Extensibility.IActionMethodParameterResolver ActionMethodParameterResolver { get; set; }
        //System.Web.SiteMapNode BuildSiteMap(); // From IStaticSiteMapProvider
        int CacheDuration { get; }
        //MvcSiteMapProvider.Extensibility.IControllerTypeResolver ControllerTypeResolver { get; set; }
        //System.Web.SiteMapNode CurrentNode { get; } // From ISiteMapProvider
        //System.Web.SiteMapNode FindSiteMapNode(System.Web.HttpContext context); // From ISiteMapProvider
        System.Web.SiteMapNode FindSiteMapNode(System.Web.Mvc.ControllerContext context);
        //void Initialize(string name, System.Collections.Specialized.NameValueCollection attributes); // From ISiteMapProvider
        //bool IsAccessibleToUser(System.Web.HttpContext context, System.Web.SiteMapNode node); // From ISiteMapProvider
        //MvcSiteMapProvider.Extensibility.INodeKeyGenerator NodeKeyGenerator { get; set; }
        void Refresh();
        //System.Web.SiteMapNode RootNode { get; } // From ISiteMapProvider
        //MvcSiteMapProvider.Extensibility.ISiteMapNodeUrlResolver SiteMapNodeUrlResolver { get; set; }
        //MvcSiteMapProvider.Extensibility.ISiteMapNodeVisibilityProvider SiteMapNodeVisibilityProvider { get; set; }
        //MvcSiteMapProvider.Extensibility.ISiteMapProviderEventHandler SiteMapProviderEventHandler { get; set; }
    }
}
