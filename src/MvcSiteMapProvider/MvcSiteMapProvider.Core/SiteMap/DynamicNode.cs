using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// DynamicNode class
    /// </summary>
    public class DynamicNode
    {

        /// <summary>
        /// Gets or sets the route.
        /// </summary>
        /// <value>The route.</value>
        public string Route { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the parent key (optional).
        /// </summary>
        /// <value>The parent key.</value>
        public string ParentKey { get; set; }

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

        /// <summary>
        /// Gets or sets the title (optional).
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description (optional).
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

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

        /// <summary>
        /// Gets or sets the route values.
        /// </summary>
        /// <value>The route values.</value>
        public IDictionary<string, object> RouteValues { get; set; }

        /// <summary>
        /// Gets or sets the attributes (optional).
        /// </summary>
        /// <value>The attributes.</value>
        public IDictionary<string, string> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the preserved route parameter names (= values that will be used from the current request route).
        /// </summary>
        /// <value>The attributes.</value>
        public IList<string> PreservedRouteParameters { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public IList<string> Roles { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

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
        /// Initializes a new instance of the <see cref="DynamicNode"/> class.
        /// </summary>
        public DynamicNode()
        {
            RouteValues = new Dictionary<string, object>();
            Attributes = new Dictionary<string, string>();
            PreservedRouteParameters = new List<string>();
            Roles = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicNode"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="title">The title.</param>
        public DynamicNode(string key, string title)
            : this(key, null, title, "")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicNode"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="parentKey">The parent key.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        public DynamicNode(string key, string parentKey, string title, string description)
            : this()
        {
            Key = key;
            ParentKey = parentKey;
            Title = title;
            Description = description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicNode"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="parentKey">The parent key.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="controller">The controller (optional).</param>
        /// <param name="action">The action (optional).</param>
        public DynamicNode(string key, string parentKey, string title, string description, string controller, string action)
            : this(key, parentKey, title, description, null, controller, action)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicNode"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="parentKey">The parent key.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="action">The action (optional).</param>
        public DynamicNode(string key, string parentKey, string title, string description, string action)
            : this(key, parentKey, title, description, null, null, action)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicNode"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="parentKey">The parent key.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="area">The area (optional).</param>
        /// <param name="controller">The controller (optional).</param>
        /// <param name="action">The action (optional).</param>
        public DynamicNode(string key, string parentKey, string title, string description, string area, string controller, string action)
            : this(key, parentKey, title, description)
        {
            Area = area;
            Controller = controller;
            Action = action;
        }
    }
}
