using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider.Tests.Unit.Web.Html
{
    class FakeSiteMap
        : ISiteMap
    {
        public FakeSiteMap(
            bool securityTrimmingEnabled,
            bool visibilityAffectsDescendants
            )
        {
            this.securityTrimmingEnabled = securityTrimmingEnabled;
            this.visibilityAffectsDescendants = visibilityAffectsDescendants;
        }

        // Services
        protected readonly IUrlPath urlPath = new FakeUrlPath();

        // Child collections
        protected readonly IDictionary<ISiteMapNode, ISiteMapNodeCollection> childNodeCollectionTable = new Dictionary<ISiteMapNode, ISiteMapNodeCollection>();
        protected readonly IDictionary<string, ISiteMapNode> keyTable = new Dictionary<string, ISiteMapNode>();
        protected readonly IDictionary<ISiteMapNode, ISiteMapNode> parentNodeTable = new Dictionary<ISiteMapNode, ISiteMapNode>();
        protected readonly IDictionary<string, ISiteMapNode> urlTable = new Dictionary<string, ISiteMapNode>();

        // Object State
        protected readonly bool securityTrimmingEnabled;
        protected readonly bool visibilityAffectsDescendants;
        protected ISiteMapNode root;
        protected ISiteMapNode currentNode;

        #region ISiteMap Members

        public bool IsReadOnly
        {
            get { return true; }
        }

        public void AddNode(ISiteMapNode node)
        {
            this.AddNode(node, null);
        }

        public void AddNode(ISiteMapNode node, ISiteMapNode parentNode)
        {
            // Avoid issue with url table not clearing correctly.
            if (this.FindSiteMapNode(node.Url) != null)
            {
                this.RemoveNode(node);
            }

            // Add the node
            try
            {
                AddNodeInternal(node, parentNode);
            }
            catch
            {
                if (parentNode != null) this.RemoveNode(parentNode);
                AddNodeInternal(node, parentNode);
            }
        }

        private void AddNodeInternal(ISiteMapNode node, ISiteMapNode parentNode)
        {
            if (keyTable.Count == 0 && urlTable.Count == 0 && parentNodeTable.Count == 0 && childNodeCollectionTable.Count == 0)
            {
                // If this is the first node added, assume it is the root node
                root = node;
            }
            string url = node.Url;
            string key = node.Key;
            if (this.keyTable.ContainsKey(key))
            {
                throw new InvalidOperationException(string.Format("Multiple nodes with identical key: {0}", key));
            }
            this.keyTable[key] = node;
            this.urlTable[url] = node;

            if (parentNode != null)
            {
                this.parentNodeTable[node] = parentNode;
                if (!this.childNodeCollectionTable.ContainsKey(parentNode))
                {
                    this.childNodeCollectionTable[parentNode] = new FakeSiteMapNodeCollection();
                }
                this.childNodeCollectionTable[parentNode].Add(node);
            }
        }

        public void RemoveNode(ISiteMapNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            ISiteMapNode parentNode = null;
            if (this.parentNodeTable.ContainsKey(node))
            {
                parentNode = this.parentNodeTable[node];
                this.parentNodeTable.Remove(node);
            }
            if (parentNode != null)
            {
                var nodes = this.childNodeCollectionTable[parentNode];
                if ((nodes != null) && nodes.Contains(node))
                {
                    nodes.Remove(node);
                }
            }
            string url = node.Url;
            if (((url != null) && (url.Length > 0)) && this.urlTable.ContainsKey(url))
            {
                this.urlTable.Remove(url);
            }
            string key = node.Key;
            if (this.keyTable.ContainsKey(key))
            {
                this.keyTable.Remove(key);
            }
        }

        public void Clear()
        {
            root = null;
            this.childNodeCollectionTable.Clear();
            this.urlTable.Clear();
            this.parentNodeTable.Clear();
            this.keyTable.Clear();
        }

        public ISiteMapNode RootNode
        {
            get { return this.ReturnNodeIfAccessible(root); }
        }

        public void BuildSiteMap()
        {
            throw new NotImplementedException();
        }

        public void SetCurrentNode(ISiteMapNode node)
        {
            this.currentNode = node;
        }

        public ISiteMapNode CurrentNode
        {
            get { return this.currentNode; }
        }

        public bool EnableLocalization
        {
            get { return false; }
        }

        public ISiteMapNode FindSiteMapNode(string rawUrl)
        {
            if (rawUrl == null)
            {
                throw new ArgumentNullException("rawUrl");
            }
            rawUrl = rawUrl.Trim();
            if (rawUrl.Length == 0)
            {
                return null;
            }
            if (urlPath.IsAppRelativePath(rawUrl))
            {
                rawUrl = urlPath.MakeVirtualPathAppAbsolute(rawUrl, "/");
            }
            if (this.urlTable.ContainsKey(rawUrl))
            {
                return this.ReturnNodeIfAccessible(this.urlTable[rawUrl]);
            }
            return null;
        }

        public ISiteMapNode FindSiteMapNodeFromCurrentContext()
        {
            throw new NotImplementedException();
        }

        public ISiteMapNode FindSiteMapNode(ControllerContext context)
        {
            throw new NotImplementedException();
        }

        public ISiteMapNode FindSiteMapNodeFromKey(string key)
        {
            ISiteMapNode node = this.FindSiteMapNode(key);
            if (node == null && this.keyTable.ContainsKey(key))
            {
                node = this.keyTable[key];
            }
            return this.ReturnNodeIfAccessible(node);
        }

        public ISiteMapNodeCollection GetChildNodes(ISiteMapNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            ISiteMapNodeCollection collection = null;
            if (this.childNodeCollectionTable.ContainsKey(node))
            {
                collection = this.childNodeCollectionTable[node];
            }
            if (collection == null)
            {
                ISiteMapNode keyNode = null;
                if (this.keyTable.ContainsKey(node.Key))
                {
                    keyNode = this.keyTable[node.Key];
                }
                if (keyNode != null && this.childNodeCollectionTable.ContainsKey(keyNode))
                {
                    collection = this.childNodeCollectionTable[keyNode];
                }
            }
            if (collection == null)
            {
                return new FakeSiteMapNodeCollection();
            }
            if (!this.SecurityTrimmingEnabled)
            {
                return new FakeSiteMapNodeCollection(collection);
            }
            var secureCollection = new FakeSiteMapNodeCollection();
            foreach (ISiteMapNode secureNode in collection)
            {
                if (secureNode.IsAccessibleToUser())
                {
                    secureCollection.Add(secureNode);
                }
            }
            return secureCollection;
        }

        public ISiteMapNodeCollection GetDescendants(ISiteMapNode node)
        {
            throw new NotImplementedException();
        }

        public ISiteMapNodeCollection GetAncestors(ISiteMapNode node)
        {
            throw new NotImplementedException();
        }

        public ISiteMapNode GetCurrentNodeAndHintAncestorNodes(int upLevel)
        {
            throw new NotImplementedException();
        }

        public ISiteMapNode GetCurrentNodeAndHintNeighborhoodNodes(int upLevel, int downLevel)
        {
            throw new NotImplementedException();
        }

        public ISiteMapNode GetParentNode(ISiteMapNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            ISiteMapNode parentNode = null;
            if (this.parentNodeTable.ContainsKey(node))
            {
                parentNode = this.parentNodeTable[node];
            }
            if (parentNode == null)
            {
                ISiteMapNode keyNode = null;
                if (this.keyTable.ContainsKey(node.Key))
                {
                    keyNode = this.keyTable[node.Key];
                }
                if (keyNode != null)
                {
                    if (this.parentNodeTable.ContainsKey(keyNode))
                    {
                        parentNode = this.parentNodeTable[keyNode];
                    }
                }
            }
            return this.ReturnNodeIfAccessible(parentNode);
        }

        public ISiteMapNode GetParentNodeRelativeToCurrentNodeAndHintDownFromParent(int walkupLevels, int relativeDepthFromWalkup)
        {
            throw new NotImplementedException();
        }

        public ISiteMapNode GetParentNodeRelativeToNodeAndHintDownFromParent(ISiteMapNode node, int walkupLevels, int relativeDepthFromWalkup)
        {
            throw new NotImplementedException();
        }

        public void HintAncestorNodes(ISiteMapNode node, int upLevel)
        {
            throw new NotImplementedException();
        }

        public void HintNeighborhoodNodes(ISiteMapNode node, int upLevel, int downLevel)
        {
            throw new NotImplementedException();
        }

        public bool IsAccessibleToUser(ISiteMapNode node)
        {
            if (!SecurityTrimmingEnabled)
            {
                return true;
            }
            return node.IsAccessibleToUser();
        }

        public string ResourceKey { get; set; }

        public string CacheKey
        {
            get { throw new NotImplementedException(); }
        }
        
        public bool SecurityTrimmingEnabled
        {
            get { return this.securityTrimmingEnabled; }
        }

        public Type ResolveControllerType(string areaName, string controllerName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> ResolveActionMethodParameters(string areaName, string controllerName, string actionMethodName)
        {
            throw new NotImplementedException();
        }

        public bool UseTitleIfDescriptionNotProvided
        {
            get { throw new NotImplementedException(); }
        }

        public bool VisibilityAffectsDescendants
        {
            get { return this.visibilityAffectsDescendants; }
        }

        #endregion

        protected ISiteMapNode ReturnNodeIfAccessible(ISiteMapNode node)
        {
            if ((node != null) && node.IsAccessibleToUser())
            {
                return node;
            }
            return null;
        }
    }

    class FakeSiteMapNodeCollection
        : List<ISiteMapNode>, ISiteMapNodeCollection
    {
        public FakeSiteMapNodeCollection()
        {
        }

        public FakeSiteMapNodeCollection(IEnumerable<ISiteMapNode> collection)
        {
            this.AddRange(collection);
        }
    }

    class FakeUrlPath
        : IUrlPath
    {

        #region IUrlPath Members

        public string AppDomainAppVirtualPath
        {
            get { throw new NotImplementedException(); }
        }

        public string CombineUrl(params string[] uriParts)
        {
            throw new NotImplementedException();
        }

        public string Combine(string basepath, string relative)
        {
            throw new NotImplementedException();
        }

        public bool IsAbsolutePhysicalPath(string path)
        {
            throw new NotImplementedException();
        }

        public bool IsAppRelativePath(string path)
        {
            if (path == null)
            {
                return false;
            }
            int length = path.Length;
            if (length == 0)
            {
                return false;
            }
            if (path[0] != '~')
            {
                return false;
            }
            if ((length != 1) && (path[1] != '\\'))
            {
                return (path[1] == '/');
            }
            return true;
        }

        public bool IsRooted(string basepath)
        {
            if (!string.IsNullOrEmpty(basepath) && (basepath[0] != '/'))
            {
                return (basepath[0] == '\\');
            }
            return true;
        }

        public string MakeVirtualPathAppAbsolute(string virtualPath)
        {
            throw new NotImplementedException();
        }

        public string MakeVirtualPathAppAbsolute(string virtualPath, string applicationPath)
        {
            if ((virtualPath.Length == 1) && (virtualPath[0] == '~'))
            {
                return applicationPath;
            }
            if (((virtualPath.Length >= 2) && (virtualPath[0] == '~')) && ((virtualPath[1] == '/') || (virtualPath[1] == '\\')))
            {
                if (applicationPath.Length > 1)
                {
                    return (applicationPath + virtualPath.Substring(2));
                }
                return ("/" + virtualPath.Substring(2));
            }
            if (!IsRooted(virtualPath))
            {
                throw new ArgumentOutOfRangeException("virtualPath");
            }
            return virtualPath;
        }

        public string UrlEncode(string url)
        {
            throw new NotImplementedException();
        }

        public string UrlDecode(string url)
        {
            throw new NotImplementedException();
        }

        public bool IsAbsoluteUrl(string url)
        {
            throw new NotImplementedException();
        }

        public bool IsExternalUrl(string url, HttpContextBase httpContext)
        {
            throw new NotImplementedException();
        }

        public bool IsPublicHostName(string hostName, HttpContextBase httpContext)
        {
            throw new NotImplementedException();
        }

        public string MakeUrlAbsolute(string url)
        {
            throw new NotImplementedException();
        }

        public string MakeUrlAbsolute(string url, string baseUrl)
        {
            throw new NotImplementedException();
        }

        public string ResolveVirtualApplicationToRootRelativeUrl(string url)
        {
            throw new NotImplementedException();
        }

        public string ResolveUrl(string url)
        {
            throw new NotImplementedException();
        }

        public string ResolveUrl(string url, string protocol)
        {
            throw new NotImplementedException();
        }

        public string ResolveUrl(string url, string protocol, string hostName)
        {
            throw new NotImplementedException();
        }

        public string ResolveUrl(string url, string protocol, string hostName, HttpContextBase httpContext)
        {
            throw new NotImplementedException();
        }

        public string ResolveContentUrl(string url)
        {
            throw new NotImplementedException();
        }

        public string ResolveContentUrl(string url, string protocol)
        {
            throw new NotImplementedException();
        }

        public string ResolveContentUrl(string url, string protocol, string hostName)
        {
            throw new NotImplementedException();
        }

        public string ResolveContentUrl(string url, string protocol, string hostName, HttpContextBase httpContext)
        {
            throw new NotImplementedException();
        }

        public Uri GetPublicFacingUrl(HttpContextBase httpContext)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public string MakeRelativeUrlAbsolute(string url)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public string ResolveServerUrl(string serverUrl)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
