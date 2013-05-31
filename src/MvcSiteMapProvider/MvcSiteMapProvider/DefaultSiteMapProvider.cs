#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;
using MvcSiteMapProvider.Extensibility;
using MvcSiteMapProvider.External;

#endregion

namespace MvcSiteMapProvider
{
    /// <summary>
    /// DefaultSiteMapProvider class
    /// </summary>
    public class DefaultSiteMapProvider
        : StaticSiteMapProvider
    {
        #region Private

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

        #region Properties

        /// <summary>
        /// Gets or sets the node key generator.
        /// </summary>
        /// <value>The node key generator.</value>
        public INodeKeyGenerator NodeKeyGenerator { get; set; }

        /// <summary>
        /// Gets or sets the controller type resolver.
        /// </summary>
        /// <value>The controller type resolver.</value>
        public IControllerTypeResolver ControllerTypeResolver { get; set; }

        /// <summary>
        /// Gets or sets the action method parameter resolver.
        /// </summary>
        /// <value>The action method parameter resolver.</value>
        public IActionMethodParameterResolver ActionMethodParameterResolver { get; set; }

        /// <summary>
        /// Gets or sets the acl module.
        /// </summary>
        /// <value>The acl module.</value>
        public IAclModule AclModule { get; set; }

        /// <summary>
        /// Gets or sets the site map node URL resolver.
        /// </summary>
        /// <value>The site map node URL resolver.</value>
        public ISiteMapNodeUrlResolver SiteMapNodeUrlResolver { get; set; }

        /// <summary>
        /// Gets or sets the site map node visibility provider.
        /// </summary>
        /// <value>The site map node visibility provider.</value>
        public ISiteMapNodeVisibilityProvider SiteMapNodeVisibilityProvider { get; set; }

        /// <summary>
        /// Gets or sets the site map provider event handler.
        /// </summary>
        /// <value>The site map provider event handler.</value>
        public ISiteMapProviderEventHandler SiteMapProviderEventHandler { get; set; }

        /// <summary>
        /// Gets or sets the duration of the cache.
        /// </summary>
        /// <value>The duration of the cache.</value>
        public int CacheDuration { get; private set; }

        /// <summary>
        /// Get or sets the name of HTTP method to use when checking node accessibility.
        /// </summary>
        /// <value>
        /// The name of HTTP method to use when checking node accessibility or null to use
        /// the method of the current request. Defaults to null.
        /// </value>
        //public string RouteMethod { get; private set; }

        /// <summary>
        /// Gets the RootNode for the current SiteMapProvider.
        /// </summary>
        public override SiteMapNode RootNode
        {
            get
            {
                return GetRootNodeCore();
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.SiteMapNode"/> object that represents the currently requested page.
        /// </summary>
        /// <returns>A <see cref="T:System.Web.SiteMapNode"/> that represents the currently requested page; otherwise, null, if the <see cref="T:System.Web.SiteMapNode"/> is not found or cannot be returned for the current user.</returns>
        public override SiteMapNode CurrentNode
        {
            get
            {
                if (HttpContext.Current.Items[currentNodeCacheKey] == null)
                {
                    var currentNode = base.CurrentNode;
                    HttpContext.Current.Items[currentNodeCacheKey] = currentNode;
                    return currentNode;
                }
                return (SiteMapNode)HttpContext.Current.Items[currentNodeCacheKey];
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSiteMapProvider"/> class.
        /// </summary>
        public DefaultSiteMapProvider()
        {
            CacheDuration = 5;
            cacheKey = "__MVCSITEMAP_" + Guid.NewGuid().ToString();
            aclCacheItemKey = "__MVCSITEMAP_ACL_" + Guid.NewGuid().ToString();
            currentNodeCacheKey = "__MVCSITEMAP_CN_" + Guid.NewGuid().ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the current root, otherwise calls the BuildSiteMap method.
        /// </summary>
        /// <returns></returns>
        protected override SiteMapNode GetRootNodeCore()
        {
            lock (synclock)
            {
                BuildSiteMap();
                return root;
            }
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
        public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
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
                aclCacheItem[node] = AclModule.IsAccessibleToUser(ControllerTypeResolver, this, context, node);
            }
            return aclCacheItem[node];
        }

        /// <summary>
        /// Initializes our custom provider, gets the attributes that are set in the config
        /// that enable us to customise the behaiviour of this provider.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        public override void Initialize(string name, NameValueCollection attributes)
        {
            // Initialize base
            base.Initialize(name, attributes);

            // Get the siteMapFile from the passed in attributes.
            siteMapFile = attributes["siteMapFile"];
            if (!string.IsNullOrEmpty(siteMapFile))
            {
                siteMapFileAbsolute = HttpContext.Current.Server.MapPath(siteMapFile);
                if (!File.Exists(siteMapFileAbsolute))
                {
                    throw new MvcSiteMapException(Resources.Messages.SiteMapFileNotFound);
                }
            }

            // If a cacheDuration was passed in set it, otherwise
            // it will default to 5 minutes.
            if (!string.IsNullOrEmpty(attributes["cacheDuration"]))
            {
                CacheDuration = int.Parse(attributes["cacheDuration"]);
            }

            // If a cache key was set in config, set it. 
            // Otherwise it will use the default which is a GUID.
            if (!string.IsNullOrEmpty(attributes["cacheKey"]))
            {
                cacheKey = attributes["cacheKey"];
            }

            // Enable Localization
            if (!string.IsNullOrEmpty(attributes["enableLocalization"]))
            {
                EnableLocalization = Boolean.Parse(attributes["enableLocalization"]);
            }

            // Resource key
            if (!string.IsNullOrEmpty(attributes["resourceKey"]))
            {
                ResourceKey = attributes["resourceKey"];
            }
            else if (!string.IsNullOrEmpty(siteMapFile))
            {
                ResourceKey = Path.GetFileName(siteMapFile).ToLowerInvariant();
            }

            // Scan assemblies for IMvcSiteMapNodeAttribute?
            if (!string.IsNullOrEmpty(attributes["scanAssembliesForSiteMapNodes"]))
            {
                scanAssembliesForSiteMapNodes = Boolean.Parse(attributes["scanAssembliesForSiteMapNodes"]);

                // TODO Deprecated ---
                // Which assemblies should be skipped?
                if (!string.IsNullOrEmpty(attributes["skipAssemblyScanOn"]))
                {
                    throw new MvcSiteMapException("The skipAssemblyScanOn attribute in your Web.config settings is deprecated. Please use the excludeAssembliesForScan attribute instead.");
                }
                // ---

                // Is an exclude list given?
                if (!string.IsNullOrEmpty(attributes["excludeAssembliesForScan"]))
                {
                    var temp = attributes["excludeAssembliesForScan"];
                    if (!string.IsNullOrEmpty(temp))
                    {
                        excludeAssembliesForScan = temp.Split(';', ',').ToList();
                    }
                }

                // Is an include list given?
                if (!string.IsNullOrEmpty(attributes["includeAssembliesForScan"]))
                {
                    var temp = attributes["includeAssembliesForScan"];
                    if (!string.IsNullOrEmpty(temp))
                    {
                        includeAssembliesForScan = temp.Split(';', ',').ToList();
                    }
                }
            }

            // Which attributes in the sitemap XML should be ignored?
            if (!string.IsNullOrEmpty(attributes["attributesToIgnore"]))
            {
                var tempAttributesToIgnore = attributes["attributesToIgnore"];
                if (!string.IsNullOrEmpty(tempAttributesToIgnore))
                {
                    attributesToIgnore = tempAttributesToIgnore.Split(';', ',').ToList();
                }
            }

            // Is a node key generator given?
            if (!string.IsNullOrEmpty(attributes["nodeKeyGenerator"]))
            {
                NodeKeyGenerator = Activator.CreateInstance(
                    Type.GetType(attributes["nodeKeyGenerator"])) as INodeKeyGenerator;
            }
            if (NodeKeyGenerator == null)
            {
                NodeKeyGenerator =
#if !MVC2
 DependencyResolver.Current.GetService<INodeKeyGenerator>() ??
#endif
 new DefaultNodeKeyGenerator();
            }

            // Is a controller type resolver given?
            if (!string.IsNullOrEmpty(attributes["controllerTypeResolver"]))
            {
                ControllerTypeResolver = Activator.CreateInstance(
                    Type.GetType(attributes["controllerTypeResolver"])) as IControllerTypeResolver;
            }
            if (ControllerTypeResolver == null)
            {
                ControllerTypeResolver =
#if !MVC2
 DependencyResolver.Current.GetService<IControllerTypeResolver>() ??
#endif
 new DefaultControllerTypeResolver();
            }

            // Is an action method parameter resolver given?
            if (!string.IsNullOrEmpty(attributes["actionMethodParameterResolver"]))
            {
                ActionMethodParameterResolver = Activator.CreateInstance(
                    Type.GetType(attributes["actionMethodParameterResolver"])) as IActionMethodParameterResolver;
            }
            if (ActionMethodParameterResolver == null)
            {
                ActionMethodParameterResolver =
#if !MVC2
 DependencyResolver.Current.GetService<IActionMethodParameterResolver>() ??
#endif
 new DefaultActionMethodParameterResolver();
            }

            // Is an acl module given?
            if (!string.IsNullOrEmpty(attributes["aclModule"]))
            {
                AclModule = Activator.CreateInstance(
                    Type.GetType(attributes["aclModule"])) as IAclModule;
            }
            if (AclModule == null)
            {
                AclModule =
#if !MVC2
 DependencyResolver.Current.GetService<IAclModule>() ??
#endif
 new DefaultAclModule();
            }

            //this is not documented that you can even set it...default seemt to be taken...and this is used on AclModule, so drop for now
            //if (!string.IsNullOrEmpty(attributes["routeMethod"]))
            //{
            //    RouteMethod = attributes["routeMethod"];
            //}

            // Is a SiteMapNode URL resolver given?
            if (!string.IsNullOrEmpty(attributes["siteMapNodeUrlResolver"]))
            {
                SiteMapNodeUrlResolver = Activator.CreateInstance(
                    Type.GetType(attributes["siteMapNodeUrlResolver"])) as ISiteMapNodeUrlResolver;
            }
            if (SiteMapNodeUrlResolver == null)
            {
                SiteMapNodeUrlResolver =
#if !MVC2
 DependencyResolver.Current.GetService<ISiteMapNodeUrlResolver>() ??
#endif
 new DefaultSiteMapNodeUrlResolver();
            }

            // Is a SiteMapNode visibility provider given?
            if (!string.IsNullOrEmpty(attributes["siteMapNodeVisibilityProvider"]))
            {
                SiteMapNodeVisibilityProvider = Activator.CreateInstance(
                    Type.GetType(attributes["siteMapNodeVisibilityProvider"])) as ISiteMapNodeVisibilityProvider;
            }
            if (SiteMapNodeVisibilityProvider == null)
            {
                SiteMapNodeVisibilityProvider =
#if !MVC2
 DependencyResolver.Current.GetService<ISiteMapNodeVisibilityProvider>() ??
#endif
 new DefaultSiteMapNodeVisibilityProvider();
            }

            // Is a SiteMapProvider event handler given?
            if (!string.IsNullOrEmpty(attributes["siteMapProviderEventHandler"]))
            {
                SiteMapProviderEventHandler = Activator.CreateInstance(
                    Type.GetType(attributes["siteMapProviderEventHandler"])) as ISiteMapProviderEventHandler;
            }
            if (SiteMapProviderEventHandler == null)
            {
                SiteMapProviderEventHandler =
#if !MVC2
 DependencyResolver.Current.GetService<ISiteMapProviderEventHandler>() ??
#endif
 new DefaultSiteMapProviderEventHandler();
            }
        }

        /// <summary>
        /// Adds a <see cref="T:System.Web.SiteMapNode"/> object to the node collection that is maintained by the site map provider.
        /// </summary>
        /// <param name="node">The <see cref="T:System.Web.SiteMapNode"/> to add to the node collection maintained by the provider.</param>
        protected override void AddNode(SiteMapNode node)
        {
            if (SiteMapProviderEventHandler.OnAddingSiteMapNode(new SiteMapProviderEventContext(this, node, root)))
            {
                try
                {
                    // Avoid issue with url table not clearing correctly.
                    if (base.FindSiteMapNode(node.Url) != null)
                    {
                        base.RemoveNode(node);
                    }

                    // Allow for external URLs
                    var encoded = EncodeExternalUrl(node);

                    // Add the node
                    base.AddNode(node);

                    // Restore the external URL
                    if (encoded)
                    {
                        DecodeExternalUrl(node);
                    }
                }
                catch (InvalidOperationException)
                {
                    if (!isBuildingSiteMap)
                    {
                        throw;
                    }
                }
                SiteMapProviderEventHandler.OnAddedSiteMapNode(new SiteMapProviderEventContext(this, node, root));
            }
        }

        /// <summary>
        /// Adds a <see cref="T:System.Web.SiteMapNode"/> to the collections that are maintained by the site map provider and establishes a parent/child relationship between the <see cref="T:System.Web.SiteMapNode"/> objects.
        /// </summary>
        /// <param name="node">The <see cref="T:System.Web.SiteMapNode"/> to add to the site map provider.</param>
        /// <param name="parentNode">The <see cref="T:System.Web.SiteMapNode"/> under which to add <paramref name="node"/>.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="node"/> is null.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// The <see cref="P:System.Web.SiteMapNode.Url"/> or <see cref="P:System.Web.SiteMapNode.Key"/> is already registered with the <see cref="T:System.Web.StaticSiteMapProvider"/>. A site map node must be made up of pages with unique URLs or keys.
        /// </exception>
        protected override void AddNode(SiteMapNode node, SiteMapNode parentNode)
        {
            try
            {
                // Avoid issue with url table not clearing correctly.
                if (base.FindSiteMapNode(node.Url) != null)
                {
                    base.RemoveNode(node);
                }

                // Allow for external URLs
                var encoded = EncodeExternalUrl(node);

                // Add the node
                try
                {
                    base.AddNode(node, parentNode);
                }
                catch
                {
                    if (parentNode != null) base.RemoveNode(parentNode);
                    base.AddNode(node, parentNode);
                }

                // Restore the external URL
                if (encoded)
                {
                    DecodeExternalUrl(node);
                }
            }
            catch (InvalidOperationException)
            {
                if (!isBuildingSiteMap)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Node Creation

        /// <summary>
        /// Creates a site map node with the specified key and resources
        /// </summary>
        /// <param name="key">Node key</param>
        /// <param name="explicitResourceKeys">Explicit resource keys</param>
        /// <param name="implicitResourceKey">Implicit resource key.</param>
        /// <returns>New Site Map Node</returns>
        protected virtual MvcSiteMapNode CreateSiteMapNode(string key, NameValueCollection explicitResourceKeys, string implicitResourceKey)
        {
            return new MvcSiteMapNode(this, key, explicitResourceKeys, implicitResourceKey);
        }

        #endregion

        #region Namespace Override

        /// <summary>
        /// Namespace of the site map file
        /// </summary>
        /// <value>The site map namespace.</value>
        protected virtual XNamespace SiteMapNamespace
        {
            get { return ns; }
        }

        #endregion

        #region Sitemap Building/XML Parsing

        /// <summary>
        /// Builds the sitemap, firstly reads in the XML file, and grabs the outer root element and 
        /// maps this to become our main out root SiteMap node.
        /// </summary>
        /// <exception cref="MvcSiteMapException"></exception>
        /// <returns>The root SiteMapNode.</returns>
        public override SiteMapNode BuildSiteMap()
        {
            // Return immediately if this method has been called before
            if (root != null) //  && (HttpContext.Current.Cache[cacheKey] != null || isBuildingSiteMap)
            {
                return root;
            }

            // Build sitemap
            SiteMapNode rootNode;
            lock (synclock)
            {
                // Return immediately if this method has been called before
                if (root != null) //  && (HttpContext.Current.Cache[cacheKey] != null || isBuildingSiteMap)
                {
                    return root;
                }

                XDocument siteMapXml;
                try
                {
                    // Does the SiteMap XML exist?
                    if (File.Exists(siteMapFileAbsolute))
                    {
                        // Load the XML document.
                        siteMapXml = XDocument.Load(siteMapFileAbsolute);

                        // If no namespace is present (or the wrong one is present), replace it
                        foreach (var e in siteMapXml.Descendants())
                        {
                            if (string.IsNullOrEmpty(e.Name.Namespace.NamespaceName) || e.Name.Namespace != this.SiteMapNamespace)
                            {
                                e.Name = XName.Get(e.Name.LocalName, this.SiteMapNamespace.ToString());
                            }
                        }


                        // Enable Localization?
                        string enableLocalization =
                            siteMapXml.Element(this.SiteMapNamespace + RootName).GetAttributeValue("enableLocalization");
                        if (!string.IsNullOrEmpty(enableLocalization))
                        {
                            EnableLocalization = Boolean.Parse(enableLocalization);
                        }

                        // Get the root mvcSiteMapNode element, and map this to an MvcSiteMapNode
                        var rootElement = siteMapXml.Element(this.SiteMapNamespace + RootName).Element(this.SiteMapNamespace + NodeName);
                        rootNode = GetSiteMapNodeFromXmlElement(rootElement, null);
                        isBuildingSiteMap = true;
                        SetRootNode(rootNode);

                        // Process our XML file, passing in the main root sitemap node and xml element.
                        ProcessXmlNodes(root, rootElement);
                    }

                    // Look for assembly-defined nodes
                    // Process nodes in the assemblies of the current AppDomain?
                    if (scanAssembliesForSiteMapNodes)
                    {
                        isBuildingSiteMap = true;

                        // List of assemblies
                        IEnumerable<Assembly> assemblies;
                        if (includeAssembliesForScan.Any())
                        {
                            // An include list is given
                            assemblies = AppDomain.CurrentDomain.GetAssemblies()
                                .Where(a => includeAssembliesForScan.Contains(new AssemblyName(a.FullName).Name));
                        }
                        else
                        {
                            // An exclude list is given
                            assemblies = AppDomain.CurrentDomain.GetAssemblies()
                                .Where(a => !a.FullName.StartsWith("mscorlib")
                                         && !a.FullName.StartsWith("System")
                                         && !a.FullName.StartsWith("Microsoft")
                                         && !a.FullName.StartsWith("WebDev")
                                         && !a.FullName.StartsWith("SMDiagnostics")
                                         && !a.FullName.StartsWith("Anonymously")
                                         && !a.FullName.StartsWith("App_")
                                         && !excludeAssembliesForScan.Contains(new AssemblyName(a.FullName).Name));
                        }

                        foreach (Assembly assembly in assemblies)
                        {
                            // http://stackoverflow.com/questions/1423733/how-to-tell-if-a-net-assembly-is-dynamic
                            if (!(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder)
                                && assembly.ManifestModule.GetType().Namespace != "System.Reflection.Emit")
                            {
                                ProcessNodesInAssembly(assembly);
                            }
                        }
                    }

                    // Do we have a root node?
                    if (root == null)
                    {
                        throw new MvcSiteMapException(Resources.Messages.CouldNotDetermineRootNode);
                    }
                }
                catch (MvcSiteMapException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new MvcSiteMapException(Resources.Messages.UnknownException, ex);
                }
                finally
                {
                    // Create a cache item, this is used for the sole purpose of being able to invalidate our sitemap
                    // after a given time period, it also adds a dependancy on the sitemap file,
                    // so that once changed it will refresh your sitemap, unfortunately at this stage
                    // there is no dependancy for dynamic data, this could be implemented by clearing the cache item,
                    // by setting a custom cacheKey, then use this in your administration console for example to
                    // clear the cache item when the structure requires refreshing.
                    CacheDependency cacheDependency = null;
                    if (File.Exists(siteMapFileAbsolute))
                    {
                        cacheDependency = new CacheDependency(siteMapFileAbsolute);
                    }
                    HttpContext.Current.Cache.Add(cacheKey,
                                                  DateTime.Now,
                                                  cacheDependency,
                                                  DateTime.Now.AddMinutes(CacheDuration),
                                                  Cache.NoSlidingExpiration,
                                                  CacheItemPriority.Default,
                                                  new CacheItemRemovedCallback(OnSiteMapChanged));

                    isBuildingSiteMap = false;
                    siteMapXml = null;
                }
            }

            // Finally return our root SiteMapNode.
            return root;
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// When using caching, this method is being used to refresh the sitemap when the root sitemap node identifier is removed from cache.
        /// </summary>
        /// <param name="key">Cached item key.</param>
        /// <param name="item">Cached item.</param>
        /// <param name="reason">Reason the cached item was removed.</param>
        private void OnSiteMapChanged(string key, object item, CacheItemRemovedReason reason)
        {
            lock (synclock)
            {
                if (item != null) // && ((DateTime)item).AddMinutes(CacheDuration).CompareTo(DateTime.Now) <= 0)
                {
                    // Clear the sitemap
                    Clear();

                    // Try refreshing the sitemap
                    if (HttpContext.Current != null)
                    {
                        BuildSiteMap();
                    }
                }
            }
        }

        /// <summary>
        /// Sets the root node.
        /// </summary>
        /// <param name="rootNode">The root node.</param>
        protected void SetRootNode(SiteMapNode rootNode)
        {
            if (this.root != null)
            {
                throw new MvcSiteMapException(Resources.Messages.DuplicateRootNodeDetected);
            }

            root = rootNode;
            AddNode(root);
        }

        /// <summary>
        /// Recursively processes our XML document, parsing our siteMapNodes and dynamicNode(s).
        /// </summary>
        /// <param name="rootNode">The main root sitemap node.</param>
        /// <param name="rootElement">The main root XML element.</param>
        protected void ProcessXmlNodes(SiteMapNode rootNode, XElement rootElement)
        {
            // Loop through each element below the current root element.
            foreach (XElement node in rootElement.Elements())
            {
                SiteMapNode childNode;
                if (node.Name == this.SiteMapNamespace + NodeName)
                {
                    // If this is a normal mvcSiteMapNode then map the xml element
                    // to an MvcSiteMapNode, and add the node to the current root.
                    childNode = GetSiteMapNodeFromXmlElement(node, rootNode);
                    SiteMapNode parentNode = rootNode;

                    //if (childNode.ParentNode != null && childNode.ParentNode != rootNode)
                    //{
                    //   parentNode = childNode.ParentNode;
                    //}

                    if (!HasDynamicNodes(childNode))
                    {
                        AddNode(childNode, parentNode);
                    }
                    else
                    {
                        var dynamicNodesCreated = AddDynamicNodesFor(childNode, parentNode);

                        // Add non-dynamic childs for every dynamicnode
                        foreach (var dynamicNodeCreated in dynamicNodesCreated)
                        {
                            ProcessXmlNodes(dynamicNodeCreated, node);
                        }
                    }
                }
                else
                {
                    // If the current node is not one of the known node types throw and exception
                    throw new Exception(Resources.Messages.InvalidSiteMapElement);
                }

                // Continue recursively processing the XML file.
                ProcessXmlNodes(childNode, node);
            }
        }

        /// <summary>
        /// Process nodes in assembly
        /// </summary>
        /// <param name="assembly">Assembly</param>
        protected void ProcessNodesInAssembly(Assembly assembly)
        {
            // Create a list of all nodes defined in the assembly
            List<IMvcSiteMapNodeAttributeDefinition> assemblyNodes
                = new List<IMvcSiteMapNodeAttributeDefinition>();

            // Retrieve types
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types;
            }

            // Add all types
            foreach (Type type in types)
            {
                try
                {
                    var attribute =
                        type.GetCustomAttributes(typeof(IMvcSiteMapNodeAttribute), true).FirstOrDefault() as
                        IMvcSiteMapNodeAttribute;
                    if (attribute != null)
                    {
                        assemblyNodes.Add(new MvcSiteMapNodeAttributeDefinitionForController
                                              {
                                                  SiteMapNodeAttribute = attribute,
                                                  ControllerType = type
                                              });
                    }
                }
                catch
                {
                }

                // Add their methods
                try
                {
                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(x => x.GetCustomAttributes(typeof(IMvcSiteMapNodeAttribute), true).Any());

                    foreach (var method in methods)
                    {
                        var attributes =
                            (IMvcSiteMapNodeAttribute[])
                            method.GetCustomAttributes(typeof(IMvcSiteMapNodeAttribute), false);
                        foreach (var attribute in attributes)
                        {
                            assemblyNodes.Add(new MvcSiteMapNodeAttributeDefinitionForAction
                                                  {
                                                      SiteMapNodeAttribute = attribute,
                                                      ControllerType = type,
                                                      ActionMethodInfo = method
                                                  });
                        }
                    }
                }
                catch
                {
                }
            }

            // Create nodes from MVC site map node attribute definitions
            CreateNodesFromMvcSiteMapNodeAttributeDefinitions(assemblyNodes.OrderBy(n => n.SiteMapNodeAttribute.Order));
        }

        /// <summary>
        /// Creates the nodes from MVC site map node attribute definitions.
        /// </summary>
        /// <param name="definitions">The definitions.</param>
        protected void CreateNodesFromMvcSiteMapNodeAttributeDefinitions(IEnumerable<IMvcSiteMapNodeAttributeDefinition> definitions)
        {
            // A dictionary of nodes to process later (node, parentKey)
            Dictionary<SiteMapNode, string> nodesToProcessLater
                = new Dictionary<SiteMapNode, string>();

            //SiteMap.Provider.Name
            bool isDefaultProvider = this.Name == SiteMap.Provider.Name;

            // Search root node prior to searching other nodes
            if (definitions.Any(t => String.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)
                                       && (isDefaultProvider && string.IsNullOrEmpty(t.SiteMapNodeAttribute.SiteMapProviderName)
                                           || t.SiteMapNodeAttribute.SiteMapProviderName == this.Name)))
            {
                SiteMapNode attributedRootNode = null;

                var item = definitions.Single(t => String.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey));

                var actionNode = item as MvcSiteMapNodeAttributeDefinitionForAction;
                if (actionNode != null)
                {
                    // Create node for action
                    attributedRootNode = GetSiteMapNodeFromMvcSiteMapNodeAttribute(
                        actionNode.SiteMapNodeAttribute, actionNode.ControllerType, actionNode.ActionMethodInfo);
                }
                else
                {
                    var controllerNode = item as MvcSiteMapNodeAttributeDefinitionForController;
                    if (controllerNode != null)
                    {
                        // Create node for controller
                        attributedRootNode = GetSiteMapNodeFromMvcSiteMapNodeAttribute(
                            controllerNode.SiteMapNodeAttribute, controllerNode.ControllerType, null);
                    }
                }

                SetRootNode(attributedRootNode);
            }

            // Create nodes
            foreach (var assemblyNode in definitions.Where(t => !String.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)
                    && (isDefaultProvider && string.IsNullOrEmpty(t.SiteMapNodeAttribute.SiteMapProviderName)
                                           || t.SiteMapNodeAttribute.SiteMapProviderName == this.Name)))
            {
                SiteMapNode nodeToAdd = null;

                // Create node
                var actionNode = assemblyNode as MvcSiteMapNodeAttributeDefinitionForAction;
                if (actionNode != null)
                {
                    // Create node for action
                    nodeToAdd = GetSiteMapNodeFromMvcSiteMapNodeAttribute(
                        actionNode.SiteMapNodeAttribute, actionNode.ControllerType, actionNode.ActionMethodInfo);
                }
                else
                {
                    var controllerNode = assemblyNode as MvcSiteMapNodeAttributeDefinitionForController;
                    if (controllerNode != null)
                    {
                        // Create node for controller
                        nodeToAdd = GetSiteMapNodeFromMvcSiteMapNodeAttribute(
                            controllerNode.SiteMapNodeAttribute, controllerNode.ControllerType, null);
                    }
                }

                // Add node
                if (nodeToAdd != null)
                {
                    // Root node?
                    if (string.IsNullOrEmpty(assemblyNode.SiteMapNodeAttribute.ParentKey))
                    {
                        SetRootNode(nodeToAdd);
                    }
                    else
                    {
                        var parentNode = FindSiteMapNodeFromKey(assemblyNode.SiteMapNodeAttribute.ParentKey);
                        if (parentNode == null)
                        {
                            nodesToProcessLater.Add(nodeToAdd, assemblyNode.SiteMapNodeAttribute.ParentKey);
                        }
                        else
                        {
                            if (!HasDynamicNodes(nodeToAdd))
                            {
                                AddNode(nodeToAdd, parentNode);
                            }
                            else
                            {
                                AddDynamicNodesFor(nodeToAdd, parentNode);
                            }
                        }
                    }
                }
            }

            // Process list of nodes that did not have a parent defined.
            // If this does not succeed at this time, parent will default to root node.
            foreach (var nodeToAdd in nodesToProcessLater)
            {
                var parentNode = FindSiteMapNodeFromKey(nodeToAdd.Value);
                if (parentNode == null)
                {
                    var temp = nodesToProcessLater.Keys.FirstOrDefault(t => t.Key == nodeToAdd.Value);
                    if (temp != null)
                    {
                        parentNode = temp;
                    }
                }
                parentNode = parentNode ?? root;

                if (parentNode != null)
                {
                    if (!HasDynamicNodes(nodeToAdd.Key))
                    {
                        AddNode(nodeToAdd.Key, parentNode);
                    }
                    else
                    {
                        AddDynamicNodesFor(nodeToAdd.Key, parentNode);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the dynamic nodes for node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="parentNode">The parent node.</param>
        private IEnumerable<SiteMapNode> AddDynamicNodesFor(SiteMapNode node, SiteMapNode parentNode)
        {
            // List of dynamic nodes that have been created
            var createdDynamicNodes = new List<SiteMapNode>();

            // Dynamic nodes available?
            if (!HasDynamicNodes(node))
            {
                return createdDynamicNodes;
            }

            // Build dynamic nodes
            var mvcNode = node as MvcSiteMapNode;
            foreach (var dynamicNode in mvcNode.DynamicNodeProvider.GetDynamicNodeCollection().ToList())
            {
                string key = dynamicNode.Key;
                if (string.IsNullOrEmpty(key))
                {
                    key = NodeKeyGenerator.GenerateKey(parentNode == null ? "" : parentNode.Key, Guid.NewGuid().ToString(), mvcNode.Url, mvcNode.Title, mvcNode.Area, mvcNode.Controller, mvcNode.Action, mvcNode.Clickable);
                }

                var clone = mvcNode.Clone(key) as MvcSiteMapNode;
                foreach (var kvp in dynamicNode.RouteValues)
                {
                    clone.RouteValues[kvp.Key] = kvp.Value;
                }
                foreach (var kvp in dynamicNode.Attributes)
                {
                    clone[kvp.Key] = kvp.Value;
                }
                clone.DynamicNodeProvider = null;
                clone.IsDynamic = true;

                if (!string.IsNullOrEmpty(dynamicNode.Title))
                {
                    clone.Title = dynamicNode.Title;
                }
                if (!string.IsNullOrEmpty(dynamicNode.Description))
                {
                    clone.Description = dynamicNode.Description;
                }
                if (!string.IsNullOrEmpty(dynamicNode.TargetFrame))
                {
                    clone.TargetFrame = dynamicNode.TargetFrame;
                }
                if (!string.IsNullOrEmpty(dynamicNode.ImageUrl))
                {
                    clone.ImageUrl = dynamicNode.ImageUrl;
                }

                if (!string.IsNullOrEmpty(dynamicNode.Route))
                {
                    clone.Route = dynamicNode.Route;
                }

                if (dynamicNode.LastModifiedDate.HasValue)
                {
                    clone.LastModifiedDate = dynamicNode.LastModifiedDate.Value;
                }

                if (dynamicNode.ChangeFrequency != ChangeFrequency.Undefined)
                {
                    clone.ChangeFrequency = dynamicNode.ChangeFrequency;
                }

                if (dynamicNode.UpdatePriority != UpdatePriority.Undefined)
                {
                    clone.UpdatePriority = dynamicNode.UpdatePriority;
                }

                if (dynamicNode.PreservedRouteParameters.Any())
                {
                    clone.PreservedRouteParameters = String.Join(";", dynamicNode.PreservedRouteParameters.ToArray());
                }

                if (dynamicNode.Roles != null && dynamicNode.Roles.Any())
                {
                    clone.Roles = dynamicNode.Roles.ToArray();
                }

                // Optionally, an area, controller and action can be specified. If so, override the clone.
                if (!string.IsNullOrEmpty(dynamicNode.Area))
                {
                    clone.Area = dynamicNode.Area;
                }
                if (!string.IsNullOrEmpty(dynamicNode.Controller))
                {
                    clone.Controller = dynamicNode.Controller;
                }
                if (!string.IsNullOrEmpty(dynamicNode.Action))
                {
                    clone.Action = dynamicNode.Action;
                }

                // If the dynamic node has a parent key set, use that as the parent. Otherwise use the parentNode.
                if (dynamicNode.ParentKey != null && !string.IsNullOrEmpty(dynamicNode.ParentKey))
                {
                    var parent = FindSiteMapNodeFromKey(dynamicNode.ParentKey);
                    if (parent != null)
                    {
                        AddNode(clone, parent);
                        createdDynamicNodes.Add(clone);
                    }
                }
                else
                {
                    AddNode(clone, parentNode);
                    createdDynamicNodes.Add(clone);
                }
            }

            // Insert cache dependency
            CacheDescription cacheDescription = mvcNode.DynamicNodeProvider.GetCacheDescription();
            if (cacheDescription != null)
            {
                HttpContext.Current.Cache.Add(
                    cacheDescription.Key,
                    "",
                    cacheDescription.Dependencies,
                    cacheDescription.AbsoluteExpiration,
                    cacheDescription.SlidingExpiration,
                    cacheDescription.Priority,
                    new CacheItemRemovedCallback(OnSiteMapChanged));
            }

            // Done!
            return createdDynamicNodes;
        }

        /// <summary>
        /// Determines whether the specified node has dynamic nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node has dynamic nodes; otherwise, <c>false</c>.
        /// </returns>
        protected bool HasDynamicNodes(SiteMapNode node)
        {
            // Dynamic nodes available?
            var mvcNode = node as MvcSiteMapNode;
            if (mvcNode == null || mvcNode.DynamicNodeProvider == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clears the current sitemap.
        /// </summary>
        protected override void Clear()
        {
            root = null;
            base.Clear();
        }

        /// <summary>
        /// Retrieves a <see cref="T:System.Web.SiteMapNode"/> object that represents the currently requested page using the specified <see cref="T:System.Web.HttpContext"/> object.
        /// </summary>
        /// <param name="context">The <see cref="T:System.Web.HttpContext"/> used to match node information with the URL of the requested page.</param>
        /// <returns>
        /// A <see cref="T:System.Web.SiteMapNode"/> that represents the currently requested page; otherwise, null, if no corresponding <see cref="T:System.Web.SiteMapNode"/> can be found in the <see cref="T:System.Web.SiteMapNode"/> or if the page context is null.
        /// </returns>
        public override SiteMapNode FindSiteMapNode(HttpContext context)
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
                node = base.FindSiteMapNode(httpContext.Request.Path) as MvcSiteMapNode;

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
                node = base.FindSiteMapNode(context);
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
                    actionParameters = ActionMethodParameterResolver.ResolveActionMethodParameters(
                        ControllerTypeResolver, mvcNode.Area, mvcNode.Controller, mvcNode.Action);
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

        #endregion

        #region Mappers

        /// <summary>
        /// Maps an XMLElement from the XML file to an MvcSiteMapNode.
        /// </summary>
        /// <param name="node">The element to map.</param>
        /// <param name="parentNode">The parent SiteMapNode</param>
        /// <returns>An MvcSiteMapNode which represents the XMLElement.</returns>
        protected MvcSiteMapNode GetSiteMapNodeFromXmlElement(XElement node, SiteMapNode parentNode)
        {
            // Get area, controller and action from node declaration
            string area = node.GetAttributeValue("area");
            string controller = node.GetAttributeValue("controller");

            // Inherit area and controller from parent
            var parentMvcNode = parentNode as MvcSiteMapNode;
            if (parentMvcNode != null)
            {
                if (string.IsNullOrEmpty(area))
                {
                    area = parentMvcNode.Area;
                }
                if (string.IsNullOrEmpty(controller))
                {
                    controller = parentMvcNode.Controller;
                }
            }

            // Generate key for node
            string key = NodeKeyGenerator.GenerateKey(
                parentNode == null ? "" : parentNode.Key,
                node.GetAttributeValue("key"),
                node.GetAttributeValue("url"),
                node.GetAttributeValue("title"),
                area,
                controller,
                node.GetAttributeValue("action"),
                !(node.GetAttributeValue("clickable") == "false"));

            // Handle title and description globalization
            var explicitResourceKeys = new NameValueCollection();
            var title = node.GetAttributeValue("title");
            var description = node.GetAttributeValue("description") ?? title;
            HandleResourceAttribute("title", ref title, ref explicitResourceKeys);
            HandleResourceAttribute("description", ref description, ref explicitResourceKeys);

            // Handle implicit resources
            var implicitResourceKey = node.GetAttributeValue("resourceKey");
            if (!string.IsNullOrEmpty(implicitResourceKey))
            {
                title = null;
                description = null;
            }

            // Create a new SiteMap node, setting the key and url
            var siteMapNode = CreateSiteMapNode(key, explicitResourceKeys, implicitResourceKey);
            siteMapNode.Title = title;
            siteMapNode.Description = description;
            siteMapNode.ResourceKey = implicitResourceKey;

            // Create a route data dictionary
            IDictionary<string, object> routeValues = new Dictionary<string, object>();
            AttributesToRouteValues(node, siteMapNode, routeValues);

            // Inherit area and controller from parent
            if (parentMvcNode != null)
            {
                if (siteMapNode["area"] == null)
                {
                    siteMapNode["area"] = parentMvcNode.Area;
                    routeValues.Add("area", parentMvcNode.Area);
                }
                if (node.GetAttributeValue("controller") == "")
                {
                    siteMapNode["controller"] = parentMvcNode.Controller;
                    routeValues.Add("controller", parentMvcNode.Controller);
                }
                var action = "action";
                if (node.GetAttributeValue(action) == String.Empty)
                {
                    siteMapNode[action] = parentMvcNode.Action;
                    routeValues.Add(action, parentMvcNode.Action);
                }
            }

            // Add defaults for area
            if (!routeValues.ContainsKey("area"))
            {
                siteMapNode["area"] = "";
                routeValues.Add("area", "");
            }

            // Add defaults for SiteMapNodeUrlResolver
            if (siteMapNode.UrlResolver == null)
            {
                siteMapNode.UrlResolver = this.SiteMapNodeUrlResolver;
            }

            // Add defaults for SiteMapNodeVisibilityProvider
            if (siteMapNode.VisibilityProvider == null)
            {
                siteMapNode.VisibilityProvider = this.SiteMapNodeVisibilityProvider;
            }

            // Clickable?
            if (!siteMapNode.Clickable)
            {
                siteMapNode.Url = "";
            }

            // Add inherited route values to sitemap node
            foreach (var inheritedRouteParameter in siteMapNode.InheritedRouteParameters.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var item = inheritedRouteParameter.Trim();
                if (parentMvcNode != null && parentMvcNode.RouteValues.ContainsKey(item))
                {
                    routeValues[item] = parentMvcNode.RouteValues[item];
                }
            }

            // Add route values to sitemap node
            siteMapNode.RouteValues = routeValues;

            // Add node's route defaults
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            RouteData routeData = siteMapNode.GetRouteData(httpContext);
            if (routeData != null)
            {
                // Specify defaults from route on siteMapNode
                var route = routeData.Route as Route;
                if (route != null && route.Defaults != null)
                {
                    foreach (var defaultValue in route.Defaults)
                    {
                        if (siteMapNode[defaultValue.Key] == null && defaultValue.Value != null)
                        {
                            siteMapNode[defaultValue.Key] = defaultValue.Value.ToString();
                        }
                    }
                }
            }

            siteMapNode.ParentNode = parentMvcNode;
            return siteMapNode;
        }

        /// <summary>
        /// Add each attribute to our attributes collection on the siteMapNode
        /// and to a route data dictionary.
        /// </summary>
        /// <param name="node">The element to map.</param>
        /// <param name="siteMapNode">The SiteMapNode to map to</param>
        /// <param name="routeValues">The RouteValueDictionary to fill</param>
        protected virtual void AttributesToRouteValues(XElement node, MvcSiteMapNode siteMapNode, IDictionary<string, object> routeValues)
        {
            foreach (XAttribute attribute in node.Attributes())
            {
                var attributeName = attribute.Name.ToString();
                var attributeValue = attribute.Value;

                if (attributeName != "title"
                    && attributeName != "description")
                {
                    siteMapNode[attributeName] = attributeValue;
                }

                // Process route values
                if (
                    attributeName != "title"
                    && attributeName != "description"
                    && attributeName != "resourceKey"
                    && attributeName != "key"
                    && attributeName != "roles"
                    && attributeName != "url"
                    && attributeName != "clickable"
                    && attributeName != "dynamicNodeProvider"
                    && attributeName != "urlResolver"
                    && attributeName != "visibilityProvider"
                    && attributeName != "lastModifiedDate"
                    && attributeName != "changeFrequency"
                    && attributeName != "updatePriority"
                    && attributeName != "targetFrame"
                    && attributeName != "imageUrl"
                    && attributeName != "inheritedRouteParameters"
                    && attributeName != "preservedRouteParameters"
                    && !attributesToIgnore.Contains(attributeName)
                    && !attributeName.StartsWith("data-")
                    )
                {
                    routeValues.Add(attributeName, attributeValue);
                }

                if (attributeName == "roles")
                {
                    siteMapNode.Roles = attribute.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
        }

        /// <summary>
        /// Gets the site map node from MVC site map node attribute.
        /// </summary>
        /// <param name="attribute">IMvcSiteMapNodeAttribute to map</param>
        /// <param name="type">Type.</param>
        /// <param name="methodInfo">MethodInfo on which the IMvcSiteMapNodeAttribute is applied</param>
        /// <returns>
        /// A SiteMapNode which represents the IMvcSiteMapNodeAttribute.
        /// </returns>
        protected SiteMapNode GetSiteMapNodeFromMvcSiteMapNodeAttribute(IMvcSiteMapNodeAttribute attribute, Type type, MethodInfo methodInfo)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (methodInfo == null) // try to find Index action
            {
                var ms = type.FindMembers(MemberTypes.Method, BindingFlags.Instance | BindingFlags.Public,
                                          (mi, o) => mi != null && string.Equals(mi.Name, "Index"), null);
                foreach (MethodInfo m in ms.OfType<MethodInfo>())
                {
                    var pars = m.GetParameters();
                    if (pars.Length == 0)
                    {
                        methodInfo = m;
                        break;
                    }
                }
            }



            // Determine area (will only work if controller is defined as <anything>.Areas.<Area>.Controllers.HomeController - which is normal convention)
            string area = "";
            if (!string.IsNullOrEmpty(attribute.AreaName))
            {
                area = attribute.AreaName;
            }
            if (string.IsNullOrEmpty(area))
            {
                if (type.Namespace!=null)
                {
                    var areaIndex = type.Namespace.LastIndexOf("Areas.", StringComparison.OrdinalIgnoreCase);
                    if (areaIndex > 0)
                    {
                        var parts = type.Namespace.Substring(areaIndex).Split('.');
                        if (parts.Length > 0)
                            area = parts[1];
                    }
                }
            }

            // Determine controller and (index) action
            string controller = type.Name.Substring(0, type.Name.IndexOf("Controller"));
            string action = (methodInfo != null ? methodInfo.Name : null) ?? "Index";
            if (methodInfo != null) // handle custom action name
            {
                var actionNameAttribute = methodInfo.GetCustomAttributes(typeof(ActionNameAttribute), true).FirstOrDefault() as ActionNameAttribute;
                if (actionNameAttribute != null)
                {
                    action = actionNameAttribute.Name;
                }
            }

            // Generate key for node
            string key = NodeKeyGenerator.GenerateKey(
                null,
                attribute.Key,
                attribute.Url,
                attribute.Title,
                area,
                controller, action,
                attribute.Clickable);

            // Handle title and description globalization
            var explicitResourceKeys = new NameValueCollection();
            var title = attribute.Title;
            var description = attribute.Description;
            HandleResourceAttribute("title", ref title, ref explicitResourceKeys);
            HandleResourceAttribute("description", ref description, ref explicitResourceKeys);

            // Create a new SiteMap node, setting the key and url
            var siteMapNode = CreateSiteMapNode(key, explicitResourceKeys, null);

            // Set the properties on siteMapNode.
            siteMapNode.Title = title;
            siteMapNode.Description = description;
            siteMapNode.Roles = attribute.Roles;
            if (!string.IsNullOrEmpty(attribute.Route))
            {
                siteMapNode["route"] = attribute.Route;
            }
            siteMapNode["area"] = area;
            siteMapNode["controller"] = controller;
            siteMapNode["action"] = action;
            siteMapNode["dynamicNodeProvider"] = attribute.DynamicNodeProvider;
            siteMapNode["urlResolver"] = attribute.UrlResolver;
            siteMapNode["visibilityProvider"] = attribute.VisibilityProvider;
            siteMapNode.LastModifiedDate = attribute.LastModifiedDate;
            siteMapNode.ChangeFrequency = attribute.ChangeFrequency;
            siteMapNode.UpdatePriority = attribute.UpdatePriority;
            siteMapNode.TargetFrame = attribute.TargetFrame;
            siteMapNode.ImageUrl = attribute.ImageUrl;
            siteMapNode.PreservedRouteParameters = attribute.PreservedRouteParameters;

            if (!string.IsNullOrEmpty(attribute.Url))
                siteMapNode.Url = attribute.Url;

            siteMapNode.ResourceKey = attribute.ResourceKey;

            // Create a route data dictionary
            IDictionary<string, object> routeValues = new Dictionary<string, object>();
            routeValues.Add("area", area);
            routeValues.Add("controller", controller);
            routeValues.Add("action", action);

            // Add route values to sitemap node
            siteMapNode.RouteValues = routeValues;

            // Add defaults for SiteMapNodeUrlResolver
            if (siteMapNode.UrlResolver == null)
            {
                siteMapNode.UrlResolver = this.SiteMapNodeUrlResolver;
            }

            // Add defaults for SiteMapNodeVisibilityProvider
            if (siteMapNode.VisibilityProvider == null)
            {
                siteMapNode.VisibilityProvider = this.SiteMapNodeVisibilityProvider;
            }

            // Clickable?
            siteMapNode.Clickable = attribute.Clickable;

            return siteMapNode;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Handle resource attribute
        /// </summary>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="text">Text</param>
        /// <param name="collection">NameValueCollection to be used for localization</param>
        private static void HandleResourceAttribute(string attributeName, ref string text, ref NameValueCollection collection)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string tempStr1;
                var tempStr2 = text.TrimStart(new[] { ' ' });
                if (((tempStr2.Length > 10)) && tempStr2.ToLower(CultureInfo.InvariantCulture).StartsWith("$resources:", StringComparison.Ordinal))
                {
                    tempStr1 = tempStr2.Substring(11);
                    string tempStr3;
                    string tempStr4;
                    var index = tempStr1.IndexOf(',');
                    tempStr3 = tempStr1.Substring(0, index);
                    tempStr4 = tempStr1.Substring(index + 1);
                    var length = tempStr4.IndexOf(',');
                    if (length != -1)
                    {
                        text = tempStr4.Substring(length + 1);
                        tempStr4 = tempStr4.Substring(0, length);
                    }
                    else
                    {
                        text = null;
                    }
                    if (collection == null)
                    {
                        collection = new NameValueCollection();
                    }
                    collection.Add(attributeName, tempStr3.Trim());
                    collection.Add(attributeName, tempStr4.Trim());
                }
            }
        }

        /// <summary>
        /// Encodes the external URL.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        protected bool EncodeExternalUrl(SiteMapNode node)
        {
            var url = node.Url;
            if (url.StartsWith("http") || url.StartsWith("ftp"))
            {
                node.Url = HttpContext.Current.Server.UrlEncode(url);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Decodes the external URL.
        /// </summary>
        /// <param name="node">The node.</param>
        protected void DecodeExternalUrl(SiteMapNode node)
        {
            node.Url = HttpContext.Current.Server.UrlDecode(node.Url);
        }

        #endregion
    }
}
