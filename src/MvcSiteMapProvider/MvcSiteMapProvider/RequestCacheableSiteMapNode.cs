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
    /// TODO: Update summary.
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
            set { base.Title = value; }
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
            set { base.Description = value; }
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

        #endregion

        #region Private Methods

        private string GetCacheKey(string memberName)
        {
            return "__MVCSITEMAPNODE_" + memberName + "_" + this.Key + "_" + this.instanceId.ToString();
        }

        #endregion
    }
}
