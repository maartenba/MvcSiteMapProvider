using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider.Core.SiteMap.Builder;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LockableSiteMap
        : ISiteMap
    {
        public LockableSiteMap(
            ISiteMap siteMap
            )
        {
            if (siteMap == null)
                throw new ArgumentNullException("siteMap");

            this.innerSiteMap = siteMap;
        }

        private readonly ISiteMap innerSiteMap;


        #region ISiteMap Members

        public bool IsReadOnly { get; protected set; }

        public void AddNode(ISiteMapNode node)
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
            }
            this.innerSiteMap.AddNode(node);
        }

        public void AddNode(ISiteMapNode node, ISiteMapNode parentNode)
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
            }
            this.innerSiteMap.AddNode(node, parentNode);
        }

        public void RemoveNode(ISiteMapNode node)
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
            }
            this.innerSiteMap.RemoveNode(node);
        }

        public void Clear()
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
            }
            this.innerSiteMap.Clear();
        }

        public ISiteMapNode RootNode
        {
            get { return this.innerSiteMap.RootNode; }
        }

        public void BuildSiteMap(ISiteMap siteMap)
        {
            if (siteMap == null)
            {
                siteMap = this;
            }
            // Set the sitemap to read-write so we can populate it.
            this.IsReadOnly = false;
            this.innerSiteMap.BuildSiteMap(siteMap);
            // Set the sitemap to read-only so the nodes cannot be inadvertantly modified by the UI layer.
            this.IsReadOnly = true;
        }

        public ISiteMapNode CurrentNode
        {
            get { return this.innerSiteMap.CurrentNode; }
        }

        public bool EnableLocalization
        {
            get { return this.innerSiteMap.EnableLocalization; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
                }
                this.innerSiteMap.EnableLocalization = value;
            }
        }

        public ISiteMapNode FindSiteMapNode(string rawUrl)
        {
            return this.innerSiteMap.FindSiteMapNode(rawUrl);
        }

        public ISiteMapNode FindSiteMapNode(HttpContext context)
        {
            return this.innerSiteMap.FindSiteMapNode(context);
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
            return this.innerSiteMap.IsAccessibleToUser(context, node);
        }

        public string ResourceKey
        {
            get { return this.innerSiteMap.ResourceKey; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
                }
                this.innerSiteMap.ResourceKey = value;
            }
        }

        public bool SecurityTrimmingEnabled
        {
            get { return this.innerSiteMap.SecurityTrimmingEnabled; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
                }
                this.innerSiteMap.SecurityTrimmingEnabled = value;
            }
        }

        public ISiteMapNode FindSiteMapNode(ControllerContext context)
        {
            return this.innerSiteMap.FindSiteMapNode(context);
        }

        #endregion
    }
}
