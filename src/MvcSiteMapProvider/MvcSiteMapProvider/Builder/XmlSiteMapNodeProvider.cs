using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
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
                throw new MvcSiteMapException(string.Format(Resources.Messages.XmlSiteMapNodeProviderXmlNotDefined, helper.SiteMapCacheKey));
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
                throw new MvcSiteMapException(string.Format(Resources.Messages.XmlSiteMapNodeProviderRootNodeNotDefined, helper.SiteMapCacheKey));
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
        /// <param name="parentElement">The corresponding parent XML element.</param>
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
                    throw new MvcSiteMapException(string.Format(Resources.Messages.XmlSiteMapNodeProviderInvalidSiteMapElement, helper.SiteMapCacheKey));
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
                            // Recursively add non-dynamic children for every dynamic node
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
            // Get data required to generate the node instance

            // Get area and controller from node declaration or the parent node
            var area = this.InheritAreaIfNotProvided(node, parentNode);
            var controller = this.InheritControllerIfNotProvided(node, parentNode);
            var action = node.GetAttributeValue("action");
            var url = node.GetAttributeValue("url");
            var explicitKey = node.GetAttributeValue("key");
            var parentKey = parentNode == null ? "" : parentNode.Key;
            var httpMethod = node.GetAttributeValueOrFallback("httpMethod", HttpVerbs.Get.ToString()).ToUpper();
            var clickable = bool.Parse(node.GetAttributeValueOrFallback("clickable", "true"));
            var title = node.GetAttributeValue("title");
            var implicitResourceKey = node.GetAttributeValue("resourceKey");

            // Generate key for node
            string key = helper.CreateNodeKey(
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
            var nodeParentMap = helper.CreateNode(key, parentKey, SourceName, implicitResourceKey);
            var siteMapNode = nodeParentMap.Node;

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
                if (node.Attribute(item) != null)
                    throw new MvcSiteMapException(string.Format(Resources.Messages.SiteMapNodeSameKeyInRouteValueAndInheritedRouteParameter, key, title, item));

                if (parentNode.RouteValues.ContainsKey(item))
                {
                    siteMapNode.RouteValues.Add(item, parentNode.RouteValues[item]);
                }
            }

            return nodeParentMap;
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
    }
}