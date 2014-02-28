using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// SiteMapNode class. This class represents a node within the SiteMap hierarchy.
    /// It contains all business logic to maintain the node's internal state.
    /// </summary>
    public class SiteMapNode
        : SiteMapNodePositioningBase, ISiteMapNode
    {
        public SiteMapNode(
            ISiteMap siteMap, 
            string key,
            bool isDynamic,
            ISiteMapNodePluginProvider pluginProvider,
            IMvcContextFactory mvcContextFactory,
            ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory,
            ILocalizationService localizationService,
            IUrlPath urlPath
            )
        {
            if (siteMap == null)
                throw new ArgumentNullException("siteMap");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (pluginProvider == null)
                throw new ArgumentNullException("pluginProvider");
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");
            if (siteMapNodeChildStateFactory == null)
                throw new ArgumentNullException("siteMapNodeChildStateFactory");
            if (localizationService == null)
                throw new ArgumentNullException("localizationService");
            if (urlPath == null)
                throw new ArgumentNullException("urlPath");
            
            this.siteMap = siteMap;
            this.key = key;
            this.isDynamic = isDynamic;
            this.pluginProvider = pluginProvider;
            this.mvcContextFactory = mvcContextFactory;
            this.localizationService = localizationService;
            this.urlPath = urlPath;

            // Initialize child collections
            this.attributes = siteMapNodeChildStateFactory.CreateAttributeDictionary(key, "Attributes", siteMap, localizationService);
            this.routeValues = siteMapNodeChildStateFactory.CreateRouteValueDictionary(key, "RouteValues", siteMap);
            this.preservedRouteParameters = siteMapNodeChildStateFactory.CreatePreservedRouteParameterCollection(siteMap);
            this.roles = siteMapNodeChildStateFactory.CreateRoleCollection(siteMap);
            this.metaRobotsValues = siteMapNodeChildStateFactory.CreateMetaRobotsValueCollection(siteMap);
        }

        // Services
        protected readonly ISiteMapNodePluginProvider pluginProvider;
        protected readonly IMvcContextFactory mvcContextFactory;
        protected readonly ILocalizationService localizationService;
        protected readonly IUrlPath urlPath;

        // Child collections and dictionaries
        protected readonly IAttributeDictionary attributes;
        protected readonly IRouteValueDictionary routeValues;
        protected readonly IPreservedRouteParameterCollection preservedRouteParameters;
        protected readonly IRoleCollection roles;
        protected readonly IMetaRobotsValueCollection metaRobotsValues;

        // Object State
        protected readonly ISiteMap siteMap;
        protected readonly string key;
        protected readonly bool isDynamic;
        protected string httpMethod = HttpVerbs.Get.ToString().ToUpperInvariant();
        protected string title = String.Empty;
        protected string description = String.Empty;
        protected string imageUrl = String.Empty;
        protected DateTime lastModifiedDate = DateTime.MinValue;
        protected ChangeFrequency changeFrequency = ChangeFrequency.Always;
        protected UpdatePriority updatePriority = UpdatePriority.Undefined;
        protected bool clickable = true;
        protected string url = String.Empty;
        protected string resolvedUrl = String.Empty;
        protected string canonicalUrl = String.Empty;
        protected string canonicalKey = String.Empty;

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public override string Key { get { return this.key; } }

        /// <summary>
        /// Gets whether the current node was created from a dynamic source.
        /// </summary>
        /// <value>True if the current node is dynamic.</value>
        public override bool IsDynamic { get { return this.isDynamic; } }

        /// <summary>
        /// Gets whether the current node is read-only.
        /// </summary>
        /// <value>True if the current node is read-only.</value>
        public override bool IsReadOnly { get { return this.SiteMap.IsReadOnly; } }

        /// <summary>
        /// A reference to the root SiteMap object for the current graph.
        /// </summary>
        public override ISiteMap SiteMap
        {
            get { return this.siteMap; }
        }

        /// <summary>
        /// Gets or sets the HTTP method (such as GET, POST, or HEAD) to use to determine
        /// node accessibility.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public override string HttpMethod 
        {
            get { return this.httpMethod; }
            set { this.httpMethod = value; }
        }

        /// <summary>
        /// Gets the implicit resource key (optional).
        /// </summary>
        /// <value>The implicit resource key.</value>
        public override string ResourceKey
        {
            get { return this.localizationService.ResourceKey; }
        }

        /// <summary>
        /// Gets or sets the title (optional).
        /// </summary>
        /// <value>The title.</value>
        /// <remarks>The title can be localized using a resource provider.</remarks>
        public override string Title 
        {
            get { return localizationService.GetResourceString("title", this.title, this.SiteMap); }
            set { this.title = localizationService.ExtractExplicitResourceKey("title", value); }
        }

        /// <summary>
        /// Gets or sets the description (optional).
        /// </summary>
        /// <value>The description.</value>
        /// <remarks>The description can be localized using a resource provider.</remarks>
        public override string Description 
        {
            get 
            { 
                var result = localizationService.GetResourceString("description", this.description, this.SiteMap);
                if (this.SiteMap.UseTitleIfDescriptionNotProvided && string.IsNullOrEmpty(result))
                {
                    result = this.Title;
                }

                return result;
            }
            set 
            { 
                this.description = localizationService.ExtractExplicitResourceKey("description", value); 
            }
        }

        /// <summary>
        /// Gets or sets the target frame (optional).
        /// </summary>
        /// <value>The target frame.</value>
        public override string TargetFrame { get; set; }

        /// <summary>
        /// Gets or sets the image URL (optional).
        /// </summary>
        /// <value>The image URL.</value>
        /// <remarks>The image URL can be localized using a resource provider.</remarks>
        public override string ImageUrl 
        {
            get { return localizationService.GetResourceString("imageUrl", this.imageUrl, this.SiteMap); }
            set { this.imageUrl = localizationService.ExtractExplicitResourceKey("imageUrl", value); }
        }

        /// <summary>
        /// Gets the attributes (optional).
        /// </summary>
        /// <value>The attributes.</value>
        /// <remarks>The attributes can be localized using a resource provider.</remarks>
        public override IAttributeDictionary Attributes { get { return this.attributes; } }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public override IRoleCollection Roles { get { return this.roles; } }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public override DateTime LastModifiedDate 
        { 
            get { return this.lastModifiedDate; } 
            set { this.lastModifiedDate = value; } 
        }

        /// <summary>
        /// Gets or sets the change frequency.
        /// </summary>
        /// <value>The change frequency.</value>
        public override ChangeFrequency ChangeFrequency 
        { 
            get { return this.changeFrequency; }
            set { this.changeFrequency = value; } 
        }

        /// <summary>
        /// Gets or sets the update priority.
        /// </summary>
        /// <value>The update priority.</value>
        public override UpdatePriority UpdatePriority
        { 
            get { return this.updatePriority; } 
            set { this.updatePriority = value; } 
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
        public override string VisibilityProvider { get; set; }


        /// <summary>
        /// Determines whether the node is visible.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sourceMetadata">The source metadata.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is visible; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsVisible(IDictionary<string, object> sourceMetadata)
        {
            // use strategy factory to provide implementation logic from concrete provider
            // http://stackoverflow.com/questions/1499442/best-way-to-use-structuremap-to-implement-strategy-pattern
            return pluginProvider.VisibilityProviderStrategy.IsVisible(this.VisibilityProvider, this, sourceMetadata);
        }

        #endregion

        #region Dynamic Nodes

        /// <summary>
        /// Gets or sets the name or type of the Dynamic Node Provider.
        /// </summary>
        /// <value>
        /// The name or type of the Dynamic Node Provider.
        /// </value>
        public override string DynamicNodeProvider { get; set; }

        /// <summary>
        /// Gets the dynamic node collection.
        /// </summary>
        /// <returns>A dynamic node collection.</returns>
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection()
        {
            // use strategy factory to provide implementation logic from concrete provider
            // http://stackoverflow.com/questions/1499442/best-way-to-use-structuremap-to-implement-strategy-pattern
            return pluginProvider.DynamicNodeProviderStrategy.GetDynamicNodeCollection(this.DynamicNodeProvider, this);
        }

        /// <summary>
        /// Gets whether the current node has a dynamic node provider.
        /// </summary>
        /// <value>
        /// <c>true</c> if there is a provider; otherwise <c>false</c>.
        /// </value>
        public override bool HasDynamicNodeProvider
        {
            // use strategy factory to provide implementation logic from concrete provider
            // http://stackoverflow.com/questions/1499442/best-way-to-use-structuremap-to-implement-strategy-pattern
            get { return (pluginProvider.DynamicNodeProviderStrategy.GetProvider(this.DynamicNodeProvider) != null); }
        }

        #endregion

        #region URL Resolver

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SiteMapNode" /> is clickable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if clickable; otherwise, <c>false</c>.
        /// </value>
        public override bool Clickable
        { 
            get { return this.clickable; }
            set { this.clickable = value; }
        }

        /// <summary>
        /// Gets or sets the name or type of the URL resolver.
        /// </summary>
        /// <value>
        /// The name or type of the URL resolver.
        /// </value>
        public override string UrlResolver { get; set; }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public override string Url 
        {
            get
            {
                if (!this.Clickable)
                {
                    return string.Empty;
                }
                if (!string.IsNullOrEmpty(this.ResolvedUrl))
                {
                    return this.ResolvedUrl;
                }
                // Only resolve the url if an absolute url is not already set
                // IMPORTANT: Must not call HasAbsoluteUrl here because that method calls this property.
                var unresolved = this.UnresolvedUrl;
                if (this.urlPath.IsAbsoluteUrl(unresolved))
                {
                    return unresolved;
                }
                return GetResolvedUrl();
            }
            set
            {
                this.url = value;
            }
        }

        /// <summary>
        /// The raw URL before being evaluated by any URL resolver.
        /// </summary>
        public override string UnresolvedUrl { get { return this.url; } }

        /// <summary>
        /// The resolved url that has been cached, if any.
        /// </summary>
        public override string ResolvedUrl { get { return this.resolvedUrl; } }

        /// <summary>
        /// A value indicating to cache the resolved URL. If false, the URL will be 
        /// resolved every time it is accessed.
        /// </summary>
        public override bool CacheResolvedUrl { get; set; }

        /// <summary>
        /// Sets the ResolvedUrl using the current Url or Url resolver.
        /// </summary>
        public override void ResolveUrl()
        {
            if (this.CacheResolvedUrl && string.IsNullOrEmpty(this.UnresolvedUrl) && 
                this.preservedRouteParameters.Count == 0 && !this.IncludeAmbientRequestValues)
            {
                this.resolvedUrl = this.GetResolvedUrl();
            }
        }

        protected string GetResolvedUrl()
        {
            // use strategy factory to provide implementation logic from concrete provider
            // http://stackoverflow.com/questions/1499442/best-way-to-use-structuremap-to-implement-strategy-pattern
            return pluginProvider.UrlResolverStrategy.ResolveUrl(
                this.UrlResolver, this, this.Area, this.Controller, this.Action, this.RouteValues);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include ambient request values 
        /// (from the RouteValues and/or query string) when resolving URLs.
        /// </summary>
        /// <value><b>true</b> to include ambient values (like MVC does); otherwise <b>false</b>.</value>
        public override bool IncludeAmbientRequestValues { get; set; }

        /// <summary>
        /// Gets a boolean value that indicates this is an external URL by checking whether it
        /// looks like an absolute path.
        /// </summary>
        /// <returns></returns>
        public override bool HasAbsoluteUrl()
        {
            return this.urlPath.IsAbsoluteUrl(this.Url);
        }

        /// <summary>
        /// Gets a boolean value that indicates this is an external URL by checking whether it
        /// looks like an absolute path and comparing the DnsSafeHost with the passed in context.
        /// </summary>
        /// <param name="httpContext">The http context for the current request.</param>
        /// <returns></returns>
        public override bool HasExternalUrl(HttpContextBase httpContext)
        {
            if (!this.HasAbsoluteUrl())
            {
                return false;
            }
            Uri uri = null;
            if (Uri.TryCreate(this.Url, UriKind.Absolute, out uri))
            {
                var isDifferentHost = !httpContext.Request.Url.DnsSafeHost.Equals(uri.DnsSafeHost);
                var isDifferentApplication = !uri.AbsolutePath.StartsWith(httpContext.Request.ApplicationPath, StringComparison.InvariantCultureIgnoreCase);
                return (isDifferentHost || isDifferentApplication);
            }
            return false;
        }

        #endregion

        #region Canonical Tag

        /// <summary>
        /// Gets or sets the canonical URL.
        /// </summary>
        /// <remarks>May not be used in conjunction with CanonicalKey. Only 1 canonical value is allowed.</remarks>
        public override string CanonicalUrl 
        {
            get 
            { 
                var absoluteCanonicalUrl = this.GetAbsoluteCanonicalUrl();
                if (!String.IsNullOrEmpty(absoluteCanonicalUrl))
                {
                    var httpContext = mvcContextFactory.CreateHttpContext();
                    if (absoluteCanonicalUrl.Equals(urlPath.UrlDecode(httpContext.Request.Url.AbsoluteUri)))
                    {
                        return String.Empty;
                    }
                }
                return absoluteCanonicalUrl;
            }
            set
            {
                if (!this.canonicalUrl.Equals(value))
                {
                    if (!String.IsNullOrEmpty(this.canonicalKey))
                    {
                        throw new ArgumentException(Resources.Messages.SiteMapNodeCanonicalValueAlreadySet, "CanonicalUrl");
                    }
                    this.canonicalUrl = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the canonical key. The key is used to reference another <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> to get the canonical URL.
        /// </summary>
        /// <remarks>May not be used in conjunction with CanonicalUrl. Only 1 canonical value is allowed.</remarks>
        public override string CanonicalKey 
        {
            get { return this.canonicalKey; }
            set
            {
                if (!this.canonicalKey.Equals(value))
                {
                    if (!String.IsNullOrEmpty(this.canonicalUrl))
                    {
                        throw new ArgumentException(Resources.Messages.SiteMapNodeCanonicalValueAlreadySet, "CanonicalKey");
                    }
                    this.canonicalKey = value;
                }
            }
        }

        /// <summary>
        /// Gets the absolute value of the canonical URL, finding the value by 
        /// <see cref="P:MvcSiteMapProvider.ISiteMapNode.CanonicalKey"/> if necessary.
        /// </summary>
        /// <returns>The absolute canonical URL.</returns>
        protected virtual string GetAbsoluteCanonicalUrl()
        {
            var url = this.canonicalUrl;
            if (!String.IsNullOrEmpty(url))
            {
                if (urlPath.IsAbsoluteUrl(url))
                {
                    return url;
                }
                return urlPath.MakeRelativeUrlAbsolute(url);
            }
            var key = this.canonicalKey;
            if (!String.IsNullOrEmpty(key))
            {
                var node = this.SiteMap.FindSiteMapNodeFromKey(key);
                if (node != null)
                {
                    url = node.Url;
                    if (urlPath.IsAbsoluteUrl(url))
                    {
                        return url;
                    }
                    return urlPath.MakeRelativeUrlAbsolute(url);
                }
            }
            return String.Empty;
        }

        #endregion

        #region Meta Robots Tag

        /// <summary>
        /// Gets the robots meta values.
        /// </summary>
        /// <value>The robots meta values.</value>
        public override IMetaRobotsValueCollection MetaRobotsValues { get { return this.metaRobotsValues; } }

        /// <summary>
        /// Gets a string containing the preformatted comma delimited list of values that can be inserted into the
        /// content attribute of the meta robots tag.
        /// </summary>
        public override string GetMetaRobotsContentString()
        {
            return this.MetaRobotsValues.GetMetaRobotsContentString();
        }

        /// <summary>
        /// Gets a boolean value indicating whether both the noindex and nofollow values are included in the
        /// list of robots meta values.
        /// </summary>
        public override bool HasNoIndexAndNoFollow
        {
            get { return this.MetaRobotsValues.HasNoIndexAndNoFollow; }
        }

        #endregion

        #region Route

        /// <summary>
        /// Gets or sets the route.
        /// </summary>
        /// <value>The route.</value>
        public override string Route { get; set; }

        /// <summary>
        /// Gets the route values.
        /// </summary>
        /// <value>The route values.</value>
        public override IRouteValueDictionary RouteValues 
        { 
            get 
            {
                if (this.IsReadOnly && !this.AreRouteParametersPreserved)
                {
                    this.PreserveRouteParameters();
                    this.AreRouteParametersPreserved = true;
                }
                return this.routeValues; 
            } 
        }

        /// <summary>
        /// Gets the preserved route parameter names (= values that will be used from the current request route).
        /// </summary>
        /// <value>The preserved route parameters.</value>
        public override IPreservedRouteParameterCollection PreservedRouteParameters { get { return this.preservedRouteParameters; } }


        /// <summary>
        /// Sets the preserved route parameters of the current request to the routeValues collection.
        /// </summary>
        /// <remarks>
        /// This method relies on the fact that the route value collection is request cached. The
        /// values written are for the current request only, after which they will be discarded.
        /// </remarks>
        protected virtual void PreserveRouteParameters()
        {
            if (this.PreservedRouteParameters.Count > 0)
            {
                var requestContext = mvcContextFactory.CreateRequestContext();
                var routeDataValues = requestContext.RouteData.Values;
                var queryStringValues = requestContext.HttpContext.Request.QueryString;

                foreach (var item in this.PreservedRouteParameters)
                {
                    var preservedParameterName = item.Trim();
                    if (!string.IsNullOrEmpty(preservedParameterName))
                    {
                        if (routeDataValues.ContainsKey(preservedParameterName))
                        {
                            this.routeValues[preservedParameterName] =
                                routeDataValues[preservedParameterName];
                        }
                        else if (queryStringValues[preservedParameterName] != null)
                        {
                            this.routeValues[preservedParameterName] =
                                queryStringValues[preservedParameterName];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Flag to ensure the route values are only preserved from the current request a single time.
        /// </summary>
        /// <returns><c>true</c> if the route values have been preserved for the current request; otherwise <c>false</c>.</returns>
        /// <remarks>This property must be overridden and provide an implementation that is stored in the request cache.</remarks>
        protected virtual bool AreRouteParametersPreserved 
        {
            get { return false; }
            set { } 
        }


        /// <summary>
        /// Gets the route data associated with the current node.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>The route data associated with the current node.</returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routes = mvcContextFactory.GetRoutes();
            RouteData routeData;
            if (!string.IsNullOrEmpty(this.Route))
            {
                routeData = routes[this.Route].GetRouteData(httpContext);
            }
            else
            {
                routeData = routes.GetRouteData(httpContext);
            }
            return routeData;
        }

        /// <summary>
        /// Determines whether this node matches the supplied route values.
        /// </summary>
        /// <param name="routeValues">An IDictionary<string, object> of route values.</param>
        /// <returns><c>true</c> if the route matches this node's RouteValues collection; otherwise <c>false</c>.</returns>
        public override bool MatchesRoute(IDictionary<string, object> routeValues)
        {
            // If not clickable, we never want to match the node.
            if (!this.Clickable)
                return false;

            // If URL is set explicitly, we should never match based on route values.
            if (!string.IsNullOrEmpty(this.UnresolvedUrl))
                return false;

            return this.RouteValues.MatchesRoute(routeValues);
        }

        #endregion

        #region MVC

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        /// <value>The area.</value>
        public override string Area
        {
            get { return RouteValues.ContainsKey("area") && RouteValues["area"] != null ? RouteValues["area"].ToString() : string.Empty; }
            set { RouteValues["area"] = value; }
        }

        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public override string Controller
        {
            get { return RouteValues.ContainsKey("controller") && RouteValues["controller"] != null ? RouteValues["controller"].ToString() : string.Empty; }
            set { RouteValues["controller"] = value; }
        }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public override string Action
        {
            get { return RouteValues.ContainsKey("action") && RouteValues["action"] != null ? RouteValues["action"].ToString() : string.Empty; }
            set { RouteValues["action"] = value; }
        }

        #endregion

        #region CopyTo

        public override void CopyTo(ISiteMapNode node)
        {
            // NOTE: Expected behavior is to reference 
            // the same child nodes, so this is okay.
            foreach (var child in this.ChildNodes)
                node.ChildNodes.Add(child);
            node.HttpMethod = this.HttpMethod;
            node.Title = this.title; // Get protected member
            node.Description = this.description; // Get protected member
            node.TargetFrame = this.TargetFrame;
            node.ImageUrl = this.ImageUrl;
            this.Attributes.CopyTo(node.Attributes);
            this.Roles.CopyTo(node.Roles);
            node.LastModifiedDate = this.LastModifiedDate;
            node.ChangeFrequency = this.ChangeFrequency;
            node.UpdatePriority = this.UpdatePriority;
            node.VisibilityProvider = this.VisibilityProvider;
            node.Clickable = this.Clickable;
            node.UrlResolver = this.UrlResolver;
            node.Url = this.url; // Get protected member
            node.CacheResolvedUrl = this.CacheResolvedUrl;
            node.CanonicalUrl = this.canonicalUrl; // Get protected member
            node.CanonicalKey = this.CanonicalKey;
            this.MetaRobotsValues.CopyTo(node.MetaRobotsValues);
            node.DynamicNodeProvider = this.DynamicNodeProvider;
            node.Route = this.Route;
            this.RouteValues.CopyTo(node.RouteValues);
            this.PreservedRouteParameters.CopyTo(node.PreservedRouteParameters);
            node.IncludeAmbientRequestValues = this.IncludeAmbientRequestValues;
            // NOTE: Area, Controller, and Action are covered under RouteValues.
        }

        #endregion
    }
}
