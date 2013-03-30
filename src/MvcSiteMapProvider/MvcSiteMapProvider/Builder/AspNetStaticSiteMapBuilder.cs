using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Reflection;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// XmlSiteMapBuilder class. Builds a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> tree based on a 
    /// <see cref="T:System.Web.StaticSiteMapProvider"/> instance.
    /// </summary>
    /// <remarks>
    /// Use this class for interoperability with ASP.NET classic. To get a sitemap instance, you will need
    /// to configure a Web.sitemap XML file using the ASP.NET classic schema, then configure it for use in the
    /// sitemap/providers section of the Web.config file. Consult MSDN for information on how to do this.
    /// 
    /// The sitemap provider can be retrieved from ASP.NET classic for injection into this class using 
    /// System.Web.SiteMap.Providers[providerName] or for the default provider System.Web.SiteMap.Provider.
    /// </remarks>
    public class AspNetStaticSiteMapBuilder
        : ISiteMapBuilder
    {
        public AspNetStaticSiteMapBuilder(
            bool reflectAttributes,
            bool reflectRouteValues,
            ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider,
            StaticSiteMapProvider provider,
            ISiteMapNodeFactory siteMapNodeFactory
            )
        {
            if (reservedAttributeNameProvider == null)
                throw new ArgumentNullException("reservedAttributeNameProvider");
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");

            this.reflectAttributes = reflectAttributes;
            this.reflectRouteValues = reflectRouteValues;
            this.reservedAttributeNameProvider = reservedAttributeNameProvider;
            this.provider = provider;
            this.siteMapNodeFactory = siteMapNodeFactory;
        }

        protected readonly bool reflectAttributes;
        protected readonly bool reflectRouteValues;
        protected readonly ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider;
        protected readonly StaticSiteMapProvider provider;
        protected readonly ISiteMapNodeFactory siteMapNodeFactory;

        #region ISiteMapBuilder Members

        public ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode)
        {
            siteMap.EnableLocalization = provider.EnableLocalization;
            siteMap.SecurityTrimmingEnabled = provider.SecurityTrimmingEnabled;

            rootNode = GetRootNode(siteMap, provider);

            ProcessNodes(siteMap, rootNode, provider.RootNode);

            return rootNode;
        }

        #endregion

        protected virtual ISiteMapNode GetRootNode(ISiteMap siteMap, StaticSiteMapProvider provider)
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
            siteMapNode.Description = node.Description;
            if (this.reflectAttributes)
            {
                AcquireAttributesFrom(node, siteMapNode.Attributes);
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

            // Handle route details

            // Assign to node
            siteMapNode.Route = node.GetAttributeValue("route");
            if (this.reflectRouteValues)
            {
                AcquireRouteValuesFrom(node, siteMapNode.RouteValues);
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
        /// Acquires the attributes from a given <see cref="T:System.Web.SiteMapNode"/>.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        protected virtual void AcquireAttributesFrom(System.Web.SiteMapNode node, IDictionary<string, string> attributes)
        {
            // Unfortunately, the ASP.NET implementation uses a protected member variable to store
            // the attributes, so there is no way to loop through them without reflection or some
            // fancy dynamic subclass implementation.
            var attributeCollection = this.GetPrivateFieldValue<NameValueCollection>(node, "_attributes");
            foreach (string key in attributeCollection.Keys)
            {
                var attributeName = key;
                var attributeValue = node[key];

                if (reservedAttributeNameProvider.IsRegularAttribute(attributeName))
                {
                    attributes.Add(attributeName, attributeValue);
                }
            }
        }

        /// <summary>
        /// Acquires the route values from a given <see cref="T:System.Web.SiteMapNode"/>.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        protected virtual void AcquireRouteValuesFrom(System.Web.SiteMapNode node, IRouteValueCollection routeValues)
        {
            if (this.reflectAttributes)
            {
                // Unfortunately, the ASP.NET implementation uses a protected member variable to store
                // the attributes, so there is no way to loop through them without reflection or some
                // fancy dynamic subclass implementation.
                var attributeCollection = this.GetPrivateFieldValue<NameValueCollection>(node, "_attributes");
                foreach (string key in attributeCollection.Keys)
                {
                    var attributeName = key;
                    var attributeValue = node[key];

                    if (reservedAttributeNameProvider.IsRouteAttribute(attributeName))
                    {
                        routeValues.Add(attributeName, attributeValue);
                    }
                }
            }
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
                preservedRouteParameters.Add(parameter);
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

        /// <summary>
        /// Returns a private Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <returns>PropertyValue</returns>
        protected T GetPrivateFieldValue<T>(object obj, string propName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }
            if (fi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
            return (T)fi.GetValue(obj);
        }

    }
}
