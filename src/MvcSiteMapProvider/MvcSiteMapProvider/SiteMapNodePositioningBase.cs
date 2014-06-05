using System;
using System.Linq;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// An abstract base class that contains methods that deal with locating the position of the current node within
    /// the site map.
    /// </summary>
    public abstract class SiteMapNodePositioningBase
        : SiteMapNodeSecurityBase
    {
        #region Node Map Positioning

        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        public override ISiteMapNode ParentNode
        {
            get { return this.SiteMap.GetParentNode(this); }
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
        /// Gets the descendant nodes.
        /// </summary>
        /// <value>
        /// The descendant nodes.
        /// </value>
        public override ISiteMapNodeCollection Descendants
        {
            get { return this.SiteMap.GetDescendants(this); }
        }

        /// <summary>
        /// Gets the ancestor nodes.
        /// </summary>
        /// <value>
        /// The ancestor nodes.
        /// </value>
        public override ISiteMapNodeCollection Ancestors
        {
            get { return this.SiteMap.GetAncestors(this); }
        }

        /// <summary>
        /// Gets a value indicating whether the current site map node is a child or a direct descendant of the specified node.
        /// </summary>
        /// <param name="node">The <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> to check if the current node is a child or descendant of.</param>
        /// <returns>true if the current node is a child or descendant of the specified node; otherwise, false.</returns>
        public override bool IsDescendantOf(ISiteMapNode node)
        {
            for (var parent = this.ParentNode; parent != null; parent = parent.ParentNode)
            {
                if (parent.Equals(node))
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
            get { return this.SiteMap.RootNode; }
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

        /// <summary>
        /// Determines whether the specified node is in current path.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the specified node is in current path; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsInCurrentPath()
        {
            ISiteMapNode node = this;
            return (this.SiteMap.CurrentNode != null && (this.SiteMap.CurrentNode.Equals(node) || this.SiteMap.CurrentNode.IsDescendantOf(node)));
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

        /// <summary>
        /// Gets or sets the display sort order for the node relative to its sibling nodes.
        /// </summary>
        public override int Order { get; set; }

        #endregion
    }
}
