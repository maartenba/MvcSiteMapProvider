using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.DI;
using MvcSiteMapProvider.Matching;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Text;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.Routing;

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
            this.childNodeCollectionTable = siteMapChildStateFactory.CreateChildNodeCollectionDictionary();
            this.keyTable = siteMapChildStateFactory.CreateKeyDictionary();
            this.parentNodeTable = siteMapChildStateFactory.CreateParentNodeDictionary();
            this.urlTable = siteMapChildStateFactory.CreateUrlDictionary();
        }

        // TODO: In version 5, we should refactor this into separate services that each manage a single dictionary
        // and hide those services behind a facade service so there isn't so many responsibilities in this class.
        // This will help the process of eliminating child state factory and plugin provider which only serve to 
        // reduce the number of dependencies in this class, but technically are providing unrelated services.

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
        protected readonly IDictionary<IUrlKey, ISiteMapNode> urlTable;
    
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

            this.AssertSiteMapNodeConfigurationIsValid(node);

            // Add the node
            this.AddNodeInternal(node, parentNode);
        }

        protected virtual void AddNodeInternal(ISiteMapNode node, ISiteMapNode parentNode)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            lock (this.synclock)
            {
                IUrlKey url = null;
                bool isMvcUrl = string.IsNullOrEmpty(node.UnresolvedUrl) && node.UsesDefaultUrlResolver();

                // Only store URLs if they are clickable and are configured using the Url
                // property or provided by a custom URL resolver.
                if (!isMvcUrl && node.Clickable)
                {
                    url = this.siteMapChildStateFactory.CreateUrlKey(node);

                    // Check for duplicates (including matching or empty host names).
                    if (this.urlTable
                        .Where(k => string.Equals(k.Key.RootRelativeUrl, url.RootRelativeUrl, StringComparison.OrdinalIgnoreCase))
                        .Where(k => string.IsNullOrEmpty(k.Key.HostName) || string.IsNullOrEmpty(url.HostName) || string.Equals(k.Key.HostName, url.HostName, StringComparison.OrdinalIgnoreCase))
                        .Count() > 0)
                    {
                        var absoluteUrl = this.urlPath.ResolveUrl(node.UnresolvedUrl, string.IsNullOrEmpty(node.Protocol) ? Uri.UriSchemeHttp : node.Protocol, node.HostName);
                        throw new InvalidOperationException(string.Format(Resources.Messages.MultipleNodesWithIdenticalUrl, absoluteUrl));
                    }
                }

                // Add the key
                string key = node.Key;
                if (this.keyTable.ContainsKey(key))
                {
                    throw new InvalidOperationException(string.Format(Resources.Messages.MultipleNodesWithIdenticalKey, key));
                }
                this.keyTable[key] = node;

                // Add the URL
                if (url != null)
                {
                    this.urlTable[url] = node;
                }

                // Add the parent-child relationship
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
                // Remove the parent node relationship
                ISiteMapNode parentNode = null;
                if (this.parentNodeTable.ContainsKey(node))
                {
                    parentNode = this.parentNodeTable[node];
                    this.parentNodeTable.Remove(node);
                }

                // Remove the child node relationship
                if (parentNode != null)
                {
                    var nodes = this.childNodeCollectionTable[parentNode];
                    if ((nodes != null) && nodes.Contains(node))
                    {
                        nodes.Remove(node);
                    }
                }

                // Remove the URL
                var url = this.siteMapChildStateFactory.CreateUrlKey(node);
                if (this.urlTable.ContainsKey(url))
                {
                    this.urlTable.Remove(url);
                }

                // Remove the key
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

        /// <summary>
        /// Retrieves a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> object that represents the page at the specified URL.
        /// </summary>
        /// <param name="rawUrl">A URL that identifies the page for which to retrieve a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/>.</param>
        /// <returns>A <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> that represents the page identified by rawURL; otherwise, <b>null</b>, 
        /// if no corresponding <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> is found or if security trimming is enabled and the 
        /// <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> cannot be returned for the current user.</returns>
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

            // NOTE: If the URL passed is absolute, the public facing URL will be ignored
            // and the current URL will be the absolute URL that is passed.
            var publicFacingUrl = this.urlPath.GetPublicFacingUrl(this.HttpContext);
            var currentUrl = new Uri(publicFacingUrl, rawUrl);

            // Search the internal dictionary for the URL that is registered manually.
            var node = this.FindSiteMapNodeFromUrl(currentUrl.PathAndQuery, currentUrl.AbsolutePath, currentUrl.Host, HttpContext.CurrentHandler);

            // Search for the URL by creating a context based on the new URL and matching route values.
            if (node == null)
            {
                // Create a TextWriter with null stream as a backing stream 
                // which doesn't consume resources
                using (var nullWriter = new StreamWriter(Stream.Null))
                {
                    // Create a new HTTP context using the current URL.
                    var currentUrlHttpContext = this.mvcContextFactory.CreateHttpContext(null, currentUrl, nullWriter);

                    // Find node for the passed-in URL using the new HTTP context. This will do a
                    // match based on route values and/or query string values.
                    node = this.FindSiteMapNodeFromMvc(currentUrlHttpContext);
                }
            }

            return this.ReturnNodeIfAccessible(node);
        }

        /// <summary>
        /// Retrieves a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> object that represents the currently requested page using the current <see cref="T:System.Web.HttpContext"/> object.
        /// </summary>
        /// <returns>
        /// A <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> that represents the currently requested page; otherwise, <b>null</b>, 
        /// if no corresponding <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> can be found in the <see cref="T:MvcSiteMapProvider.SiteMapNode"/> or if the page context is null.
        /// </returns>
        public virtual ISiteMapNode FindSiteMapNodeFromCurrentContext()
        {
            return FindSiteMapNode(this.HttpContext);
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

        /// <summary>
        /// Retrieves a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> object based on a specified key.
        /// </summary>
        /// <param name="key">A lookup key with which a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> instance is created.</param>
        /// <returns>A <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> that represents the page identified by key; otherwise, <b>null</b>, 
        /// if no corresponding <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> is found or if security trimming is enabled and the 
        /// <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> cannot be returned for the current user. The default is null.</returns>
        public virtual ISiteMapNode FindSiteMapNodeFromKey(string key)
        {
            ISiteMapNode node = null;
            if (this.keyTable.ContainsKey(key))
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
        ///     <paramref name="node"/> is null.
        /// </exception>
        public virtual bool IsAccessibleToUser(ISiteMapNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

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
        public virtual string ResourceKey
        {
            get { return this.siteMapSettings.SiteMapCacheKey; }
            set { /* do nothing */ }
        }

        /// <summary>
        /// Gets a string representing the cache key of the current SiteMap object. This key (which can be though of as the name) can be used
        /// to retrieve the SiteMap object. It is also used to build request-cache keys so values can persist in the same request across SiteMap builds.
        /// </summary>
        public virtual string CacheKey
        {
            get { return this.siteMapSettings.SiteMapCacheKey; }
        }

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
        [Obsolete("ResolveActionMethodParameters is deprecated and will be removed in version 5.")]
        public IEnumerable<string> ResolveActionMethodParameters(string areaName, string controllerName, string actionMethodName)
        {
            return pluginProvider.MvcResolver.ResolveActionMethodParameters(areaName, controllerName, actionMethodName);
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// Gets the current HTTP context.
        /// </summary>
        protected virtual HttpContextBase HttpContext { get { return this.mvcContextFactory.CreateHttpContext(); } }

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
            // Try URL
            var node = this.FindSiteMapNodeFromPublicFacingUrl(httpContext);

            // Try MVC
            if (node == null)
            {
                node = this.FindSiteMapNodeFromMvc(httpContext);
            }

            // Check accessibility
            return this.ReturnNodeIfAccessible(node);
        }

        protected virtual ISiteMapNode FindSiteMapNodeFromPublicFacingUrl(HttpContextBase httpContext)
        {
            var publicFacingUrl = this.urlPath.GetPublicFacingUrl(httpContext);
            return this.FindSiteMapNodeFromUrl(publicFacingUrl.PathAndQuery, publicFacingUrl.AbsolutePath, publicFacingUrl.Host, httpContext.CurrentHandler);
        }

        protected virtual ISiteMapNode FindSiteMapNodeFromUrl(string relativeUrl, string relativePath, string hostName, IHttpHandler handler)
        {
            ISiteMapNode node = null;

            // Try absolute match with querystring
            var absoluteMatch = this.siteMapChildStateFactory.CreateUrlKey(relativeUrl, hostName);
            node = this.FindSiteMapNodeFromUrlMatch(absoluteMatch);

            // Try absolute match without querystring
            if (node == null && !string.IsNullOrEmpty(relativePath))
            {
                var absoluteMatchWithoutQueryString = this.siteMapChildStateFactory.CreateUrlKey(relativePath, hostName);
                node = this.FindSiteMapNodeFromUrlMatch(absoluteMatchWithoutQueryString);
            }

            // Try relative match
            if (node == null)
            {
                var relativeMatch = this.siteMapChildStateFactory.CreateUrlKey(relativeUrl, string.Empty);
                node = this.FindSiteMapNodeFromUrlMatch(relativeMatch);
            }

            // Try relative match with ASP.NET handler querystring
            if (node == null)
            {
                Page currentHandler = handler as Page;
                if (currentHandler != null)
                {
                    string clientQueryString = currentHandler.ClientQueryString;
                    if (clientQueryString.Length > 0)
                    {
                        var aspNetRelativeMatch = this.siteMapChildStateFactory.CreateUrlKey(relativePath + "?" + clientQueryString, string.Empty);
                        node = this.FindSiteMapNodeFromUrlMatch(aspNetRelativeMatch);
                    }
                }
            }

            // Try relative match without querystring
            if (node == null && !string.IsNullOrEmpty(relativePath))
            {
                var relativeMatchWithoutQueryString = this.siteMapChildStateFactory.CreateUrlKey(relativePath, string.Empty);
                node = this.FindSiteMapNodeFromUrlMatch(relativeMatchWithoutQueryString);
            }

            return node;
        }

        protected virtual ISiteMapNode FindSiteMapNodeFromUrlMatch(IUrlKey urlToMatch)
        {
            if (this.urlTable.ContainsKey(urlToMatch))
            {
                return this.urlTable[urlToMatch];
            }

            return null;
        }

        protected virtual ISiteMapNode FindSiteMapNodeFromMvc(HttpContextBase httpContext)
        {
            var routeData = this.GetMvcRouteData(httpContext);
            if (routeData != null)
            {
                return FindSiteMapNodeFromMvcRoute(routeData.Values, routeData.Route);
            }
            return null;
        }

        /// <summary>
        /// Finds the node that matches the MVC route.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="route">The route.</param>
        /// <returns>
        /// A controller action node represented as a <see cref="SiteMapNode"/> instance
        /// </returns>
        protected virtual ISiteMapNode FindSiteMapNodeFromMvcRoute(IDictionary<string, object> values, RouteBase route)
        {
            var routes = mvcContextFactory.GetRoutes();

            // keyTable contains every node in the SiteMap
            foreach (var node in this.keyTable.Values)
            {
                // Look at the route property
                if (!string.IsNullOrEmpty(node.Route))
                {
                    // This looks a bit weird, but if I set up a node to a general route i.e. /Controller/Action/ID
                    // I need to check that the values are the same so that it doesn't swallow all of the nodes that also use that same general route
                    if (routes[node.Route] == route && node.MatchesRoute(values))
                    {
                        return node;
                    }
                }
                else if (node.MatchesRoute(values))
                {
                    return node;
                }
            }

            return null;
        }

        protected virtual RouteData GetMvcRouteData(HttpContextBase httpContext)
        {
            var routes = mvcContextFactory.GetRoutes();
            var routeData = routes.GetRouteData(httpContext);
            if (routeData != null)
            {
                if (routeData.Values.ContainsKey("MS_DirectRouteMatches"))
                {
                    routeData = ((IEnumerable<RouteData>)routeData.Values["MS_DirectRouteMatches"]).First();
                }
                this.SetMvcArea(routeData);
            }

            return routeData;
        }

        protected virtual void SetMvcArea(RouteData routeData)
        {
            if (routeData != null)
            {
                if (!routeData.Values.ContainsKey("area"))
                {
                    routeData.Values.Add("area", routeData.GetAreaName());
                }
            }
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
            ThrowIfHostNameInvalid(node);
            ThrowIfCanonicalUrlHostNameInvalid(node);
            ThrowIfImageUrlHostNameInvalid(node);
        }

        protected virtual void ThrowIfRouteValueIsPreservedRouteParameter(ISiteMapNode node)
        {
            if (node.PreservedRouteParameters.Count > 0)
            {
                foreach (var key in node.PreservedRouteParameters)
                {
                    if (node.RouteValues.ContainsKey(key))
                        throw new MvcSiteMapException(string.Format(Resources.Messages.SiteMapNodeSameKeyInRouteValueAndPreservedRouteParameter, node.Key, node.Title, key));
                }
            }
        }

        protected virtual void ThrowIfActionAndUrlNotSet(ISiteMapNode node)
        {
            if (node.Clickable && string.IsNullOrEmpty(node.Action) && string.IsNullOrEmpty(node.UnresolvedUrl))
            {
                throw new MvcSiteMapException(string.Format(Resources.Messages.SiteMapNodeActionAndURLNotSet, node.Key, node.Title));
            }
        }

        protected virtual void ThrowIfTitleNotSet(ISiteMapNode node)
        {
            if (string.IsNullOrEmpty(node.Title))
            {
                throw new MvcSiteMapException(string.Format(Resources.Messages.SiteMapNodeTitleNotSet, node.Key));
            }
        }

        protected virtual void ThrowIfHttpMethodInvalid(ISiteMapNode node)
        {
            HttpVerbs verbs;
            if (string.IsNullOrEmpty(node.HttpMethod) || 
                (!EnumHelper.TryParse<HttpVerbs>(node.HttpMethod, true, out verbs) && 
                !node.HttpMethod.Equals("*") && 
                !node.HttpMethod.Equals("Request", StringComparison.InvariantCultureIgnoreCase)))
            {
                var allowedVerbs = string.Join(Environment.NewLine, Enum.GetNames(typeof(HttpVerbs))) + Environment.NewLine + "Request" + Environment.NewLine + "*";
                throw new MvcSiteMapException(string.Format(Resources.Messages.SiteMapNodeHttpMethodInvalid, node.Key, node.Title, node.HttpMethod, allowedVerbs));
            }
        }

        protected virtual void ThrowIfControllerNameInvalid(ISiteMapNode node)
        {
            if (!string.IsNullOrEmpty(node.Controller))
            {
                if (!node.Controller.IsValidIdentifier() || node.Controller.EndsWith("Controller"))
                {
                    throw new MvcSiteMapException(string.Format(Resources.Messages.SiteMapNodeControllerNameInvalid, node.Key, node.Title, node.Controller));
                }
            }
        }

        protected virtual void ThrowIfAreaNameInvalid(ISiteMapNode node)
        {
            if (!string.IsNullOrEmpty(node.Area))
            {
                if (!node.Area.IsValidIdentifier())
                {
                    throw new MvcSiteMapException(string.Format(Resources.Messages.SiteMapNodeAreaNameInvalid, node.Key, node.Title, node.Area));
                }
            }
        }

        protected virtual void ThrowIfHostNameInvalid(ISiteMapNode node)
        {
            if (!string.IsNullOrEmpty(node.HostName) && node.HostName.Contains(":"))
            {
                throw new MvcSiteMapException(string.Format(Resources.Messages.SiteMapNodeHostNameInvalid, node.Key, node.Title, node.HostName, "HostName"));
            }
        }

        protected virtual void ThrowIfImageUrlHostNameInvalid(ISiteMapNode node)
        {
            if (!string.IsNullOrEmpty(node.ImageUrlHostName) && node.ImageUrlHostName.Contains(":"))
            {
                throw new MvcSiteMapException(string.Format(Resources.Messages.SiteMapNodeHostNameInvalid, node.Key, node.Title, node.ImageUrlHostName, "ImageUrlHostName"));
            }
        }

        protected virtual void ThrowIfCanonicalUrlHostNameInvalid(ISiteMapNode node)
        {
            if (!string.IsNullOrEmpty(node.CanonicalUrlHostName) && node.CanonicalUrlHostName.Contains(":"))
            {
                throw new MvcSiteMapException(string.Format(Resources.Messages.SiteMapNodeHostNameInvalid, node.Key, node.Title, node.CanonicalUrlHostName, "CanonicalUrlHostName"));
            }
        }

        #endregion
    }
}
