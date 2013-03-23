﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Provides overrides of the <see cref="T:MvcSiteMapProvider.SiteMap"/> that track the return values of specific 
    /// resource-intensive members in case they are accessed more than one time during a single request.
    /// </summary>
    public class RequestCacheableSiteMap
        : LockableSiteMap
    {
        public RequestCacheableSiteMap(
            ISiteMapPluginProvider pluginProvider,
            IMvcContextFactory mvcContextFactory,
            ISiteMapChildStateFactory siteMapChildStateFactory,
            IUrlPath urlPath,
            IRequestCache requestCache
            )
            : base(pluginProvider, mvcContextFactory, siteMapChildStateFactory, urlPath)
        {
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.requestCache = requestCache;
        }

        private readonly IRequestCache requestCache;
        private readonly Guid instanceId = Guid.NewGuid();

        #region Request Cacheable Members

        public override ISiteMapNode FindSiteMapNode(string rawUrl)
        {
            var key = this.GetCacheKey("FindSiteMapNode_" + rawUrl);
            var result = this.requestCache.GetValue<ISiteMapNode>(key);
            if (result == null)
            {
                result = base.FindSiteMapNode(rawUrl);
                if (result != null)
                {
                    this.requestCache.SetValue<ISiteMapNode>(key, result);
                }
            }
            return result;
        }

        public override ISiteMapNode FindSiteMapNodeFromCurrentContext()
        {
            var key = this.GetCacheKey("FindSiteMapNodeFromCurrentContext");
            var result = this.requestCache.GetValue<ISiteMapNode>(key);
            if (result == null)
            {
                result = base.FindSiteMapNodeFromCurrentContext();
                if (result != null)
                {
                    this.requestCache.SetValue<ISiteMapNode>(key, result);
                }
            }
            return result;
        }

        public override ISiteMapNode FindSiteMapNode(ControllerContext context)
        {
            var key = this.GetCacheKey("FindSiteMapNode_ControllerContext" + this.GetDictionaryKey(context.RouteData.Values));
            var result = this.requestCache.GetValue<ISiteMapNode>(key);
            if (result == null)
            {
                result = base.FindSiteMapNode(context);
                if (result != null)
                {
                    this.requestCache.SetValue<ISiteMapNode>(key, result);
                }
            }
            return result;
        }

        public override bool IsAccessibleToUser(ISiteMapNode node)
        {
            var key = this.GetCacheKey("IsAccessibleToUser_" + node.Key);
            var result = this.requestCache.GetValue<bool?>(key);
            if (result == null)
            {
                result = base.IsAccessibleToUser(node);
                this.requestCache.SetValue<bool>(key, (bool)result);
            }
            return (bool)result;
        }

        #endregion

        #region Protected Members

        protected virtual string GetCacheKey(string memberName)
        {
            return "__MVCSITEMAP_" + memberName + "_" + this.instanceId.ToString();
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

        #endregion

    }
}
