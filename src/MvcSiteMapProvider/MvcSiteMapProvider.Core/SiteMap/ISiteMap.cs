using System;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMap
    {
        // ISiteMapProvider members
        ISiteMapNode CurrentNode { get; }
        bool EnableLocalization { get; set; }
        ISiteMapNode FindSiteMapNode(string rawUrl);
        ISiteMapNode FindSiteMapNode(System.Web.HttpContext context);
        ISiteMapNode FindSiteMapNodeFromKey(string key);
        SiteMapNodeCollection GetChildNodes(ISiteMapNode node);
        ISiteMapNode GetCurrentNodeAndHintAncestorNodes(int upLevel);
        ISiteMapNode GetCurrentNodeAndHintNeighborhoodNodes(int upLevel, int downLevel);
        ISiteMapNode GetParentNode(ISiteMapNode node);
        ISiteMapNode GetParentNodeRelativeToCurrentNodeAndHintDownFromParent(int walkupLevels, int relativeDepthFromWalkup);
        ISiteMapNode GetParentNodeRelativeToNodeAndHintDownFromParent(ISiteMapNode node, int walkupLevels, int relativeDepthFromWalkup);
        void HintAncestorNodes(ISiteMapNode node, int upLevel);
        void HintNeighborhoodNodes(ISiteMapNode node, int upLevel, int downLevel);
        //void Initialize(string name, System.Collections.Specialized.NameValueCollection attributes);
        bool IsAccessibleToUser(System.Web.HttpContext context, ISiteMapNode node);
        //SiteMap ParentProvider { get; set; }
        string ResourceKey { get; set; }
        ISiteMapNode RootNode { get; }
        //System.Web.SiteMapProvider RootProvider { get; }
        bool SecurityTrimmingEnabled { get; set; }
        //event System.Web.SiteMapResolveEventHandler SiteMapResolve;

        // IStaticSiteMapProvider members

        ISiteMapNode BuildSiteMap();
        //System.Web.SiteMapNode FindSiteMapNode(string rawUrl);
        //System.Web.SiteMapNode FindSiteMapNodeFromKey(string key);
        //System.Web.SiteMapNodeCollection GetChildNodes(System.Web.SiteMapNode node);
        //System.Web.SiteMapNode GetParentNode(System.Web.SiteMapNode node);

        // IDefaultSiteMapProvider members
        //MvcSiteMapProvider.Extensibility.IAclModule AclModule { get; set; }
        //MvcSiteMapProvider.Extensibility.IActionMethodParameterResolver ActionMethodParameterResolver { get; set; }
        //System.Web.SiteMapNode BuildSiteMap(); // From IStaticSiteMapProvider
        //int CacheDuration { get; }
        //MvcSiteMapProvider.Extensibility.IControllerTypeResolver ControllerTypeResolver { get; set; }
        //System.Web.SiteMapNode CurrentNode { get; } // From ISiteMapProvider
        //System.Web.SiteMapNode FindSiteMapNode(System.Web.HttpContext context); // From ISiteMapProvider
        ISiteMapNode FindSiteMapNode(System.Web.Mvc.ControllerContext context);
        //void Initialize(string name, System.Collections.Specialized.NameValueCollection attributes); // From ISiteMapProvider
        //bool IsAccessibleToUser(System.Web.HttpContext context, System.Web.SiteMapNode node); // From ISiteMapProvider
        //MvcSiteMapProvider.Extensibility.INodeKeyGenerator NodeKeyGenerator { get; set; }
        //void Refresh();
        //System.Web.SiteMapNode RootNode { get; } // From ISiteMapProvider
        //MvcSiteMapProvider.Extensibility.ISiteMapNodeUrlResolver SiteMapNodeUrlResolver { get; set; }
        //MvcSiteMapProvider.Extensibility.ISiteMapNodeVisibilityProvider SiteMapNodeVisibilityProvider { get; set; }
        //MvcSiteMapProvider.Extensibility.ISiteMapProviderEventHandler SiteMapProviderEventHandler { get; set; }


        // Data structure management
        void AddNode(ISiteMapNode node);
        void AddNode(ISiteMapNode node, ISiteMapNode parentNode);
        void RemoveNode(ISiteMapNode node);
        void Clear();
    }
}
