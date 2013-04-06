using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// SiteMap node attribute, used to decorate action methods with SiteMap node metadata
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class MvcSiteMapNodeAttribute : Attribute, IMvcSiteMapNodeAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MvcSiteMapNodeAttribute()
        {
            Clickable = true;
            Attributes = new Dictionary<string, string>();
        }

        /// <summary>
        /// SiteMap node key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// SiteMap node route (autodetected by default)
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// Gets or sets the name of the area.
        /// </summary>
        /// <value>The name of the area.</value>
        public string AreaName { get; set; }

        /// <summary>
        /// SiteMap node title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// SiteMap node description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// SiteMap node URL (optional)
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// SiteMap node parent key
        /// </summary>
        public string ParentKey { get; set; }

        /// <summary>
        /// Resource key, used when working with localization
        /// </summary>
        public string ResourceKey { get; set; }

        /// <summary>
        /// Gets or sets the roles that may access the SiteMap node.
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// Is it a clickable node?
        /// </summary>
        public bool Clickable { get; set; }

        /// <summary>
        /// Dynamic node provider
        /// </summary>
        public string DynamicNodeProvider { get; set; }

        /// <summary>
        /// Gets or sets the site map node URL resolver.
        /// </summary>
        /// <value>The site map node URL resolver.</value>
        public string UrlResolver { get; set; }

        /// <summary>
        /// Gets or sets the visibility provider.
        /// </summary>
        /// <value>The visibility provider.</value>
        public string VisibilityProvider { get; set; }

        /// <summary>
        /// Used for ordering nodes
        /// </summary>
        public int Order { get; set; }

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

        /// <summary>
        /// Gets or sets the target frame.
        /// </summary>
        /// <value>The target frame.</value>
        public string TargetFrame { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl { get; set; }

        /// <summary>
        /// A value indicating to cache the resolved URL. If false, the URL will be 
        /// resolved every time it is accessed.
        /// </summary>
        public bool CacheResolvedUrl { get; set; }


        /// <summary>
        /// Gets or sets the canonical URL.
        /// </summary>
        /// <remarks>May not be used in conjuntion with CanonicalKey. Only 1 canonical value is allowed.</remarks>
        public string CanonicalUrl { get; set; }

        /// <summary>
        /// Gets or sets the canonical key. The key is used to reference another ISiteMapNode to get the canonical URL.
        /// </summary>
        /// <remarks>May not be used in conjuntion with CanonicalUrl. Only 1 canonical value is allowed.</remarks>
        public string CanonicalKey { get; set; }

        /// <summary>
        /// Gets or sets the robots meta values.
        /// </summary>
        /// <value>The robots meta values.</value>
        public string[] MetaRobotsValues { get; set; }

        /// <summary>
        /// Gets or sets the preserved route parameter names (= values that will be used from the current request route).
        /// </summary>
        /// <value>
        /// The preserved route parameter names.
        /// </value>
        public string PreservedRouteParameters { get; set; }

        /// <summary>
        /// Gets or sets the attributes (optional).
        /// </summary>
        /// <value>The attributes.</value>
        public IDictionary<string, string> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the name of the cache key this node is associated with
        /// </summary>
        public string SiteMapCacheKey { get; set; }
    }
}
