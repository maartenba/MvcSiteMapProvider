using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Reflection;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// AspNetSiteMapNodeProvider class. Builds a <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeToParentRelation"/> list based on a 
    /// <see cref="T:System.Web.SiteMapProvider"/> instance.
    /// </summary>
    /// <remarks>
    /// Use this class for interoperability with ASP.NET. To get a sitemap instance, you will need
    /// to configure a Web.sitemap XML file using the ASP.NET classic schema, then configure it for use in the
    /// sitemap/providers section of the Web.config file. Consult MSDN for information on how to do this.
    /// 
    /// The sitemap provider can be retrieved from ASP.NET for injection into this class using 
    /// an implementation of IAspNetSiteMapProvider. You may implement this interface to provide custom
    /// logic for retrieving a provider by name or other means by using 
    /// System.Web.SiteMap.Providers[providerName] or for the default provider System.Web.SiteMap.Provider.
    /// 
    /// We have provided the <see cref="T:MvcSiteMapProvider.Builder.AspNetDefaultSiteMapProvider"/> and 
    /// <see cref="T:MvcSiteMapProvider.Builder.AspNetNamedSiteMapProvider"/> that you can use as well.
    /// 
    /// Attributes and route values are obtained from a protected member variable of the 
    /// System.Web.SiteMapProvider named _attributes using reflection. You may disable this functionality for 
    /// performance reasons if the data is not required by setting reflectAttributes and/or reflectRouteValues to false.
    /// </remarks>
    public class AspNetSiteMapNodeProvider
        : ISiteMapNodeProvider
    {
        public AspNetSiteMapNodeProvider(
            bool includeRootNode,
            bool reflectAttributes,
            bool reflectRouteValues,
            IAspNetSiteMapProvider siteMapProvider
            )
        {
            if (siteMapProvider == null)
                throw new ArgumentNullException("siteMapProvider");

            this.includeRootNode = includeRootNode;
            this.reflectAttributes = reflectAttributes;
            this.reflectRouteValues = reflectRouteValues;
            this.siteMapProvider = siteMapProvider;
        }
        protected readonly bool includeRootNode;
        protected readonly bool reflectAttributes;
        protected readonly bool reflectRouteValues;
        protected readonly IAspNetSiteMapProvider siteMapProvider;
        protected const string SourceName = "ASP.NET SiteMap Provider";

        #region ISiteMapNodeProvider Members

        public IEnumerable<ISiteMapNodeToParentRelation> GetSiteMapNodes(ISiteMapNodeHelper helper)
        {
            var result = new List<ISiteMapNodeToParentRelation>();
            var provider = siteMapProvider.GetProvider();

            var rootNode = GetRootNode(provider, helper);
            if (this.includeRootNode)
            {
                result.Add(rootNode);
            }

            result.AddRange(ProcessNodes(rootNode, provider.RootNode, helper));

            return result;
        }

        #endregion

        protected virtual ISiteMapNodeToParentRelation GetRootNode(SiteMapProvider provider, ISiteMapNodeHelper helper)
        {
            var root = provider.RootNode;
            return helper.CreateNode(root.Key, null, SourceName, root.ResourceKey);
        }

        protected virtual IEnumerable<ISiteMapNodeToParentRelation> ProcessNodes(ISiteMapNodeToParentRelation parentNode, System.Web.SiteMapNode providerParentNode, ISiteMapNodeHelper helper)
        {
            var result = new List<ISiteMapNodeToParentRelation>();

            foreach (System.Web.SiteMapNode childNode in providerParentNode.ChildNodes)
            {
                var node = GetSiteMapNodeFromProviderNode(childNode, parentNode.Node, helper);

                result.Add(node);

                // Continue recursively processing
                ProcessNodes(node, childNode, helper);
            }
            return result;
        }

        protected virtual ISiteMapNodeToParentRelation GetSiteMapNodeFromProviderNode(System.Web.SiteMapNode node, ISiteMapNode parentNode, ISiteMapNodeHelper helper)
        {
            // Use the same keys as the underlying provider.
            string key = node.Key;
            var implicitResourceKey = node.ResourceKey;

            // Create Node
            var nodeParentMap = helper.CreateNode(key, parentNode.Key, SourceName, implicitResourceKey);
            var siteMapNode = nodeParentMap.Node;

            siteMapNode.Title = node.Title;
            siteMapNode.Description = String.IsNullOrEmpty(node.Description) ? siteMapNode.Title : node.Description;
            if (this.reflectAttributes)
            {
                // Unfortunately, the ASP.NET implementation uses a protected member variable to store
                // the attributes, so there is no way to loop through them without reflection or some
                // fancy dynamic subclass implementation.
                var attributeDictionary = node.GetPrivateFieldValue<NameValueCollection>("_attributes");
                siteMapNode.Attributes.AddRange(attributeDictionary);
            }
            siteMapNode.Roles.AddRange(node.Roles);
            siteMapNode.Clickable = bool.Parse(node.GetAttributeValueOrFallback("clickable", "true"));
            siteMapNode.VisibilityProvider = node.GetAttributeValue("visibilityProvider");
            siteMapNode.DynamicNodeProvider = node.GetAttributeValue("dynamicNodeProvider");
            siteMapNode.ImageUrl = node.GetAttributeValue("imageUrl");
            siteMapNode.TargetFrame = node.GetAttributeValue("targetFrame");
            siteMapNode.HttpMethod = node.GetAttributeValueOrFallback("httpMethod", "*").ToUpperInvariant();
            siteMapNode.Url = node.Url;
            siteMapNode.CacheResolvedUrl = bool.Parse(node.GetAttributeValueOrFallback("cacheResolvedUrl", "true"));
            siteMapNode.CanonicalUrl = node.GetAttributeValue("canonicalUrl");
            siteMapNode.CanonicalKey = node.GetAttributeValue("canonicalKey");
            siteMapNode.MetaRobotsValues.AddRange(node.GetAttributeValue("metaRobotsValues"), new[] { ' ' });
            siteMapNode.ChangeFrequency = (ChangeFrequency)Enum.Parse(typeof(ChangeFrequency), node.GetAttributeValueOrFallback("changeFrequency", "Undefined"));
            siteMapNode.UpdatePriority = (UpdatePriority)Enum.Parse(typeof(UpdatePriority), node.GetAttributeValueOrFallback("updatePriority", "Undefined"));
            siteMapNode.LastModifiedDate = DateTime.Parse(node.GetAttributeValueOrFallback("lastModifiedDate", DateTime.MinValue.ToString()));
            siteMapNode.Order = int.Parse(node.GetAttributeValueOrFallback("order", "0"));

            // Handle route details

            // Assign to node
            siteMapNode.Route = node.GetAttributeValue("route");
            if (this.reflectRouteValues)
            {
                // Unfortunately, the ASP.NET implementation uses a protected member variable to store
                // the attributes, so there is no way to loop through them without reflection or some
                // fancy dynamic subclass implementation.
                var attributeDictionary = node.GetPrivateFieldValue<NameValueCollection>("_attributes");
                siteMapNode.RouteValues.AddRange(attributeDictionary);
            }
            siteMapNode.PreservedRouteParameters.AddRange(node.GetAttributeValue("preservedRouteParameters"), new[] { ',', ';' });
            siteMapNode.UrlResolver = node.GetAttributeValue("urlResolver");

            // Add inherited route values to sitemap node
            foreach (var inheritedRouteParameter in node.GetAttributeValue("inheritedRouteParameters").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var item = inheritedRouteParameter.Trim();
                if (parentNode.RouteValues.ContainsKey(item))
                {
                    siteMapNode.RouteValues.Add(item, parentNode.RouteValues[item]);
                }
            }

            // Handle MVC details

            // Get area, controller and action from node declaration
            string area = node.GetAttributeValue("area");
            string controller = node.GetAttributeValue("controller");

            siteMapNode.Area = area;
            siteMapNode.Controller = controller;

            // Inherit area and controller from parent
            if (parentNode != null)
            {
                if (string.IsNullOrEmpty(area))
                {
                    siteMapNode.Area = parentNode.Area;
                }
                if (string.IsNullOrEmpty(controller))
                {
                    siteMapNode.Controller = parentNode.Controller;
                }
            }

            // Add defaults for area
            if (!siteMapNode.RouteValues.ContainsKey("area"))
            {
                siteMapNode.RouteValues.Add("area", "");
            }

            return nodeParentMap;
        }
    }
}
