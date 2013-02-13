using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class SiteMapNodePositioningBase
        : SiteMapNodeSecurityBase
    {
        protected ISiteMapNode parentNode;
        protected bool isParentNodeSet = false;


        #region Node Map Positioning

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        public override ISiteMapNode ParentNode
        {
            get
            {
                if (this.isParentNodeSet)
                {
                    return this.parentNode;
                }
                return this.SiteMap.GetParentNode(this);
            }
            set
            {
                this.parentNode = value;
                this.isParentNodeSet = true;
            }
        }

        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <value>
        /// The child nodes.
        /// </value>
        public override ISiteMapNodeCollection ChildNodes
        {
            get { return this.SiteMap.GetChildNodes(this); }
        }

        /// <summary>
        /// Gets a value indicating whether the current site map node is a child or a direct descendant of the specified node.
        /// </summary>
        /// <param name="node">The <see cref="T:MvcSiteMapProvider.Core.SiteMap.ISiteMapNode"/> to check if the current node is a child or descendant of.</param>
        /// <returns>true if the current node is a child or descendant of the specified node; otherwise, false.</returns>
        public override bool IsDescendantOf(ISiteMapNode node)
        {
            for (var node2 = this.ParentNode; node2 != null; node2 = node2.ParentNode)
            {
                if (node2.Equals(node))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the next sibling in the map relative to this node.
        /// </summary>
        /// <value>
        /// The sibling node.
        /// </value>
        public override ISiteMapNode NextSibling
        {
            get
            {
                var siblingNodes = this.SiblingNodes;
                if (siblingNodes != null)
                {
                    int index = siblingNodes.IndexOf(this);
                    if ((index >= 0) && (index < (siblingNodes.Count - 1)))
                    {
                        return siblingNodes[index + 1];
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the previous sibling in the map relative to this node.
        /// </summary>
        /// <value>
        /// The sibling node.
        /// </value>
        public override ISiteMapNode PreviousSibling
        {
            get
            {
                var siblingNodes = this.SiblingNodes;
                if (siblingNodes != null)
                {
                    int index = siblingNodes.IndexOf(this);
                    if ((index > 0) && (index <= (siblingNodes.Count - 1)))
                    {
                        return siblingNodes[index - 1];
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the root node in the current map.
        /// </summary>
        /// <value>
        /// The root node.
        /// </value>
        public override ISiteMapNode RootNode
        {
            get
            {
                var rootNode = this.SiteMap.RootNode;
                if (rootNode == null)
                {
                    throw new InvalidOperationException(Resources.Messages.SiteMapInvalidRootNode);
                }
                return rootNode;
            }
        }

        /// <summary>
        /// Gets the sibling nodes relative to the current node.
        /// </summary>
        /// <value>
        /// The sibling nodes.
        /// </value>
        protected virtual ISiteMapNodeCollection SiblingNodes
        {
            get
            {
                var parentNode = this.ParentNode;
                if (parentNode != null)
                {
                    return parentNode.ChildNodes;
                }
                return null;
            }
        }

        // TODO: rework... (maartenba)

        /// <summary>
        /// Determines whether the specified node is in current path.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the specified node is in current path; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsInCurrentPath()
        {
            ISiteMapNode node = this;
            return (this.SiteMap.CurrentNode != null && (node == this.SiteMap.CurrentNode || this.SiteMap.CurrentNode.IsDescendantOf(node)));
        }

        /// <summary>
        /// Gets a value indicating whether the current SiteMapNode has any child nodes.
        /// </summary>
        public override bool HasChildNodes
        {
            get
            {
                var childNodes = this.ChildNodes;
                return ((childNodes != null) && (childNodes.Count > 0));
            }
        }

        /// <summary>
        /// Gets the level of the current SiteMapNode
        /// </summary>
        /// <returns>The level of the current SiteMapNode</returns>
        public override int GetNodeLevel()
        {
            var level = 0;
            ISiteMapNode node = this;

            if (node != null)
            {
                while (node.ParentNode != null)
                {
                    level++;
                    node = node.ParentNode;
                }
            }
            return level;
        }

        #endregion
    }
}
