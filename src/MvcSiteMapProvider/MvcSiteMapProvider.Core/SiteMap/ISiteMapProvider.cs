//using System;
//namespace MvcSiteMapProvider.Web
//{
//    interface ISiteMapProvider
//    {
//        System.Web.SiteMapNode CurrentNode { get; }
//        bool EnableLocalization { get; set; }
//        System.Web.SiteMapNode FindSiteMapNode(string rawUrl);
//        System.Web.SiteMapNode FindSiteMapNode(System.Web.HttpContext context);
//        System.Web.SiteMapNode FindSiteMapNodeFromKey(string key);
//        System.Web.SiteMapNodeCollection GetChildNodes(System.Web.SiteMapNode node);
//        System.Web.SiteMapNode GetCurrentNodeAndHintAncestorNodes(int upLevel);
//        System.Web.SiteMapNode GetCurrentNodeAndHintNeighborhoodNodes(int upLevel, int downLevel);
//        System.Web.SiteMapNode GetParentNode(System.Web.SiteMapNode node);
//        System.Web.SiteMapNode GetParentNodeRelativeToCurrentNodeAndHintDownFromParent(int walkupLevels, int relativeDepthFromWalkup);
//        System.Web.SiteMapNode GetParentNodeRelativeToNodeAndHintDownFromParent(System.Web.SiteMapNode node, int walkupLevels, int relativeDepthFromWalkup);
//        void HintAncestorNodes(System.Web.SiteMapNode node, int upLevel);
//        void HintNeighborhoodNodes(System.Web.SiteMapNode node, int upLevel, int downLevel);
//        void Initialize(string name, System.Collections.Specialized.NameValueCollection attributes);
//        bool IsAccessibleToUser(System.Web.HttpContext context, System.Web.SiteMapNode node);
//        //System.Web.SiteMapProvider ParentProvider { get; set; }
//        string ResourceKey { get; set; }
//        System.Web.SiteMapNode RootNode { get; }
//        //System.Web.SiteMapProvider RootProvider { get; }
//        bool SecurityTrimmingEnabled { get; }
//        event System.Web.SiteMapResolveEventHandler SiteMapResolve;
//    }
//}
