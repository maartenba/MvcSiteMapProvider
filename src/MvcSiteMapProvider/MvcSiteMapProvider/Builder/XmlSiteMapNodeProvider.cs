using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// XmlSiteMapNodeProvider class. Builds a <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeParentMap"/> list based on a 
    /// <see cref="T:MvcSiteMapProvider.Xml.IXmlSource"/> instance.
    /// </summary>
    public class XmlSiteMapNodeProvider
        : ISiteMapNodeProvider
    {
        public XmlSiteMapNodeProvider(
            IXmlSource xmlSource,
            ISiteMapXmlNameProvider xmlNameProvider
            )
        {
            if (xmlSource == null)
                throw new ArgumentNullException("xmlSource");
            if (xmlNameProvider == null)
                throw new ArgumentNullException("xmlNameProvider");

            this.xmlSource = xmlSource;
            this.xmlNameProvider = xmlNameProvider;
        }
        protected readonly IXmlSource xmlSource;
        protected readonly ISiteMapXmlNameProvider xmlNameProvider;
        protected const string SourceName = ".sitemap XML File";

        #region ISiteMapNodeProvider Members

        public IEnumerable<ISiteMapNodeParentMap> GetSiteMapNodes(ISiteMapNodeHelper helper)
        {
            var result = new List<ISiteMapNodeParentMap>();
            var xml = xmlSource.GetXml();
            if (xml != null)
            {
                result.AddRange(LoadSiteMapNodesFromXml(xml, helper));
            }
            else
            {
                // Throw exception because XML was not defined
                throw new MvcSiteMapException(String.Format(Resources.Messages.XmlSiteMapNodeProviderXmlNotDefined, helper.SiteMapCacheKey));
            }

            return result;
        }

        #endregion

        protected virtual IEnumerable<ISiteMapNodeParentMap> LoadSiteMapNodesFromXml(XDocument xml, ISiteMapNodeHelper helper)
        {
            var result = new List<ISiteMapNodeParentMap>();
            xmlNameProvider.FixXmlNamespaces(xml);

            // Get the root mvcSiteMapNode element, and map this to an MvcSiteMapNode
            var rootElement = GetRootElement(xml);
            if (rootElement == null)
            {
                // No root element - inform the user this isn't allowed.
                throw new MvcSiteMapException(String.Format(Resources.Messages.XmlSiteMapNodeProviderRootNodeNotDefined, helper.SiteMapCacheKey));
            }
            // Add the root node
            var rootNode = GetRootNode(xml, rootElement, helper);
            result.Add(rootNode);

            // Process our XML, passing in the main root sitemap node and xml element.
            result.AddRange(ProcessXmlNodes(rootNode.Node, rootElement, helper));

            // Done!
            return result;
        }

        protected virtual XElement GetRootElement(XDocument xml)
        {
            // Get the root mvcSiteMapNode element, and map this to an MvcSiteMapNode
            return xml.Element(xmlNameProvider.RootName).Element(xmlNameProvider.NodeName);
        }

        protected virtual ISiteMapNodeParentMap GetRootNode(XDocument xml, XElement rootElement, ISiteMapNodeHelper helper)
        {
            return GetSiteMapNodeFromXmlElement(rootElement, null, helper);
        }


        /// <summary>
        /// Recursively processes our XML document, parsing our siteMapNodes and dynamicNode(s).
        /// </summary>
        /// <param name="rootNode">The main root sitemap node.</param>
        /// <param name="rootElement">The main root XML element.</param>
        /// <param name="helper">The node helper.</param>
        protected virtual IList<ISiteMapNodeParentMap> ProcessXmlNodes(ISiteMapNode parentNode, XElement parentElement, ISiteMapNodeHelper helper)
        {
            var result = new List<ISiteMapNodeParentMap>();

            // Loop through each element below the current root element.
            foreach (XElement node in parentElement.Elements())
            {
                ISiteMapNodeParentMap child;
                if (node.Name == xmlNameProvider.NodeName)
                {
                    // If this is a normal mvcSiteMapNode then map the xml element
                    // to an MvcSiteMapNode, and add the node to the current root.
                    child = GetSiteMapNodeFromXmlElement(node, parentNode, helper);

                    if (!child.Node.HasDynamicNodeProvider)
                    {
                        result.Add(child);
                    }
                    else
                    {
                        var dynamicNodes = helper.CreateDynamicNodes(child);

                        foreach (var dynamicNode in dynamicNodes)
                        {
                            result.Add(dynamicNode);
                            // Add non-dynamic childs for every dynamicnode
                            result.AddRange(ProcessXmlNodes(dynamicNode.Node, node, helper));
                        }
                    }
                }
                else
                {
                    // If the current node is not one of the known node types throw and exception
                    throw new Exception(String.Format(Resources.Messages.XmlSiteMapNodeProviderInvalidSiteMapElement, helper.SiteMapCacheKey));
                }

                // Continue recursively processing the XML file.
                result.AddRange(ProcessXmlNodes(child.Node, node, helper));
            }
            return result;
        }


        /// <summary>
        /// Maps an XMLElement from the XML file to an MvcSiteMapNode.
        /// </summary>
        /// <param name="node">The element to map.</param>
        /// <param name="parentNode">The parent ISiteMapNode</param>
        /// <param name="helper">The node helper.</param>
        /// <returns>An MvcSiteMapNode which represents the XMLElement.</returns>
        protected virtual ISiteMapNodeParentMap GetSiteMapNodeFromXmlElement(XElement node, ISiteMapNode parentNode, ISiteMapNodeHelper helper)
        {
            //// Get area, controller and action from node declaration
            string area = node.GetAttributeValue("area");
            string controller = node.GetAttributeValue("controller");
            var parentKey = parentNode == null ? "" : parentNode.Key;

            // Generate key for node
            string key = helper.CreateNodeKey(
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
            var nodeParentMap = helper.CreateNode(key, parentKey, SourceName, implicitResourceKey);
            var siteMapNode = nodeParentMap.Node;

            // Handle title and description
            var title = node.GetAttributeValue("title");
            var description = String.IsNullOrEmpty(node.GetAttributeValue("description")) ? title : node.GetAttributeValue("description");

            // Assign defaults
            siteMapNode.Title = title;
            siteMapNode.Description = description;
            AcquireAttributesFrom(node, siteMapNode.Attributes, helper);
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
            siteMapNode.Order = int.Parse(node.GetAttributeValueOrFallback("order", "0"));

            // Handle route details

            // Assign to node
            siteMapNode.Route = node.GetAttributeValue("route");
            AcquireRouteValuesFrom(node, siteMapNode.RouteValues, helper);
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
                if (string.IsNullOrEmpty(area) && !siteMapNode.RouteValues.ContainsKey("area"))
                {
                    siteMapNode.Area = parentNode.Area;
                }
                if (string.IsNullOrEmpty(controller) && !siteMapNode.RouteValues.ContainsKey("controller"))
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

        /// <summary>
        /// Acquires the attributes from a given XElement.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="attributes">The attributes dictionary to populate.</param>
        /// <param name="helper">The node helper.</param>
        /// <returns></returns>
        protected virtual void AcquireAttributesFrom(XElement node, IDictionary<string, object> attributes, ISiteMapNodeHelper helper)
        {
            foreach (XAttribute attribute in node.Attributes())
            {
                var attributeName = attribute.Name.ToString();
                var attributeValue = attribute.Value;

                if (helper.ReservedAttributeNames.IsRegularAttribute(attributeName))
                {
                    attributes.Add(attributeName, attributeValue);
                }
            }
        }

        /// <summary>
        /// Acquires the route values from a given XElement.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="routeValues">The route values dictionary to populate.</param>
        /// <param name="helper">The node helper.</param>
        /// <returns></returns>
        protected virtual void AcquireRouteValuesFrom(XElement node, IRouteValueDictionary routeValues, ISiteMapNodeHelper helper)
        {
            foreach (XAttribute attribute in node.Attributes())
            {
                var attributeName = attribute.Name.ToString();
                var attributeValue = attribute.Value;

                if (helper.ReservedAttributeNames.IsRouteAttribute(attributeName))
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
    }
}