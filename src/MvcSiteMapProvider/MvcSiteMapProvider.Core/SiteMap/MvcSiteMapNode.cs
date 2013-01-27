//#region Using directives

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Web;
//using System.Web.Routing;
//using MvcSiteMapProvider.Core;
////using MvcSiteMapProvider.Extensibility;

//#endregion

//namespace MvcSiteMapProvider.Core.SiteMap
//{
//    /// <summary>
//    /// MvcSiteMapNode class
//    /// </summary>
//    public class MvcSiteMapNode
//        : SiteMapNode
//    {
//        #region Properties

//        /// <summary>
//        /// Gets the meta attributes.
//        /// </summary>
//        /// <value>The meta attributes.</value>
//        public IDictionary<string, string> MetaAttributes
//        {
//            get
//            {
//                Dictionary<string, string> tempDictionary = new Dictionary<string, string>();
//                foreach (var key in Attributes.AllKeys)
//                {
//                    tempDictionary.Add(key, Attributes[key]);
//                }
//                return tempDictionary;
//            }
//        }

//        /// <summary>
//        /// Gets or sets the route.
//        /// </summary>
//        /// <value>The route.</value>
//        public string Route
//        {
//            get { return this["route"] != null ? this["route"] : null; }
//            set { this["route"] = value; }
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
//            get { return this["clickable"] != null ? bool.Parse(this["clickable"]) : true; }
//            set { this["clickable"] = value.ToString(); }
//        }

//        /// <summary>
//        /// Gets or sets the last modified date.
//        /// </summary>
//        /// <value>The last modified date.</value>
//        public DateTime LastModifiedDate
//        {
//            get { return this["lastModifiedDate"] != null ? DateTime.Parse(this["lastModifiedDate"]) : DateTime.MinValue; }
//            set { this["lastModifiedDate"] = value.ToString(); }
//        }

//        /// <summary>
//        /// Gets or sets the change frequency.
//        /// </summary>
//        /// <value>The change frequency.</value>
//        public ChangeFrequency ChangeFrequency
//        {
//            get { return this["changeFrequency"] != null ? (ChangeFrequency)Enum.Parse(typeof(ChangeFrequency), this["changeFrequency"]) : ChangeFrequency.Undefined; }
//            set { this["changeFrequency"] = value.ToString(); }
//        }

//        /// <summary>
//        /// Gets or sets the update priority.
//        /// </summary>
//        /// <value>The update priority.</value>
//        public UpdatePriority UpdatePriority
//        {
//            get { return this["updatePriority"] != null ? (UpdatePriority)Enum.Parse(typeof(UpdatePriority), this["updatePriority"]) : UpdatePriority.Undefined; }
//            set { this["updatePriority"] = value.ToString(); }
//        }


//        /// <summary>
//        /// Gets or sets the target frame.
//        /// </summary>
//        /// <value>The target frame.</value>
//        public string TargetFrame
//        {
//            get { return this["targetFrame"] != null ? this["targetFrame"] : ""; }
//            set { this["targetFrame"] = value; }
//        }

//        /// <summary>
//        /// Gets or sets the image URL.
//        /// </summary>
//        /// <value>The image URL.</value>
//        public string ImageUrl
//        {
//            get { return this["imageUrl"] != null ? this["imageUrl"] : ""; }
//            set { this["imageUrl"] = value; }
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
//                if (!string.IsNullOrEmpty(this["dynamicNodeProvider"]))
//                {
//                    if (dynamicNodeProvider == null)
//                    {
//                        dynamicNodeProvider = Activator.CreateInstance(
//                            Type.GetType(this["dynamicNodeProvider"])) as IDynamicNodeProvider;
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
//                    this["dynamicNodeProvider"] = value.GetType().AssemblyQualifiedName;
//                }
//                else
//                {
//                    this["dynamicNodeProvider"] = null;
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
//                if (!string.IsNullOrEmpty(this["urlResolver"]))
//                {
//                    if (urlResolver == null)
//                    {
//                        urlResolver = Activator.CreateInstance(
//                            Type.GetType(this["urlResolver"])) as ISiteMapNodeUrlResolver;
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
//                    this["urlResolver"] = value.GetType().AssemblyQualifiedName;
//                }
//                else
//                {
//                    this["urlResolver"] = null;
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
//                if (!string.IsNullOrEmpty(this["visibilityProvider"]))
//                {
//                    if (visibilityProvider == null)
//                    {
//                        visibilityProvider = Activator.CreateInstance(
//                            Type.GetType(this["visibilityProvider"])) as ISiteMapNodeVisibilityProvider;
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
//                    this["visibilityProvider"] = value.GetType().AssemblyQualifiedName;
//                }
//                else
//                {
//                    this["visibilityProvider"] = null;
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
//            get { return this["preservedRouteParameters"] != null ? this["preservedRouteParameters"] : ""; }
//            set { this["preservedRouteParameters"] = value; }
//        }

//        /// <summary>
//        /// Gets or sets the inherited route parameters (= Route values that should be inherited from the parent sitemap node. This is not a replacement for the SiteMapPreserveRouteDataAttribute.)
//        /// </summary>
//        /// <value>
//        /// The inherited route parameters.
//        /// </value>
//        public string InheritedRouteParameters
//        {
//            get { return this["inheritedRouteParameters"] != null ? this["inheritedRouteParameters"] : ""; }
//            set { this["inheritedRouteParameters"] = value; }
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
//        public override string Url
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
//                this["url"] = value;
//            }
//        }

//        string title = "";

//        /// <summary>
//        /// Gets or sets the title of the <see cref="T:System.Web.SiteMapNode"/> object.
//        /// </summary>
//        /// <value></value>
//        /// <returns>
//        /// A string that represents the title of the node. The default is <see cref="F:System.String.Empty"/>.
//        /// </returns>
//        /// <exception cref="T:System.InvalidOperationException">
//        /// The node is read-only.
//        /// </exception>
//        [Localizable(true)]
//        public override string Title
//        {
//            get
//            {
//                if (!string.IsNullOrEmpty(title))
//                {
//                    return title;
//                }
//                if (Provider.EnableLocalization)
//                {
//                    var implicitResourceString = GetImplicitResourceString("title");
//                    if (implicitResourceString != null && implicitResourceString == this["title"])
//                    {
//                        return implicitResourceString;
//                    }
//                    implicitResourceString = GetExplicitResourceString("title", this["title"], true);
//                    if (implicitResourceString != null && implicitResourceString == this["title"])
//                    {
//                        return implicitResourceString;
//                    }
//                }
//                if (this["title"] != null)
//                {
//                    return this["title"];
//                }
//                return string.Empty;
//            }
//            set
//            {
//                this["title"] = value;
//                title = value;
//            }
//        }

//        string description;

//        /// <summary>
//        /// Gets or sets a description for the <see cref="T:System.Web.SiteMapNode"/>.
//        /// </summary>
//        /// <value></value>
//        /// <returns>
//        /// A string that represents a description of the node; otherwise, <see cref="F:System.String.Empty"/>.
//        /// </returns>
//        /// <exception cref="T:System.InvalidOperationException">
//        /// The node is read-only.
//        /// </exception>
//        [Localizable(true)]
//        public override string Description
//        {
//            get
//            {
//                if (!string.IsNullOrEmpty(description))
//                {
//                    return description;
//                }
//                if (Provider.EnableLocalization)
//                {
//                    var implicitResourceString = GetImplicitResourceString("description");
//                    if (implicitResourceString != null && implicitResourceString == this["description"])
//                    {
//                        return implicitResourceString;
//                    }
//                    implicitResourceString = GetExplicitResourceString("description", this["description"], true);
//                    if (implicitResourceString != null && implicitResourceString == this["description"])
//                    {
//                        return implicitResourceString;
//                    }
//                }

//                if (this["description"] != null)
//                {
//                    return this["description"];
//                }
//                return string.Empty;
//            }
//            set
//            {
//                this["description"] = value;
//                description = value;
//            }
//        }

//        /// <summary>
//        /// Gets or sets a value indicating whether this instance is dynamic.
//        /// </summary>
//        /// <value>
//        /// 	<c>true</c> if this instance is dynamic; otherwise, <c>false</c>.
//        /// </value>
//        public bool IsDynamic { get; set; }

//        #endregion

//        #region Constructor

//        /// <summary>
//        /// Initializes a new instance of the <see cref="MvcSiteMapNode"/> class.
//        /// </summary>
//        /// <param name="provider">The provider.</param>
//        /// <param name="key">The key.</param>
//        /// <param name="explicitResourceKeys">The explicit resource keys.</param>
//        /// <param name="implicitResourceKey">The implicit resource key.</param>
//        public MvcSiteMapNode(SiteMapProvider provider, string key, NameValueCollection explicitResourceKeys, string implicitResourceKey)
//            : base(provider, key, "", "", "", new ArrayList(), new NameValueCollection(), explicitResourceKeys, implicitResourceKey)
//        {
//        }

//        #endregion

//        #region ICloneable

//        /// <summary>
//        /// Creates a new node that is a copy of the current node.
//        /// </summary>
//        /// <returns>
//        /// A new node that is a copy of the current node.
//        /// </returns>
//        public override SiteMapNode Clone()
//        {
//            return Clone(Key);
//        }

//        /// <summary>
//        /// Creates a new node that is a copy of the current node with a specified key.
//        /// </summary>
//        /// <param name="key">The key.</param>
//        /// <returns>
//        /// A new node that is a copy of the current node.
//        /// </returns>
//        public virtual SiteMapNode Clone(string key)
//        {
//            MvcSiteMapNode clone = new MvcSiteMapNode(Provider, key, new NameValueCollection(), null);
//            if (RouteValues != null)
//            {
//                clone.RouteValues = new Dictionary<string, object>(RouteValues);
//            }
//            if (Attributes != null)
//            {
//                clone.Attributes = new NameValueCollection(Attributes);
//            }
//            if (Roles != null)
//            {
//                clone.Roles = new ArrayList(Roles);
//            }

//            clone.Title = Title;
//            clone.Description = Description;
//            clone.Clickable = Clickable;
//            clone.LastModifiedDate = LastModifiedDate;
//            clone.ChangeFrequency = ChangeFrequency;
//            clone.UpdatePriority = UpdatePriority;
//            clone.TargetFrame = TargetFrame;
//            clone.ImageUrl = ImageUrl;
//            clone.PreservedRouteParameters = PreservedRouteParameters;
//            clone.InheritedRouteParameters = InheritedRouteParameters;

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
//    }
//}