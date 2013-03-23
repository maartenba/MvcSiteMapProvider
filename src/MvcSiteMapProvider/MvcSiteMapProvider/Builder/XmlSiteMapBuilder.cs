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
            IEnumerable<string> attributesToIgnore,
            IXmlSource xmlSource,
            INodeKeyGenerator nodeKeyGenerator,
            IDynamicNodeBuilder dynamicNodeBuilder,
            ISiteMapNodeFactory siteMapNodeFactory,
            ISiteMapXmlNameProvider xmlNameProvider
            )
        {
            if (attributesToIgnore == null)
                throw new ArgumentNullException("attributesToIgnore");
            if (xmlSource == null)
                throw new ArgumentNullException("xmlSource");
            if (nodeKeyGenerator == null)
                throw new ArgumentNullException("nodeKeyGenerator");
            if (dynamicNodeBuilder == null)
                throw new ArgumentNullException("dynamicNodeBuilder");
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");
            if (xmlNameProvider == null)
                throw new ArgumentNullException("xmlNameProvider");

            this.attributesToIgnore = attributesToIgnore;
            this.xmlSource = xmlSource;
            this.nodeKeyGenerator = nodeKeyGenerator;
            this.dynamicNodeBuilder = dynamicNodeBuilder;
            this.siteMapNodeFactory = siteMapNodeFactory;
            this.xmlNameProvider = xmlNameProvider;
        }

        protected readonly IEnumerable<string> attributesToIgnore;
        protected readonly IXmlSource xmlSource;
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
            SetSecurityTrimmingEnabled(siteMap, xml);

            // Get the root mvcSiteMapNode element, and map this to an MvcSiteMapNode
            var rootElement = GetRootElement(xml);
            var root = GetRootNode(siteMap, xml, rootElement);

            // Process our XML, passing in the main root sitemap node and xml element.
            ProcessXmlNodes(siteMap, root, rootElement);

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
        /// Add each attribute to our attributes collection on the siteMapNode
        /// and to a route data dictionary.
        /// </summary>
        /// <param name="node">The element to map.</param>
        /// <param name="siteMapNode">The SiteMapNode to map to</param>
        /// <param name="routeValues">The RouteValueDictionary to fill</param>
        protected virtual void AttributesToRouteValues(XElement node, ISiteMapNode siteMapNode, IDictionary<string, object> routeValues)
        {
            foreach (XAttribute attribute in node.Attributes())
            {
                var attributeName = attribute.Name.ToString();
                var attributeValue = attribute.Value;

                if (IsRegularAttribute(attributeName))
                {
                    siteMapNode.Attributes[attributeName] = attributeValue;
                }

                // Process route values
                if (IsRouteAttribute(attributeName))
                {
                    routeValues.Add(attributeName, attributeValue);
                }

                if (attributeName == "roles")
                {
                    AcquireRolesFrom(attribute, siteMapNode.Roles);
                }
            }
        }

        /// <summary>
        /// Determines whether the attribute is a regular attribute.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>
        ///   <c>true</c> if the attribute is a regular attribute; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsRegularAttribute(string attributeName)
        {
            return attributeName != "title"
                   && attributeName != "description";
        }

        /// <summary>
        /// Determines whether the attribute is a route attribute.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>
        ///   <c>true</c> if the attribute is a route attribute; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsRouteAttribute(string attributeName)
        {
            return attributeName != "title"
               && attributeName != "description"
               && attributeName != "resourceKey"
               && attributeName != "key"
               && attributeName != "roles"
               && attributeName != "route"
               && attributeName != "url"
               && attributeName != "cacheResolvedUrl"
               && attributeName != "clickable"
               && attributeName != "httpMethod"
               && attributeName != "dynamicNodeProvider"
               && attributeName != "urlResolver"
               && attributeName != "visibilityProvider"
               && attributeName != "visibility"
               && attributeName != "lastModifiedDate"
               && attributeName != "changeFrequency"
               && attributeName != "updatePriority"
               && attributeName != "targetFrame"
               && attributeName != "imageUrl"
               && attributeName != "inheritedRouteParameters"
               && attributeName != "preservedRouteParameters"
               && attributeName != "canonicalUrl"
               && attributeName != "canonicalKey"
               && attributeName != "metaRobotsValues"
               && !attributesToIgnore.Contains(attributeName)
               && !attributeName.StartsWith("data-");
        }

        /// <summary>
        /// Acquires the attributes from a given XElement.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        protected virtual void AcquireAttributesFrom(XElement node, IDictionary<string, string> attributes)
        {
            foreach (XAttribute attribute in node.Attributes())
            {
                var attributeName = attribute.Name.ToString();
                var attributeValue = attribute.Value;

                if (IsRegularAttribute(attributeName))
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
        protected virtual void AcquireRouteValuesFrom(XElement node, IRouteValueCollection routeValues)
        {
            foreach (XAttribute attribute in node.Attributes())
            {
                var attributeName = attribute.Name.ToString();
                var attributeValue = attribute.Value;

                if (IsRouteAttribute(attributeName))
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
                roles.Add(role);
            }
        }

        /// <summary>
        /// Acquires the roles list from a given XAttribute
        /// </summary>
        /// <param name="node">The attribute.</param>
        /// <param name="roles">The roles IList to populate.</param>
        protected virtual void AcquireRolesFrom(XAttribute attribute, IList<string> roles)
        {
            var localRoles = attribute.Value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in localRoles)
            {
                roles.Add(role);
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
                preservedRouteParameters.Add(parameter);
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
                    childNode.ParentNode = parentNode;

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
