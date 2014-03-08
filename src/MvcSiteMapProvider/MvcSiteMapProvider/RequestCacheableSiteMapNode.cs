using System;
using System.Text;
using System.Collections.Generic;
using MvcSiteMapProvider.Caching;
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
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");

            this.requestCache = mvcContextFactory.GetRequestCache();
        }

        private readonly IRequestCache requestCache;

        #region ISiteMapNode Members

        public override int Order
        {
            get { return (int)this.GetCachedOrMemberValue<int?>(() => base.Order, "Order", false); }
            set { this.SetCachedOrMemberValue<int>(x => base.Order = x, "Order", value); }
        }

        public override string Title
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.Title, "Title", true); }
            set { this.SetCachedOrMemberValue<string>(x => base.Title = x, "Title", value); }
        }

        public override string Description
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.Description, "Description", true); }
            set { this.SetCachedOrMemberValue<string>(x => base.Description = x, "Description", value); }
        }

        public override string TargetFrame
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.TargetFrame, "TargetFrame", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.TargetFrame = x, "TargetFrame", value); }
        }

        public override string ImageUrl
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.ImageUrl, "ImageUrl", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.ImageUrl = x, "ImageUrl", value); }
        }

        public override string ImageUrlProtocol
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.ImageUrlProtocol, "ImageUrlProtocol", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.ImageUrlProtocol = x, "ImageUrlProtocol", value); }
        }

        public override string ImageUrlHostName
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.ImageUrlHostName, "ImageUrlHostName", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.ImageUrlHostName = x, "ImageUrlHostName", value); }
        }

        public override string VisibilityProvider
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.VisibilityProvider, "VisibilityProvider", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.VisibilityProvider = x, "VisibilityProvider", value); }
        }

        public override bool Clickable
        {
            get { return (bool)this.GetCachedOrMemberValue<bool?>(() => base.Clickable, "Clickable", false); }
            set { this.SetCachedOrMemberValue<bool>(x => base.Clickable = x, "Clickable", value); }
        }

        public override string UrlResolver
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.UrlResolver, "UrlResolver", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.UrlResolver = x, "UrlResolver", value); }
        }

        public override bool IsVisible(IDictionary<string, object> sourceMetadata)
        {
            var key = this.GetCacheKey("IsVisible" + this.GetDictionaryKey(sourceMetadata));
            var result = this.requestCache.GetValue<bool?>(key);
            if (result == null)
            {
                result = base.IsVisible(sourceMetadata);
                this.requestCache.SetValue<bool>(key, (bool)result);
            }
            return (bool)result;
        }

        public override string Url
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.Url, "Url", true); }
            set { base.Url = value; }
        }

        public override string Protocol
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.Protocol, "Protocol", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.Protocol = x, "Protocol", value); }
        }

        public override string HostName
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.HostName, "HostName", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.HostName = x, "HostName", value); }
        }

        public override string CanonicalKey
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.CanonicalKey, "CanonicalKey", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.CanonicalKey = x, "CanonicalKey", value); }
        }

        public override string CanonicalUrl
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.CanonicalUrl, "CanonicalUrl", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.CanonicalUrl = x, "CanonicalUrl", value); }
        }

        public override string CanonicalUrlProtocol
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.CanonicalUrlProtocol, "CanonicalUrlProtocol", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.CanonicalUrlProtocol = x, "CanonicalUrlProtocol", value); }
        }

        public override string CanonicalUrlHostName
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.CanonicalUrlHostName, "CanonicalUrlHostName", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.CanonicalUrlHostName = x, "CanonicalUrlHostName", value); }
        }

        public override string Route
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.Route, "Route", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.Route = x, "Route", value); }
        }

        protected override bool AreRouteParametersPreserved
        {
            get 
            {
                var key = this.GetCacheKey("AreRouteParametersPreserved");
                var result = this.requestCache.GetValue<bool?>(key);
                if (result == null)
                {
                    result = false;
                }
                return (bool)result;
            }
            set 
            {
                var key = this.GetCacheKey("AreRouteParametersPreserved");
                this.requestCache.SetValue<bool>(key, value);
            }
        }

        #endregion

        #region Protected Members

        protected string GetCacheKey(string memberName)
        {
            // NOTE: We must include IsReadOnly in the request cache key because we may have a different 
            // result when the sitemap is being constructed than when it is being read by the presentation layer.
            return "__MVCSITEMAPNODE_" + this.SiteMap.CacheKey + "_" + this.Key + "_" + memberName + "_" + this.IsReadOnly.ToString() + "_";
        }

        protected virtual string GetDictionaryKey(IDictionary<string, object> dictionary)
        {
            var builder = new StringBuilder();
            foreach (var pair in dictionary)
            {
                builder.Append(pair.Key);
                builder.Append("_");
                builder.Append(GetStringFromValue(pair.Value));
                builder.Append("|");
            }
            return builder.ToString();
        }

        protected virtual string GetStringFromValue(object value)
        {
            if (value.GetType().Equals(typeof(string)))
            {
                return value.ToString();
            }
            return value.GetHashCode().ToString();
        }

        protected virtual T GetCachedOrMemberValue<T>(Func<T> member, string memberName, bool storeInCache)
        {
            var key = this.GetCacheKey(memberName);
            var result = this.requestCache.GetValue<T>(key);
            if (result == null)
            {
                result = member.Invoke();
                if (storeInCache)
                {
                    this.requestCache.SetValue<T>(key, result);
                }
            }
            return result;
        }

        protected virtual void SetCachedOrMemberValue<T>(Action<T> member, string memberName, T value)
        {
            if (this.IsReadOnly)
            {
                var key = this.GetCacheKey(memberName);
                this.requestCache.SetValue<T>(key, value);
            }
            else
            {
                member(value);
            }
        }

        #endregion
    }
}
