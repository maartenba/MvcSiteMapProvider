//#region Using directives

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Web;
//using System.Web.Routing;
//using MvcSiteMapProvider.Core;

//#endregion

//namespace MvcSiteMapProvider.Core.SiteMap
//{

    

//    /// <summary>
//    /// TODO: Update summary.
//    /// </summary>
//    public class SiteMapNode2 : ICloneable
//    {

//        #region Constructor

//        /// <summary>
//        /// Initializes a new instance of the <see cref="MvcSiteMapNode"/> class.
//        /// </summary>
//        /// <param name="provider">The provider.</param>
//        /// <param name="key">The key.</param>
//        /// <param name="explicitResourceKeys">The explicit resource keys.</param>
//        /// <param name="implicitResourceKey">The implicit resource key.</param>
//        public SiteMapNode2(SiteMap siteMap, string key, NameValueCollection explicitResourceKeys, string implicitResourceKey)
//        {
//            this.siteMap = siteMap;
//            this.key = key;

//            // TODO: finish localization
//        }

//        #endregion

//        protected SiteMap siteMap;
//        protected string key = String.Empty;

//        protected SiteMapNode2 parentNode;
//        protected bool isParentNodeSet = false;
//        protected SiteMapNodeCollection childNodes;
//        protected bool isChildNodesSet = false;

//        #region Properties

//        public string Key
//        {
//            get { return this.key; }
//        }

//        ///// <summary>
//        ///// Gets the meta attributes.
//        ///// </summary>
//        ///// <value>The meta attributes.</value>
//        //public IDictionary<string, string> MetaAttributes
//        //{
//        //    get
//        //    {
//        //        Dictionary<string, string> tempDictionary = new Dictionary<string, string>();
//        //        foreach (var key in Attributes.AllKeys)
//        //        {
//        //            tempDictionary.Add(key, Attributes[key]);
//        //        }
//        //        return tempDictionary;
//        //    }
//        //}

//                /// <summary>
//        /// Gets or sets the attributes (optional).
//        /// </summary>
//        /// <value>The attributes.</value>
//        public IDictionary<string, string> Attributes { get; set; }

//        ///// <summary>
//        ///// Gets or sets the route.
//        ///// </summary>
//        ///// <value>The route.</value>
//        //public string Route
//        //{
//        //    get { return this["route"] != null ? this["route"] : null; }
//        //    set { this["route"] = value; }
//        //}

//        /// <summary>
//        /// Gets or sets the route.
//        /// </summary>
//        /// <value>The route.</value>
//        public string Route
//        {
//            get { return this.Attributes["route"] != null ? this.Attributes["route"] : null; }
//            set { this.Attributes["route"] = value; }
//        }

//        /// <summary>
//        /// Gets or sets the area.
//        /// </summary>
//        /// <value>The area.</value>
//        public string Area
//        {
//            get { return RouteValues.ContainsKey("area") && RouteValues["area"] != null ? RouteValues["area"].ToString() : ""; }
//            set { RouteValues["area"] = value; }
//        }

//        /// <summary>
//        /// Gets or sets the controller.
//        /// </summary>
//        /// <value>The controller.</value>
//        public string Controller
//        {
//            get { return RouteValues.ContainsKey("controller") ? RouteValues["controller"].ToString() : ""; }
//            set { RouteValues["controller"] = value; }
//        }

//        /// <summary>
//        /// Gets or sets the action.
//        /// </summary>
//        /// <value>The action.</value>
//        public string Action
//        {
//            get { return RouteValues.ContainsKey("action") ? RouteValues["action"].ToString() : ""; }
//            set { RouteValues["action"] = value; }
//        }

//        /// <summary>
//        /// Gets or sets a value indicating whether this <see cref="MvcSiteMapNode"/> is clickable.
//        /// </summary>
//        /// <value><c>true</c> if clickable; otherwise, <c>false</c>.</value>
//        public bool Clickable
//        {
//            get { return this.Attributes["clickable"] != null ? bool.Parse(this.Attributes["clickable"]) : true; }
//            set { this.Attributes["clickable"] = value.ToString(); }
//        }

//        /// <summary>
//        /// Gets or sets the last modified date.
//        /// </summary>
//        /// <value>The last modified date.</value>
//        public DateTime LastModifiedDate
//        {
//            get { return this.Attributes["lastModifiedDate"] != null ? DateTime.Parse(this.Attributes["lastModifiedDate"]) : DateTime.MinValue; }
//            set { this.Attributes["lastModifiedDate"] = value.ToString(); }
//        }

//        /// <summary>
//        /// Gets or sets the change frequency.
//        /// </summary>
//        /// <value>The change frequency.</value>
//        public ChangeFrequency ChangeFrequency
//        {
//            get { return this.Attributes["changeFrequency"] != null ? (ChangeFrequency)Enum.Parse(typeof(ChangeFrequency), this.Attributes["changeFrequency"]) : ChangeFrequency.Undefined; }
//            set { this.Attributes["changeFrequency"] = value.ToString(); }
//        }

//        /// <summary>
//        /// Gets or sets the update priority.
//        /// </summary>
//        /// <value>The update priority.</value>
//        public UpdatePriority UpdatePriority
//        {
//            get { return this.Attributes["updatePriority"] != null ? (UpdatePriority)Enum.Parse(typeof(UpdatePriority), this.Attributes["updatePriority"]) : UpdatePriority.Undefined; }
//            set { this.Attributes["updatePriority"] = value.ToString(); }
//        }


//        /// <summary>
//        /// Gets or sets the target frame.
//        /// </summary>
//        /// <value>The target frame.</value>
//        public string TargetFrame
//        {
//            get { return this.Attributes["targetFrame"] != null ? this.Attributes["targetFrame"] : ""; }
//            set { this.Attributes["targetFrame"] = value; }
//        }

//        /// <summary>
//        /// Gets or sets the image URL.
//        /// </summary>
//        /// <value>The image URL.</value>
//        public string ImageUrl
//        {
//            get { return this.Attributes["imageUrl"] != null ? this.Attributes["imageUrl"] : ""; }
//            set { this.Attributes["imageUrl"] = value; }
//        }

//        // TODO: (NightOwl888) determine how to make extensibility points

//        IDynamicNodeProvider dynamicNodeProvider;

//        /// <summary>
//        /// Gets or sets the dynamic node provider.
//        /// </summary>
//        /// <value>The dynamic node provider.</value>
//        public IDynamicNodeProvider DynamicNodeProvider
//        {
//            get
//            {
//                if (!string.IsNullOrEmpty(this.Attributes["dynamicNodeProvider"]))
//                {
//                    if (dynamicNodeProvider == null)
//                    {
//                        dynamicNodeProvider = Activator.CreateInstance(
//                            Type.GetType(this.Attributes["dynamicNodeProvider"])) as IDynamicNodeProvider;
//                    }
//                    return dynamicNodeProvider;
//                }
//                return null;
//            }
//            set
//            {
//                dynamicNodeProvider = value;
//                if (value != null)
//                {
//                    this.Attributes["dynamicNodeProvider"] = value.GetType().AssemblyQualifiedName;
//                }
//                else
//                {
//                    this.Attributes["dynamicNodeProvider"] = null;
//                }
//            }
//        }

//        ISiteMapNodeUrlResolver urlResolver;

//        /// <summary>
//        /// Gets or sets the site map node URL resolver.
//        /// </summary>
//        /// <value>The site map node URL resolver.</value>
//        public ISiteMapNodeUrlResolver UrlResolver
//        {
//            get
//            {
//                if (!string.IsNullOrEmpty(this.Attributes["urlResolver"]))
//                {
//                    if (urlResolver == null)
//                    {
//                        urlResolver = Activator.CreateInstance(
//                            Type.GetType(this.Attributes["urlResolver"])) as ISiteMapNodeUrlResolver;
//                    }
//                    return urlResolver;
//                }
//                return null;
//            }
//            set
//            {
//                urlResolver = value;
//                if (value != null)
//                {
//                    this.Attributes["urlResolver"] = value.GetType().AssemblyQualifiedName;
//                }
//                else
//                {
//                    this.Attributes["urlResolver"] = null;
//                }
//            }
//        }

//        // TODO: (NightOwl888) determine how to make node visible/invisible

//        ISiteMapNodeVisibilityProvider visibilityProvider;

//        /// <summary>
//        /// Gets or sets the visibility provider.
//        /// </summary>
//        /// <value>The visibility provider.</value>
//        public ISiteMapNodeVisibilityProvider VisibilityProvider
//        {
//            get
//            {
//                if (!string.IsNullOrEmpty(this.Attributes["visibilityProvider"]))
//                {
//                    if (visibilityProvider == null)
//                    {
//                        visibilityProvider = Activator.CreateInstance(
//                            Type.GetType(this.Attributes["visibilityProvider"])) as ISiteMapNodeVisibilityProvider;
//                    }
//                    return visibilityProvider;
//                }
//                return null;
//            }
//            set
//            {
//                visibilityProvider = value;
//                if (value != null)
//                {
//                    this.Attributes["visibilityProvider"] = value.GetType().AssemblyQualifiedName;
//                }
//                else
//                {
//                    this.Attributes["visibilityProvider"] = null;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the preserved route parameter names (= values that will be used from the current request route).
//        /// </summary>
//        /// <value>
//        /// The preserved route parameter names.
//        /// </value>
//        public string PreservedRouteParameters
//        {
//            get { return this.Attributes["preservedRouteParameters"] != null ? this.Attributes["preservedRouteParameters"] : ""; }
//            set { this.Attributes["preservedRouteParameters"] = value; }
//        }

//        /// <summary>
//        /// Gets or sets the inherited route parameters (= Route values that should be inherited from the parent sitemap node. This is not a replacement for the SiteMapPreserveRouteDataAttribute.)
//        /// </summary>
//        /// <value>
//        /// The inherited route parameters.
//        /// </value>
//        public string InheritedRouteParameters
//        {
//            get { return this.Attributes["inheritedRouteParameters"] != null ? this.Attributes["inheritedRouteParameters"] : ""; }
//            set { this.Attributes["inheritedRouteParameters"] = value; }
//        }

//        /// <summary>
//        /// Gets or sets the route values.
//        /// </summary>
//        /// <value>The route values.</value>
//        public IDictionary<string, object> RouteValues { get; set; }

//        /// <summary>
//        /// Gets or sets the URL of the page that the <see cref="T:System.Web.SiteMapNode"/> object represents.
//        /// </summary>
//        /// <value></value>
//        /// <returns>
//        /// The URL of the page that the node represents. The default is <see cref="F:System.String.Empty"/>.
//        /// </returns>
//        /// <exception cref="T:System.InvalidOperationException">
//        /// The node is read-only.
//        /// </exception>
//        public virtual string Url
//        {
//            get
//            {
//                if (this.UrlResolver == null)
//                {
//                    throw new MvcSiteMapException(Resources.Messages.NoUrlResolverProvided);
//                }

//                return this.UrlResolver.ResolveUrl(this, Area, Controller, Action, RouteValues);
//            }
//            set
//            {
//                this.Attributes["url"] = value;
//            }
//        }

//        //string title = "";

//        ///// <summary>
//        ///// Gets or sets the title of the <see cref="T:System.Web.SiteMapNode"/> object.
//        ///// </summary>
//        ///// <value></value>
//        ///// <returns>
//        ///// A string that represents the title of the node. The default is <see cref="F:System.String.Empty"/>.
//        ///// </returns>
//        ///// <exception cref="T:System.InvalidOperationException">
//        ///// The node is read-only.
//        ///// </exception>
//        //[Localizable(true)]
//        //public override string Title
//        //{
//        //    get
//        //    {
//        //        if (!string.IsNullOrEmpty(title))
//        //        {
//        //            return title;
//        //        }
//        //        if (Provider.EnableLocalization)
//        //        {
//        //            var implicitResourceString = GetImplicitResourceString("title");
//        //            if (implicitResourceString != null && implicitResourceString == this["title"])
//        //            {
//        //                return implicitResourceString;
//        //            }
//        //            implicitResourceString = GetExplicitResourceString("title", this["title"], true);
//        //            if (implicitResourceString != null && implicitResourceString == this["title"])
//        //            {
//        //                return implicitResourceString;
//        //            }
//        //        }
//        //        if (this["title"] != null)
//        //        {
//        //            return this["title"];
//        //        }
//        //        return string.Empty;
//        //    }
//        //    set
//        //    {
//        //        this["title"] = value;
//        //        title = value;
//        //    }
//        //}

//        //string description;

//        ///// <summary>
//        ///// Gets or sets a description for the <see cref="T:System.Web.SiteMapNode"/>.
//        ///// </summary>
//        ///// <value></value>
//        ///// <returns>
//        ///// A string that represents a description of the node; otherwise, <see cref="F:System.String.Empty"/>.
//        ///// </returns>
//        ///// <exception cref="T:System.InvalidOperationException">
//        ///// The node is read-only.
//        ///// </exception>
//        //[Localizable(true)]
//        //public override string Description
//        //{
//        //    get
//        //    {
//        //        if (!string.IsNullOrEmpty(description))
//        //        {
//        //            return description;
//        //        }
//        //        if (Provider.EnableLocalization)
//        //        {
//        //            var implicitResourceString = GetImplicitResourceString("description");
//        //            if (implicitResourceString != null && implicitResourceString == this["description"])
//        //            {
//        //                return implicitResourceString;
//        //            }
//        //            implicitResourceString = GetExplicitResourceString("description", this["description"], true);
//        //            if (implicitResourceString != null && implicitResourceString == this["description"])
//        //            {
//        //                return implicitResourceString;
//        //            }
//        //        }

//        //        if (this["description"] != null)
//        //        {
//        //            return this["description"];
//        //        }
//        //        return string.Empty;
//        //    }
//        //    set
//        //    {
//        //        this["description"] = value;
//        //        description = value;
//        //    }
//        //}


//        // TODO: Localize get if enabled
//        /// <summary>
//        /// Gets or sets the title (optional).
//        /// </summary>
//        /// <value>The title.</value>
//        public string Title { get; set; }


//        // TODO: Localize get if enabled
//        /// <summary>
//        /// Gets or sets the description (optional).
//        /// </summary>
//        /// <value>The description.</value>
//        public string Description { get; set; }

//        /// <summary>
//        /// Gets or sets the roles.
//        /// </summary>
//        /// <value>The roles.</value>
//        public IList<string> Roles { get; set; }

//        /// <summary>
//        /// Gets or sets a value indicating whether this instance is dynamic.
//        /// </summary>
//        /// <value>
//        /// 	<c>true</c> if this instance is dynamic; otherwise, <c>false</c>.
//        /// </value>
//        public bool IsDynamic { get; set; }


//        public string ResourceKey { get; set; }

        
//        #endregion



//        #region ICloneable

//        /// <summary>
//        /// Creates a new node that is a copy of the current node.
//        /// </summary>
//        /// <returns>
//        /// A new node that is a copy of the current node.
//        /// </returns>
//        public virtual object Clone()
//        {
//            return Clone(Key);
//        }

//        ///// <summary>
//        ///// Creates a new node that is a copy of the current node with a specified key.
//        ///// </summary>
//        ///// <param name="key">The key.</param>
//        ///// <returns>
//        ///// A new node that is a copy of the current node.
//        ///// </returns>
//        //public virtual SiteMapNode Clone(string key)
//        //{
//        //    MvcSiteMapNode clone = new MvcSiteMapNode(Provider, key, new NameValueCollection(), null);
//        //    if (RouteValues != null)
//        //    {
//        //        clone.RouteValues = new Dictionary<string, object>(RouteValues);
//        //    }
//        //    if (Attributes != null)
//        //    {
//        //        clone.Attributes = new NameValueCollection(Attributes);
//        //    }
//        //    if (Roles != null)
//        //    {
//        //        clone.Roles = new ArrayList(Roles);
//        //    }

//        //    clone.Title = Title;
//        //    clone.Description = Description;
//        //    clone.Clickable = Clickable;
//        //    clone.LastModifiedDate = LastModifiedDate;
//        //    clone.ChangeFrequency = ChangeFrequency;
//        //    clone.UpdatePriority = UpdatePriority;
//        //    clone.TargetFrame = TargetFrame;
//        //    clone.ImageUrl = ImageUrl;
//        //    clone.PreservedRouteParameters = PreservedRouteParameters;
//        //    clone.InheritedRouteParameters = InheritedRouteParameters;

//        //    return clone;
//        //}

//        /// <summary>
//        /// Creates a new node that is a copy of the current node with a specified key.
//        /// </summary>
//        /// <param name="key">The key.</param>
//        /// <returns>
//        /// A new node that is a copy of the current node.
//        /// </returns>
//        public virtual SiteMapNode2 Clone(string key)
//        {
//            var clone = new SiteMapNode2(null, key, new NameValueCollection(), null);
//            if (RouteValues != null)
//            {
//                clone.RouteValues = new Dictionary<string, object>(RouteValues);
//            }
//            if (Attributes != null)
//            {
//                clone.Attributes = new Dictionary<string, string>(this.Attributes);
//            }
//            if (Roles != null)
//            {
//                clone.Roles = new List<string>(this.Roles);
//            }

//            clone.Title = Title;
//            clone.Description = Description;
//            //clone.Clickable = Clickable;
//            //clone.LastModifiedDate = LastModifiedDate;
//            //clone.ChangeFrequency = ChangeFrequency;
//            //clone.UpdatePriority = UpdatePriority;
//            //clone.TargetFrame = TargetFrame;
//            //clone.ImageUrl = ImageUrl;
//            //clone.PreservedRouteParameters = PreservedRouteParameters;
//            //clone.InheritedRouteParameters = InheritedRouteParameters;

//            return clone;
//        }

//        #endregion

//        /// <summary>
//        /// Gets the route data associated with the current node.
//        /// </summary>
//        /// <param name="httpContext">The HTTP context.</param>
//        /// <returns>The route data associated with the current node.</returns>
//        public RouteData GetRouteData(HttpContextBase httpContext)
//        {
//            RouteData routeData;
//            if (!string.IsNullOrEmpty(this.Route))
//            {
//                routeData = RouteTable.Routes[this.Route].GetRouteData(httpContext);
//            }
//            else
//            {
//                routeData = RouteTable.Routes.GetRouteData(httpContext);
//            }
//            return routeData;
//        }


//        #region Position Determination

//        public virtual bool IsAccessibleToUser(HttpContext context)
//        {
//            return this.siteMap.IsAccessibleToUser(context, this);
//        }

//        public virtual bool IsDescendantOf(SiteMapNode2 node)
//        {
//            for (SiteMapNode2 node2 = this.ParentNode; node2 != null; node2 = node2.ParentNode)
//            {
//                if (node2.Equals(node))
//                {
//                    return true;
//                }
//            }
//            return false;
//        }

//        public virtual SiteMapNode2 NextSibling
//        {
//            get
//            {
//                IList siblingNodes = this.SiblingNodes;
//                if (siblingNodes != null)
//                {
//                    int index = siblingNodes.IndexOf(this);
//                    if ((index >= 0) && (index < (siblingNodes.Count - 1)))
//                    {
//                        return (SiteMapNode2)siblingNodes[index + 1];
//                    }
//                }
//                return null;
//            }
//        }

//        public virtual SiteMapNode2 ParentNode
//        {
//            get
//            {
//                if (this.isParentNodeSet)
//                {
//                    return this.parentNode;
//                }
//                return this.siteMap.GetParentNode(this);
//            }
//            set
//            {
//                this.parentNode = value;
//                this.isParentNodeSet = true;
//            }
//        }

//        public virtual SiteMapNode2 PreviousSibling
//        {
//            get
//            {
//                IList siblingNodes = this.SiblingNodes;
//                if (siblingNodes != null)
//                {
//                    int index = siblingNodes.IndexOf(this);
//                    if ((index > 0) && (index <= (siblingNodes.Count - 1)))
//                    {
//                        return (SiteMapNode2)siblingNodes[index - 1];
//                    }
//                }
//                return null;
//            }
//        }

//        public virtual SiteMapNode2 RootNode
//        {
//            get
//            {
//                SiteMapNode2 rootNode = this.siteMap.RootNode;
//                if (rootNode == null)
//                {
//                    throw new InvalidOperationException(Resources.Messages.SiteMapInvalidRootNode);
//                }
//                return rootNode;
//            }
//        }

//        private SiteMapNodeCollection SiblingNodes
//        {
//            get
//            {
//                SiteMapNode2 parentNode = this.ParentNode;
//                if (parentNode != null)
//                {
//                    return parentNode.ChildNodes;
//                }
//                return null;
//            }
//        }

//        public virtual SiteMapNodeCollection ChildNodes
//        {
//            get
//            {
//                if (this.isChildNodesSet)
//                {
//                    return this.childNodes;
//                }
//                return this.siteMap.GetChildNodes(this);
//            }
//            set
//            {
//                this.childNodes = value;
//                this.isChildNodesSet = true;
//            }
//        }

//        #endregion

//    }
//}
