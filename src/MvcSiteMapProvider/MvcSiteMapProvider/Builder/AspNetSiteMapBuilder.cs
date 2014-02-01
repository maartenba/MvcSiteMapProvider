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
    /// AspNetSiteMapBuilder class. Builds a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> tree based on a 
    /// <see cref="T:System.Web.SiteMapProvider"/> instance.
    /// </summary>
    /// <remarks>
    /// Use this class for interoperability with ASP.NET classic. To get a sitemap instance, you will need
    /// to configure a Web.sitemap XML file using the ASP.NET classic schema, then configure it for use in the
    /// sitemap/providers section of the Web.config file. Consult MSDN for information on how to do this.
    /// 
    /// The sitemap provider can be retrieved from ASP.NET classic for injection into this class using 
    /// an implementation of IAspNetSiteMapProvider. You may implement this interface to provide custom
    /// logic for retrieving a provider by name or other means by using 
    /// System.Web.SiteMap.Providers[providerName] or for the default provider System.Web.SiteMap.Provider.
    /// 
    /// Attributes and route values are obtained from a protected member variable of the 
    /// System.Web.SiteMapProvider named _attributes using reflection. You may disable this functionality for 
    /// performance reasons if the data is not required by setting reflectAttributes and/or reflectRouteValues to false.
    /// </remarks>
    [Obsolete("AspNetSiteMapBuilder is deprecated and will be removed in version 5. Use AspNetSiteMapNodeProvider in conjunction with SiteMapBuilder instead.")]
    public class AspNetSiteMapBuilder
        : ISiteMapBuilder
    {
        public AspNetSiteMapBuilder(
            bool reflectAttributes,
            bool reflectRouteValues,
            ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider,
            IAspNetSiteMapProvider siteMapProvider,
            ISiteMapNodeFactory siteMapNodeFactory
            )
        {
            if (reservedAttributeNameProvider == null)
                throw new ArgumentNullException("reservedAttributeNameProvider");
            if (siteMapProvider == null)
                throw new ArgumentNullException("siteMapProvider");
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");

            this.reflectAttributes = reflectAttributes;
            this.reflectRouteValues = reflectRouteValues;
            this.reservedAttributeNameProvider = reservedAttributeNameProvider;
            this.siteMapProvider = siteMapProvider;
            this.siteMapNodeFactory = siteMapNodeFactory;
        }

        protected readonly bool reflectAttributes;
        protected readonly bool reflectRouteValues;
        protected readonly ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider;
        protected readonly IAspNetSiteMapProvider siteMapProvider;
        protected readonly ISiteMapNodeFactory siteMapNodeFactory;

        #region ISiteMapBuilder Members

        public ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode)
        {
            var provider = siteMapProvider.GetProvider();

            rootNode = GetRootNode(siteMap, provider);
            // Fixes #192 root node not added to sitemap
            if (siteMap.FindSiteMapNodeFromKey(rootNode.Key) == null)
            {
                // Add the root node to the sitemap
                siteMap.AddNode(rootNode);
            }

            ProcessNodes(siteMap, rootNode, provider.RootNode);

            return rootNode;
        }

        #endregion

        protected virtual ISiteMapNode GetRootNode(ISiteMap siteMap, SiteMapProvider provider)
        {
            var root = provider.RootNode;
            return GetSiteMapNodeFromProviderNode(siteMap, root, null);
        }

        protected virtual void ProcessNodes(ISiteMap siteMap, ISiteMapNode rootNode, System.Web.SiteMapNode providerRootNode)
        {
            foreach (System.Web.SiteMapNode node in providerRootNode.ChildNodes)
            {
                var childNode = GetSiteMapNodeFromProviderNode(siteMap, node, rootNode);
                ISiteMapNode parentNode = rootNode;

                siteMap.AddNode(childNode, parentNode);

                // Continue recursively processing
                ProcessNodes(siteMap, childNode, node);
            }
        }

        protected virtual ISiteMapNode GetSiteMapNodeFromProviderNode(ISiteMap siteMap, System.Web.SiteMapNode node, ISiteMapNode parentNode)
        {
            // Use the same keys as the underlying provider.
            string key = node.Key;
            var implicitResourceKey = node.ResourceKey;

            // Create Node
            ISiteMapNode siteMapNode = siteMapNodeFactory.Create(siteMap, key, implicitResourceKey);

            siteMapNode.Title = node.Title;
            siteMapNode.Description = String.IsNullOrEmpty(node.Description) ? siteMapNode.Title : node.Description;
            if (this.reflectAttributes)
            {
                // Unfortunately, the ASP.NET implementation uses a protected member variable to store
                // the attributes, so there is no way to loop through them without reflection or some
                // fancy dynamic subclass implementation.
                var attributeDictionary = node.GetPrivateFieldValue<NameValueCollection>("_attributes");
                siteMapNode.Attributes.AddRange(attributeDictionary);
                siteMapNode.RouteValues.AddRange(attributeDictionary);
            }
            AcquireRolesFrom(node, siteMapNode.Roles);
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
            this.AcquireMetaRobotsValuesFrom(node, siteMapNode.MetaRobotsValues);
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
            AcquirePreservedRouteParametersFrom(node, siteMapNode.PreservedRouteParameters);
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

            return siteMapNode;
        }

        /// <summary>
        /// Acquires the roles list from a given <see cref="T:System.Web.SiteMapNode"/>
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="roles">The roles IList to populate.</param>
        protected virtual void AcquireRolesFrom(System.Web.SiteMapNode node, IList<string> roles)
        {
            foreach (var role in node.Roles)
            {
                roles.Add(role.ToString());
            }
        }

        /// <summary>
        /// Acquires the preserved route parameters list from a given <see cref="T:System.Web.SiteMapNode"/>
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="roles">The preserved route parameters IList to populate.</param>
        protected virtual void AcquirePreservedRouteParametersFrom(System.Web.SiteMapNode node, IList<string> preservedRouteParameters)
        {
            var localParameters = node.GetAttributeValue("preservedRouteParameters").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var parameter in localParameters)
            {
                preservedRouteParameters.Add(parameter.Trim());
            }
        }

        /// <summary>
        /// Acquires the robots meta values list from a given <see cref="T:System.Web.SiteMapNode"/>
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="roles">The robots meta values IList to populate.</param>
        protected virtual void AcquireMetaRobotsValuesFrom(System.Web.SiteMapNode node, IList<string> metaRobotsValues)
        {
            var values = node.GetAttributeValue("metaRobotsValues").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var value in values)
            {
                metaRobotsValues.Add(value);
            }
        }
    }
}
