using System;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Provides the means to make the <see cref="T:MvcSiteMapProvider.SiteMapNode"/> instance read-only so it cannot be 
    /// inadvertently altered while it is in the cache.
    /// </summary>
    public class LockableSiteMapNode
        : SiteMapNode
    {
        public LockableSiteMapNode(
            ISiteMap siteMap,
            string key,
            bool isDynamic,
            ISiteMapNodePluginProvider pluginProvider,
            IMvcContextFactory mvcContextFactory,
            ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory,
            ILocalizationService localizationService,
            IUrlPath urlPath
            )
            : base(
                siteMap, 
                key, 
                isDynamic,
                pluginProvider,
                mvcContextFactory,
                siteMapNodeChildStateFactory, 
                localizationService, 
                urlPath
            )
        {
        }

        /// <summary>
        /// Gets or sets the display sort order for the node relative to its sibling nodes.
        /// </summary>
        public override int Order 
        {
            get { return base.Order; }
            set
            {
                this.ThrowIfReadOnly("Order");
                base.Order = value;
            }
        }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public override string HttpMethod
        {
            get { return base.HttpMethod; }
            set
            {
                this.ThrowIfReadOnly("HttpMethod");
                base.HttpMethod = value;
            }
        }

        /// <summary>
        /// Gets or sets the title (optional).
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return base.Title; }
            set
            {
                this.ThrowIfReadOnly("Title");
                base.Title = value;
            }
        }

        /// <summary>
        /// Gets or sets the description (optional).
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get { return base.Description; }
            set
            {
                this.ThrowIfReadOnly("Description");
                base.Description = value;
            }
        }

        /// <summary>
        /// Gets or sets the target frame (optional).
        /// </summary>
        /// <value>The target frame.</value>
        public override string TargetFrame
        {
            get { return base.TargetFrame; }
            set
            {
                this.ThrowIfReadOnly("TargetFrame");
                base.TargetFrame = value;
            }
        }

        /// <summary>
        /// Gets or sets the image URL (optional).
        /// </summary>
        /// <value>The image URL.</value>
        public override string ImageUrl
        {
            get { return base.ImageUrl; }
            set
            {
                this.ThrowIfReadOnly("ImageUrl");
                base.ImageUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the image URL protocol, such as http, https (optional).
        /// If not provided, it will default to the protocol of the current request.
        /// </summary>
        /// <value>The protocol of the image URL.</value>
        public override string ImageUrlProtocol 
        {
            get { return base.ImageUrlProtocol; }
            set
            {
                this.ThrowIfReadOnly("ImageUrlProtocol");
                base.ImageUrlProtocol = value;
            }
        }

        /// <summary>
        /// Gets or sets the image URL host name, such as www.somewhere.com (optional).
        /// </summary>
        /// <value>The protocol of the image URL.</value>
        public override string ImageUrlHostName 
        {
            get { return base.ImageUrlHostName; }
            set
            {
                this.ThrowIfReadOnly("ImageUrlHostName");
                base.ImageUrlHostName = value;
            }
        }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public override DateTime LastModifiedDate
        {
            get { return base.LastModifiedDate; }
            set
            {
                this.ThrowIfReadOnly("LastModifiedDate");
                base.LastModifiedDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the change frequency.
        /// </summary>
        /// <value>The change frequency.</value>
        public override ChangeFrequency ChangeFrequency
        {
            get { return base.ChangeFrequency; }
            set
            {
                this.ThrowIfReadOnly("ChangeFrequency");
                base.ChangeFrequency = value;
            }
        }

        /// <summary>
        /// Gets or sets the update priority.
        /// </summary>
        /// <value>The update priority.</value>
        public override UpdatePriority UpdatePriority
        {
            get { return base.UpdatePriority; }
            set
            {
                this.ThrowIfReadOnly("UpdatePriority");
                base.UpdatePriority = value;
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
        public override string VisibilityProvider
        {
            get { return base.VisibilityProvider; }
            set
            {
                this.ThrowIfReadOnly("VisibilityProvider");
                base.VisibilityProvider = value;
            }
        }

        #endregion

        #region Dynamic Nodes

        /// <summary>
        /// Gets or sets the name or type of the Dynamic Node Provider.
        /// </summary>
        /// <value>
        /// The name or type of the Dynamic Node Provider.
        /// </value>
        public override string DynamicNodeProvider
        {
            get { return base.DynamicNodeProvider; }
            set
            {
                this.ThrowIfReadOnly("DynamicNodeProvider");
                base.DynamicNodeProvider = value;
            }
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
            get { return base.Clickable; }
            set
            {
                this.ThrowIfReadOnly("Clickable");
                base.Clickable = value;
            }
        }

        /// <summary>
        /// Gets or sets the name or type of the URL resolver.
        /// </summary>
        /// <value>
        /// The name or type of the URL resolver.
        /// </value>
        public override string UrlResolver
        {
            get { return base.UrlResolver; }
            set
            {
                this.ThrowIfReadOnly("UrlResolver");
                base.UrlResolver = value;
            }
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public override string Url
        {
            get { return base.Url; }
            set
            {
                this.ThrowIfReadOnly("Url");
                base.Url = value;
            }
        }

        /// <summary>
        /// A value indicating to cache the resolved URL. If false, the URL will be 
        /// resolved every time it is accessed.
        /// </summary>
        public override bool CacheResolvedUrl 
        {
            get { return base.CacheResolvedUrl; }
            set
            {
                this.ThrowIfReadOnly("CacheResolvedUrl");
                base.CacheResolvedUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include ambient request values 
        /// (from the RouteValues and/or query string) when resolving URLs.
        /// </summary>
        /// <value><b>true</b> to include ambient values (like MVC does); otherwise <b>false</b>.</value>
        public override bool IncludeAmbientValuesInUrl
        {
            get { return base.IncludeAmbientValuesInUrl; }
            set
            {
                this.ThrowIfReadOnly("IncludeAmbientValuesInUrl");
                base.IncludeAmbientValuesInUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the protocol, such as http or https that will 
        /// be built into the URL.
        /// </summary>
        /// <value>The protocol.</value>
        public override string Protocol
        {
            get { return base.Protocol; }
            set
            {
                this.ThrowIfReadOnly("Protocol");
                base.Protocol = value;
            }
        }

        /// <summary>
        /// Gets or sets the host name that will be built into the URL.
        /// </summary>
        /// <value>The host name.</value>
        public override string HostName
        {
            get { return base.HostName; }
            set
            {
                this.ThrowIfReadOnly("HostName");
                base.HostName = value;
            }
        }

        /// <summary>
        /// Sets the ResolvedUrl using the current Url or Url resolver.
        /// </summary>
        public override void ResolveUrl()
        {
            this.ThrowIfReadOnly("ResolveUrl");
            base.ResolveUrl();
        }

        #endregion

        #region Canonical Tag

        /// <summary>
        /// Gets or sets the canonical key. The key is used to reference another <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> to get the canonical URL.
        /// </summary>
        /// <remarks>May not be used in conjunction with CanonicalUrl; only 1 canonical value is allowed.</remarks>
        public override string CanonicalKey
        {
            get { return base.CanonicalKey; }
            set
            {
                this.ThrowIfReadOnly("CanonicalKey");
                base.CanonicalKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the canonical URL.
        /// </summary>
        /// <remarks>May not be used in conjunction with CanonicalKey; only 1 canonical value is allowed.</remarks>
        public override string CanonicalUrl
        {
            get { return base.CanonicalUrl; }
            set
            {
                this.ThrowIfReadOnly("CanonicalUrl");
                base.CanonicalUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the canonical URL protocol, such as http, https (optional).
        /// </summary>
        /// <value>The protocol of the image URL.</value>
        public override string CanonicalUrlProtocol 
        {
            get { return base.CanonicalUrlProtocol; }
            set
            {
                this.ThrowIfReadOnly("CanonicalUrlProtocol");
                base.CanonicalUrlProtocol = value;
            }
        }

        /// <summary>
        /// Gets or sets the canonical URL host name, such as www.somewhere.com (optional).
        /// </summary>
        /// <value>The protocol of the image URL.</value>
        public override string CanonicalUrlHostName 
        {
            get { return base.CanonicalUrlHostName; }
            set
            {
                this.ThrowIfReadOnly("CanonicalUrlHostName");
                base.CanonicalUrlHostName = value;
            }
        }

        #endregion

        #region Route

        /// <summary>
        /// Gets or sets the route.
        /// </summary>
        /// <value>The route.</value>
        public override string Route
        {
            get { return base.Route; }
            set
            {
                this.ThrowIfReadOnly("Route");
                base.Route = value;
            }
        }

        #endregion

        #region MVC

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        /// <value>The area.</value>
        public override string Area
        {
            get { return base.Area; }
            set
            {
                this.ThrowIfReadOnly("Area");
                base.Area = value;
            }
        }

        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public override string Controller
        {
            get { return base.Controller; }
            set
            {
                this.ThrowIfReadOnly("Controller");
                base.Controller = value;
            }
        }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public override string Action
        {
            get { return base.Action; }
            set
            {
                this.ThrowIfReadOnly("Action");
                base.Action = value;
            }
        }

        #endregion


        protected virtual void ThrowIfReadOnly(string memberName)
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(string.Format(Resources.Messages.SiteMapNodeReadOnly, memberName));
            }
        }
    }
}
