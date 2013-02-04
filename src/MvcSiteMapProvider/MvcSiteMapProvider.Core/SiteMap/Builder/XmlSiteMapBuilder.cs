// -----------------------------------------------------------------------
// <copyright file="XmlSiteMapBuilder.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Hosting;
    using System.Xml.Linq;
    using System.IO;
    using System.Globalization;
    using MvcSiteMapProvider.Core.Xml;
    using MvcSiteMapProvider.Core.Globalization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XmlSiteMapBuilder : ISiteMapBuilder
    {
        public XmlSiteMapBuilder(
            string xmlSiteMapFilePath,
            IEnumerable<string> attributesToIgnore,
            INodeKeyGenerator nodeKeyGenerator,
            IDynamicNodeBuilder dynamicNodeBuilder,
            ISiteMapNodeFactory siteMapNodeFactory
            )
        {
            if (string.IsNullOrEmpty(xmlSiteMapFilePath))
                throw new ArgumentNullException("xmlSiteMapFilePath");
            if (attributesToIgnore == null)
                throw new ArgumentNullException("attributesToIgnore");
            if (nodeKeyGenerator == null)
                throw new ArgumentNullException("nodeKeyGenerator");
            if (dynamicNodeBuilder == null)
                throw new ArgumentNullException("dynamicNodeBuilder");
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");

            this.xmlSiteMapFilePath = xmlSiteMapFilePath;
            this.attributesToIgnore = attributesToIgnore;
            this.nodeKeyGenerator = nodeKeyGenerator;
            this.dynamicNodeBuilder = dynamicNodeBuilder;
            this.siteMapNodeFactory = siteMapNodeFactory;
        }

        protected readonly string xmlSiteMapFilePath;
        protected readonly IEnumerable<string> attributesToIgnore;
        protected readonly INodeKeyGenerator nodeKeyGenerator;
        protected readonly IDynamicNodeBuilder dynamicNodeBuilder;
        protected readonly ISiteMapNodeFactory siteMapNodeFactory;
        

        protected const string xmlRootName = "mvcSiteMap";
        protected const string xmlNodeName = "mvcSiteMapNode";
        protected readonly XNamespace xmlSiteMapNamespace = "http://mvcsitemap.codeplex.com/schemas/MvcSiteMap-File-3.0";


        #region ISiteMapBuilder Members

        public ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode)
        {
            var xml = GetSiteMapXmlFromFile(this.xmlSiteMapFilePath);
            if (xml != null)
            {
                rootNode = LoadSiteMapFromXml(siteMap, xml);
            }

            // Done!
            return rootNode;
        }

        #endregion

        protected virtual XDocument GetSiteMapXmlFromFile(string xmlSiteMapFilePath)
        {
            XDocument result = null;
            var siteMapFileAbsolute = HostingEnvironment.MapPath(this.xmlSiteMapFilePath);
            if (File.Exists(siteMapFileAbsolute))
            {
                result = XDocument.Load(siteMapFileAbsolute);
            }
            else
            {
                throw new FileNotFoundException(string.Format(Resources.Messages.SiteMapFileNotFound, this.xmlSiteMapFilePath), siteMapFileAbsolute);
            }
            return result;
        }

        protected virtual ISiteMapNode LoadSiteMapFromXml(ISiteMap siteMap, XDocument xml)
        {
            FixXmlNamespaces(xml);
            SetEnableLocalization(siteMap, xml);
            SetSecurityTrimmingEnabled(siteMap, xml);

            // Get the root mvcSiteMapNode element, and map this to an MvcSiteMapNode
            var rootElement = GetRootElement(xml);
            var root = GetRootNode(siteMap, xml, rootElement);

            // Process our XML file, passing in the main root sitemap node and xml element.
            ProcessXmlNodes(siteMap, root, rootElement);

            // Done!
            return root;
        }

        protected virtual void FixXmlNamespaces(XDocument xml)
        {
            // If no namespace is present (or the wrong one is present), replace it
            foreach (var node in xml.Descendants())
            {
                if (string.IsNullOrEmpty(node.Name.Namespace.NamespaceName) || node.Name.Namespace != this.xmlSiteMapNamespace)
                {
                    node.Name = XName.Get(node.Name.LocalName, this.xmlSiteMapNamespace.ToString());
                }
            }
        }

        protected virtual void SetEnableLocalization(ISiteMap siteMap, XDocument xml)
        {
            // Enable Localization?
            string enableLocalization =
                xml.Element(this.xmlSiteMapNamespace + xmlRootName).GetAttributeValue("enableLocalization");
            if (!string.IsNullOrEmpty(enableLocalization))
            {
                siteMap.EnableLocalization = Boolean.Parse(enableLocalization);
            }
        }

        protected virtual void SetSecurityTrimmingEnabled(ISiteMap siteMap, XDocument xml)
        {
            // Enable Security Trimming?
            string securityTrimmingEnabled =
                xml.Element(this.xmlSiteMapNamespace + xmlRootName).GetAttributeValue("securityTrimmingEnabled");
            if (!string.IsNullOrEmpty(securityTrimmingEnabled))
            {
                siteMap.SecurityTrimmingEnabled = Boolean.Parse(securityTrimmingEnabled);
            }
        }

        protected virtual XElement GetRootElement(XDocument xml)
        {
            // Get the root mvcSiteMapNode element, and map this to an MvcSiteMapNode
            return xml.Element(this.xmlSiteMapNamespace + xmlRootName).Element(this.xmlSiteMapNamespace + xmlNodeName);
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
            //string action = node.GetAttributeValue("action");
            //string route = node.GetAttributeValue("route");

            //// Determine the node type ??
            //XSiteMapNode siteMapNode = null;
            //if (!string.IsNullOrEmpty(area) || !string.IsNullOrEmpty(controller) || !string.IsNullOrEmpty(action))
            //{
            //    siteMapNode = new XMvcSiteMapNode();
            //}
            //else if (!string.IsNullOrEmpty(route))
            //{
            //    siteMapNode = new XRouteSiteMapNode();
            //}
            //else
            //{
            //    siteMapNode = new XSiteMapNode();
            //}

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

            // Handle title and description
            var title = node.GetAttributeValue("title");
            //var description = node.GetAttributeValue("description") ?? title;
            var description = String.IsNullOrEmpty(node.GetAttributeValue("description")) ? title : node.GetAttributeValue("description");

            // Handle implicit resources
            var implicitResourceKey = node.GetAttributeValue("resourceKey");

            // Create node
            ISiteMapNode siteMapNode = siteMapNodeFactory.Create(siteMap, key, implicitResourceKey);

            // Assign defaults
            siteMapNode.Title = title;
            siteMapNode.Description = description;
            //siteMapNode.ResourceKey = implicitResourceKey;
            //siteMapNode.Attributes = AcquireAttributesFrom(node);

            AcquireAttributesFrom(node, siteMapNode.Attributes);
            AcquireRolesFrom(node, siteMapNode.Roles);
            //siteMapNode.Roles = node.GetAttributeValue("roles").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            siteMapNode.Clickable = bool.Parse(node.GetAttributeValueOrFallback("clickable", "true"));

            // TODO: move these to sitemapnode constructor so they can't be changed at runtime.
            siteMapNode.VisibilityProvider = node.GetAttributeValue("visibilityProvider");
            siteMapNode.DynamicNodeProvider = node.GetAttributeValue("dynamicNodeProvider");


            siteMapNode.ImageUrl = node.GetAttributeValue("imageUrl");
            siteMapNode.TargetFrame = node.GetAttributeValue("targetFrame");
            siteMapNode.HttpMethod = node.GetAttributeValueOrFallback("httpMethod", "*").ToUpperInvariant();
            siteMapNode.Url = node.GetAttributeValue("url");

            //if (!siteMapNode.Clickable)
            //{
            //    siteMapNode.Url = "";
            //}
            //else
            //{
            //    siteMapNode.Url = node.GetAttributeValue("url");
            //}
            if (!string.IsNullOrEmpty(node.GetAttributeValue("changeFrequency")))
            {
                siteMapNode.ChangeFrequency = (ChangeFrequency)Enum.Parse(typeof(ChangeFrequency), node.GetAttributeValue("changeFrequency"));
            }
            else
            {
                siteMapNode.ChangeFrequency = ChangeFrequency.Undefined;
            }
            if (!string.IsNullOrEmpty(node.GetAttributeValue("updatePriority")))
            {
                siteMapNode.UpdatePriority = (UpdatePriority)Enum.Parse(typeof(UpdatePriority), node.GetAttributeValue("updatePriority"));
            }
            else
            {
                siteMapNode.UpdatePriority = UpdatePriority.Undefined;
            }
            if (!string.IsNullOrEmpty(node.GetAttributeValue("lastModifiedDate")))
            {
                siteMapNode.LastModifiedDate = DateTime.Parse(node.GetAttributeValue("lastModifiedDate"));
            }
            else
            {
                siteMapNode.LastModifiedDate = DateTime.MinValue;
            }

            // Handle route details
            var routeNode = siteMapNode;
            if (routeNode != null)
            {
                // Assign to node
                routeNode.Route = node.GetAttributeValue("route");
                //routeNode.RouteValues = AcquireRouteValuesFrom(node);
                AcquireRouteValuesFrom(node, routeNode.RouteValues);

                routeNode.PreservedRouteParameters = node.GetAttributeValue("preservedRouteParameters").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                //routeNode.Url = "";

                // TODO: move this to sitemapnode constructor so they can't be changed at runtime.
                routeNode.UrlResolver = node.GetAttributeValue("urlResolver");

                // Add inherited route values to sitemap node
                var parentRouteNode = parentNode;
                if (parentRouteNode != null)
                {
                    foreach (var inheritedRouteParameter in node.GetAttributeValue("inheritedRouteParameters").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var item = inheritedRouteParameter.Trim();
                        if (parentRouteNode.RouteValues.ContainsKey(item))
                        {
                            routeNode.RouteValues.Add(item, parentRouteNode.RouteValues[item]);
                        }
                    }
                }
            }

            // Handle MVC details
            var mvcNode = siteMapNode;
            if (mvcNode != null)
            {
                //// MVC properties
                //mvcNode.Area = area;
                //mvcNode.Controller = controller;
                //mvcNode.Action = action;

                // Inherit area and controller from parent
                var parentMvcNode = parentNode;
                if (parentMvcNode != null)
                {
                    if (string.IsNullOrEmpty(area))
                    {
                        mvcNode.Area = parentMvcNode.Area;
                    }
                    if (string.IsNullOrEmpty(controller))
                    {
                        mvcNode.Controller = parentMvcNode.Controller;
                    }
                }

                // Add defaults for area
                if (!mvcNode.RouteValues.ContainsKey("area"))
                {
                    mvcNode.RouteValues.Add("area", "");
                }
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
                    //siteMapNode.Roles = attribute.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

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
        protected virtual void AcquireRouteValuesFrom(XElement node, RouteValueCollection routeValues)
        {
            //var returnValue = new Dictionary<string, object>();
            foreach (XAttribute attribute in node.Attributes())
            {
                var attributeName = attribute.Name.ToString();
                var attributeValue = attribute.Value;

                if (IsRouteAttribute(attributeName))
                {
                    routeValues.Add(attributeName, attributeValue);
                }
            }
            //return returnValue;
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
            //siteMapNode.Roles = attribute.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var localRoles = attribute.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in localRoles)
            {
                roles.Add(role);
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
                if (node.Name == this.xmlSiteMapNamespace + xmlNodeName)
                {
                    // If this is a normal mvcSiteMapNode then map the xml element
                    // to an MvcSiteMapNode, and add the node to the current root.
                    childNode = GetSiteMapNodeFromXmlElement(siteMap, node, rootNode);
                    ISiteMapNode parentNode = rootNode;
                    childNode.ParentNode = parentNode;

                    //if (!dynamicNodeBuilder.HasDynamicNodes(childNode))
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
