using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.DI;
using MvcSiteMapProvider.Text;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// This class acts as the root of a SiteMap object graph and maintains a map
    /// between the child <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> nodes.
    /// </summary>
    /// <remarks>
    /// This class was created by extracting the public interfaces of SiteMapProvider, 
    /// StaticSiteMapProvider, and MvcSiteMapProvider.DefaultSiteMapProvider.
    /// </remarks>
    [ExcludeFromAutoRegistration]
    public class SiteMap
        : ISiteMap
    {
        public SiteMap(
            ISiteMapPluginProvider pluginProvider,
            IMvcContextFactory mvcContextFactory,
            ISiteMapChildStateFactory siteMapChildStateFactory,
            IUrlPath urlPath,
            ISiteMapSettings siteMapSettings
            )
        {
            if (pluginProvider == null)
                throw new ArgumentNullException("pluginProvider");
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");
            if (siteMapChildStateFactory == null)
                throw new ArgumentNullException("siteMapChildStateFactory");
            if (urlPath == null)
                throw new ArgumentNullException("urlPath");
            if (siteMapSettings == null)
                throw new ArgumentNullException("siteMapSettings");

            this.pluginProvider = pluginProvider;
            this.mvcContextFactory = mvcContextFactory;
            this.siteMapChildStateFactory = siteMapChildStateFactory;
            this.urlPath = urlPath;
            this.siteMapSettings = siteMapSettings;

            // Initialize dictionaries
            this.childNodeCollectionTable = siteMapChildStateFactory.CreateGenericDictionary<ISiteMapNode, ISiteMapNodeCollection>();
            this.keyTable = siteMapChildStateFactory.CreateGenericDictionary<string, ISiteMapNode>();
            this.parentNodeTable = siteMapChildStateFactory.CreateGenericDictionary<ISiteMapNode, ISiteMapNode>();
            this.urlTable = siteMapChildStateFactory.CreateGenericDictionary<string, ISiteMapNode>();
        }

        // Services
        protected readonly ISiteMapPluginProvider pluginProvider;
        protected readonly IMvcContextFactory mvcContextFactory;
        protected readonly ISiteMapChildStateFactory siteMapChildStateFactory;
        protected readonly IUrlPath urlPath;
        private readonly ISiteMapSettings siteMapSettings;

        // Child collections
        protected readonly IDictionary<ISiteMapNode, ISiteMapNodeCollection> childNodeCollectionTable;
        protected readonly IDictionary<string, ISiteMapNode> keyTable;
        protected readonly IDictionary<ISiteMapNode, ISiteMapNode> parentNodeTable;
        protected readonly IDictionary<string, ISiteMapNode> urlTable;

        // Object state
        protected readonly object synclock = new object();
        protected ISiteMapNode root;

        #region ISiteMap Members

        /// <summary>
        /// Gets whether the current sitemap is read-only.
        /// </summary>
        /// <value><c>true</c> if the current sitemap is read-only; otherwise <c>false</c>.</value>
        public bool IsReadOnly { get; protected set; }

        /// <summary>
        /// Adds a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> object to the node collection that is maintained by the site map provider.
        /// </summary>
        /// <param name="node">The <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> to add to the node collection maintained by the provider.</param>
        public virtual void AddNode(ISiteMapNode node)
        {
            this.AddNode(node, null);
        }

        /// <summary>
        /// Adds a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> to the collections that are maintained by the site map provider and establishes a 
        /// parent/child relationship between the <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> objects.
        /// </summary>
        /// <param name="node">The <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> to add to the site map provider.</param>
        /// <param name="parentNode">The <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> under which to add <paramref name="node"/>.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="node"/> is null.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// The <see cref="P:MvcSiteMapProvider.SiteMapNode.Url"/> or <see cref="P:MvcSiteMapProvider.SiteMapNode.Key"/> is already registered with 
        /// the <see cref="T:MvcSiteMapProvider.SiteMap"/>. A site map node must be made up of pages with unique URLs or keys.
        /// </exception>
        public virtual void AddNode(ISiteMapNode node, ISiteMapNode parentNode)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            AssertSiteMapNodeConfigurationIsValid(node);

            // Avoid issue with url table not clearing correctly.
            if (this.FindSiteMapNode(node.Url) != null)
            {
                this.RemoveNode(node);
            }

            // Add the node
            try
            {
                AddNodeInternal(node, parentNode);
            }
            catch
            {
                if (parentNode != null) this.RemoveNode(parentNode);
                AddNodeInternal(node, parentNode);
            }
        }

        protected virtual void AddNodeInternal(ISiteMapNode node, ISiteMapNode parentNode)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            lock (this.synclock)
            {
                bool urlPrepared = false;
                bool urlEncoded = false;
                string url = node.Url;
                if (!string.IsNullOrEmpty(url))
                {
                    if (node.HasAbsoluteUrl())
                    {
                        // This is an external url, so we will encode it
                        url = urlPath.UrlEncode(url);
                        urlEncoded = true;
                    }
                    if (urlPath.AppDomainAppVirtualPath != null)
                    {
                        if (!urlPath.IsAbsolutePhysicalPath(url))
                        {
                            url = urlPath.MakeVirtualPathAppAbsolute(urlPath.Combine(urlPath.AppDomainAppVirtualPath, url));
                        }
                        if (this.urlTable.ContainsKey(url))
                        {
                            if (urlEncoded)
                            {
                                url = urlPath.UrlDecode(url);
                            }
                            throw new InvalidOperationException(String.Format(Resources.Messages.MultipleNodesWithIdenticalUrl, url));
                        }
                    }
                    urlPrepared = true;
                }
                string key = node.Key;
                if (this.keyTable.ContainsKey(key))
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.MultipleNodesWithIdenticalKey, key));
                }
                this.keyTable[key] = node;
                if (urlPrepared)
                {
                    this.urlTable[url] = node;
                }
                if (parentNode != null)
                {
                    this.parentNodeTable[node] = parentNode;
                    if (!this.childNodeCollectionTable.ContainsKey(parentNode))
                    {
                        this.childNodeCollectionTable[parentNode] = siteMapChildStateFactory.CreateLockableSiteMapNodeCollection(this);
                    }
                    this.childNodeCollectionTable[parentNode].Add(node);
                }
            }
        }

        public virtual void RemoveNode(ISiteMapNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            lock (this.synclock)
            {
                ISiteMapNode parentNode = null;
                if (this.parentNodeTable.ContainsKey(node))
                {
                    parentNode = this.parentNodeTable[node];
                    this.parentNodeTable.Remove(node);
                }
                if (parentNode != null)
                {
                    var nodes = this.childNodeCollectionTable[parentNode];
                    if ((nodes != null) && nodes.Contains(node))
                    {
                        nodes.Remove(node);
                    }
                }
                string url = node.Url;
                if (((url != null) && (url.Length > 0)) && this.urlTable.ContainsKey(url))
                {
                    this.urlTable.Remove(url);
                }
                string key = node.Key;
                if (this.keyTable.ContainsKey(key))
                {
                    this.keyTable.Remove(key);
                }
            }
        }

        public virtual void Clear()
        {
            lock (this.synclock)
            {
                root = null;
                this.childNodeCollectionTable.Clear();
                this.urlTable.Clear();
                this.parentNodeTable.Clear();
                this.keyTable.Clear();
            }
        }

        /// <summary>
        /// Gets the root <see cref="T:MvcSiteMapProvider.SiteMapNode"/> object of the site map data that the current provider represents.
        /// </summary>
        public virtual ISiteMapNode RootNode
        {
            get { return this.ReturnNodeIfAccessible(root); }
        }

        public virtual void BuildSiteMap()
        {
            // If this was called before, just ignore this call.
            if (root != null) return;
            root = pluginProvider.SiteMapBuilder.BuildSiteMap(this, root);
            if (root == null)
            {
                throw new MvcSiteMapException(Resources.Messages.SiteMapRootNodeNotDefined);
            }
        }

        /// <summary>
        /// Gets the <see cref="T:MvcSiteMapProvider.SiteMapNode"/> object that represents the currently requested page.
        /// </summary>
        /// <returns>A <see cref="T:MvcSiteMapProvider.SiteMapNode"/> that represents the currently requested page; otherwise, 
        /// null, if the <see cref="T:MvcSiteMapProvider.SiteMapNode"/> is not found or cannot be returned for the current user.</returns>
        public virtual ISiteMapNode CurrentNode
        {
            get
            {
                var currentNode = this.FindSiteMapNodeFromCurrentContext();
                return this.ReturnNodeIfAccessible(currentNode);
            }
        }

        /// <summary>
        /// Gets a Boolean value indicating whether localized values of <see cref="T:MvcSiteMapProvider.SiteMapNode">SiteMapNode</see> 
        /// attributes are returned.
        /// </summary>
        /// <remarks>
        /// The EnableLocalization property is used for the get accessor of the Title and Description properties, as well as additional 
        /// Attributes properties of a SiteMapNode object.
        /// </remarks>
        public virtual bool EnableLocalization
        {
            get { return this.siteMapSettings.EnableLocalization; }
        }

        public virtual ISiteMapNode FindSiteMapNode(string rawUrl)
        {
            if (rawUrl == null)
            {
                throw new ArgumentNullException("rawUrl");
            }
            rawUrl = rawUrl.Trim();
            if (rawUrl.Length == 0)
            {
                return null;
            }
            if (urlPath.IsAppRelativePath(rawUrl))
            {
                rawUrl = urlPath.MakeVirtualPathAppAbsolute(rawUrl);
            }
            if (this.urlTable.ContainsKey(rawUrl))
            {
                return this.ReturnNodeIfAccessible(this.urlTable[rawUrl]);
            }
            return null;
        }

        /// <summary>
        /// Retrieves a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> object that represents the currently requested page using the current <see cref="T:System.Web.HttpContext"/> object.
        /// </summary>
        /// <returns>
        /// A <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> that represents the currently requested page; otherwise, null, if no corresponding <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> can be found in the <see cref="T:MvcSiteMapProvider.SiteMapNode"/> or if the page context is null.
        /// </returns>
        public virtual ISiteMapNode FindSiteMapNodeFromCurrentContext()
        {
            var httpContext = mvcContextFactory.CreateHttpContext();
            return FindSiteMapNode(httpContext);
        }

        /// <summary>
        /// Finds the site map node.
        /// </summary>
        /// <param name="context">The controller context.</param>
        /// <returns></returns>
        public virtual ISiteMapNode FindSiteMapNode(ControllerContext context)
        {
            return this.FindSiteMapNode(context.HttpContext);
        }

        public virtual ISiteMapNode FindSiteMapNodeFromKey(string key)
        {
            ISiteMapNode node = this.FindSiteMapNode(key);
            if (node == null && this.keyTable.ContainsKey(key))
            {
                node = this.keyTable[key];
            }
            return this.ReturnNodeIfAccessible(node);
        }

        public virtual ISiteMapNodeCollection GetChildNodes(ISiteMapNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            ISiteMapNodeCollection collection = null;
            if (this.childNodeCollectionTable.ContainsKey(node))
            {
                collection = this.childNodeCollectionTable[node];
            }
            if (collection == null)
            {
                ISiteMapNode keyNode = null;
                if (this.keyTable.ContainsKey(node.Key))
                {
                    keyNode = this.keyTable[node.Key];
                }
                if (keyNode != null && this.childNodeCollectionTable.ContainsKey(keyNode))
                {
                    collection = this.childNodeCollectionTable[keyNode];
                }
            }
            if (collection == null)
            {
                return siteMapChildStateFactory.CreateEmptyReadOnlySiteMapNodeCollection();
            }
            if (!this.SecurityTrimmingEnabled)
            {
                return siteMapChildStateFactory.CreateReadOnlySiteMapNodeCollection(collection);
            }
            var secureCollection = siteMapChildStateFactory.CreateSiteMapNodeCollection();
            foreach (ISiteMapNode secureNode in collection)
            {
                if (secureNode.IsAccessibleToUser())
                {
                    secureCollection.Add(secureNode);
                }
            }
            return siteMapChildStateFactory.CreateReadOnlySiteMapNodeCollection(secureCollection);
        }

        public virtual ISiteMapNodeCollection GetDescendants(ISiteMapNode node)
        {
            var descendants = siteMapChildStateFactory.CreateSiteMapNodeCollection();
            GetDescendantsInternal(node, descendants);
            return siteMapChildStateFactory.CreateReadOnlySiteMapNodeCollection(descendants);
        }

        public virtual ISiteMapNodeCollection GetAncestors(ISiteMapNode node)
        {
            var ancestors = siteMapChildStateFactory.CreateSiteMapNodeCollection();
            GetAncestorsInternal(node, ancestors);
            return siteMapChildStateFactory.CreateReadOnlySiteMapNodeCollection(ancestors);
        }

        protected virtual void GetDescendantsInternal(ISiteMapNode node, ISiteMapNodeCollection descendants)
        {
            foreach (var child in node.ChildNodes)
            {
                descendants.Add(child);
                GetDescendantsInternal(child, descendants);
            }
        }

        protected virtual void GetAncestorsInternal(ISiteMapNode node, ISiteMapNodeCollection ancestors)
        {
            if (node.ParentNode != null)
            {
                ancestors.Add(node.ParentNode);
                GetAncestorsInternal(node.ParentNode, ancestors);
            }
        }

        public virtual ISiteMapNode GetCurrentNodeAndHintAncestorNodes(int upLevel)
        {
            if (upLevel < -1)
            {
                throw new ArgumentOutOfRangeException("upLevel");
            }
            return this.CurrentNode;
        }

        public virtual ISiteMapNode GetCurrentNodeAndHintNeighborhoodNodes(int upLevel, int downLevel)
        {
            if (upLevel < -1)
            {
                throw new ArgumentOutOfRangeException("upLevel");
            }
            if (downLevel < -1)
            {
                throw new ArgumentOutOfRangeException("downLevel");
            }
            return this.CurrentNode;

        }

        public virtual ISiteMapNode GetParentNode(ISiteMapNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            ISiteMapNode parentNode = null;
            if (this.parentNodeTable.ContainsKey(node))
            {
                parentNode = this.parentNodeTable[node];
            }
            if (parentNode == null)
            {
                ISiteMapNode keyNode = null;
                if (this.keyTable.ContainsKey(node.Key))
                {
                    keyNode = this.keyTable[node.Key];
                }
                if (keyNode != null)
                {
                    if (this.parentNodeTable.ContainsKey(keyNode))
                    {
                        parentNode = this.parentNodeTable[keyNode];
                    }
                }
            }
            return this.ReturnNodeIfAccessible(parentNode);
        }

        public virtual ISiteMapNode GetParentNodeRelativeToCurrentNodeAndHintDownFromParent(int walkupLevels, int relativeDepthFromWalkup)
        {
            if (walkupLevels < 0)
            {
                throw new ArgumentOutOfRangeException("walkupLevels");
            }
            if (relativeDepthFromWalkup < 0)
            {
                throw new ArgumentOutOfRangeException("relativeDepthFromWalkup");
            }
            var currentNodeAndHintAncestorNodes = this.GetCurrentNodeAndHintAncestorNodes(walkupLevels);
            if (currentNodeAndHintAncestorNodes == null)
            {
                return null;
            }
            var parentNodesInternal = this.GetParentNodesInternal(currentNodeAndHintAncestorNodes, walkupLevels);
            if (parentNodesInternal == null)
            {
                return null;
            }
            this.HintNeighborhoodNodes(parentNodesInternal, 0, relativeDepthFromWalkup);
            return parentNodesInternal;

        }

        public virtual ISiteMapNode GetParentNodeRelativeToNodeAndHintDownFromParent(ISiteMapNode node, int walkupLevels, int relativeDepthFromWalkup)
        {
            if (walkupLevels < 0)
            {
                throw new ArgumentOutOfRangeException("walkupLevels");
            }
            if (relativeDepthFromWalkup < 0)
            {
                throw new ArgumentOutOfRangeException("relativeDepthFromWalkup");
            }
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            this.HintAncestorNodes(node, walkupLevels);
            var parentNodesInternal = this.GetParentNodesInternal(node, walkupLevels);
            if (parentNodesInternal == null)
            {
                return null;
            }
            this.HintNeighborhoodNodes(parentNodesInternal, 0, relativeDepthFromWalkup);
            return parentNodesInternal;
        }

        public virtual void HintAncestorNodes(ISiteMapNode node, int upLevel)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (upLevel < -1)
            {
                throw new ArgumentOutOfRangeException("upLevel");
            }
        }

        public virtual void HintNeighborhoodNodes(ISiteMapNode node, int upLevel, int downLevel)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (upLevel < -1)
            {
                throw new ArgumentOutOfRangeException("upLevel");
            }
            if (downLevel < -1)
            {
                throw new ArgumentOutOfRangeException("downLevel");
            }

        }

        /// <summary>
        /// Retrieves a Boolean value indicating whether the specified <see cref="T:MvcSiteMapProvider.SiteMapNode"/> object can be viewed by the user in the specified context.
        /// </summary>
        /// <param name="node">The <see cref="T:MvcSiteMapProvider.SiteMapNode"/> that is requested by the user.</param>
        /// <returns>
        /// true if security trimming is enabled and <paramref name="node"/> can be viewed by the user or security trimming is not enabled; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="context"/> is null.
        /// - or -
        /// <paramref name="node"/> is null.
        /// </exception>
        public virtual bool IsAccessibleToUser(ISiteMapNode node)
        {
            // If the sitemap is still being constructed, always
            // make all nodes accessible regardless of security trimming.
            if (!IsReadOnly)
            {
                return true;
            }
            if (!SecurityTrimmingEnabled)
            {
                return true;
            }
            return pluginProvider.AclModule.IsAccessibleToUser(this, node);
        }

        /// <summary>
        /// Get or sets the resource key that is used for localizing <see cref="T:MvcSiteMapProvider.SiteMapNode"/> attributes. 
        /// </summary>
        /// <remarks>
        /// The ResourceKey property is used with the GetImplicitResourceString method of the <see cref="T:MvcSiteMapProvider.SiteMapNode"/> class. 
        /// For the Title and Description properties, as well as any additional attributes that are defined in the Attributes collection of the 
        /// <see cref="T:MvcSiteMapProvider.SiteMapNode"/> object, the GetImplicitResourceString method takes precedence over the 
        /// GetExplicitResourceString when the localization is enabled with the EnableLocalization property set to true. 
        /// </remarks>
        public virtual string ResourceKey { get; set; }

        /// <summary>
        /// Gets a Boolean value indicating whether a site map provider filters site map nodes based on a user's role.
        /// </summary>
        public bool SecurityTrimmingEnabled
        {
            get { return this.siteMapSettings.SecurityTrimmingEnabled; }
        }

        /// <summary>
        /// Gets a Boolean value indicating whether the site map nodes should use the value of the Title 
        /// property for the default if no value for Description is provided.
        /// </summary>
        public bool UseTitleIfDescriptionNotProvided
        {
            get { return this.siteMapSettings.UseTitleIfDescriptionNotProvided; }
        }

        /// <summary>
        /// Gets a Boolean value indicating whether the visibility property of the current node
        /// will affect the descendant nodes.
        /// </summary>
        public bool VisibilityAffectsDescendants {
            get { return this.siteMapSettings.VisibilityAffectsDescendants; }
        }

        /// <summary>
        /// Resolves the controller type based on the current SiteMap instance.
        /// </summary>
        /// <remarks>There is 1 instance of controller type resolver per site map.</remarks>
        public Type ResolveControllerType(string areaName, string controllerName)
        {
            return pluginProvider.MvcResolver.ResolveControllerType(areaName, controllerName);
        }

        /// <summary>
        /// Resolves the action method parameters based on the current SiteMap instance.
        /// </summary>
        /// <remarks>There is 1 instance of action method parameter resolver per site map.</remarks>
        public IEnumerable<string> ResolveActionMethodParameters(string areaName, string controllerName, string actionMethodName)
        {
            return pluginProvider.MvcResolver.ResolveActionMethodParameters(areaName, controllerName, actionMethodName);
        }

        #endregion

        #region Protected Members

        protected virtual ISiteMapNode GetParentNodesInternal(ISiteMapNode node, int walkupLevels)
        {
            if (walkupLevels > 0)
            {
                do
                {
                    node = node.ParentNode;
                    walkupLevels--;
                }
                while ((node != null) && (walkupLevels != 0));
            }
            return node;
        }

        /// <summary>
        /// Finds the site map node.
        /// </summary>
        /// <param name="httpContext">The context.</param>
        /// <returns></returns>
        protected virtual ISiteMapNode FindSiteMapNode(HttpContextBase httpContext)
        {
            // Match RawUrl
            var node = this.FindSiteMapNodeFromRawUrl(httpContext);

            // Try MVC
            if (node == null)
            {
                node = this.FindSiteMapNodeFromMvc(httpContext);
            }

            // Try ASP.NET Classic (for interop)
            if (node == null)
            {
                node = this.FindSiteMapNodeFromAspNetClassic(httpContext);
            }

            // Try the path without the querystring
            if (node == null)
            {
                node = this.FindSiteMapNode(httpContext.Request.Path);
            }

            // Check accessibility
            return this.ReturnNodeIfAccessible(node);
        }

        protected virtual ISiteMapNode FindSiteMapNodeFromRawUrl(HttpContextBase httpContext)
        {
            var rawUrl = httpContext.Request.RawUrl;
            var node = this.FindSiteMapNode(rawUrl);
            if (node == null)
            {
                // Trim off the querystring from RawUrl and try again
                int index = rawUrl.IndexOf("?", StringComparison.Ordinal);
                if (index != -1)
                {
                    node = this.FindSiteMapNode(rawUrl.Substring(0, index));
                }
            }
            return node;
        }

        protected virtual ISiteMapNode FindSiteMapNodeFromMvc(HttpContextBase httpContext)
        {
            ISiteMapNode node = null;
            var routes = mvcContextFactory.GetRoutes();
            var routeData = routes.GetRouteData(httpContext);
            if (routeData != null)
            {
                if (!routeData.Values.ContainsKey("area"))
                {
                    if (routeData.DataTokens["area"] != null)
                    {
                        routeData.Values.Add("area", routeData.DataTokens["area"]);
                    }
                    else
                    {
                        routeData.Values.Add("area", "");
                    }
                }
                if (this.RootNode != null && this.RootNode.MatchesRoute(routeData.Values))
                {
                    node = RootNode;
                }
                if (node == null)
                {
                    node = FindSiteMapNodeFromControllerAction(RootNode, routeData.Values, routeData.Route);
                }
            }
            return node;
        }

        /// <summary>
        /// Finds the controller action node.
        /// </summary>
        /// <param name="rootNode">The root node.</param>
        /// <param name="values">The values.</param>
        /// <param name="route">The route.</param>
        /// <returns>
        /// A controller action node represented as a <see cref="SiteMapNode"/> instance
        /// </returns>
        protected virtual ISiteMapNode FindSiteMapNodeFromControllerAction(ISiteMapNode rootNode, IDictionary<string, object> values, RouteBase route)
        {
            if (rootNode != null)
            {
                // Get all child nodes
                var childNodes = GetChildNodes(rootNode);

                var routes = mvcContextFactory.GetRoutes();

                // Search current level
                foreach (ISiteMapNode node in childNodes)
                {
                    // Look at the route property
                    if (!string.IsNullOrEmpty(node.Route))
                    {
                        if (routes[node.Route] == route)
                        {
                            // This looks a bit weird, but if i set up a node to a general route ie /Controller/Action/ID
                            // I need to check that the values are the same so that it doesn't swallow all of the nodes that also use that same general route
                            if (node.MatchesRoute(values))
                            {
                                return node;
                            }
                        }
                    }
                    else if (node.MatchesRoute(values))
                    {
                        return node;
                    }

                    // Search next level
                    var siteMapNode = FindSiteMapNodeFromControllerAction(node, values, route);
                    if (siteMapNode != null)
                    {
                        return siteMapNode;
                    }
                }
            }
            return null;
        }

        protected virtual ISiteMapNode FindSiteMapNodeFromAspNetClassic(HttpContextBase httpContext)
        {
            ISiteMapNode node = null;
            Page currentHandler = httpContext.CurrentHandler as Page;
            if (currentHandler != null)
            {
                string clientQueryString = currentHandler.ClientQueryString;
                if (clientQueryString.Length > 0)
                {
                    node = this.FindSiteMapNode(httpContext.Request.Path + "?" + clientQueryString);
                }
            }
            return node;
        }

        protected virtual ISiteMapNode ReturnNodeIfAccessible(ISiteMapNode node)
        {
            if ((node != null) && node.IsAccessibleToUser())
            {
                return node;
            }
            return null;
        }

        protected virtual void AssertSiteMapNodeConfigurationIsValid(ISiteMapNode node)
        {
            ThrowIfTitleNotSet(node);
            ThrowIfControllerNameInvalid(node);
            ThrowIfAreaNameInvalid(node);
            ThrowIfActionAndUrlNotSet(node);
            ThrowIfHttpMethodInvalid(node);
            ThrowIfRouteValueIsPreservedRouteParameter(node);
        }

        protected virtual void ThrowIfRouteValueIsPreservedRouteParameter(ISiteMapNode node)
        {
            if (node.PreservedRouteParameters.Count > 0)
            {
                foreach (var key in node.PreservedRouteParameters)
                {
                    if (node.RouteValues.ContainsKey(key))
                        throw new MvcSiteMapException(String.Format(Resources.Messages.SiteMapNodeSameKeyInRouteValueAndPreservedRouteParameter, node.Key, node.Title, key));
                }
            }
        }

        protected virtual void ThrowIfActionAndUrlNotSet(ISiteMapNode node)
        {
            if (node.Clickable && String.IsNullOrEmpty(node.Action) && String.IsNullOrEmpty(node.UnresolvedUrl))
            {
                throw new MvcSiteMapException(String.Format(Resources.Messages.SiteMapNodeActionAndURLNotSet, node.Key, node.Title));
            }
        }

        protected virtual void ThrowIfTitleNotSet(ISiteMapNode node)
        {
            if (String.IsNullOrEmpty(node.Title))
            {
                throw new MvcSiteMapException(String.Format(Resources.Messages.SiteMapNodeTitleNotSet, node.Key));
            }
        }

        protected virtual void ThrowIfHttpMethodInvalid(ISiteMapNode node)
        {
            HttpVerbs verbs;
            if (String.IsNullOrEmpty(node.HttpMethod) || 
                (!EnumHelper.TryParse<HttpVerbs>(node.HttpMethod, true, out verbs) && 
                !node.HttpMethod.Equals("*") && 
                !node.HttpMethod.Equals("Request", StringComparison.InvariantCultureIgnoreCase)))
            {
                var allowedVerbs = String.Join(Environment.NewLine, Enum.GetNames(typeof(HttpVerbs))) + Environment.NewLine + "Request" + Environment.NewLine + "*";
                throw new MvcSiteMapException(String.Format(Resources.Messages.SiteMapNodeHttpMethodInvalid, node.Key, node.Title, node.HttpMethod, allowedVerbs));
            }
        }

        protected virtual void ThrowIfControllerNameInvalid(ISiteMapNode node)
        {
            if (!String.IsNullOrEmpty(node.Controller))
            {
                if (!node.Controller.IsValidIdentifier() || node.Controller.EndsWith("Controller"))
                {
                    throw new MvcSiteMapException(String.Format(Resources.Messages.SiteMapNodeControllerNameInvalid, node.Key, node.Title, node.Controller));
                }
            }
        }

        protected virtual void ThrowIfAreaNameInvalid(ISiteMapNode node)
        {
            if (!String.IsNullOrEmpty(node.Area))
            {
                if (!node.Area.IsValidIdentifier())
                {
                    throw new MvcSiteMapException(String.Format(Resources.Messages.SiteMapNodeAreaNameInvalid, node.Key, node.Title, node.Area));
                }
            }
        }

        #endregion

    }
}
