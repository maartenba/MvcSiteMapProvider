using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using MvcSiteMapProvider.Core.RequestCache;
using MvcSiteMapProvider.Core.SiteMap.Builder;
using MvcSiteMapProvider.Core.Security;
using MvcSiteMapProvider.Core.Collections;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RequestCacheableSiteMap
        : LockableSiteMap
    {
        public RequestCacheableSiteMap(
            ISiteMapBuilder siteMapBuilder,
            IAclModule aclModule,
            ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory,
            IGenericDictionaryFactory genericDictionaryFactory,
            IRequestCache requestCache
            )
            : base(siteMapBuilder, aclModule, siteMapNodeCollectionFactory, genericDictionaryFactory)
        {
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.requestCache = requestCache;
        }

        private readonly IRequestCache requestCache;
        private readonly Guid instanceId = Guid.NewGuid();

        #region Request Cacheable Members

        public ISiteMapNode FindSiteMapNode(string rawUrl)
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

        public ISiteMapNode FindSiteMapNode(HttpContext context)
        {
            var key = this.GetCacheKey("FindSiteMapNode_HttpContext");
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

        public ISiteMapNode FindSiteMapNode(ControllerContext context)
        {
            var key = this.GetCacheKey("FindSiteMapNode_ControllerContext" + this.GetRouteDataValues(context.RouteData));
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

        public bool IsAccessibleToUser(HttpContext context, ISiteMapNode node)
        {
            var key = this.GetCacheKey("IsAccessibleToUser_" + node.Key);
            var result = this.requestCache.GetValue<bool?>(key);
            if (result == null)
            {
                result = base.IsAccessibleToUser(context, node);
                this.requestCache.SetValue<bool>(key, (bool)result);
            }
            return (bool)result;
        }

        #endregion

        #region Private Methods

        private string GetCacheKey(string memberName)
        {
            return "__MVCSITEMAP_" + memberName + "_" + this.instanceId.ToString();
        }

        private string GetRouteDataValues(RouteData routeData)
        {
            var builder = new StringBuilder();
            foreach (var value in routeData.Values)
            {
                builder.Append(value.Key);
                builder.Append("_");
                builder.Append(value.Value);
                builder.Append("|");
            }
            return builder.ToString();
        }

        #endregion

    }
}
