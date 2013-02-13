using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class SiteMapNodeSecurityBase
        : ISiteMapNode
    {
        #region Node Map Positioning

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        public abstract ISiteMapNode ParentNode { get; set; }

        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <value>
        /// The child nodes.
        /// </value>
        public abstract ISiteMapNodeCollection ChildNodes { get; }

        /// <summary>
        /// Gets a value indicating whether the current site map node is a child or a direct descendant of the specified node.
        /// </summary>
        /// <param name="node">The <see cref="T:MvcSiteMapProvider.Core.SiteMap.ISiteMapNode"/> to check if the current node is a child or descendant of.</param>
        /// <returns>true if the current node is a child or descendant of the specified node; otherwise, false.</returns>
        public abstract bool IsDescendantOf(ISiteMapNode node);

        /// <summary>
        /// Gets the next sibling in the map relative to this node.
        /// </summary>
        /// <value>
        /// The sibling node.
        /// </value>
        public abstract ISiteMapNode NextSibling { get; }

        /// <summary>
        /// Gets the previous sibling in the map relative to this node.
        /// </summary>
        /// <value>
        /// The sibling node.
        /// </value>
        public abstract ISiteMapNode PreviousSibling { get; }

        /// <summary>
        /// Gets the root node in the current map.
        /// </summary>
        /// <value>
        /// The root node.
        /// </value>
        public abstract ISiteMapNode RootNode { get; }

        ///// <summary>
        ///// Gets the sibling nodes relative to the current node.
        ///// </summary>
        ///// <value>
        ///// The sibling nodes.
        ///// </value>
        //protected abstract ISiteMapNodeCollection SiblingNodes { get; }

        // TODO: rework... (maartenba)

        /// <summary>
        /// Determines whether the specified node is in current path.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the specified node is in current path; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsInCurrentPath();

        /// <summary>
        /// Gets a value indicating whether the current SiteMapNode has any child nodes.
        /// </summary>
        public abstract bool HasChildNodes { get; }

        /// <summary>
        /// Gets the level of the current SiteMapNode
        /// </summary>
        /// <returns>The level of the current SiteMapNode</returns>
        public abstract int GetNodeLevel();

        /// <summary>
        /// A reference to the root SiteMap object for the current graph.
        /// </summary>
        public abstract ISiteMap SiteMap { get; }

        #endregion

        #region Security

        /// <summary>
        /// Determines whether the current node is accessible to the current user based on context.
        /// </summary>
        /// <value>
        /// True if the current node is accessible.
        /// </value>
        public virtual bool IsAccessibleToUser(HttpContext context)
        {
            return this.SiteMap.IsAccessibleToUser(context, this);
        }

        #endregion


        #region ISiteMapNode members


        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public abstract string Key { get; }

        /// <summary>
        /// Gets whether the current node was created from a dynamic source.
        /// </summary>
        /// <value>True if the current node is dynamic.</value>
        public abstract bool IsDynamic { get; }

        /// <summary>
        /// Gets whether the current node is read-only.
        /// </summary>
        /// <value>True if the current node is read-only.</value>
        public abstract bool IsReadOnly { get; }

        public abstract string HttpMethod { get; set; }

        public abstract string ResourceKey { get; }

        public abstract string Title { get; set; }

        public abstract string Description { get; set; }

        public abstract string TargetFrame { get; set; }

        public abstract string ImageUrl { get; set; }

        public abstract IAttributeCollection Attributes { get; }

        public abstract System.Collections.Generic.IList<string> Roles { get; }

        public abstract DateTime LastModifiedDate { get; set; }

        public abstract ChangeFrequency ChangeFrequency { get; set; }

        public abstract UpdatePriority UpdatePriority { get; set; }

        public abstract string VisibilityProvider { get; set; }

        public abstract bool IsVisible(HttpContext context, IDictionary<string, object> sourceMetadata);

        public abstract bool Clickable { get; set; }

        public abstract string UrlResolver { get; set; }

        public abstract string Url { get; set; }

        public abstract string UnresolvedUrl { get; }

        public abstract string DynamicNodeProvider { get; set; }

        public abstract IEnumerable<DynamicNode> GetDynamicNodeCollection();

        public abstract bool HasDynamicNodeProvider { get; }

        public abstract string Route { get; set; }

        public abstract IRouteValueCollection RouteValues { get; }

        public abstract IList<string> PreservedRouteParameters { get; }

        public abstract RouteData GetRouteData(HttpContextBase httpContext);

        public abstract bool MatchesRoute(IDictionary<string, object> routeValues);

        public abstract string Area { get; set; }

        public abstract string Controller { get; set; }

        public abstract string Action { get; set; }

        #endregion
    }
}
