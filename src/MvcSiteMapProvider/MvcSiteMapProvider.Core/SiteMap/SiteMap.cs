// -----------------------------------------------------------------------
// <copyright file="SiteMap.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Util;
    using System.Web.UI;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Xml.Linq;
    using MvcSiteMapProvider.Core.Security;
    using MvcSiteMapProvider.Core.Mvc;

    /// <summary>
    /// This class was created by extracting the public intefaces of SiteMapProvider, 
    /// StaticSiteMapProvider, and MvcSiteMapProvider.DefaultSiteMapProvider.
    /// </summary>
    public class SiteMap : ISiteMap
    {
        private readonly IAclModule aclModule;
        private readonly IActionMethodParameterResolver actionMethodParameterResolver;
        private readonly IControllerTypeResolver controllerTypeResolver;

        public SiteMap(
            IAclModule aclModule, 
            IActionMethodParameterResolver actionMethodParameterResolver,
            IControllerTypeResolver controllerTypeResolver
            )
        {
            if (aclModule == null)
                throw new ArgumentNullException("aclModule");
            if (actionMethodParameterResolver == null)
                throw new ArgumentNullException("actionMethodParameterResolver");
            if (controllerTypeResolver == null)
                throw new ArgumentNullException("controllerTypeResolver");

            this.aclModule = aclModule;
            this.actionMethodParameterResolver = actionMethodParameterResolver;
            this.controllerTypeResolver = controllerTypeResolver;
        }

        #region SiteMapProvider state

        private const string _allRoles = "*";
        private bool _enableLocalization;
        internal readonly object _lock = new object();
        private SiteMap _parentProvider;
        private object _resolutionTicket = new object();
        private string _resourceKey;
        private SiteMap _rootProvider;
        private bool _securityTrimmingEnabled;
        internal const string _securityTrimmingEnabledAttrName = "securityTrimmingEnabled";

        //public event SiteMapResolveEventHandler SiteMapResolve;

        #endregion

        #region StaticSiteMapProvider state

        private Hashtable _childNodeCollectionTable;
        private Hashtable _keyTable;
        private Hashtable _parentNodeTable;
        private Hashtable _urlTable;

        #endregion

        #region DefaultSiteMapProvider state

        protected const string RootName = "mvcSiteMap";
        protected const string NodeName = "mvcSiteMapNode";
        protected readonly XNamespace ns = "http://mvcsitemap.codeplex.com/schemas/MvcSiteMap-File-3.0";
        protected readonly object synclock = new object();
        protected string cacheKey;
        protected string aclCacheItemKey;
        protected string currentNodeCacheKey;
        protected SiteMapNode root;
        protected bool isBuildingSiteMap = false;
        protected bool scanAssembliesForSiteMapNodes;
        protected string siteMapFile = string.Empty;
        protected string siteMapFileAbsolute = string.Empty;
        protected List<string> excludeAssembliesForScan = new List<string>();
        protected List<string> includeAssembliesForScan = new List<string>();
        protected List<string> attributesToIgnore = new List<string>();

        #endregion

        protected SiteMapNode currentNode;


        #region ISiteMap Members

        /// <summary>
        /// Gets the <see cref="T:System.Web.SiteMapNode"/> object that represents the currently requested page.
        /// </summary>
        /// <returns>A <see cref="T:System.Web.SiteMapNode"/> that represents the currently requested page; otherwise, 
        /// null, if the <see cref="T:System.Web.SiteMapNode"/> is not found or cannot be returned for the current user.</returns>
        public System.Web.SiteMapNode CurrentNode
        {
            get 
            {
                HttpContext current = HttpContext.Current;
                currentNode = this.ResolveSiteMapNode(current);
                if (currentNode == null)
                {
                    currentNode = this.FindSiteMapNode(current);
                }
                return this.ReturnNodeIfAccessible(currentNode);
            }
        }

        public bool EnableLocalization
        {
            get
            {
                return this._enableLocalization;
            }
            set
            {
                this._enableLocalization = value;
            }
        }

        public System.Web.SiteMapNode FindSiteMapNode(string rawUrl)
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
            if (UrlPath.IsAppRelativePath(rawUrl))
            {
                rawUrl = UrlPath.MakeVirtualPathAppAbsolute(rawUrl);
            }
            this.BuildSiteMap();
            return this.ReturnNodeIfAccessible((SiteMapNode)this.UrlTable[rawUrl]);
        }

        /// <summary>
        /// Retrieves a <see cref="T:System.Web.SiteMapNode"/> object that represents the currently requested page using the specified <see cref="T:System.Web.HttpContext"/> object.
        /// </summary>
        /// <param name="context">The <see cref="T:System.Web.HttpContext"/> used to match node information with the URL of the requested page.</param>
        /// <returns>
        /// A <see cref="T:System.Web.SiteMapNode"/> that represents the currently requested page; otherwise, null, if no corresponding <see cref="T:System.Web.SiteMapNode"/> can be found in the <see cref="T:System.Web.SiteMapNode"/> or if the page context is null.
        /// </returns>
        public System.Web.SiteMapNode FindSiteMapNode(System.Web.HttpContext context)
        {
            var httpContext = new HttpContext2(context);
            var routeData = RouteTable.Routes.GetRouteData(httpContext);

            var currentNode = FindSiteMapNode(HttpContext.Current, routeData);
            if (HttpContext.Current.Items[currentNodeCacheKey] == null && currentNode != null)
            {
                HttpContext.Current.Items[currentNodeCacheKey] = currentNode;
            }
            return currentNode;
        }

        public System.Web.SiteMapNode FindSiteMapNodeFromKey(string key)
        {
            SiteMapNode node = this.FindSiteMapNode(key);
            if (node == null)
            {
                node = (SiteMapNode)this.KeyTable[key];
            }
            return this.ReturnNodeIfAccessible(node);

            //SiteMapNode node = this.FindSiteMapNode(key);
            //if (node == null)
            //{
            //    foreach (SiteMapProvider provider in this.ChildProviderList)
            //    {
            //        this.EnsureChildSiteMapProviderUpToDate(provider);
            //        node = provider.FindSiteMapNodeFromKey(key);
            //        if (node != null)
            //        {
            //            return node;
            //        }
            //    }
            //}
            //return node;

        }

        public System.Web.SiteMapNodeCollection GetChildNodes(System.Web.SiteMapNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            this.BuildSiteMap();
            SiteMapNodeCollection collection = (SiteMapNodeCollection)this.ChildNodeCollectionTable[node];
            if (collection == null)
            {
                SiteMapNode node2 = (SiteMapNode)this.KeyTable[node.Key];
                if (node2 != null)
                {
                    collection = (SiteMapNodeCollection)this.ChildNodeCollectionTable[node2];
                }
            }
            if (collection == null)
            {
                //return SiteMapNodeCollection.Empty;
                return new SiteMapNodeCollection();
            }
            if (!this.SecurityTrimmingEnabled)
            {
                return SiteMapNodeCollection.ReadOnly(collection);
            }
            HttpContext current = HttpContext.Current;
            SiteMapNodeCollection nodes2 = new SiteMapNodeCollection(collection.Count);
            foreach (SiteMapNode node3 in collection)
            {
                if (node3.IsAccessibleToUser(current))
                {
                    nodes2.Add(node3);
                }
            }
            return SiteMapNodeCollection.ReadOnly(nodes2);
        }

        public System.Web.SiteMapNode GetCurrentNodeAndHintAncestorNodes(int upLevel)
        {
            if (upLevel < -1)
            {
                throw new ArgumentOutOfRangeException("upLevel");
            }
            return this.CurrentNode;

        }

        public System.Web.SiteMapNode GetCurrentNodeAndHintNeighborhoodNodes(int upLevel, int downLevel)
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

        public System.Web.SiteMapNode GetParentNode(System.Web.SiteMapNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            this.BuildSiteMap();
            SiteMapNode parentNode = (SiteMapNode)this.ParentNodeTable[node];
            if (parentNode == null)
            {
                SiteMapNode node3 = (SiteMapNode)this.KeyTable[node.Key];
                if (node3 != null)
                {
                    parentNode = (SiteMapNode)this.ParentNodeTable[node3];
                }
            }
            if ((parentNode == null) && (this.ParentProvider != null))
            {
                parentNode = this.ParentProvider.GetParentNode(node);
            }
            return this.ReturnNodeIfAccessible(parentNode);
        }

        public System.Web.SiteMapNode GetParentNodeRelativeToCurrentNodeAndHintDownFromParent(int walkupLevels, int relativeDepthFromWalkup)
        {
            if (walkupLevels < 0)
            {
                throw new ArgumentOutOfRangeException("walkupLevels");
            }
            if (relativeDepthFromWalkup < 0)
            {
                throw new ArgumentOutOfRangeException("relativeDepthFromWalkup");
            }
            SiteMapNode currentNodeAndHintAncestorNodes = this.GetCurrentNodeAndHintAncestorNodes(walkupLevels);
            if (currentNodeAndHintAncestorNodes == null)
            {
                return null;
            }
            SiteMapNode parentNodesInternal = this.GetParentNodesInternal(currentNodeAndHintAncestorNodes, walkupLevels);
            if (parentNodesInternal == null)
            {
                return null;
            }
            this.HintNeighborhoodNodes(parentNodesInternal, 0, relativeDepthFromWalkup);
            return parentNodesInternal;

        }

        public System.Web.SiteMapNode GetParentNodeRelativeToNodeAndHintDownFromParent(System.Web.SiteMapNode node, int walkupLevels, int relativeDepthFromWalkup)
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
            SiteMapNode parentNodesInternal = this.GetParentNodesInternal(node, walkupLevels);
            if (parentNodesInternal == null)
            {
                return null;
            }
            this.HintNeighborhoodNodes(parentNodesInternal, 0, relativeDepthFromWalkup);
            return parentNodesInternal;
        }

        public void HintAncestorNodes(System.Web.SiteMapNode node, int upLevel)
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

        public void HintNeighborhoodNodes(System.Web.SiteMapNode node, int upLevel, int downLevel)
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

        public void Initialize(string name, System.Collections.Specialized.NameValueCollection attributes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a Boolean value indicating whether the specified <see cref="T:System.Web.SiteMapNode"/> object can be viewed by the user in the specified context.
        /// </summary>
        /// <param name="context">The <see cref="T:System.Web.HttpContext"/> that contains user information.</param>
        /// <param name="node">The <see cref="T:System.Web.SiteMapNode"/> that is requested by the user.</param>
        /// <returns>
        /// true if security trimming is enabled and <paramref name="node"/> can be viewed by the user or security trimming is not enabled; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="context"/> is null.
        /// - or -
        /// <paramref name="node"/> is null.
        /// </exception>
        public bool IsAccessibleToUser(System.Web.HttpContext context, System.Web.SiteMapNode node)
        {
            if ((isBuildingSiteMap && CacheDuration > 0) || !SecurityTrimmingEnabled)
            {
                return true;
            }

            // Construct call cache?
            if (context.Items[aclCacheItemKey] == null)
            {
                context.Items[aclCacheItemKey] = new Dictionary<SiteMapNode, bool>();
            }
            Dictionary<SiteMapNode, bool> aclCacheItem
                = (Dictionary<SiteMapNode, bool>)context.Items[aclCacheItemKey];

            // Is the result of this call cached?
            if (!aclCacheItem.ContainsKey(node))
            {
                aclCacheItem[node] = aclModule.IsAccessibleToUser(controllerTypeResolver, this, context, node);
            }
            return aclCacheItem[node];
        }

        public SiteMap ParentProvider
        {
            get
            {
                return this._parentProvider;

            }
            set
            {
                this._parentProvider = value;

            }
        }

        public string ResourceKey
        {
            get
            {
                return this._resourceKey;
            }
            set
            {
                this._resourceKey = value;
            }
        }

        public System.Web.SiteMapNode RootNode
        {
            get 
            {
                SiteMapNode rootNodeCore = this.GetRootNodeCore();
                return this.ReturnNodeIfAccessible(rootNodeCore);
            }
        }

        /// <summary>
        /// Returns the current root, otherwise calls the BuildSiteMap method.
        /// </summary>
        /// <returns></returns>
        protected SiteMapNode GetRootNodeCore()
        {
            lock (synclock)
            {
                BuildSiteMap();
                return root;
            }
        }

        public bool SecurityTrimmingEnabled
        {
            get { return this._securityTrimmingEnabled; }
        }

        public System.Web.SiteMapNode BuildSiteMap()
        {
            // TODO: Provide implementation
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the duration of the cache.
        /// </summary>
        /// <value>The duration of the cache.</value>
        public int CacheDuration { get; private set; }

                /// <summary>
        /// Finds the site map node.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public SiteMapNode FindSiteMapNode(ControllerContext context)
        {
            return FindSiteMapNode(HttpContext.Current, context.RouteData);
        }

        /// <summary>
        /// Finds the site map node.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="routeData">The route data.</param>
        /// <returns></returns>
        private SiteMapNode FindSiteMapNode(HttpContext context, RouteData routeData)
        {
            // Node
            SiteMapNode node = null;

            // Fetch route data
            var httpContext = new HttpContext2(context);
            if (routeData != null)
            {
                RequestContext requestContext = new RequestContext(httpContext, routeData);
                VirtualPathData vpd = routeData.Route.GetVirtualPath(
                    requestContext, routeData.Values);
                string appPathPrefix = (requestContext.HttpContext.Request.ApplicationPath
                    ?? string.Empty).TrimEnd('/') + "/";
                node = this.FindSiteMapNode(httpContext.Request.Path) as MvcSiteMapNode;

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

                MvcSiteMapNode mvcNode = node as MvcSiteMapNode;
                if (mvcNode == null || routeData.Route != RouteTable.Routes[mvcNode.Route])
                {
                    if (NodeMatchesRoute(RootNode as MvcSiteMapNode, routeData.Values))
                    {
                        node = RootNode;
                    }
                }

                if (node == null)
                {
                    node = FindControllerActionNode(RootNode, routeData.Values, routeData.Route);
                }
            }

            // Try base class
            if (node == null)
            {
                node = this.FindSiteMapNode(context);
            }

            // Check accessibility
            if (node != null)
            {
                if (node.IsAccessibleToUser(context))
                {
                    return node;
                }
            }
            return null;
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Members 

        /// <summary>
        /// Finds the controller action node.
        /// </summary>
        /// <param name="rootNode">The root node.</param>
        /// <param name="values">The values.</param>
        /// <param name="route">The route.</param>
        /// <returns>
        /// A controller action node represented as a <see cref="SiteMapNode"/> instance
        /// </returns>
        private SiteMapNode FindControllerActionNode(SiteMapNode rootNode, IDictionary<string, object> values, RouteBase route)
        {
            if (rootNode != null)
            {
                // Get all child nodes
                SiteMapNodeCollection childNodes = GetChildNodes(rootNode);

                // Search current level
                foreach (SiteMapNode node in childNodes)
                {
                    // Check if it is an MvcSiteMapNode
                    var mvcNode = node as MvcSiteMapNode;
                    if (mvcNode != null)
                    {
                        // Look at the route property
                        if (!string.IsNullOrEmpty(mvcNode.Route))
                        {
                            if (RouteTable.Routes[mvcNode.Route] == route)
                            {
                                // This looks a bit weird, but if i set up a node to a general route ie /Controller/Action/ID
                                // I need to check that the values are the same so that it doesn't swallow all of the nodes that also use that same general route
                                if (NodeMatchesRoute(mvcNode, values))
                                {
                                    return mvcNode;
                                }
                            }
                        }
                        else if (NodeMatchesRoute(mvcNode, values))
                        {
                            return mvcNode;
                        }
                    }
                }

                // Search one deeper level
                foreach (SiteMapNode node in childNodes)
                {
                    var siteMapNode = FindControllerActionNode(node, values, route);
                    if (siteMapNode != null)
                    {
                        return siteMapNode;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Nodes the matches route.
        /// </summary>
        /// <param name="mvcNode">The MVC node.</param>
        /// <param name="values">The values.</param>
        /// <returns>
        /// A matches route represented as a <see cref="bool"/> instance 
        /// </returns>
        private bool NodeMatchesRoute(MvcSiteMapNode mvcNode, IDictionary<string, object> values)
        {
            var nodeValid = true;

            if (mvcNode != null)
            {
                // Find action method parameters?
                IEnumerable<string> actionParameters = new List<string>();
                if (mvcNode.DynamicNodeProvider == null && mvcNode.IsDynamic == false)
                {
                    actionParameters = actionMethodParameterResolver.ResolveActionMethodParameters(
                        controllerTypeResolver, mvcNode.Area, mvcNode.Controller, mvcNode.Action);
                }

                // Verify route values
                if (values.Count > 0)
                {
                    // Checking for same keys and values.
                    if (!CompareMustMatchRouteValues(mvcNode.RouteValues, values))
                    {
                        return false;
                    }

                    foreach (var pair in values)
                    {
                        if (!string.IsNullOrEmpty(mvcNode[pair.Key]))
                        {
                            if (mvcNode[pair.Key].ToLowerInvariant() == pair.Value.ToString().ToLowerInvariant())
                            {
                                continue;
                            }
                            else
                            {
                                // Is the current pair.Key a parameter on the action method?
                                if (!actionParameters.Contains(pair.Key, StringComparer.InvariantCultureIgnoreCase))
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if (pair.Value == null || string.IsNullOrEmpty(pair.Value.ToString()) || pair.Value == UrlParameter.Optional)
                            {
                                continue;
                            }
                            else if (pair.Key == "area")
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            else
            {
                nodeValid = false;
            }

            return nodeValid;
        }

        /// <summary>
        /// Returns whether the two route value collections have same keys and same values.
        /// </summary>
        /// <param name="mvcNodeRouteValues">The route values of the original node.</param>
        /// <param name="routeValues">The route values to check in the given node.</param>
        /// <returns><c>True</c> if the <paramref name="mvcNodeRouteValues"/> contains all keys and the same values as the given <paramref name="routeValues"/>, otherwise <c>false</c>.</returns>
        private static bool CompareMustMatchRouteValues(IDictionary<string, object> mvcNodeRouteValues, IDictionary<string, object> routeValues)
        {
            var routeKeys = mvcNodeRouteValues.Keys;

            foreach (var pair in routeValues)
            {
                if (routeKeys.Contains(pair.Key) && !mvcNodeRouteValues[pair.Key].ToString().Equals(pair.Value.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }



        private SiteMapNode GetParentNodesInternal(SiteMapNode node, int walkupLevels)
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

        private SiteMapNode ReturnNodeIfAccessible(SiteMapNode node)
        {
            if ((node != null) && node.IsAccessibleToUser(HttpContext.Current))
            {
                return node;
            }
            return null;
        }

        private SiteMapNode ResolveSiteMapNode(HttpContext context)
        {
            //SiteMapResolveEventHandler siteMapResolve = this.SiteMapResolve;
            //if ((siteMapResolve != null) && !context.Items.Contains(this._resolutionTicket))
            //{
            //    context.Items.Add(this._resolutionTicket, true);
            //    try
            //    {
            //        Delegate[] invocationList = siteMapResolve.GetInvocationList();
            //        int length = invocationList.Length;
            //        for (int i = 0; i < length; i++)
            //        {
            //            SiteMapNode node = ((SiteMapResolveEventHandler)invocationList[i])(this, new SiteMapResolveEventArgs(context, this));
            //            if (node != null)
            //            {
            //                return node;
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        context.Items.Remove(this._resolutionTicket);
            //    }
            //}
            return null;
        }

        private IDictionary ChildNodeCollectionTable
        {
            get
            {
                if (this._childNodeCollectionTable == null)
                {
                    lock (this._lock)
                    {
                        if (this._childNodeCollectionTable == null)
                        {
                            this._childNodeCollectionTable = new Hashtable();
                        }
                    }
                }
                return this._childNodeCollectionTable;
            }
        }

        private IDictionary KeyTable
        {
            get
            {
                if (this._keyTable == null)
                {
                    lock (this._lock)
                    {
                        if (this._keyTable == null)
                        {
                            this._keyTable = new Hashtable();
                        }
                    }
                }
                return this._keyTable;
            }
        }

        private IDictionary ParentNodeTable
        {
            get
            {
                if (this._parentNodeTable == null)
                {
                    lock (this._lock)
                    {
                        if (this._parentNodeTable == null)
                        {
                            this._parentNodeTable = new Hashtable();
                        }
                    }
                }
                return this._parentNodeTable;
            }
        }

        private IDictionary UrlTable
        {
            get
            {
                if (this._urlTable == null)
                {
                    lock (this._lock)
                    {
                        if (this._urlTable == null)
                        {
                            this._urlTable = new Hashtable(StringComparer.OrdinalIgnoreCase);
                        }
                    }
                }
                return this._urlTable;
            }
        }

        #endregion

    }
}
