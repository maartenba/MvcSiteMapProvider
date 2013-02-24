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
            ISiteMapNodePluginProvider pluginProvider,
            IUrlPath urlPath,
            RouteCollection routes,
            IRequestCache requestCache
            )
            : base(
                siteMap, 
                key, 
                isDynamic, 
                siteMapNodeChildStateFactory, 
                localizationService, 
                pluginProvider,
                urlPath,
                routes
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
            return this.GetCachedOrMemberValue<IEnumerable<DynamicNode>>(() => base.GetDynamicNodeCollection(), "GetDynamicNodeCollection", true);
        }

        public override string Url
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.Url, "Url", true); }
            set { base.Url = value; }
        }

        public override string CanonicalUrl
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.CanonicalUrl, "CanonicalUrl", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.CanonicalUrl = x, "CanonicalUrl", value); }
        }

        public override string CanonicalKey
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.CanonicalKey, "CanonicalKey", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.CanonicalKey = x, "CanonicalKey", value); }
        }

        public override string Route
        {
            get { return this.GetCachedOrMemberValue<string>(() => base.Route, "Route", false); }
            set { this.SetCachedOrMemberValue<string>(x => base.Route = x, "Route", value); }
        }

        #endregion

        #region Protected Members

        protected string GetCacheKey(string memberName)
        {
            return "__MVCSITEMAPNODE_" + memberName + "_" + this.Key + "_" + this.instanceId.ToString();
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
