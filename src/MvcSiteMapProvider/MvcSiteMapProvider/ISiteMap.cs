using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for the specialized class that manages the hierarchial relationship between different 
    /// <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> instances.
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
        void BuildSiteMap();

        ISiteMapNode CurrentNode { get; }
        bool EnableLocalization { get; set; }
        ISiteMapNode FindSiteMapNode(string rawUrl);
        ISiteMapNode FindSiteMapNodeFromCurrentContext();
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
        bool IsAccessibleToUser(ISiteMapNode node);
        string ResourceKey { get; set; }
        bool SecurityTrimmingEnabled { get; set; }
        Type ResolveControllerType(string areaName, string controllerName);
        IEnumerable<string> ResolveActionMethodParameters(string areaName, string controllerName, string actionMethodName);
    }
}
