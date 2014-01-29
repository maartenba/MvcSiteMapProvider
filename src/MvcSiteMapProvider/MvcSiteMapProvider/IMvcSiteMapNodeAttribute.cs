using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// SiteMap node attribute contract
    /// </summary>
    public interface IMvcSiteMapNodeAttribute
    {
        /// <summary>
        /// SiteMap node key
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// SiteMap node route (auto-detected by default)
        /// </summary>
        string Route { get; set; }

        /// <summary>
        /// Gets or sets the name of the area.
        /// </summary>
        /// <value>The name of the area.</value>
        string Area { get; set; }

        /// <summary>
        /// Gets or sets the name of the area.
        /// </summary>
        /// <value>The name of the area.</value>
        [Obsolete("AreaName is deprecated and will be removed in version 5. Use Area instead.")]
        string AreaName { get; set; }

        /// <summary>
        /// SiteMap node title
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// SiteMap node description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// SiteMap node URL (optional)
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// SiteMap node parent key
        /// </summary>
        string ParentKey { get; set; }

        /// <summary>
        /// Resource key, used when working with localization
        /// </summary>
        string ResourceKey { get; set; }

        /// <summary>
        /// Gets or sets the roles that may access the SiteMap node.
        /// </summary>
        string[] Roles { get; set; }

        /// <summary>
        /// Is it a clickable node?
        /// </summary>
        bool Clickable { get; set; }

        /// <summary>
        /// Dynamic node provider
        /// </summary>
        string DynamicNodeProvider { get; set; }

        /// <summary>
        /// Gets or sets the site map node URL resolver.
        /// </summary>
        /// <value>The site map node URL resolver.</value>
        string UrlResolver { get; set; }

        /// <summary>
        /// Gets or sets the visibility provider.
        /// </summary>
        /// <value>The visibility provider.</value>
        string VisibilityProvider { get; set; }

        /// <summary>
        /// Used for ordering nodes
        /// </summary>
        int Order { get; set; }

        /// <summary>
        /// Gets or sets a string representation of the last modified date.
        /// 
        /// The value may be any date format supported by the invariant culture.
        /// </summary>
        /// <value>A string representation of the last modified date. May be any date format that is supported by the invariant culture.</value>
        string LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the change frequency.
        /// </summary>
        /// <value>The change frequency.</value>
        ChangeFrequency ChangeFrequency { get; set; }

        /// <summary>
        /// Gets or sets the update priority.
        /// </summary>
        /// <value>The update priority.</value>
        UpdatePriority UpdatePriority { get; set; }

        /// <summary>
        /// Gets or sets the target frame.
        /// </summary>
        /// <value>The target frame.</value>
        string TargetFrame { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        string ImageUrl { get; set; }

        /// <summary>
        /// A value indicating to cache the resolved URL. If false, the URL will be 
        /// resolved every time it is accessed.
        /// </summary>
        bool CacheResolvedUrl { get; set; }

        /// <summary>
        /// Gets or sets the canonical URL.
        /// </summary>
        /// <remarks>May not be used in conjunction with CanonicalKey. Only 1 canonical value is allowed.</remarks>
        string CanonicalUrl { get; set; }

        /// <summary>
        /// Gets or sets the canonical key. The key is used to reference another ISiteMapNode to get the canonical URL.
        /// </summary>
        /// <remarks>May not be used in conjunction with CanonicalUrl. Only 1 canonical value is allowed.</remarks>
        string CanonicalKey { get; set; }

        /// <summary>
        /// Gets or sets the robots meta values.
        /// </summary>
        /// <value>The robots meta values.</value>
        string[] MetaRobotsValues { get; set; }

        /// <summary>
        /// Gets or sets the preserved route parameter names (= values that will be used from the current request route).
        /// </summary>
        /// <value>
        /// The preserved route parameter names.
        /// </value>
        string PreservedRouteParameters { get; set; }

        /// <summary>
        /// Gets or sets the attributes (optional).
        /// 
        /// The value must be a JSON string that represents a dictionary of key-value pairs. Example: @"{ ""key-1"": ""value-1""[, ""key-x"": ""value-x""] }". 
        /// The value may be a string or primitive type (by leaving off the quotes).
        /// </summary>
        /// <value>A JSON string that represents a dictionary of key-value pairs. Example: @"{ ""key-1"": ""value-1""[, ""key-x"": ""value-x""] }". 
        /// The value may be a string or primitive type (by leaving off the quotes).</value>
        string Attributes { get; set; }

        /// <summary>
        /// Gets or sets the name of the cache key this node is associated with.
        /// </summary>
        /// <value>The SiteMap cache key.</value>
        string SiteMapCacheKey { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method (such as GET, POST, or HEAD) to use to determine
        /// node accessibility.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        string HttpMethod { get; set; }
    }
}
