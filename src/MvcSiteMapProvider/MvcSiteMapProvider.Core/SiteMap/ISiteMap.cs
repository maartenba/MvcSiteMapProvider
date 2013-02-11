using System;
using System.Web;
using System.Web.Mvc;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMap
    {
        // Data structure management
        bool IsReadOnly { get; }
        void AddNode(ISiteMapNode node);
        void AddNode(ISiteMapNode node, ISiteMapNode parentNode);
        void RemoveNode(ISiteMapNode node);
        void Clear();
        ISiteMapNode RootNode { get; }
        void BuildSiteMap(ISiteMap siteMap);

        ISiteMapNode CurrentNode { get; }
        bool EnableLocalization { get; set; }
        ISiteMapNode FindSiteMapNode(string rawUrl);
        ISiteMapNode FindSiteMapNode(HttpContext context);
        ISiteMapNode FindSiteMapNode(ControllerContext context);
        ISiteMapNode FindSiteMapNodeFromKey(string key);
        ISiteMapNodeCollection GetChildNodes(ISiteMapNode node);
        ISiteMapNode GetCurrentNodeAndHintAncestorNodes(int upLevel);
        ISiteMapNode GetCurrentNodeAndHintNeighborhoodNodes(int upLevel, int downLevel);
        ISiteMapNode GetParentNode(ISiteMapNode node);
        ISiteMapNode GetParentNodeRelativeToCurrentNodeAndHintDownFromParent(int walkupLevels, int relativeDepthFromWalkup);
        ISiteMapNode GetParentNodeRelativeToNodeAndHintDownFromParent(ISiteMapNode node, int walkupLevels, int relativeDepthFromWalkup);
        void HintAncestorNodes(ISiteMapNode node, int upLevel);
        void HintNeighborhoodNodes(ISiteMapNode node, int upLevel, int downLevel);
        bool IsAccessibleToUser(HttpContext context, ISiteMapNode node);
        string ResourceKey { get; set; }
        bool SecurityTrimmingEnabled { get; set; }
    }
}
