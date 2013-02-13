using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// This class implements the decorator pattern and can be used to make an instance of ISiteMapNode read-only after the BuildSiteMap method has been called.
    /// </summary>
    public class LockableSiteMapNode
        : ISiteMapNode
    {
        public LockableSiteMapNode(
            ISiteMapNode siteMapNode
            )
        {
            if (siteMapNode == null)
                throw new ArgumentNullException("siteMapNode");

            this.innerSiteMapNode = siteMapNode;
        }

        private readonly ISiteMapNode innerSiteMapNode;



        #region ISiteMapNode Members

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key
        {
            get { return this.innerSiteMapNode.Key; }
        }

        /// <summary>
        /// Gets whether the current node was created from a dynamic source.
        /// </summary>
        /// <value>True if the current node is dynamic.</value>
        public bool IsDynamic
        {
            get { return this.innerSiteMapNode.IsDynamic; }
        }

        /// <summary>
        /// Gets whether the current node is read-only.
        /// </summary>
        /// <value>True if the current node is read-only.</value>
        public bool IsReadOnly
        {
            get { return this.SiteMap.IsReadOnly; }
        }

        #region Node Map Positioning

        public ISiteMapNode ParentNode
        {
            get { return this.innerSiteMapNode.ParentNode; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "ParentNode"));
                }
                this.innerSiteMapNode.ParentNode = value;
            }
        }

        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <value>
        /// The child nodes.
        /// </value>
        public ISiteMapNodeCollection ChildNodes
        {
            get { return this.SiteMap.GetChildNodes(this); }
        }

        /// <summary>
        /// Gets a value indicating whether the current site map node is a child or a direct descendant of the specified node.
        /// </summary>
        /// <param name="node">The <see cref="T:MvcSiteMapProvider.Core.SiteMap.ISiteMapNode"/> to check if the current node is a child or descendant of.</param>
        /// <returns><c>true</c> if the current node is a child or descendant of the specified node; otherwise, <c>false</c>.</returns>
        public bool IsDescendantOf(ISiteMapNode node)
        {
            return this.innerSiteMapNode.IsDescendantOf(node);
        }

        /// <summary>
        /// Gets the next sibling in the map relative to this node.
        /// </summary>
        /// <value>
        /// The sibling node.
        /// </value>
        public ISiteMapNode NextSibling
        {
            get { return this.innerSiteMapNode.NextSibling; }
        }

        /// <summary>
        /// Gets the previous sibling in the map relative to this node.
        /// </summary>
        /// <value>
        /// The sibling node.
        /// </value>
        public ISiteMapNode PreviousSibling
        {
            get { return this.innerSiteMapNode.PreviousSibling; }
        }

        /// <summary>
        /// Gets the root node in the current map.
        /// </summary>
        /// <value>
        /// The root node.
        /// </value>
        public ISiteMapNode RootNode
        {
            get { return this.innerSiteMapNode.RootNode; }
        }

        /// <summary>
        /// Determines whether the specified node is in current path.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the specified node is in current path; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInCurrentPath()
        {
            ISiteMapNode node = this;
            return (this.SiteMap.CurrentNode != null && (node == this.SiteMap.CurrentNode || this.SiteMap.CurrentNode.IsDescendantOf(node)));
        }

        /// <summary>
        /// Gets a value indicating whether the current SiteMapNode has any child nodes.
        /// </summary>
        public bool HasChildNodes
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
        public int GetNodeLevel()
        {
            return this.innerSiteMapNode.GetNodeLevel();
        }

        /// <summary>
        /// A reference to the root SiteMap object for the current graph.
        /// </summary>
        public ISiteMap SiteMap
        {
            get { return this.innerSiteMapNode.SiteMap; }
        }

        #endregion

        /// <summary>
        /// Determines whether the current node is accessible to the current user based on context.
        /// </summary>
        /// <value>
        /// True if the current node is accessible.
        /// </value>
        public bool IsAccessibleToUser(HttpContext context)
        {
            return this.innerSiteMapNode.IsAccessibleToUser(context);
        }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public string HttpMethod
        {
            get { return this.innerSiteMapNode.HttpMethod; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "HttpMethod"));
                }
                this.innerSiteMapNode.HttpMethod = value;
            }
        }

        /// <summary>
        /// Gets the implicit resource key (optional).
        /// </summary>
        /// <value>The implicit resource key.</value>
        public string ResourceKey
        {
            get { return this.innerSiteMapNode.ResourceKey; }
        }

        /// <summary>
        /// Gets or sets the title (optional).
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return this.innerSiteMapNode.Title; }
            set
            {
                // TODO: Find out what the attribute is for that overwrites this from the UI layer.
                //if (this.IsReadOnly)
                //{
                //    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Title"));
                //}
                this.innerSiteMapNode.Title = value;
            }
        }

        /// <summary>
        /// Gets or sets the description (optional).
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return this.innerSiteMapNode.Description; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Description"));
                }
                this.innerSiteMapNode.Description = value;
            }
        }

        /// <summary>
        /// Gets or sets the target frame (optional).
        /// </summary>
        /// <value>The target frame.</value>
        public string TargetFrame
        {
            get { return this.innerSiteMapNode.TargetFrame; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "TargetFrame"));
                }
                this.innerSiteMapNode.TargetFrame = value;
            }
        }

        /// <summary>
        /// Gets or sets the image URL (optional).
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl
        {
            get { return this.innerSiteMapNode.ImageUrl; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "ImageUrl"));
                }
                this.innerSiteMapNode.ImageUrl = value;
            }
        }

        /// <summary>
        /// Gets the attributes (optional).
        /// </summary>
        /// <value>The attributes.</value>
        public IAttributeCollection Attributes
        {
            get { return this.innerSiteMapNode.Attributes; }
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public IList<string> Roles
        {
            get { return this.innerSiteMapNode.Roles; }
        }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime LastModifiedDate
        {
            get { return this.innerSiteMapNode.LastModifiedDate; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "LastModifiedDate"));
                }
                this.innerSiteMapNode.LastModifiedDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the change frequency.
        /// </summary>
        /// <value>The change frequency.</value>
        public ChangeFrequency ChangeFrequency
        {
            get { return this.innerSiteMapNode.ChangeFrequency; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "ChangeFrequency"));
                }
                this.innerSiteMapNode.ChangeFrequency = value;
            }
        }

        /// <summary>
        /// Gets or sets the update priority.
        /// </summary>
        /// <value>The update priority.</value>
        public UpdatePriority UpdatePriority
        {
            get { return this.innerSiteMapNode.UpdatePriority; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "UpdatePriority"));
                }
                this.innerSiteMapNode.UpdatePriority = value;
            }
        }


        #region Visibility

        /// <summary>
        /// Gets or sets the name or the type of the visibility provider.
        /// This value will be used to select the concrete type of provider to use to determine
        /// visibility.
        /// </summary>
        /// <value>
        /// The name or type of the visibility provider.
        /// </value>
        public string VisibilityProvider
        {
            get { return this.innerSiteMapNode.VisibilityProvider; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "VisibilityProvider"));
                }
                this.innerSiteMapNode.VisibilityProvider = value;
            }
        }

        /// <summary>
        /// Determines whether the node is visible.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sourceMetadata">The source metadata.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is visible; otherwise, <c>false</c>.
        /// </returns>
        public bool IsVisible(HttpContext context, IDictionary<string, object> sourceMetadata)
        {
            return this.innerSiteMapNode.IsVisible(context, sourceMetadata);
        }

        #endregion

        #region URL Resolver

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SiteMapNode" /> is clickable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if clickable; otherwise, <c>false</c>.
        /// </value>
        public bool Clickable
        {
            get { return this.innerSiteMapNode.Clickable; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Clickable"));
                }
                this.innerSiteMapNode.Clickable = value;
            }
        }

        /// <summary>
        /// Gets or sets the name or type of the URL resolver.
        /// </summary>
        /// <value>
        /// The name or type of the URL resolver.
        /// </value>
        public string UrlResolver
        {
            get { return this.innerSiteMapNode.UrlResolver; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "UrlResolver"));
                }
                this.innerSiteMapNode.UrlResolver = value;
            }
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url
        {
            get { return this.innerSiteMapNode.Url; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Url"));
                }
                this.innerSiteMapNode.Url = value;
            }
        }

        /// <summary>
        /// The raw URL before being evaluated by any URL resovler.
        /// </summary>
        public string UnresolvedUrl
        {
            get { return this.innerSiteMapNode.UnresolvedUrl; }
        }

        #endregion

        #region Dynamic Nodes

        /// <summary>
        /// Gets or sets the name or type of the Dynamic Node Provider.
        /// </summary>
        /// <value>
        /// The name or type of the Dynamic Node Provider.
        /// </value>
        public string DynamicNodeProvider
        {
            get { return this.innerSiteMapNode.DynamicNodeProvider; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "DynamicNodeProvider"));
                }
                this.innerSiteMapNode.DynamicNodeProvider = value;
            }
        }

        /// <summary>
        /// Gets the dynamic node collection.
        /// </summary>
        /// <returns>A dynamic node collection.</returns>
        public IEnumerable<DynamicNode> GetDynamicNodeCollection()
        {
            return this.innerSiteMapNode.GetDynamicNodeCollection();
        }

        /// <summary>
        /// Gets whether the current node has a dynamic node provider.
        /// </summary>
        /// <value>
        /// <c>true</c> if there is a provider; otherwise <c>false</c>.
        /// </value>
        public bool HasDynamicNodeProvider
        {
            get { return this.innerSiteMapNode.HasDynamicNodeProvider; }
        }

        #endregion

        #region Route

        /// <summary>
        /// Gets or sets the route.
        /// </summary>
        /// <value>The route.</value>
        public string Route
        {
            get { return this.innerSiteMapNode.Route; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Route"));
                }
                this.innerSiteMapNode.Route = value;
            }
        }


        /// <summary>
        /// Gets the route values.
        /// </summary>
        /// <value>The route values.</value>
        public IRouteValueCollection RouteValues
        {
            get { return this.innerSiteMapNode.RouteValues; }
        }

        /// <summary>
        /// Gets the preserved route parameter names (= values that will be used from the current request route).
        /// </summary>
        /// <value>The preserved route parameters.</value>
        public IList<string> PreservedRouteParameters
        {
            get { return this.innerSiteMapNode.PreservedRouteParameters; }
        }

        /// <summary>
        /// Gets the route data associated with the current node.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>The route data associated with the current node.</returns>
        public RouteData GetRouteData(HttpContextBase httpContext)
        {
            return this.innerSiteMapNode.GetRouteData(httpContext);
        }

        /// <summary>
        /// Determines whether this node matches the supplied route values.
        /// </summary>
        /// <param name="routeValues">An IDictionary<string, object> of route values.</param>
        /// <returns><c>true</c> if the route matches this node's RouteValues and Attributes collections; otherwise <c>false</c>.</returns>
        public bool MatchesRoute(IDictionary<string, object> routeValues)
        {
            return this.innerSiteMapNode.MatchesRoute(routeValues);
        }

        #endregion

        #region MVC

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        /// <value>The area.</value>
        public string Area
        {
            get { return this.innerSiteMapNode.Area; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Area"));
                }
                this.innerSiteMapNode.Area = value;
            }
        }

        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public string Controller
        {
            get { return this.innerSiteMapNode.Controller; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Controller"));
                }
                this.innerSiteMapNode.Controller = value;
            }
        }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action
        {
            get { return this.innerSiteMapNode.Action; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Action"));
                }
                this.innerSiteMapNode.Action = value;
            }
        }

        #endregion


        #endregion
    }
}
