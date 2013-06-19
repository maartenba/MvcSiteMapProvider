using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// XmlSiteMapBuilder class. Builds a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> tree based on a 
    /// <see cref="T:MvcSiteMapProvider.Xml.IXmlSource"/> instance.
    /// </summary>
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
            SetEnableLocalization(siteMap, xml);

            // Get the root mvcSiteMapNode element, and map this to an MvcSiteMapNode
            var rootElement = GetRootElement(xml);
            var root = GetRootNode(siteMap, xml, rootElement);

            // Process our XML, passing in the main root sitemap node and xml element.
            ProcessXmlNodes(siteMap, root, rootElement);

            // NOTE: Setting security trimming as the last step improves performance.
            SetSecurityTrimmingEnabled(siteMap, xml);

            // Done!
            return root;
        }

        protected virtual void SetEnableLocalization(ISiteMap siteMap, XDocument xml)
        {
            // Enable Localization?
            siteMap.EnableLocalization = 
                bool.Parse(xml.Element(xmlNameProvider.RootName).GetAttributeValueOrFallback("enableLocalization", "false"));
        }

        protected virtual void SetSecurityTrimmingEnabled(ISiteMap siteMap, XDocument xml)
        {
            // Enable Security Trimming?
            siteMap.SecurityTrimmingEnabled = 
                bool.Parse(xml.Element(xmlNameProvider.RootName).GetAttributeValueOrFallback("securityTrimmingEnabled", "false"));
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
            //// Get area, controller and action from node declaration
            string area = node.GetAttributeValue("area");
            string controller = node.GetAttributeValue("controller");

            // Generate key for node
            string key = nodeKeyGenerator.GenerateKey(
                parentNode == null ? "" : parentNode.Key,
                node.GetAttributeValue("key"),
                node.GetAttributeValue("url"),
                node.GetAttributeValue("title"),
                area,
                controller,
                node.GetAttributeValue("action"),
                node.GetAttributeValueOrFallback("httpMethod", "*").ToUpperInvariant(),
                !(node.GetAttributeValue("clickable") == "false"));

            // Handle implicit resources
            var implicitResourceKey = node.GetAttributeValue("resourceKey");

            // Create node
            ISiteMapNode siteMapNode = siteMapNodeFactory.Create(siteMap, key, implicitResourceKey);

            // Handle title and description
            var title = node.GetAttributeValue("title");
            var description = String.IsNullOrEmpty(node.GetAttributeValue("description")) ? title : node.GetAttributeValue("description");

            // Assign defaults
            siteMapNode.Title = title;
            siteMapNode.Description = description;
            AcquireAttributesFrom(node, siteMapNode.Attributes);
            AcquireRolesFrom(node, siteMapNode.Roles);
            siteMapNode.Clickable = bool.Parse(node.GetAttributeValueOrFallback("clickable", "true"));
            siteMapNode.VisibilityProvider = node.GetAttributeValue("visibilityProvider");
            siteMapNode.DynamicNodeProvider = node.GetAttributeValue("dynamicNodeProvider");
            siteMapNode.ImageUrl = node.GetAttributeValue("imageUrl");
            siteMapNode.TargetFrame = node.GetAttributeValue("targetFrame");
            siteMapNode.HttpMethod = node.GetAttributeValueOrFallback("httpMethod", "*").ToUpperInvariant();
            siteMapNode.Url = node.GetAttributeValue("url");
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
            AcquireRouteValuesFrom(node, siteMapNode.RouteValues);
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
        /// Acquires the attributes from a given XElement.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        protected virtual void AcquireAttributesFrom(XElement node, IDictionary<string, object> attributes)
        {
            foreach (XAttribute attribute in node.Attributes())
            {
                var attributeName = attribute.Name.ToString();
                var attributeValue = attribute.Value;

                if (reservedAttributeNameProvider.IsRegularAttribute(attributeName))
                {
                    attributes.Add(attributeName, attributeValue);
                }
            }
        }

        /// <summary>
        /// Acquires the route values from a given XElement.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        protected virtual void AcquireRouteValuesFrom(XElement node, IRouteValueDictionary routeValues)
        {
            foreach (XAttribute attribute in node.Attributes())
            {
                var attributeName = attribute.Name.ToString();
                var attributeValue = attribute.Value;

                if (reservedAttributeNameProvider.IsRouteAttribute(attributeName))
                {
                    routeValues.Add(attributeName, attributeValue);
                }
            }
        }

        /// <summary>
        /// Acquires the roles list from a given XElement
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="roles">The roles IList to populate.</param>
        protected virtual void AcquireRolesFrom(XElement node, IList<string> roles)
        {
            var localRoles = node.GetAttributeValue("roles").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in localRoles)
            {
                roles.Add(role.Trim());
            }
        }

        /// <summary>
        /// Acquires the preserved route parameters list from a given XElement
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="roles">The preserved route parameters IList to populate.</param>
        protected virtual void AcquirePreservedRouteParametersFrom(XElement node, IList<string> preservedRouteParameters)
        {
            var localParameters = node.GetAttributeValue("preservedRouteParameters").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var parameter in localParameters)
            {
                preservedRouteParameters.Add(parameter.Trim());
            }
        }

        /// <summary>
        /// Acquires the robots meta values list from a given XAttribute
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="roles">The robots meta values IList to populate.</param>
        protected virtual void AcquireMetaRobotsValuesFrom(XElement node, IList<string> metaRobotsValues)
        {
            var values = node.GetAttributeValue("metaRobotsValues").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var value in values)
            {
                metaRobotsValues.Add(value);
            }
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

                        // Add non-dynamic childs for every dynamicnode
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
