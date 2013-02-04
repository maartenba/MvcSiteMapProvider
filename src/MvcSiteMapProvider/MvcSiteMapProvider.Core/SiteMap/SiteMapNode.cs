using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
//using System.Linq;
using MvcSiteMapProvider.Core.Mvc.UrlResolver;
using MvcSiteMapProvider.Core.Collections;
using MvcSiteMapProvider.Core.Globalization;

namespace MvcSiteMapProvider.Core.SiteMap
{


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapNode
        : ISiteMapNode
    {
        public SiteMapNode(
            ISiteMap siteMap, 
            string key,
            string implicitResourceKey,
            bool isDynamic,
            //ISiteMapNodeFactory siteMapNodeFactory, 
            IExplicitResourceKeyParser explicitResourceKeyParser,
            IDynamicNodeProviderStrategy dynamicNodeProviderStrategy,
            ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy
            )
        {
            if (siteMap == null)
                throw new ArgumentNullException("siteMap");
            //if (string.IsNullOrEmpty(key))
            //    throw new ArgumentNullException("key");
            //if (siteMapNodeFactory == null)
            //    throw new ArgumentNullException("siteMapNodeFactory");
            if (explicitResourceKeyParser == null)
                throw new ArgumentNullException("explicitResourceKeyParser");
            if (dynamicNodeProviderStrategy == null)
                throw new ArgumentNullException("dynamicNodeProviderStrategy");
            if (siteMapNodeUrlResolverStrategy == null)
                throw new ArgumentNullException("siteMapNodeUrlResolverStrategy");

            this.siteMap = siteMap;
            this.key = key;
            this.ResourceKey = implicitResourceKey;
            this.IsDynamic = isDynamic;
            //this.siteMapNodeFactory = siteMapNodeFactory;
            this.explicitResourceKeyParser = explicitResourceKeyParser;
            this.dynamicNodeProviderStrategy = dynamicNodeProviderStrategy;
            this.siteMapNodeUrlResolverStrategy = siteMapNodeUrlResolverStrategy;

            // Initialize child objects
            //Attributes = new Dictionary<string, string>();
            this.attributes.CollectionChanged += new NotifyCollectionChangedEventHandler(Attributes_CollectionChanged);

            Roles = new List<string>();
            RouteValues = new Dictionary<string, object>();
            PreservedRouteParameters = new List<string>();

        }

        // Services
        //protected readonly ISiteMapNodeFactory siteMapNodeFactory;
        protected readonly IExplicitResourceKeyParser explicitResourceKeyParser;
        protected readonly IDynamicNodeProviderStrategy dynamicNodeProviderStrategy;
        protected readonly ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy;

        // Object State
        protected string key = String.Empty;
        protected ISiteMap siteMap;
        protected ISiteMapNode parentNode;
        protected bool isParentNodeSet = false;
        protected SiteMapNodeCollection childNodes;
        protected bool isChildNodesSet = false;
        protected string url = String.Empty;
        protected string title = String.Empty;
        protected string description = String.Empty;

        // Child collections and dictionaries
        protected NameValueCollection explicitResourceKeys = new NameValueCollection(); 
        protected readonly ObservableDictionary<string, string> attributes = new ObservableDictionary<string, string>();
        



        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public virtual string Key 
        { 
            get { return this.key; }
            protected set { this.key = value; }
        }

        public virtual bool IsDynamic { get; protected set; }

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
        public virtual SiteMapNodeCollection ChildNodes
        {
            get
            {
                if (this.isChildNodesSet)
                {
                    return this.childNodes;
                }
                return this.siteMap.GetChildNodes(this);
            }
            set
            {
                this.childNodes = value;
                this.isChildNodesSet = true;
            }
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
                IList siblingNodes = this.SiblingNodes;
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
                IList siblingNodes = this.SiblingNodes;
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
        private SiteMapNodeCollection SiblingNodes
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

        ///// <summary>
        ///// Gets or sets the URL.
        ///// </summary>
        ///// <value>
        ///// The URL.
        ///// </value>
        //public string Url { get; set; }

        // TODO: Determine what the value of this property is
        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public string HttpMethod { get; set; }



        /// <summary>
        /// Gets or sets the implicit resource key (optional).
        /// </summary>
        /// <value>The implicit resource key.</value>
        public string ResourceKey { get; protected set; }

        // TODO: Localize
        /// <summary>
        /// Gets or sets the title (optional).
        /// </summary>
        /// <value>The title.</value>
        public string Title 
        {
            get 
            {
                return this.GetResourceValue("title", title);
            }
            set 
            {
                this.title = value;
                explicitResourceKeyParser.HandleResourceAttribute("title", ref title, ref explicitResourceKeys);
            }
        }

        // TODO: Localize
        /// <summary>
        /// Gets or sets the description (optional).
        /// </summary>
        /// <value>The description.</value>
        public string Description 
        {
            get
            {
                return this.GetResourceValue("description", title);
            }
            set
            {
                this.title = value;
                explicitResourceKeyParser.HandleResourceAttribute("description", ref title, ref explicitResourceKeys);
            }
        }

        /// <summary>
        /// Gets or sets the target frame (optional).
        /// </summary>
        /// <value>The target frame.</value>
        public string TargetFrame { get; set; }

        /// <summary>
        /// Gets or sets the image URL (optional).
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl { get; set; }

        ///// <summary>
        ///// Gets or sets the attributes (optional).
        ///// </summary>
        ///// <value>The attributes.</value>
        //public IDictionary<string, string> Attributes { get; set; }

        /// <summary>
        /// Gets the attributes (optional).
        /// </summary>
        /// <value>The attributes.</value>
        public IDictionary<string, string> Attributes { get { return this.attributes; } }

        //private void foo()
        //{
        //    System.Collections.Specialized.NameValueCollection nvc = new System.Collections.Specialized.NameValueCollection();
        //    var x = nvc[0];

        //    IDictionary<string, string> nvc2 = new Dictionary<string, string>();
        //    var y = nvc2["test"];

        //    var a = new System.Web.Routing.RouteCollection();

        //    //var b = new System.Collections.ObjectModel.

        //    var c = new MvcSiteMapProvider.Core.Collections.ObservableDictionary<string, string>();

        //    //c.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(c_CollectionChanged);
        //}

        private void Attributes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:

                    foreach (var item in e.NewItems)
                    {
                        var pair = (KeyValuePair<string, string>)item;
                        string value = pair.Value;

                        explicitResourceKeyParser.HandleResourceAttribute(pair.Key, ref value, ref explicitResourceKeys);

                        // Update the node in the collection if it changed
                        if (this.Attributes[pair.Key] != value)
                        {
                            this.Attributes[pair.Key] = value;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:

                    foreach (var item in e.NewItems)
                    {
                        var pair = (KeyValuePair<string, string>)item;
                        explicitResourceKeys.Remove(pair.Key);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:

                    break;
            }

        }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public IList<string> Roles { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the change frequency.
        /// </summary>
        /// <value>The change frequency.</value>
        public ChangeFrequency ChangeFrequency { get; set; }

        /// <summary>
        /// Gets or sets the update priority.
        /// </summary>
        /// <value>The update priority.</value>
        public UpdatePriority UpdatePriority { get; set; }


        #region Visibility

        /// <summary>
        /// Gets or sets the name or the type of the visibility provider.
        /// This value will be used to select the concrete type of provider to use to determine
        /// visibility.
        /// </summary>
        /// <value>
        /// The name or type of the visibility provider.
        /// </value>
        public string VisibilityProvider { get; set; }


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
            // TODO: use strategy factory to provide implementation logic from concrete provider
            // http://stackoverflow.com/questions/1499442/best-way-to-use-structuremap-to-implement-strategy-pattern
            return true;
        }

        #endregion

        #region URL Resolver

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SiteMapNode" /> is clickable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if clickable; otherwise, <c>false</c>.
        /// </value>
        public bool Clickable { get; set; }

        /// <summary>
        /// Gets or sets the name or type of the URL resolver.
        /// </summary>
        /// <value>
        /// The name or type of the URL resolver.
        /// </value>
        public string UrlResolver { get; set; }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url 
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
                if (this.url != value)
                {
                    this.url = value;
                }
            }
        }

        public virtual string UnresolvedUrl { get { return this.url; } }

        #endregion

        #region Dynamic Nodes

        // TODO: make this lock after it is set so it cannot be changed by the UI?
        /// <summary>
        /// Gets or sets the name or type of the Dynamic Node Provider.
        /// </summary>
        /// <value>
        /// The name or type of the Dynamic Node Provider.
        /// </value>
        public string DynamicNodeProvider { get; set; }

        // TODO: use strategy factory to provide implementation logic from concrete provider
        // http://stackoverflow.com/questions/1499442/best-way-to-use-structuremap-to-implement-strategy-pattern

        /// <summary>
        /// Gets the dynamic node collection.
        /// </summary>
        /// <returns>A dynamic node collection.</returns>
        public IEnumerable<DynamicNode> GetDynamicNodeCollection()
        {
            return dynamicNodeProviderStrategy.GetDynamicNodeCollection(this.DynamicNodeProvider);
        }

        /// <summary>
        /// Gets whether the current node has a dynamic node provider.
        /// </summary>
        /// <value>
        /// True if there is a provider.
        /// </value>
        public bool HasDynamicNodeProvider
        {
            get { return (dynamicNodeProviderStrategy.GetProvider(this.DynamicNodeProvider) != null); }
        }

        #endregion

        #region Route

        /// <summary>
        /// Gets or sets the route.
        /// </summary>
        /// <value>The route.</value>
        public string Route { get; set; }


        // TODO: Change this to readonly property with class that inherits from RouteValueDictionary
        /// <summary>
        /// Gets or sets the route values.
        /// </summary>
        /// <value>The route values.</value>
        public IDictionary<string, object> RouteValues { get; set; }

        /// <summary>
        /// Gets or sets the preserved route parameter names (= values that will be used from the current request route).
        /// </summary>
        /// <value>The attributes.</value>
        public IList<string> PreservedRouteParameters { get; set; }


        /// <summary>
        /// Gets the route data associated with the current node.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>The route data associated with the current node.</returns>
        public RouteData GetRouteData(HttpContextBase httpContext)
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
        /// Gets or sets the area (optional).
        /// </summary>
        /// <value>The area.</value>
        public string Area { get; set; }

        /// <summary>
        /// Gets or sets the controller (optional).
        /// </summary>
        /// <value>The controller.</value>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the action (optional).
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; set; }

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


        /// <summary>
        /// Gets the level of the current SiteMapNode
        /// </summary>
        /// <param name="current">The current SiteMapNode</param>
        /// <returns>The level of the current SiteMapNode</returns>
        public int GetNodeLevel()
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

        // TODO: rework... (maartenba)

        /// <summary>
        /// Determines whether the specified node is in current path.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is in current path; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInCurrentPath()
        {
            ISiteMapNode node = this;
            return (this.siteMap.CurrentNode != null && (node == this.siteMap.CurrentNode || this.siteMap.CurrentNode.IsDescendantOf(node)));
        }


        public virtual bool HasChildNodes
        {
            get
            {
                IList childNodes = this.ChildNodes;
                return ((childNodes != null) && (childNodes.Count > 0));
            }
        }

        public virtual ISiteMap SiteMap
        {
            get
            {
                return this.siteMap;
            }
        }




//        #region Bubbling event Hooks

//        /// <summary>
//        /// For internal use.
//        /// </summary>
//        /// <param name="child">Child object.</param>
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        protected void AddEventHooks(IBusinessObject child)
//        {
//            OnAddEventHooks(child);
//        }

//        /// <summary>
//        /// Hook child object events.
//        /// </summary>
//        /// <param name="child">Child object.</param>
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        protected virtual void OnAddEventHooks(IBusinessObject child)
//        {
//            INotifyBusy busy = child as INotifyBusy;
//            if (busy != null)
//                busy.BusyChanged += Child_BusyChanged;

//            INotifyUnhandledAsyncException unhandled = child as INotifyUnhandledAsyncException;
//            if (unhandled != null)
//                unhandled.UnhandledAsyncException += Child_UnhandledAsyncException;

//            INotifyPropertyChanged pc = child as INotifyPropertyChanged;
//            if (pc != null)
//                pc.PropertyChanged += Child_PropertyChanged;

//#if !SILVERLIGHT
//            IBindingList bl = child as IBindingList;
//            if (bl != null)
//                bl.ListChanged += Child_ListChanged;
//#endif

//            INotifyCollectionChanged ncc = child as INotifyCollectionChanged;
//            if (ncc != null)
//                ncc.CollectionChanged += Child_CollectionChanged;

//            INotifyChildChanged cc = child as INotifyChildChanged;
//            if (cc != null)
//                cc.ChildChanged += Child_Changed;
//        }

//        /// <summary>
//        /// For internal use only.
//        /// </summary>
//        /// <param name="child">Child object.</param>
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        protected void RemoveEventHooks(IBusinessObject child)
//        {
//            OnRemoveEventHooks(child);
//        }

//        /// <summary>
//        /// Unhook child object events.
//        /// </summary>
//        /// <param name="child">Child object.</param>
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        protected virtual void OnRemoveEventHooks(IBusinessObject child)
//        {
//            INotifyBusy busy = child as INotifyBusy;
//            if (busy != null)
//                busy.BusyChanged -= Child_BusyChanged;

//            INotifyUnhandledAsyncException unhandled = child as INotifyUnhandledAsyncException;
//            if (unhandled != null)
//                unhandled.UnhandledAsyncException -= Child_UnhandledAsyncException;

//            INotifyPropertyChanged pc = child as INotifyPropertyChanged;
//            if (pc != null)
//                pc.PropertyChanged -= Child_PropertyChanged;

//#if !SILVERLIGHT
//            IBindingList bl = child as IBindingList;
//            if (bl != null)
//                bl.ListChanged -= Child_ListChanged;
//#endif

//            INotifyCollectionChanged ncc = child as INotifyCollectionChanged;
//            if (ncc != null)
//                ncc.CollectionChanged -= Child_CollectionChanged;

//            INotifyChildChanged cc = child as INotifyChildChanged;
//            if (cc != null)
//                cc.ChildChanged -= Child_Changed;
//        }

//        #endregion


        /// <summary>
        /// Gets the localized text for the attribute.
        /// </summary>
        /// <param name="attributeName">The name of the attribute (as it is in the original XML file).</param>
        /// <param name="attributeValue">The current value of the attribute in the object.</param>
        /// <returns></returns>
        protected string GetResourceValue(string attributeName, string attributeValue)
        {
            if (this.siteMap.EnableLocalization)
            {
                string resourceString = this.GetImplicitResourceString(attributeName);
                if (resourceString != null)
                {
                    return resourceString;
                }
                resourceString = this.GetExplicitResourceString(attributeName, attributeValue, true);
                if (resourceString != null)
                {
                    return resourceString;
                }
            }
            if (attributeValue != null)
            {
                return attributeValue;
            }
            return string.Empty;
        }



        // TODO: Move to either an injected service or a base class
        protected string GetImplicitResourceString(string attributeName)
        {
            if (attributeName == null)
            {
                throw new ArgumentNullException("attributeName");
            }
            string globalResourceObject = null;
            if (!string.IsNullOrEmpty(this.ResourceKey))
            {
                try
                {
                    globalResourceObject = HttpContext.GetGlobalResourceObject(this.siteMap.ResourceKey, this.ResourceKey + "." + attributeName) as string;
                }
                catch
                {
                }
            }
            return globalResourceObject;
        }

        // TODO: Move to either an injected service or a base class
        protected string GetExplicitResourceString(string attributeName, string defaultValue, bool throwIfNotFound)
        {
            if (attributeName == null)
            {
                throw new ArgumentNullException("attributeName");
            }
            string globalResourceObject = null;
            if (this.explicitResourceKeys != null)
            {
                string[] values = this.explicitResourceKeys.GetValues(attributeName);
                if ((values == null) || (values.Length <= 1))
                {
                    return globalResourceObject;
                }
                try
                {
                    globalResourceObject = HttpContext.GetGlobalResourceObject(values[0], values[1]) as string;
                }
                catch (System.Resources.MissingManifestResourceException)
                {
                    if (defaultValue != null)
                    {
                        return defaultValue;
                    }
                }
                if ((globalResourceObject == null) && throwIfNotFound)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.ResourceNotFoundWithClassAndKey, values[0], values[1]));
                }
            }
            return globalResourceObject;
        }


    }
}
