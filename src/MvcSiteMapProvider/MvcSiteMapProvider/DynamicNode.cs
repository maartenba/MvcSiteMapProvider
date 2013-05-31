using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// DynamicNode class
    /// </summary>
    public class DynamicNode
    {
        private ChangeFrequency changeFrequency = ChangeFrequency.Undefined;
        private UpdatePriority updatePriority = UpdatePriority.Undefined;

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
        /// Gets or sets the Url (optional).
        /// </summary>
        /// <value>The area.</value>
        public string Url { get; set; }

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
        public ChangeFrequency ChangeFrequency 
        { 
            get { return changeFrequency; } 
            set { changeFrequency = value; } 
        }

        /// <summary>
        /// Gets or sets the update priority.
        /// </summary>
        /// <value>The update priority.</value>
        public UpdatePriority UpdatePriority 
        {
            get { return updatePriority; }
            set { updatePriority = value; }
        }

        /// <summary>
        /// A value indicating to cache the resolved URL. If false, the URL will be 
        /// resolved every time it is accessed.
        /// </summary>
        public bool? CacheResolvedUrl { get; set; }


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
        public IList<string> MetaRobotsValues { get; set; }

        /// <summary>
        /// Copies the values for matching properties on an <see cref="T:MvcSiteMapNodeProvider.ISiteMapNode"/> instance, but
        /// doesn't overwrite any values that are not set in this <see cref="T:MvcSiteMapNodeProvider.DynamicNode"/> instance.
        /// </summary>
        /// <param name="node">The site map node to copy the values into.</param>
        public void SafeCopyTo(ISiteMapNode node)
        {
            if (!string.IsNullOrEmpty(this.Route))
                node.Route = this.Route;
            if (!string.IsNullOrEmpty(this.Url))
                node.Url = this.Url;
            if (!string.IsNullOrEmpty(this.Area))
                node.Area = this.Area;
            if (!string.IsNullOrEmpty(this.Controller))
                node.Controller = this.Controller;
            if (!string.IsNullOrEmpty(this.Action))
                node.Action = this.Action;
            if (!string.IsNullOrEmpty(this.Title))
                node.Title = this.Title;
            if (!string.IsNullOrEmpty(this.Description))
                node.Description = this.Description;
            if (!string.IsNullOrEmpty(this.TargetFrame))
                node.TargetFrame = this.TargetFrame;
            if (!string.IsNullOrEmpty(this.ImageUrl))
                node.ImageUrl = this.ImageUrl;
            foreach (var kvp in this.RouteValues)
            {
                node.RouteValues[kvp.Key] = kvp.Value;
            }
            foreach (var kvp in this.Attributes)
            {
                node.Attributes[kvp.Key] = kvp.Value;
            }
            if (this.PreservedRouteParameters.Any())
            {
                foreach (var p in this.PreservedRouteParameters)
                {
                    if (!node.PreservedRouteParameters.Contains(p))
                    {
                        node.PreservedRouteParameters.Add(p);
                    }
                }
            }
            if (this.Roles.Any())
            {
                foreach (var role in this.Roles)
                {
                    if (!node.Roles.Contains(role))
                    {
                        node.Roles.Add(role);
                    }
                }
            }
            if (this.LastModifiedDate != null && this.LastModifiedDate.HasValue)
                node.LastModifiedDate = this.LastModifiedDate.Value;
            if (this.ChangeFrequency != ChangeFrequency.Undefined)
                node.ChangeFrequency = this.ChangeFrequency;
            if (this.UpdatePriority != UpdatePriority.Undefined)
                node.UpdatePriority = this.UpdatePriority;
            if (this.CacheResolvedUrl != null)
                node.CacheResolvedUrl = (bool)this.CacheResolvedUrl;
            if (!string.IsNullOrEmpty(this.CanonicalKey))
                node.CanonicalKey = this.CanonicalKey;
            if (!string.IsNullOrEmpty(this.CanonicalUrl))
                node.CanonicalUrl = this.CanonicalUrl;
            if (this.MetaRobotsValues.Any())
            {
                foreach (var value in this.MetaRobotsValues)
                {
                    if (!node.MetaRobotsValues.Contains(value))
                    {
                        node.MetaRobotsValues.Add(value);
                    }
                }
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicNode"/> class.
        /// </summary>
        public DynamicNode()
        {
            RouteValues = new Dictionary<string, object>();
            Attributes = new Dictionary<string, string>();
            PreservedRouteParameters = new List<string>();
            Roles = new List<string>();
            MetaRobotsValues = new List<string>();
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
