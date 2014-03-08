using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// XmlSiteMapBuilder class. Builds a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> tree based on a 
    /// <see cref="T:MvcSiteMapProvider.Xml.IXmlSource"/> instance.
    /// </summary>
    [Obsolete("XmlSiteMapBuilder has been deprecated and will be removed in version 5. Use XmlSiteMapNodeProvider in conjunction with SiteMapBuilder instead.")]
    public class XmlSiteMapBuilder
        : ISiteMapBuilder
    {
        public XmlSiteMapBuilder(
            IXmlSource xmlSource,
            ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider,
            INodeKeyGenerator nodeKeyGenerator,
            IDynamicNodeBuilder dynamicNodeBuilder,
            ISiteMapNodeFactory siteMapNodeFactory,
            ISiteMapXmlNameProvider xmlNameProvider
            )
        {
            if (xmlSource == null)
                throw new ArgumentNullException("xmlSource");
            if (reservedAttributeNameProvider == null)
                throw new ArgumentNullException("reservedAttributeNameProvider");
            if (nodeKeyGenerator == null)
                throw new ArgumentNullException("nodeKeyGenerator");
            if (dynamicNodeBuilder == null)
                throw new ArgumentNullException("dynamicNodeBuilder");
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");
            if (xmlNameProvider == null)
                throw new ArgumentNullException("xmlNameProvider");

            this.xmlSource = xmlSource;
            this.reservedAttributeNameProvider = reservedAttributeNameProvider;
            this.nodeKeyGenerator = nodeKeyGenerator;
            this.dynamicNodeBuilder = dynamicNodeBuilder;
            this.siteMapNodeFactory = siteMapNodeFactory;
            this.xmlNameProvider = xmlNameProvider;
        }

        protected readonly IXmlSource xmlSource;
        protected readonly ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider;
        protected readonly INodeKeyGenerator nodeKeyGenerator;
        protected readonly IDynamicNodeBuilder dynamicNodeBuilder;
        protected readonly ISiteMapNodeFactory siteMapNodeFactory;
        protected readonly ISiteMapXmlNameProvider xmlNameProvider;


        #region ISiteMapBuilder Members

        public virtual ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode)
        {
            var xml = xmlSource.GetXml();
            if (xml != null)
            {
                rootNode = LoadSiteMapFromXml(siteMap, xml);
            }

            // Done!
            return rootNode;
        }

        #endregion

        protected virtual ISiteMapNode LoadSiteMapFromXml(ISiteMap siteMap, XDocument xml)
        {
            xmlNameProvider.FixXmlNamespaces(xml);

            // Get the root mvcSiteMapNode element, and map this to an MvcSiteMapNode
            var rootElement = GetRootElement(xml);
            var root = GetRootNode(siteMap, xml, rootElement);

            // Fixes #192 root node not added to sitemap
            if (siteMap.FindSiteMapNodeFromKey(root.Key) == null)
            {
                // Add the root node to the sitemap
                siteMap.AddNode(root);
            }

            // Process our XML, passing in the main root sitemap node and XML element.
            ProcessXmlNodes(siteMap, root, rootElement);

            // Done!
            return root;
        }

        protected virtual XElement GetRootElement(XDocument xml)
        {
            // Get the root mvcSiteMapNode element, and map this to an MvcSiteMapNode
            return xml.Element(xmlNameProvider.RootName).Element(xmlNameProvider.NodeName);
        }

        protected virtual ISiteMapNode GetRootNode(ISiteMap siteMap, XDocument xml, XElement rootElement)
        {
            return GetSiteMapNodeFromXmlElement(siteMap, rootElement, null);
        }


        /// <summary>
        /// Maps an XMLElement from the XML file to an MvcSiteMapNode.
        /// </summary>
        /// <param name="node">The element to map.</param>
        /// <param name="parentNode">The parent SiteMapNode</param>
        /// <returns>An MvcSiteMapNode which represents the XMLElement.</returns>
        protected virtual ISiteMapNode GetSiteMapNodeFromXmlElement(ISiteMap siteMap, XElement node, ISiteMapNode parentNode)
        {
            // Get data required to generate the node instance

            // Get area and controller from node declaration or the parent node
            var area = this.InheritAreaIfNotProvided(node, parentNode);
            var controller = this.InheritControllerIfNotProvided(node, parentNode);
            var action = node.GetAttributeValue("action");
            var url = node.GetAttributeValue("url");
            var explicitKey = node.GetAttributeValue("key");
            var parentKey = parentNode == null ? "" : parentNode.Key;
            var httpMethod = node.GetAttributeValueOrFallback("httpMethod", HttpVerbs.Get.ToString()).ToUpperInvariant();
            var clickable = bool.Parse(node.GetAttributeValueOrFallback("clickable", "true"));
            var title = node.GetAttributeValue("title");
            var implicitResourceKey = node.GetAttributeValue("resourceKey");

            // Generate key for node
            string key = nodeKeyGenerator.GenerateKey(
                parentKey,
                explicitKey,
                url,
                title,
                area,
                controller,
                action,
                httpMethod,
                clickable);

            // Create node
            ISiteMapNode siteMapNode = siteMapNodeFactory.Create(siteMap, key, implicitResourceKey);

            // Assign values
            siteMapNode.Title = title;
            siteMapNode.Description = node.GetAttributeValue("description");
            siteMapNode.Attributes.AddRange(node, false);
            siteMapNode.Roles.AddRange(node.GetAttributeValue("roles"), new[] { ',', ';' });
            siteMapNode.Clickable = clickable;
            siteMapNode.VisibilityProvider = node.GetAttributeValue("visibilityProvider");
            siteMapNode.DynamicNodeProvider = node.GetAttributeValue("dynamicNodeProvider");
            siteMapNode.ImageUrl = node.GetAttributeValue("imageUrl");
            siteMapNode.ImageUrlProtocol = node.GetAttributeValue("imageUrlProtocol");
            siteMapNode.ImageUrlHostName = node.GetAttributeValue("imageUrlHostName");
            siteMapNode.TargetFrame = node.GetAttributeValue("targetFrame");
            siteMapNode.HttpMethod = httpMethod;
            siteMapNode.Url = url;
            siteMapNode.CacheResolvedUrl = bool.Parse(node.GetAttributeValueOrFallback("cacheResolvedUrl", "true"));
            siteMapNode.IncludeAmbientValuesInUrl = bool.Parse(node.GetAttributeValueOrFallback("includeAmbientValuesInUrl", "false"));
            siteMapNode.Protocol = node.GetAttributeValue("protocol");
            siteMapNode.HostName = node.GetAttributeValue("hostName");
            siteMapNode.CanonicalKey = node.GetAttributeValue("canonicalKey");
            siteMapNode.CanonicalUrl = node.GetAttributeValue("canonicalUrl");
            siteMapNode.CanonicalUrlProtocol = node.GetAttributeValue("canonicalUrlProtocol");
            siteMapNode.CanonicalUrlHostName = node.GetAttributeValue("canonicalUrlHostName");
            siteMapNode.MetaRobotsValues.AddRange(node.GetAttributeValue("metaRobotsValues"), new[] { ' ' });
            siteMapNode.ChangeFrequency = (ChangeFrequency)Enum.Parse(typeof(ChangeFrequency), node.GetAttributeValueOrFallback("changeFrequency", "Undefined"));
            siteMapNode.UpdatePriority = (UpdatePriority)Enum.Parse(typeof(UpdatePriority), node.GetAttributeValueOrFallback("updatePriority", "Undefined"));
            siteMapNode.LastModifiedDate = DateTime.Parse(node.GetAttributeValueOrFallback("lastModifiedDate", DateTime.MinValue.ToString()));
            siteMapNode.Order = int.Parse(node.GetAttributeValueOrFallback("order", "0"));

            // Handle route details

            // Assign to node
            siteMapNode.Route = node.GetAttributeValue("route");
            siteMapNode.RouteValues.AddRange(node, false);
            siteMapNode.PreservedRouteParameters.AddRange(node.GetAttributeValue("preservedRouteParameters"), new[] { ',', ';' });
            siteMapNode.UrlResolver = node.GetAttributeValue("urlResolver");
            
            // Area and controller may need inheriting from the parent node, so set (or reset) them explicitly
            siteMapNode.Area = area;
            siteMapNode.Controller = controller;
            siteMapNode.Action = action;

            // Add inherited route values to sitemap node
            foreach (var inheritedRouteParameter in node.GetAttributeValue("inheritedRouteParameters").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var item = inheritedRouteParameter.Trim();
                if (parentNode.RouteValues.ContainsKey(item))
                {
                    siteMapNode.RouteValues.Add(item, parentNode.RouteValues[item]);
                }
            }

            return siteMapNode;
        }

        /// <summary>
        /// Inherits the area from the parent node if it is not provided in the current siteMapNode XML element and the parent node is not null.
        /// </summary>
        /// <param name="node">The siteMapNode element.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <returns>The value provided by either the siteMapNode or parentNode.Area.</returns>
        protected virtual string InheritAreaIfNotProvided(XElement node, ISiteMapNode parentNode)
        {
            var result = node.GetAttributeValue("area");
            if (node.Attribute("area") == null && parentNode != null)
            {
                result = parentNode.Area;
            }

            return result;
        }

        /// <summary>
        /// Inherits the controller from the parent node if it is not provided in the current siteMapNode XML element and the parent node is not null.
        /// </summary>
        /// <param name="node">The siteMapNode element.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <returns>The value provided by either the siteMapNode or parentNode.Controller.</returns>
        protected virtual string InheritControllerIfNotProvided(XElement node, ISiteMapNode parentNode)
        {
            var result = node.GetAttributeValue("controller");
            if (node.Attribute("controller") == null && parentNode != null)
            {
                result = parentNode.Controller;
            }

            return result;
        }

        /// <summary>
        /// Recursively processes our XML document, parsing our siteMapNodes and dynamicNode(s).
        /// </summary>
        /// <param name="rootNode">The main root sitemap node.</param>
        /// <param name="rootElement">The main root XML element.</param>
        protected virtual void ProcessXmlNodes(ISiteMap siteMap, ISiteMapNode rootNode, XElement rootElement)
        {
            // Loop through each element below the current root element.
            foreach (XElement node in rootElement.Elements())
            {
                ISiteMapNode childNode;
                if (node.Name == xmlNameProvider.NodeName)
                {
                    // If this is a normal mvcSiteMapNode then map the xml element
                    // to an MvcSiteMapNode, and add the node to the current root.
                    childNode = GetSiteMapNodeFromXmlElement(siteMap, node, rootNode);
                    ISiteMapNode parentNode = rootNode;

                    if (!childNode.HasDynamicNodeProvider)
                    {
                        siteMap.AddNode(childNode, parentNode);
                    }
                    else
                    {
                        var dynamicNodesCreated = dynamicNodeBuilder.BuildDynamicNodesFor(siteMap, childNode, parentNode);

                        // Add non-dynamic children for every dynamic node
                        foreach (var dynamicNodeCreated in dynamicNodesCreated)
                        {
                            ProcessXmlNodes(siteMap, dynamicNodeCreated, node);
                        }
                    }
                }
                else
                {
                    // If the current node is not one of the known node types throw and exception
                    throw new Exception(Resources.Messages.InvalidSiteMapElement);
                }

                // Continue recursively processing the XML file.
                ProcessXmlNodes(siteMap, childNode, node);
            }
        }
    }
}