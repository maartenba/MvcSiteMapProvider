using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// XmlSiteMapNodeProvider class. Builds a <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeToParentRelation"/> list based on a 
    /// <see cref="T:MvcSiteMapProvider.Xml.IXmlSource"/> instance.
    /// </summary>
    public class XmlSiteMapNodeProvider
        : ISiteMapNodeProvider
    {
        public XmlSiteMapNodeProvider(
            bool includeRootNode,
            bool useNestedDynamicNodeRecursion,
            IXmlSource xmlSource,
            ISiteMapXmlNameProvider xmlNameProvider
            )
        {
            if (xmlSource == null)
                throw new ArgumentNullException("xmlSource");
            if (xmlNameProvider == null)
                throw new ArgumentNullException("xmlNameProvider");

            this.includeRootNode = includeRootNode;
            this.useNestedDynamicNodeRecursion = useNestedDynamicNodeRecursion;
            this.xmlSource = xmlSource;
            this.xmlNameProvider = xmlNameProvider;
        }
        protected readonly bool includeRootNode;
        protected readonly bool useNestedDynamicNodeRecursion;
        protected readonly IXmlSource xmlSource;
        protected readonly ISiteMapXmlNameProvider xmlNameProvider;
        protected const string SourceName = ".sitemap XML File";

        [Flags]
        protected enum NodesToProcess
        {
            StandardNodes = 1,
            DynamicNodes = 2,
            All = StandardNodes | DynamicNodes
        }

        #region ISiteMapNodeProvider Members

        public IEnumerable<ISiteMapNodeToParentRelation> GetSiteMapNodes(ISiteMapNodeHelper helper)
        {
            var result = new List<ISiteMapNodeToParentRelation>();
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

        protected virtual IEnumerable<ISiteMapNodeToParentRelation> LoadSiteMapNodesFromXml(XDocument xml, ISiteMapNodeHelper helper)
        {
            var result = new List<ISiteMapNodeToParentRelation>();
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
            if (includeRootNode)
            {
                result.Add(rootNode);
            }

            // Process our XML, passing in the main root sitemap node and xml element.
            result.AddRange(ProcessXmlNodes(rootNode.Node, rootElement, NodesToProcess.All, helper));

            // Done!
            return result;
        }

        protected virtual XElement GetRootElement(XDocument xml)
        {
            // Get the root mvcSiteMapNode element, and map this to an MvcSiteMapNode
            return xml.Element(xmlNameProvider.RootName).Element(xmlNameProvider.NodeName);
        }

        protected virtual ISiteMapNodeToParentRelation GetRootNode(XDocument xml, XElement rootElement, ISiteMapNodeHelper helper)
        {
            return GetSiteMapNodeFromXmlElement(rootElement, null, helper);
        }


        /// <summary>
        /// Recursively processes our XML document, parsing our siteMapNodes and dynamicNode(s).
        /// </summary>
        /// <param name="parentNode">The parent node to process.</param>
        /// <param name="parentElement">The correspoding parent XML element.</param>
        /// <param name="processFlags">Flags to indicate which nodes to process.</param>
        /// <param name="helper">The node helper.</param>
        protected virtual IList<ISiteMapNodeToParentRelation> ProcessXmlNodes(ISiteMapNode parentNode, XElement parentElement, NodesToProcess processFlags, ISiteMapNodeHelper helper)
        {
            var result = new List<ISiteMapNodeToParentRelation>();
            bool processStandardNodes = (processFlags & NodesToProcess.StandardNodes) == NodesToProcess.StandardNodes;
            bool processDynamicNodes = (processFlags & NodesToProcess.DynamicNodes) == NodesToProcess.DynamicNodes;

            foreach (XElement node in parentElement.Elements())
            {
                if (node.Name != xmlNameProvider.NodeName)
                {
                    // If the current node is not one of the known node types throw and exception
                    throw new MvcSiteMapException(String.Format(Resources.Messages.XmlSiteMapNodeProviderInvalidSiteMapElement, helper.SiteMapCacheKey));
                }

                var child = GetSiteMapNodeFromXmlElement(node, parentNode, helper);

                if (processStandardNodes && !child.Node.HasDynamicNodeProvider)
                {
                    result.Add(child);

                    // Continue recursively processing the XML file.
                    result.AddRange(ProcessXmlNodes(child.Node, node, processFlags, helper));
                }
                else if (processDynamicNodes && child.Node.HasDynamicNodeProvider)
                {
                    // We pass in the parent node key as the default parent because the dynamic node (child) is never added to the sitemap.
                    var dynamicNodes = helper.CreateDynamicNodes(child, parentNode.Key);

                    foreach (var dynamicNode in dynamicNodes)
                    {
                        result.Add(dynamicNode);

                        if (!this.useNestedDynamicNodeRecursion)
                        {
                            // Recursively add non-dynamic childs for every dynamic node
                            result.AddRange(ProcessXmlNodes(dynamicNode.Node, node, NodesToProcess.StandardNodes, helper));
                        }
                        else
                        {
                            // Recursively process both dynamic nodes and static nodes.
                            // This is to allow V3 recursion behavior for those who depended on it - it is not a feature.
                            result.AddRange(ProcessXmlNodes(dynamicNode.Node, node, NodesToProcess.All, helper));
                        }
                    }

                    if (!this.useNestedDynamicNodeRecursion)
                    {
                        // Process the next nested dynamic node provider. We pass in the parent node as the default 
                        // parent because the dynamic node definition node (child) is never added to the sitemap.
                        result.AddRange(ProcessXmlNodes(parentNode, node, NodesToProcess.DynamicNodes, helper));
                    }
                    else
                    {
                        // Continue recursively processing the XML file.
                        // Can't figure out why this is here, but this is the way it worked in V3 and if
                        // anyone depends on the broken recursive behavior, they probably also depend on this.
                        result.AddRange(ProcessXmlNodes(child.Node, node, processFlags, helper));
                    }
                }
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
        protected virtual ISiteMapNodeToParentRelation GetSiteMapNodeFromXmlElement(XElement node, ISiteMapNode parentNode, ISiteMapNodeHelper helper)
        {
            // Get area, controller and action from node declaration
            string area = node.GetAttributeValue("area");
            string controller = node.GetAttributeValue("controller");
            var parentKey = parentNode == null ? "" : parentNode.Key;

            // Generate key for node
            string key = helper.CreateNodeKey(
                parentKey,
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