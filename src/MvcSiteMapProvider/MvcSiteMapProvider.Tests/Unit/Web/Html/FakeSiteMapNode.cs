using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Xml.Linq;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider.Tests.Unit.Web.Html
{
    class FakeSiteMapNode
        : ISiteMapNode
    {
        public FakeSiteMapNode(
            ISiteMap siteMap, 
            string key,
            string title,
            bool isDynamic,
            bool isAccessibleToUser,
            bool isVisible,
            bool isClickable,
            string url,
            string metaRobotsContentString
            )
        {
            if (siteMap == null)
                throw new ArgumentNullException("siteMap");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");
            
            this.siteMap = siteMap;
            this.key = key;
            this.Title = title;
            this.isDynamic = isDynamic;
            this.isAccessibleToUser = isAccessibleToUser;
            this.isVisible = isVisible;
            this.Clickable = isClickable;
            this.Url = url;
            this.metaRobotsContentString = metaRobotsContentString;
        }

        // Child collections and dictionaries
        protected readonly IAttributeDictionary attributes = new FakeAttributeDictionary();
        protected readonly IRouteValueDictionary routeValues = new FakeRouteValueDictionary();

        // Object State
        protected readonly ISiteMap siteMap;
        protected readonly string key;
        protected readonly bool isDynamic;
        protected readonly bool isAccessibleToUser;
        protected readonly bool isVisible;
        protected readonly string metaRobotsContentString;

        #region ISiteMapNode Members

        public string Key
        {
            get { return this.key; }
        }

        public bool IsDynamic
        {
            get { return this.isDynamic; }
        }

        public bool IsReadOnly
        {
            get { return siteMap.IsReadOnly; }
        }

        public ISiteMapNode ParentNode
        {
            get { return this.siteMap.GetParentNode(this); }
        }

        public ISiteMapNodeCollection ChildNodes
        {
            get { return this.siteMap.GetChildNodes(this); }
        }

        public ISiteMapNodeCollection Descendants
        {
            get { return this.siteMap.GetDescendants(this); }
        }

        public ISiteMapNodeCollection Ancestors
        {
            get { return this.siteMap.GetAncestors(this); }
        }

        public bool IsDescendantOf(ISiteMapNode node)
        {
            for (var node2 = this.ParentNode; node2 != null; node2 = node2.ParentNode)
            {
                if (node2.Equals(node))
                {
                    return true;
                }
            }
            return false;
        }

        public ISiteMapNode NextSibling
        {
            get { throw new NotImplementedException(); }
        }

        public ISiteMapNode PreviousSibling
        {
            get { throw new NotImplementedException(); }
        }

        public ISiteMapNode RootNode
        {
            get { return this.siteMap.RootNode; }
        }

        public bool IsInCurrentPath()
        {
            ISiteMapNode node = this;
            return (this.SiteMap.CurrentNode != null && (node == this.SiteMap.CurrentNode || this.SiteMap.CurrentNode.IsDescendantOf(node)));
        }

        public bool HasChildNodes
        {
            get 
            {
                var childNodes = this.ChildNodes;
                return ((childNodes != null) && (childNodes.Count > 0));
            }
        }

        public int GetNodeLevel()
        {
            var level = 0;
            ISiteMapNode node = this;

            if (node != null)
            {
                while (node.ParentNode != null)
                {
                    level++;
                    node = node.ParentNode;
                }
            }
            return level;
        }

        public ISiteMap SiteMap
        {
            get { return this.siteMap; }
        }

        public int Order { get; set; }

        public bool IsAccessibleToUser()
        {
            if (this.siteMap.SecurityTrimmingEnabled)
                return this.isAccessibleToUser;
            return true;
        }

        public string HttpMethod { get; set; }

        public string ResourceKey
        {
            get { throw new NotImplementedException(); }
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TargetFrame { get; set; }

        public string ImageUrl { get; set; }

        public string ImageUrlProtocol { get; set; }

        public string ImageUrlHostName { get; set; }

        public IAttributeDictionary Attributes
        {
            get { return this.attributes; }
        }

        public IRoleCollection Roles
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime LastModifiedDate { get; set; }

        public ChangeFrequency ChangeFrequency { get; set; }

        public UpdatePriority UpdatePriority { get; set; }

        public string VisibilityProvider { get; set; }

        public bool IsVisible(IDictionary<string, object> sourceMetadata)
        {
            return this.isVisible;
        }

        public string DynamicNodeProvider { get; set; }
       
        public IEnumerable<DynamicNode> GetDynamicNodeCollection()
        {
            throw new NotImplementedException();
        }

        public bool HasDynamicNodeProvider
        {
            get { throw new NotImplementedException(); }
        }

        public bool Clickable { get; set; }

        public string UrlResolver { get; set; }

        public string Url { get; set; }

        public string UnresolvedUrl
        {
            get { throw new NotImplementedException(); }
        }

        public string ResolvedUrl
        {
            get { throw new NotImplementedException(); }
        }

        public bool CacheResolvedUrl
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void ResolveUrl()
        {
            throw new NotImplementedException();
        }

        public bool IncludeAmbientValuesInUrl { get; set; }

        public string Protocol { get; set; }

        public string HostName { get; set; }

        public bool HasAbsoluteUrl()
        {
            throw new NotImplementedException();
        }

        public bool HasExternalUrl(HttpContextBase httpContext)
        {
            throw new NotImplementedException();
        }

        public string CanonicalKey { get; set; }

        public string CanonicalUrl { get; set; }

        public string CanonicalUrlProtocol { get; set; }

        public string CanonicalUrlHostName { get; set; }

        public IMetaRobotsValueCollection MetaRobotsValues
        {
            get { throw new NotImplementedException(); }
        }

        public string GetMetaRobotsContentString()
        {
            return this.metaRobotsContentString;
        }

        public bool HasNoIndexAndNoFollow
        {
            get { throw new NotImplementedException(); }
        }

        public string Route { get; set; }

        public IRouteValueDictionary RouteValues
        {
            get { return this.routeValues; }
        }

        public IPreservedRouteParameterCollection PreservedRouteParameters
        {
            get { throw new NotImplementedException(); }
        }

        public RouteData GetRouteData(HttpContextBase httpContext)
        {
            throw new NotImplementedException();
        }

        public bool MatchesRoute(IDictionary<string, object> routeValues)
        {
            throw new NotImplementedException();
        }

        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public void CopyTo(ISiteMapNode node)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ISiteMapNode node)
        {
            if (base.Equals(node))
            {
                return true;
            }

            return this.Key.Equals(node.Key);
        }

        #endregion
    }

    class FakeAttributeDictionary
        : Dictionary<string, object>, IAttributeDictionary
    {

        #region IAttributeDictionary Members

        public void Add(string key, object value, bool throwIfReservedKey)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<string, object> item, bool throwIfReservedKey)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IDictionary<string, object> items)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IDictionary<string, object> items, bool throwIfReservedKey)
        {
            throw new NotImplementedException();
        }

        public void AddRange(string jsonString)
        {
            throw new NotImplementedException();
        }

        public void AddRange(string jsonString, bool throwIfReservedKey)
        {
            throw new NotImplementedException();
        }

        public void AddRange(XElement xmlNode)
        {
            throw new NotImplementedException();
        }

        public void AddRange(XElement xmlNode, bool throwIfReservedKey)
        {
            throw new NotImplementedException();
        }

        public void AddRange(NameValueCollection nameValueCollection)
        {
            throw new NotImplementedException();
        }

        public void AddRange(NameValueCollection nameValueCollection, bool throwIfReservedKey)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IDictionary<string, object> destination)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    class FakeRouteValueDictionary
        : Dictionary<string, object>, IRouteValueDictionary
    {
        #region IRouteValueDictionary Members

        public void Add(string key, object value, bool throwIfReservedKey)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<string, object> item, bool throwIfReservedKey)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IDictionary<string, object> items)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IDictionary<string, object> items, bool throwIfReservedKey)
        {
            throw new NotImplementedException();
        }

        public void AddRange(string jsonString)
        {
            throw new NotImplementedException();
        }

        public void AddRange(string jsonString, bool throwIfReservedKey)
        {
            throw new NotImplementedException();
        }

        public void AddRange(XElement xmlNode)
        {
            throw new NotImplementedException();
        }

        public void AddRange(XElement xmlNode, bool throwIfReservedKey)
        {
            throw new NotImplementedException();
        }

        public void AddRange(NameValueCollection nameValueCollection)
        {
            throw new NotImplementedException();
        }

        public void AddRange(NameValueCollection nameValueCollection, bool throwIfReservedKey)
        {
            throw new NotImplementedException();
        }

        public bool ContainsCustomKeys
        {
            get { throw new NotImplementedException(); }
        }

        public bool MatchesRoute(IEnumerable<string> actionParameters, IDictionary<string, object> routeValues)
        {
            throw new NotImplementedException();
        }

        public bool MatchesRoute(IDictionary<string, object> routeValues)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IDictionary<string, object> destination)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
