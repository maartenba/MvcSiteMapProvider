using MvcSiteMapProvider.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for the specialized class that manages the hierarchical relationship between different 
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

        // Hide the BuildSiteMap method because it is only for use by MvcSiteMapProvider.
        [EditorBrowsable(EditorBrowsableState.Never)]
        void BuildSiteMap();

        ISiteMapNode CurrentNode { get; }
        bool EnableLocalization { get; }
        ISiteMapNode FindSiteMapNode(string rawUrl);
        ISiteMapNode FindSiteMapNodeFromCurrentContext();
        ISiteMapNode FindSiteMapNode(ControllerContext context);
        ISiteMapNode FindSiteMapNodeFromKey(string key);
        ISiteMapNodeCollection GetChildNodes(ISiteMapNode node);
        ISiteMapNodeCollection GetDescendants(ISiteMapNode node);
        ISiteMapNodeCollection GetAncestors(ISiteMapNode node);
        ISiteMapNode GetCurrentNodeAndHintAncestorNodes(int upLevel);
        ISiteMapNode GetCurrentNodeAndHintNeighborhoodNodes(int upLevel, int downLevel);
        ISiteMapNode GetParentNode(ISiteMapNode node);
        ISiteMapNode GetParentNodeRelativeToCurrentNodeAndHintDownFromParent(int walkupLevels, int relativeDepthFromWalkup);
        ISiteMapNode GetParentNodeRelativeToNodeAndHintDownFromParent(ISiteMapNode node, int walkupLevels, int relativeDepthFromWalkup);
        void HintAncestorNodes(ISiteMapNode node, int upLevel);
        void HintNeighborhoodNodes(ISiteMapNode node, int upLevel, int downLevel);
        bool IsAccessibleToUser(ISiteMapNode node);
        string CacheKey { get; }
        string ResourceKey { get; set; } // TODO: Remove setter in version 5.
        bool SecurityTrimmingEnabled { get; }
        bool UseTitleIfDescriptionNotProvided { get; }
        bool VisibilityAffectsDescendants { get; }
        Type ResolveControllerType(string areaName, string controllerName);

        [Obsolete("ResolveActionMethodParameters is deprecated and will be removed in version 5.")]
        IEnumerable<string> ResolveActionMethodParameters(string areaName, string controllerName, string actionMethodName);
    }

    public static class SiteMapExtensions
    {
        public static string GetUrlContextKey(this ISiteMap siteMap)
        {
            return "__MVCSITEMAP_" + siteMap.CacheKey + "_UrlContext_" + siteMap.IsReadOnly.ToString() + "_";
        }
    }
}
