using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Collections;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Provides overrides of the <see cref="T:MvcSiteMapProvider.SiteMapNode"/> that track the return values of specific 
    /// resource-intensive members in case they are accessed more than one time during a single request. Also stores 
    /// values set from specific read-write properties in the request cache for later retrieval.
    /// </summary>
    public class RequestCacheableSiteMapNode
        : LockableSiteMapNode
    {
        public RequestCacheableSiteMapNode(
            ISiteMap siteMap,
            string key,
            bool isDynamic,
            ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory,
            ILocalizationService localizationService,
            IDynamicNodeProviderStrategy dynamicNodeProviderStrategy,
            ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy,
            ISiteMapNodeVisibilityProviderStrategy siteMapNodeVisibilityProviderStrategy,
            IActionMethodParameterResolver actionMethodParameterResolver,
            IUrlPath urlPath,
            IRequestCache requestCache
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
                actionMethodParameterResolver,
                urlPath
            )
        {
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.requestCache = requestCache;
        }

        private readonly IRequestCache requestCache;
        private readonly Guid instanceId = Guid.NewGuid();


        #region ISiteMapNode Members

        public override string Title
        {
            get 
            {
                var key = this.GetCacheKey("Title");
                var result = this.requestCache.GetValue<string>(key);
                if (result == null)
                {
                    result = base.Title;
                    this.requestCache.SetValue<string>(key, result);
                }
                return result;
            }
            set 
            { 
                if (this.IsReadOnly)
                {
                    var key = this.GetCacheKey("Title");
                    this.requestCache.SetValue<string>(key, value);
                }
                else
                {
                    base.Title = value;
                }
            }
        }

        public override string Description
        {
            get
            {
                var key = this.GetCacheKey("Description");
                var result = this.requestCache.GetValue<string>(key);
                if (result == null)
                {
                    result = base.Description;
                    this.requestCache.SetValue<string>(key, result);
                }
                return result;
            }
            set 
            {
                if (this.IsReadOnly)
                {
                    var key = this.GetCacheKey("Description");
                    this.requestCache.SetValue<string>(key, value);
                }
                else
                {
                    base.Description = value;
                }
            }
        }

        public override string TargetFrame
        {
            get
            {
                var key = this.GetCacheKey("TargetFrame");
                var result = this.requestCache.GetValue<string>(key);
                if (result == null)
                {
                    result = base.TargetFrame;
                }
                return result;
            }
            set
            {
                if (this.IsReadOnly)
                {
                    var key = this.GetCacheKey("TargetFrame");
                    this.requestCache.SetValue<string>(key, value);
                }
                else
                {
                    base.TargetFrame = value;
                }
            }
        }

        public override string ImageUrl
        {
            get
            {
                var key = this.GetCacheKey("ImageUrl");
                var result = this.requestCache.GetValue<string>(key);
                if (result == null)
                {
                    result = base.ImageUrl;
                }
                return result;
            }
            set
            {
                if (this.IsReadOnly)
                {
                    var key = this.GetCacheKey("ImageUrl");
                    this.requestCache.SetValue<string>(key, value);
                }
                else
                {
                    base.ImageUrl = value;
                }
            }
        }

        public override string VisibilityProvider
        {
            get
            {
                var key = this.GetCacheKey("VisibilityProvider");
                var result = this.requestCache.GetValue<string>(key);
                if (result == null)
                {
                    result = base.VisibilityProvider;
                }
                return result;
            }
            set
            {
                if (this.IsReadOnly)
                {
                    var key = this.GetCacheKey("VisibilityProvider");
                    this.requestCache.SetValue<string>(key, value);
                }
                else
                {
                    base.VisibilityProvider = value;
                }
            }
        }

        public override bool Clickable
        {
            get
            {
                var key = this.GetCacheKey("Clickable");
                var result = this.requestCache.GetValue<bool?>(key);
                if (result == null)
                {
                    result = base.Clickable;
                }
                return (bool)result;
            }
            set
            {
                if (this.IsReadOnly)
                {
                    var key = this.GetCacheKey("Clickable");
                    this.requestCache.SetValue<bool>(key, value);
                }
                else
                {
                    base.Clickable = value;
                }
            }
        }

        public override string UrlResolver
        {
            get
            {
                var key = this.GetCacheKey("UrlResolver");
                var result = this.requestCache.GetValue<string>(key);
                if (result == null)
                {
                    result = base.UrlResolver;
                }
                return result;
            }
            set
            {
                if (this.IsReadOnly)
                {
                    var key = this.GetCacheKey("UrlResolver");
                    this.requestCache.SetValue<string>(key, value);
                }
                else
                {
                    base.UrlResolver = value;
                }
            }
        }

        public override bool IsVisible(IDictionary<string, object> sourceMetadata)
        {
            var key = this.GetCacheKey("IsVisible");
            var result = this.requestCache.GetValue<bool?>(key);
            if (result == null)
            {
                result = base.IsVisible(sourceMetadata);
                this.requestCache.SetValue<bool>(key, (bool)result);
            }
            return (bool)result;
        }

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection()
        {
            var key = GetCacheKey("GetDynamicNodeCollection");
            var result = this.requestCache.GetValue<IEnumerable<DynamicNode>>(key);
            if (result == null)
            {
                result = base.GetDynamicNodeCollection();
                this.requestCache.SetValue<IEnumerable<DynamicNode>>(key, result);
            }
            return result;
        }

        public override string Url
        {
            get
            {
                var key = this.GetCacheKey("Url");
                var result = this.requestCache.GetValue<string>(key);
                if (result == null)
                {
                    result = base.Url;
                    this.requestCache.SetValue<string>(key, result);
                }
                return result;
            }
            set { base.Url = value; }
        }

        public override string CanonicalUrl
        {
            get
            {
                var key = this.GetCacheKey("CanonicalUrl");
                var result = this.requestCache.GetValue<string>(key);
                if (result == null)
                {
                    result = base.CanonicalUrl;
                }
                return result;
            }
            set
            {
                if (this.IsReadOnly)
                {
                    var key = this.GetCacheKey("CanonicalUrl");
                    this.requestCache.SetValue<string>(key, value);
                }
                else
                {
                    base.CanonicalUrl = value;
                }
            }
        }

        public override string CanonicalKey
        {
            get
            {
                var key = this.GetCacheKey("CanonicalKey");
                var result = this.requestCache.GetValue<string>(key);
                if (result == null)
                {
                    result = base.CanonicalKey;
                }
                return result;
            }
            set
            {
                if (this.IsReadOnly)
                {
                    var key = this.GetCacheKey("CanonicalKey");
                    this.requestCache.SetValue<string>(key, value);
                }
                else
                {
                    base.CanonicalKey = value;
                }
            }
        }

        public override string Route
        {
            get
            {
                var key = this.GetCacheKey("Route");
                var result = this.requestCache.GetValue<string>(key);
                if (result == null)
                {
                    result = base.Route;
                }
                return result;
            }
            set
            {
                if (this.IsReadOnly)
                {
                    var key = this.GetCacheKey("Route");
                    this.requestCache.SetValue<string>(key, value);
                }
                else
                {
                    base.Route = value;
                }
            }
        }

        #endregion

        #region Protected Methods

        protected virtual string GetCacheKey(string memberName)
        {
            return "__MVCSITEMAPNODE_" + memberName + "_" + this.Key + "_" + this.instanceId.ToString();
        }


        //protected virtual T GetCacheOrValue<T>(Func<T> property, string propertyName, bool storeAlso)
        //{
        //    var key = this.GetCacheKey(propertyName);
        //    var result = this.requestCache.GetValue<T>(key);
        //    if (result == null)
        //    {
        //        result = property.Invoke();
        //        if (storeAlso)
        //        {
        //            this.requestCache.SetValue<T>(key, result);
        //        }
        //    }
        //    return result;
        //}

        //protected virtual void SetCacheOrValue<T>(Action<T> property, string propertyName, T value)
        //{
        //    if (this.IsReadOnly)
        //    {
        //        var key = this.GetCacheKey(propertyName);
        //        this.requestCache.SetValue<T>(key, value);
        //    }
        //    else
        //    {
        //        property.Invoke(value);
        //    }
        //}

        #endregion
    }
}
