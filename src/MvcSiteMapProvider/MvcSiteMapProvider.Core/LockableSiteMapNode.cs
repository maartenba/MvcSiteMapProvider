using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using MvcSiteMapProvider.Core.Mvc.UrlResolver;
using MvcSiteMapProvider.Core.Collections;
using MvcSiteMapProvider.Core.Globalization;
using MvcSiteMapProvider.Core.Mvc;

namespace MvcSiteMapProvider.Core
{
    /// <summary>
    /// This class implements the decorator pattern and can be used to make an instance of ISiteMapNode read-only after the BuildSiteMap method has been called.
    /// </summary>
    public class LockableSiteMapNode
        : SiteMapNode
    {
        public LockableSiteMapNode(
            ISiteMap siteMap,
            string key,
            bool isDynamic,
            ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory,
            ILocalizationService localizationService,
            IDynamicNodeProviderStrategy dynamicNodeProviderStrategy,
            ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy,
            ISiteMapNodeVisibilityProviderStrategy siteMapNodeVisibilityProviderStrategy,
            IActionMethodParameterResolver actionMethodParameterResolver
            )
            : base(
                siteMap, 
                key, 
                isDynamic, 
                siteMapNodeChildStateFactory, 
                localizationService, 
                dynamicNodeProviderStrategy, 
                siteMapNodeUrlResolverStrategy, 
                siteMapNodeVisibilityProviderStrategy, 
                actionMethodParameterResolver
            )
        {
        }


        public override ISiteMapNode ParentNode
        {
            get { return base.ParentNode; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "ParentNode"));
                }
                base.ParentNode = value;
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "HttpMethod"));
                }
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
                // TODO: Find out what the attribute is for that overwrites this from the UI layer.
                //if (this.IsReadOnly)
                //{
                //    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Title"));
                //}
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Description"));
                }
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "TargetFrame"));
                }
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "ImageUrl"));
                }
                base.ImageUrl = value;
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "LastModifiedDate"));
                }
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "ChangeFrequency"));
                }
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "UpdatePriority"));
                }
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "VisibilityProvider"));
                }
                base.VisibilityProvider = value;
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Clickable"));
                }
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "UrlResolver"));
                }
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Url"));
                }
                base.Url = value;
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "DynamicNodeProvider"));
                }
                base.DynamicNodeProvider = value;
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Route"));
                }
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Area"));
                }
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Controller"));
                }
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
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, "Action"));
                }
                base.Action = value;
            }
        }

        #endregion
    }
}
