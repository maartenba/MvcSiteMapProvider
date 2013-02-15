using System;

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
        /// SiteMap node route (autodetected by default)
        /// </summary>
        string Route { get; set; }

        /// <summary>
        /// Gets or sets the name of the area.
        /// </summary>
        /// <value>The name of the area.</value>
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
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        DateTime LastModifiedDate { get; set; }

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
        /// Gets or sets the preserved route parameter names (= values that will be used from the current request route).
        /// </summary>
        /// <value>
        /// The preserved route parameter names.
        /// </value>
        string PreservedRouteParameters { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider this node is associated with
        /// </summary>
        string SiteMapProviderName { get; set; }
    }
}
