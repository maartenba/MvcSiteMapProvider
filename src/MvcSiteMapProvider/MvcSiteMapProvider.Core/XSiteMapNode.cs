//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.Diagnostics;

//namespace MvcSiteMapProvider.Core
//{
//    public class XSiteMapNode
//        : ICloneable
//    {
//        /// <summary>
//        /// Gets or sets the key.
//        /// </summary>
//        /// <value>The key.</value>
//        public string Key { get; set; }

//        /// <summary>
//        /// Gets or sets the parent node.
//        /// </summary>
//        /// <value>
//        /// The parent node.
//        /// </value>
//        public XSiteMapNode ParentNode { get; set; }

//        /// <summary>
//        /// Gets or sets the child nodes.
//        /// </summary>
//        /// <value>
//        /// The child nodes.
//        /// </value>
//        public List<XSiteMapNode> ChildNodes { get; set; }

//        /// <summary>
//        /// Gets or sets the URL.
//        /// </summary>
//        /// <value>
//        /// The URL.
//        /// </value>
//        public string Url { get; set; }

//        /// <summary>
//        /// Gets or sets the HTTP method.
//        /// </summary>
//        /// <value>
//        /// The HTTP method.
//        /// </value>
//        public string HttpMethod { get; set; }

//        /// <summary>
//        /// Gets or sets a value indicating whether this <see cref="XSiteMapNode" /> is clickable.
//        /// </summary>
//        /// <value>
//        ///   <c>true</c> if clickable; otherwise, <c>false</c>.
//        /// </value>
//        public bool Clickable { get; set; }

//        /// <summary>
//        /// Gets or sets the implicit resource key (optional).
//        /// </summary>
//        /// <value>The implicit resource key.</value>
//        public string ResourceKey { get; set; }

//        /// <summary>
//        /// Gets or sets the title (optional).
//        /// </summary>
//        /// <value>The title.</value>
//        public string Title { get; set; }

//        /// <summary>
//        /// Gets or sets the description (optional).
//        /// </summary>
//        /// <value>The description.</value>
//        public string Description { get; set; }

//        /// <summary>
//        /// Gets or sets the target frame (optional).
//        /// </summary>
//        /// <value>The target frame.</value>
//        public string TargetFrame { get; set; }

//        /// <summary>
//        /// Gets or sets the image URL (optional).
//        /// </summary>
//        /// <value>The image URL.</value>
//        public string ImageUrl { get; set; }

//        /// <summary>
//        /// Gets or sets the attributes (optional).
//        /// </summary>
//        /// <value>The attributes.</value>
//        public IDictionary<string, string> Attributes { get; set; }

//        /// <summary>
//        /// Gets or sets the roles.
//        /// </summary>
//        /// <value>The roles.</value>
//        public IList<string> Roles { get; set; }

//        /// <summary>
//        /// Gets or sets the last modified date.
//        /// </summary>
//        /// <value>The last modified date.</value>
//        public DateTime LastModifiedDate { get; set; }

//        /// <summary>
//        /// Gets or sets the change frequency.
//        /// </summary>
//        /// <value>The change frequency.</value>
//        public ChangeFrequency ChangeFrequency { get; set; }

//        /// <summary>
//        /// Gets or sets the update priority.
//        /// </summary>
//        /// <value>The update priority.</value>
//        public UpdatePriority UpdatePriority { get; set; }

//        /// <summary>
//        /// Gets or sets the type of the visibility provider.
//        /// </summary>
//        /// <value>
//        /// The type of the visibility provider.
//        /// </value>
//        public string VisibilityProviderType { get; set; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="XSiteMapNode"/> class.
//        /// </summary>
//        public XSiteMapNode()
//        {
//            ChildNodes = new List<XSiteMapNode>();
//            Attributes = new Dictionary<string, string>();
//            Roles = new List<string>();
//        }

//        /// <summary>
//        /// Creates a new object that is a copy of the current instance.
//        /// </summary>
//        /// <returns>
//        /// A new object that is a copy of this instance.
//        /// </returns>
//        public virtual object Clone()
//        {
//            var clone = new XSiteMapNode();
//            clone.Key = this.Key;
//            clone.ParentNode = this.ParentNode;
//            clone.ChildNodes = new List<XSiteMapNode>();
//            foreach (var childNode in ChildNodes)
//            {
//                var childClone = childNode.Clone() as XSiteMapNode;
//                childClone.ParentNode = clone;
//                clone.ChildNodes.Add(childClone);
//            }
//            clone.Url = this.Url;
//            clone.HttpMethod = this.HttpMethod;
//            clone.Clickable = this.Clickable;
//            clone.ResourceKey = this.ResourceKey;
//            clone.Title = this.Title;
//            clone.Description = this.Description;
//            clone.TargetFrame = this.TargetFrame;
//            clone.ImageUrl = this.ImageUrl;
//            clone.Attributes = new Dictionary<string, string>(this.Attributes);
//            clone.Roles = new List<string>(this.Roles);
//            clone.LastModifiedDate = this.LastModifiedDate;
//            clone.ChangeFrequency = this.ChangeFrequency;
//            clone.UpdatePriority = this.UpdatePriority;
//            clone.VisibilityProviderType = this.VisibilityProviderType;
//            return clone;
//        }

//        /// <summary>
//        /// Makes the current node dynamic.
//        /// </summary>
//        /// <returns></returns>
//        internal XSiteMapNode AsDynamicNode()
//        {
//            var clone = this.Clone() as XSiteMapNode;
//            clone.Key = "";
//            return clone;
//        }

//        // TODO: move to extension method
//        /// <summary>
//        /// Finds the closest parent.
//        /// </summary>
//        /// <param name="parentKey">The parent key.</param>
//        /// <returns></returns>
//        public virtual XSiteMapNode FindClosestParent(string parentKey)
//        {
//            XSiteMapNode current = this;
//            while (current.ParentNode != null)
//            {
//                if (current.Key == parentKey)
//                {
//                    return current;
//                }
//                current = current.ParentNode;
//            }
//            return null;
//        }

//        // TODO: move to extension method
//        /// <summary>
//        /// Finds the node for a given key by searching parents and children.
//        /// </summary>
//        /// <param name="key">The key.</param>
//        /// <returns></returns>
//        public virtual XSiteMapNode FindForKey(string key)
//        {
//            XSiteMapNode current = this;

//            // Search up
//            while (current.ParentNode != null)
//            {
//                if (current.Key == key)
//                {
//                    return current;
//                }
//                current = current.ParentNode;
//            }

//            // Search down
//            return FindChildWithKey(key, current);
//        }

//        // TODO: move to extension method
//        /// <summary>
//        /// Finds the node for a given key by searching children.
//        /// </summary>
//        /// <param name="key">The key.</param>
//        /// <param name="root">The root.</param>
//        /// <returns></returns>
//        public virtual XSiteMapNode FindChildWithKey(string key, XSiteMapNode root)
//        {
//            foreach (var childNode in root.ChildNodes)
//            {
//                if (childNode.Key == key)
//                {
//                    return childNode;
//                }
//                return FindChildWithKey(key, childNode);
//            }
//            return null;
//        }
//    }
//}
