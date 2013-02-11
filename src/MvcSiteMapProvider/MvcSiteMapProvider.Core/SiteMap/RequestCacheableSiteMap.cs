using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using MvcSiteMapProvider.Core.RequestCache;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RequestCacheableSiteMap
        : ISiteMap
    {
        public RequestCacheableSiteMap(
            ISiteMap siteMap,
            IRequestCache requestCache
            )
        {
            if (siteMap == null)
                throw new ArgumentNullException("siteMap");
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.innerSiteMap = siteMap;
            this.requestCache = requestCache;
        }

        private readonly ISiteMap innerSiteMap;
        private readonly IRequestCache requestCache;
        private readonly Guid instanceId = Guid.NewGuid();

        #region ISiteMap Members

        public bool IsReadOnly
        {
            get { return this.innerSiteMap.IsReadOnly; }
        }

        public void AddNode(ISiteMapNode node)
        {
            this.innerSiteMap.AddNode(node);
        }

        public void AddNode(ISiteMapNode node, ISiteMapNode parentNode)
        {
            this.innerSiteMap.AddNode(node, parentNode);
        }

        public void RemoveNode(ISiteMapNode node)
        {
            this.innerSiteMap.RemoveNode(node);
        }

        public void Clear()
        {
            this.innerSiteMap.Clear();
        }

        public ISiteMapNode RootNode
        {
            get { return this.innerSiteMap.RootNode; }
        }

        public ISiteMapNode BuildSiteMap()
        {
            return this.innerSiteMap.BuildSiteMap();
        }

        public ISiteMapNode CurrentNode
        {
            get 
            {
                var key = this.GetCacheKey("CurrentNode");
                var result = this.requestCache.GetValue<ISiteMapNode>(key);
                if (result == null)
                {
                    result = this.innerSiteMap.CurrentNode;
                    this.requestCache.SetValue<ISiteMapNode>(key, result);
                }
                return result;
            }
        }

        public bool EnableLocalization
        {
            get { return this.innerSiteMap.EnableLocalization; }
            set { this.innerSiteMap.EnableLocalization = value; }
        }

        public ISiteMapNode FindSiteMapNode(string rawUrl)
        {
            var key = this.GetCacheKey("FindSiteMapNode_" + rawUrl);
            var result = this.requestCache.GetValue<ISiteMapNode>(key);
            if (result == null)
            {
                result = this.innerSiteMap.FindSiteMapNode(rawUrl);
                this.requestCache.SetValue<ISiteMapNode>(key, result);
            }
            return result;
        }

        public ISiteMapNode FindSiteMapNode(HttpContext context)
        {
            var key = this.GetCacheKey("FindSiteMapNode_HttpContext");
            var result = this.requestCache.GetValue<ISiteMapNode>(key);
            if (result == null)
            {
                result = this.innerSiteMap.FindSiteMapNode(context);
                this.requestCache.SetValue<ISiteMapNode>(key, result);
            }
            return result;
        }

        public ISiteMapNode FindSiteMapNode(ControllerContext context)
        {
            var key = this.GetCacheKey("FindSiteMapNode_ControllerContext" + this.GetRouteDataValues(context.RouteData));
            var result = this.requestCache.GetValue<ISiteMapNode>(key);
            if (result == null)
            {
                result = this.innerSiteMap.FindSiteMapNode(context);
                this.requestCache.SetValue<ISiteMapNode>(key, result);
            }
            return result;
        }

        public ISiteMapNode FindSiteMapNodeFromKey(string key)
        {
            return this.innerSiteMap.FindSiteMapNodeFromKey(key);
        }

        public ISiteMapNodeCollection GetChildNodes(ISiteMapNode node)
        {
            return this.innerSiteMap.GetChildNodes(node);
        }

        public ISiteMapNode GetCurrentNodeAndHintAncestorNodes(int upLevel)
        {
            return this.innerSiteMap.GetCurrentNodeAndHintAncestorNodes(upLevel);
        }

        public ISiteMapNode GetCurrentNodeAndHintNeighborhoodNodes(int upLevel, int downLevel)
        {
            return this.innerSiteMap.GetCurrentNodeAndHintNeighborhoodNodes(upLevel, downLevel);
        }

        public ISiteMapNode GetParentNode(ISiteMapNode node)
        {
            return this.innerSiteMap.GetParentNode(node);
        }

        public ISiteMapNode GetParentNodeRelativeToCurrentNodeAndHintDownFromParent(int walkupLevels, int relativeDepthFromWalkup)
        {
            return this.innerSiteMap.GetParentNodeRelativeToCurrentNodeAndHintDownFromParent(walkupLevels, relativeDepthFromWalkup);
        }

        public ISiteMapNode GetParentNodeRelativeToNodeAndHintDownFromParent(ISiteMapNode node, int walkupLevels, int relativeDepthFromWalkup)
        {
            return this.innerSiteMap.GetParentNodeRelativeToNodeAndHintDownFromParent(node, walkupLevels, relativeDepthFromWalkup);
        }

        public void HintAncestorNodes(ISiteMapNode node, int upLevel)
        {
            this.innerSiteMap.HintAncestorNodes(node, upLevel);
        }

        public void HintNeighborhoodNodes(ISiteMapNode node, int upLevel, int downLevel)
        {
            this.innerSiteMap.HintNeighborhoodNodes(node, upLevel, downLevel);
        }

        public bool IsAccessibleToUser(HttpContext context, ISiteMapNode node)
        {
            // TODO: find out what happens if we cast null to boolean
            var key = this.GetCacheKey("IsAccessibleToUser_" + node.Key);
            var result = this.requestCache.GetValue<bool>(key);
            if (result == null)
            {
                result = this.innerSiteMap.IsAccessibleToUser(context, node);
                this.requestCache.SetValue<bool>(key, result);
            }
            return result;
        }

        public string ResourceKey
        {
            get { return this.innerSiteMap.ResourceKey; }
            set { this.innerSiteMap.ResourceKey = value; }
        }

        public bool SecurityTrimmingEnabled
        {
            get { return this.innerSiteMap.SecurityTrimmingEnabled; }
            set { this.innerSiteMap.SecurityTrimmingEnabled = value; }
        }

        #endregion

        #region Private Methods

        private string GetCacheKey(string memberName)
        {
            return "__MVCSITEMAP_" + memberName + "_" + this.instanceId.ToString();
        }

        private string GetRouteDataValues(RouteData routeData)
        {
            var builder = new StringBuilder();
            foreach (var value in routeData.Values)
            {
                builder.Append(value.Key);
                builder.Append("_");
                builder.Append(value.Value);
                builder.Append("|");
            }
            return builder.ToString();
        }

        #endregion

    }
}
