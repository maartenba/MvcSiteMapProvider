using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using MvcSiteMapProvider.Core.RequestCache;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RequestCacheableSiteMapNode
        : ISiteMapNode
    {
        public RequestCacheableSiteMapNode(
            ISiteMapNode siteMapNode,
            IRequestCache requestCache
            )
        {
            if (siteMapNode == null)
                throw new ArgumentNullException("siteMapNode");
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.innerSiteMapNode = siteMapNode;
            this.requestCache = requestCache;
        }

        private readonly ISiteMapNode innerSiteMapNode;
        private readonly IRequestCache requestCache;
        private readonly Guid instanceId = Guid.NewGuid();


        #region ISiteMapNode Members

        public string Key
        {
            get { return this.innerSiteMapNode.Key; }
        }

        public bool IsDynamic
        {
            get { return this.innerSiteMapNode.IsDynamic; }
        }

        public bool IsReadOnly
        {
            get { return this.innerSiteMapNode.IsReadOnly; }
        }

        public ISiteMapNode ParentNode
        {
            get { return this.innerSiteMapNode.ParentNode; }
            set { this.innerSiteMapNode.ParentNode = value; }
        }

        public ISiteMapNodeCollection ChildNodes
        {
            get { return this.innerSiteMapNode.ChildNodes; }
        }

        public bool IsDescendantOf(ISiteMapNode node)
        {
            return this.innerSiteMapNode.IsDescendantOf(node);
        }

        public ISiteMapNode NextSibling
        {
            get { return this.innerSiteMapNode.NextSibling; }
        }

        public ISiteMapNode PreviousSibling
        {
            get { return this.innerSiteMapNode.PreviousSibling; }
        }

        public ISiteMapNode RootNode
        {
            get { return this.innerSiteMapNode.RootNode; }
        }

        public bool IsInCurrentPath()
        {
            return this.innerSiteMapNode.IsInCurrentPath();
        }

        public bool HasChildNodes
        {
            get { return this.innerSiteMapNode.HasChildNodes; }
        }

        public int GetNodeLevel()
        {
            return this.innerSiteMapNode.GetNodeLevel();
        }

        public ISiteMap SiteMap
        {
            get { return this.innerSiteMapNode.SiteMap; }
        }

        public bool IsAccessibleToUser(HttpContext context)
        {
            return this.innerSiteMapNode.IsAccessibleToUser(context);
        }

        public string HttpMethod
        {
            get { return this.innerSiteMapNode.HttpMethod; }
            set { this.innerSiteMapNode.HttpMethod = value; }
        }

        public string ResourceKey
        {
            get { return this.innerSiteMapNode.ResourceKey; }
        }

        public string Title
        {
            get 
            {
                var key = this.GetCacheKey("Title");
                var result = this.requestCache.GetValue<string>(key);
                if (result == null)
                {
                    result = this.innerSiteMapNode.Title;
                    this.requestCache.SetValue<string>(key, result);
                }
                return result;
            }
            set { this.innerSiteMapNode.Title = value; }
        }

        public string Description
        {
            get
            {
                var key = this.GetCacheKey("Description");
                var result = this.requestCache.GetValue<string>(key);
                if (result == null)
                {
                    result = this.innerSiteMapNode.Description;
                    this.requestCache.SetValue<string>(key, result);
                }
                return result;
            }
            set { this.innerSiteMapNode.Description = value; }
        }

        public string TargetFrame
        {
            get { return this.innerSiteMapNode.TargetFrame; }
            set { this.innerSiteMapNode.TargetFrame = value; }
        }

        public string ImageUrl
        {
            get { return this.innerSiteMapNode.ImageUrl; }
            set { this.innerSiteMapNode.ImageUrl = value; }
        }

        public IAttributeCollection Attributes
        {
            get { return this.innerSiteMapNode.Attributes; }
        }

        public IList<string> Roles
        {
            get { return this.innerSiteMapNode.Roles; }
        }

        public DateTime LastModifiedDate
        {
            get { return this.innerSiteMapNode.LastModifiedDate; }
            set { this.innerSiteMapNode.LastModifiedDate = value; }
        }

        public ChangeFrequency ChangeFrequency
        {
            get { return this.innerSiteMapNode.ChangeFrequency; }
            set { this.innerSiteMapNode.ChangeFrequency = value; }
        }

        public UpdatePriority UpdatePriority
        {
            get { return this.innerSiteMapNode.UpdatePriority; }
            set { this.innerSiteMapNode.UpdatePriority = value; }
        }

        public string VisibilityProvider
        {
            get { return this.innerSiteMapNode.VisibilityProvider; }
            set { this.innerSiteMapNode.VisibilityProvider = value; }
        }

        public bool IsVisible(HttpContext context, IDictionary<string, object> sourceMetadata)
        {
            var key = this.GetCacheKey("IsVisible");
            var result = this.requestCache.GetValue<bool>(key);
            if (result == null)
            {
                result = this.innerSiteMapNode.IsVisible(context, sourceMetadata);
                this.requestCache.SetValue<bool>(key, result);
            }
            return result;
        }

        public bool Clickable
        {
            get { return this.innerSiteMapNode.Clickable; }
            set { this.innerSiteMapNode.Clickable = value; }
        }

        public string UrlResolver
        {
            get { return this.innerSiteMapNode.UrlResolver; }
            set { this.innerSiteMapNode.UrlResolver = value; }
        }

        public string Url
        {
            get
            {
                var key = this.GetCacheKey("Url");
                var result = this.requestCache.GetValue<string>(key);
                if (result == null)
                {
                    result = this.innerSiteMapNode.Url;
                    this.requestCache.SetValue<string>(key, result);
                }
                return result;
            }
            set { this.innerSiteMapNode.Url = value; }
        }

        public string UnresolvedUrl
        {
            get { return this.innerSiteMapNode.UnresolvedUrl; }
        }

        public string DynamicNodeProvider
        {
            get { return this.innerSiteMapNode.DynamicNodeProvider; }
            set { this.innerSiteMapNode.DynamicNodeProvider = value; }
        }

        public IEnumerable<DynamicNode> GetDynamicNodeCollection()
        {
            var key = GetCacheKey("GetDynamicNodeCollection");
            var result = this.requestCache.GetValue<IEnumerable<DynamicNode>>(key);
            if (result == null)
            {
                result = this.innerSiteMapNode.GetDynamicNodeCollection();
                this.requestCache.SetValue<IEnumerable<DynamicNode>>(key, result);
            }
            return result;
        }

        public bool HasDynamicNodeProvider
        {
            get { return this.innerSiteMapNode.HasDynamicNodeProvider; }
        }

        public string Route
        {
            get { return this.innerSiteMapNode.Route; }
            set { this.innerSiteMapNode.Route = value; }
        }

        public IRouteValueCollection RouteValues
        {
            get { return this.innerSiteMapNode.RouteValues; }
        }

        public IList<string> PreservedRouteParameters
        {
            get { return this.innerSiteMapNode.PreservedRouteParameters; }
        }

        public RouteData GetRouteData(HttpContextBase httpContext)
        {
            return this.innerSiteMapNode.GetRouteData(httpContext);
        }

        public bool MatchesRoute(IDictionary<string, object> routeValues)
        {
            return this.innerSiteMapNode.MatchesRoute(routeValues);
        }

        public string Area
        {
            get { return this.innerSiteMapNode.Area; }
            set { this.innerSiteMapNode.Area = value; }
        }

        public string Controller
        {
            get { return this.innerSiteMapNode.Controller; }
            set { this.innerSiteMapNode.Controller = value; }
        }

        public string Action
        {
            get { return this.innerSiteMapNode.Action; }
            set { this.innerSiteMapNode.Action = value; }
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
