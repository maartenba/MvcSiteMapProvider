using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
using System.Linq;
using MvcSiteMapProvider.Core.Mvc.UrlResolver;
using MvcSiteMapProvider.Core.Collections;
using MvcSiteMapProvider.Core.Globalization;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// SiteMapNode class. This class represents a node within the SiteMap hierarchy.
    /// It contains all business logic to maintain the node's internal state.
    /// </summary>
    public class SiteMapNode
        : ISiteMapNode
    {
        public SiteMapNode(
            ISiteMap siteMap, 
            string key,
            bool isDynamic,
            IDictionary<string, string> attributes,
            IRouteValueCollection routeValues,
            IList<string> preservedRouteParameters,
            IList<string> roles,
            ILocalizationService localizationService,
            IDynamicNodeProviderStrategy dynamicNodeProviderStrategy,
            ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy,
            ISiteMapNodeVisibilityProviderStrategy siteMapNodeVisibilityProviderStrategy
            )
        {
            if (siteMap == null)
                throw new ArgumentNullException("siteMap");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (attributes == null)
                throw new ArgumentNullException("attributes");
            if (routeValues == null)
                throw new ArgumentNullException("routeValues");
            if (preservedRouteParameters == null)
                throw new ArgumentNullException("preservedRouteParameters");
            if (roles == null)
                throw new ArgumentNullException("roles");
            if (localizationService == null)
                throw new ArgumentNullException("localizationService");
            if (dynamicNodeProviderStrategy == null)
                throw new ArgumentNullException("dynamicNodeProviderStrategy");
            if (siteMapNodeUrlResolverStrategy == null)
                throw new ArgumentNullException("siteMapNodeUrlResolverStrategy");
            if (siteMapNodeVisibilityProviderStrategy == null)
                throw new ArgumentNullException("siteMapNodeVisibilityProviderStrategy");

            this.siteMap = siteMap;
            this.key = key;
            this.isDynamic = isDynamic;
            this.attributes = attributes;
            this.routeValues = routeValues;
            this.preservedRouteParameters = preservedRouteParameters;
            this.roles = roles;
            this.localizationService = localizationService;
            this.dynamicNodeProviderStrategy = dynamicNodeProviderStrategy;
            this.siteMapNodeUrlResolverStrategy = siteMapNodeUrlResolverStrategy;
            this.siteMapNodeVisibilityProviderStrategy = siteMapNodeVisibilityProviderStrategy;
        }

        // Services
        protected readonly ILocalizationService localizationService;
        protected readonly IDynamicNodeProviderStrategy dynamicNodeProviderStrategy;
        protected readonly ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy;
        protected readonly ISiteMapNodeVisibilityProviderStrategy siteMapNodeVisibilityProviderStrategy;

        // Child collections and dictionaries
        protected readonly IDictionary<string, string> attributes;
        protected readonly IRouteValueCollection routeValues;
        protected readonly IList<string> preservedRouteParameters;
        protected readonly IList<string> roles;

        // Object State
        protected readonly string key;
        protected readonly bool isDynamic;
        protected ISiteMap siteMap;
        protected ISiteMapNode parentNode;
        protected bool isParentNodeSet = false;
        protected string url = String.Empty;
        protected string title = String.Empty;
        protected string description = String.Empty;
        protected string httpMethod = String.Empty;
        protected string targetFrame = String.Empty;
        protected string imageUrl = String.Empty;
        protected DateTime lastModifiedDate = DateTime.MinValue;
        protected ChangeFrequency changeFrequency = ChangeFrequency.Always;
        protected UpdatePriority updatePriority = UpdatePriority.Undefined;
        protected string visibilityProvider = String.Empty;
        protected bool clickable = true;
        protected string urlResolver = String.Empty;
        protected string dynamicNodeProvider = String.Empty;
        protected string route = String.Empty;


        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public virtual string Key { get { return this.key; } }

        /// <summary>
        /// Gets whether the current node was created from a dynamic source.
        /// </summary>
        /// <value>True if the current node is dynamic.</value>
        public virtual bool IsDynamic { get { return this.isDynamic; } }


        #region Node Map Positioning

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        public virtual ISiteMapNode ParentNode
        {
            get
            {
                if (this.isParentNodeSet)
                {
                    return this.parentNode;
                }
                return this.siteMap.GetParentNode(this);
            }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "ParentNode"));
                }
                this.parentNode = value;
                this.isParentNodeSet = true;
            }
        }

        /// <summary>
        /// Gets or sets the child nodes.
        /// </summary>
        /// <value>
        /// The child nodes.
        /// </value>
        public virtual ISiteMapNodeCollection ChildNodes
        {
            get { return this.siteMap.GetChildNodes(this); }
        }

        public virtual bool IsDescendantOf(ISiteMapNode node)
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
        public virtual ISiteMapNode NextSibling
        {
            get
            {
                var siblingNodes = this.SiblingNodes;
                if (siblingNodes != null)
                {
                    int index = siblingNodes.IndexOf(this);
                    if ((index >= 0) && (index < (siblingNodes.Count - 1)))
                    {
                        return (ISiteMapNode)siblingNodes[index + 1];
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
        public virtual ISiteMapNode PreviousSibling
        {
            get
            {
                var siblingNodes = this.SiblingNodes;
                if (siblingNodes != null)
                {
                    int index = siblingNodes.IndexOf(this);
                    if ((index > 0) && (index <= (siblingNodes.Count - 1)))
                    {
                        return (ISiteMapNode)siblingNodes[index - 1];
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
        public virtual ISiteMapNode RootNode
        {
            get
            {
                var rootNode = this.siteMap.RootNode;
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
        /// <param name="current">The current.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is in current path; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsInCurrentPath()
        {
            ISiteMapNode node = this;
            return (this.siteMap.CurrentNode != null && (node == this.siteMap.CurrentNode || this.siteMap.CurrentNode.IsDescendantOf(node)));
        }


        public virtual bool HasChildNodes
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
        /// <param name="current">The current SiteMapNode</param>
        /// <returns>The level of the current SiteMapNode</returns>
        public virtual int GetNodeLevel()
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

        public virtual ISiteMap SiteMap
        {
            get
            {
                return this.siteMap;
            }
        }

        #endregion


        /// <summary>
        /// Determines whether the current node is accessible to the current user based on context.
        /// </summary>
        /// <value>
        /// True if the current node is accessible.
        /// </value>
        public virtual bool IsAccessibleToUser(HttpContext context)
        {
            return this.siteMap.IsAccessibleToUser(context, this);
        }

        // TODO: Determine what the value of this property is
        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public virtual string HttpMethod 
        {
            get { return this.httpMethod; }
            set
            {
                // TODO: Move lockable logic into another class that wraps this one.
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "HttpMethod"));
                }
                this.httpMethod = value;
            }
        }

        /// <summary>
        /// Gets the implicit resource key (optional).
        /// </summary>
        /// <value>The implicit resource key.</value>
        public virtual string ResourceKey
        {
            get { return this.localizationService.ResourceKey; }
        }

        /// <summary>
        /// Gets or sets the title (optional).
        /// </summary>
        /// <value>The title.</value>
        public virtual string Title 
        {
            get 
            {
                return localizationService.GetResourceString("title", this.title, this.siteMap);
            }
            set 
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Title"));
                }
                SetTitle(value);
            }
        }

        /// <summary>
        /// Used internally to set the title when the node is in read-only state.
        /// </summary>
        /// <param name="title">The new title to use.</param>
        internal virtual void SetTitle(string title)
        {
            this.title = localizationService.ExtractExplicitResourceKey("title", title);
        }

        /// <summary>
        /// Gets or sets the description (optional).
        /// </summary>
        /// <value>The description.</value>
        public virtual string Description 
        {
            get
            {
                return localizationService.GetResourceString("description", this.description, this.siteMap);
            }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Description"));
                }
                this.description = localizationService.ExtractExplicitResourceKey("description", value);
            }
        }

        /// <summary>
        /// Gets or sets the target frame (optional).
        /// </summary>
        /// <value>The target frame.</value>
        public virtual string TargetFrame 
        {
            get { return this.targetFrame; }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "TargetFrame"));
                }
                this.targetFrame = value;
            }
        }

        /// <summary>
        /// Gets or sets the image URL (optional).
        /// </summary>
        /// <value>The image URL.</value>
        public virtual string ImageUrl
        {
            get { return this.imageUrl; }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "ImageUrl"));
                }
                this.imageUrl = value;
            }
        }

        /// <summary>
        /// Gets the attributes (optional).
        /// </summary>
        /// <value>The attributes.</value>
        public virtual IDictionary<string, string> Attributes { get { return this.attributes; } }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public virtual IList<string> Roles { get { return this.roles; } }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public virtual DateTime LastModifiedDate
        {
            get { return this.lastModifiedDate; }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "LastModifiedDate"));
                }
                this.lastModifiedDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the change frequency.
        /// </summary>
        /// <value>The change frequency.</value>
        public virtual ChangeFrequency ChangeFrequency
        {
            get { return this.changeFrequency; }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "ChangeFrequency"));
                }
                this.changeFrequency = value;
            }
        }

        /// <summary>
        /// Gets or sets the update priority.
        /// </summary>
        /// <value>The update priority.</value>
        public virtual UpdatePriority UpdatePriority
        {
            get { return this.updatePriority; }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "UpdatePriority"));
                }
                this.updatePriority = value;
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
        public virtual string VisibilityProvider
        {
            get { return this.visibilityProvider; }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "VisibilityProvider"));
                }
                this.visibilityProvider = value;
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
        public virtual bool IsVisible(HttpContext context, IDictionary<string, object> sourceMetadata)
        {
            // use strategy factory to provide implementation logic from concrete provider
            // http://stackoverflow.com/questions/1499442/best-way-to-use-structuremap-to-implement-strategy-pattern
            return siteMapNodeVisibilityProviderStrategy.IsVisible(this.VisibilityProvider, this, context, sourceMetadata);
        }

        #endregion

        #region URL Resolver

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SiteMapNode" /> is clickable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if clickable; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Clickable
        {
            get { return this.clickable; }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Clickable"));
                }
                this.clickable = value;
            }
        }

        /// <summary>
        /// Gets or sets the name or type of the URL resolver.
        /// </summary>
        /// <value>
        /// The name or type of the URL resolver.
        /// </value>
        public virtual string UrlResolver
        {
            get { return this.urlResolver; }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "UrlResolver"));
                }
                this.urlResolver = value;
            }
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public virtual string Url 
        {
            get
            {
                if (!this.Clickable)
                {
                    return string.Empty;
                }
                // Only resolve the url if an absolute url is not already set
                if (String.IsNullOrEmpty(this.url) || (!this.url.StartsWith("http") && !this.url.StartsWith("ftp")))
                {
                    // use strategy factory to provide implementation logic from concrete provider
                    // http://stackoverflow.com/questions/1499442/best-way-to-use-structuremap-to-implement-strategy-pattern
                    return siteMapNodeUrlResolverStrategy.ResolveUrl(
                        this.UrlResolver, this, this.Area, this.Controller, this.Action, this.RouteValues);
                }
                return this.url;
            }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Url"));
                }
                this.url = value;
            }
        }

        public virtual string UnresolvedUrl { get { return this.url; } }

        #endregion

        #region Dynamic Nodes

        /// <summary>
        /// Gets or sets the name or type of the Dynamic Node Provider.
        /// </summary>
        /// <value>
        /// The name or type of the Dynamic Node Provider.
        /// </value>
        public virtual string DynamicNodeProvider
        {
            get { return this.dynamicNodeProvider; }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "DynamicNodeProvider"));
                }
                this.dynamicNodeProvider = value;
            }
        }

        /// <summary>
        /// Gets the dynamic node collection.
        /// </summary>
        /// <returns>A dynamic node collection.</returns>
        public virtual IEnumerable<DynamicNode> GetDynamicNodeCollection()
        {
            // use strategy factory to provide implementation logic from concrete provider
            // http://stackoverflow.com/questions/1499442/best-way-to-use-structuremap-to-implement-strategy-pattern
            return dynamicNodeProviderStrategy.GetDynamicNodeCollection(this.DynamicNodeProvider);
        }

        /// <summary>
        /// Gets whether the current node has a dynamic node provider.
        /// </summary>
        /// <value>
        /// True if there is a provider.
        /// </value>
        public virtual bool HasDynamicNodeProvider
        {
            // use strategy factory to provide implementation logic from concrete provider
            // http://stackoverflow.com/questions/1499442/best-way-to-use-structuremap-to-implement-strategy-pattern
            get { return (dynamicNodeProviderStrategy.GetProvider(this.DynamicNodeProvider) != null); }
        }

        #endregion

        #region Route

        /// <summary>
        /// Gets or sets the route.
        /// </summary>
        /// <value>The route.</value>
        public virtual string Route
        {
            get { return this.route; }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Route"));
                }
                this.route = value;
            }
        }

        /// <summary>
        /// Gets or sets the route values.
        /// </summary>
        /// <value>The route values.</value>
        public virtual IRouteValueCollection RouteValues { get { return this.routeValues; } }

        /// <summary>
        /// Gets or sets the preserved route parameter names (= values that will be used from the current request route).
        /// </summary>
        /// <value>The attributes.</value>
        public virtual IList<string> PreservedRouteParameters { get { return this.preservedRouteParameters; } }


        /// <summary>
        /// Gets the route data associated with the current node.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>The route data associated with the current node.</returns>
        public virtual RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData routeData;
            if (!string.IsNullOrEmpty(this.Route))
            {
                routeData = RouteTable.Routes[this.Route].GetRouteData(httpContext);
            }
            else
            {
                routeData = RouteTable.Routes.GetRouteData(httpContext);
            }
            return routeData;
        }

        #endregion

        #region MVC

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        /// <value>The area.</value>
        public virtual string Area
        {
            get { return RouteValues.ContainsKey("area") && RouteValues["area"] != null ? RouteValues["area"].ToString() : ""; }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Area"));
                }
                RouteValues["area"] = value; 
            }
        }

        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public virtual string Controller
        {
            get { return RouteValues.ContainsKey("controller") ? RouteValues["controller"].ToString() : ""; }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Controller"));
                }
                RouteValues["controller"] = value; 
            }
        }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public virtual string Action
        {
            get { return RouteValues.ContainsKey("action") ? RouteValues["action"].ToString() : ""; }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Action"));
                }
                RouteValues["action"] = value;
            }
        }

        #endregion


        //#region ICloneable Members

        ///// <summary>
        ///// Creates a new object that is a copy of the current instance.
        ///// </summary>
        ///// <returns>
        ///// A new object that is a copy of this instance.
        ///// </returns>
        //public virtual object Clone()
        //{
        //    //var clone = new SiteMapNode(this.siteMap, this.key, this.ResourceKey);

        //    var clone = siteMapNodeFactory.Create(this.siteMap, this.key, this.ResourceKey);
        //    clone.ParentNode = this.ParentNode;

        //    // TODO: implement and cascade call to SiteMapNodeCollection instead of looping here
        //    //clone.ChildNodes = new SiteMapNodeCollection();
        //    //foreach (var childNode in ChildNodes)
        //    //{
        //    //    var childClone = ((SiteMapNode)childNode).Clone() as SiteMapNode;
        //    //    childClone.ParentNode = clone;
        //    //    clone.ChildNodes.Add(childClone);
        //    //}

        //    clone.ChildNodes = (SiteMapNodeCollection)ChildNodes.Clone();
        //    clone.Url = this.Url;
        //    clone.HttpMethod = this.HttpMethod;
        //    clone.Clickable = this.Clickable;
        //    //clone.ResourceKey = this.ResourceKey;
        //    clone.Title = this.Title;
        //    clone.Description = this.Description;
        //    clone.TargetFrame = this.TargetFrame;
        //    clone.ImageUrl = this.ImageUrl;
        //    clone.Attributes = new Dictionary<string, string>(this.Attributes);
        //    clone.Roles = new List<string>(this.Roles);
        //    clone.LastModifiedDate = this.LastModifiedDate;
        //    clone.ChangeFrequency = this.ChangeFrequency;
        //    clone.UpdatePriority = this.UpdatePriority;
        //    clone.VisibilityProvider = this.VisibilityProvider;

        //    // Route
        //    clone.Route = this.Route;
        //    clone.RouteValues = new Dictionary<string, object>(this.RouteValues);
        //    clone.PreservedRouteParameters = this.PreservedRouteParameters;
        //    clone.UrlResolver = this.UrlResolver;

        //    // MVC
        //    clone.Action = this.Action;
        //    clone.Area = this.Area;
        //    clone.Controller = this.Controller;

        //    return clone;
        //}

        //#endregion
    }
}
