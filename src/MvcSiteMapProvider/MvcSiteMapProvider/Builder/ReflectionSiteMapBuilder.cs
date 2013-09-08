using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Reflection;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// ReflectionSiteMapBuilder class. 
    /// Builds a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> tree based on a
    /// set of attributes within an assembly.
    /// </summary>
    public class ReflectionSiteMapBuilder 
        : ISiteMapBuilder
    {
        public ReflectionSiteMapBuilder(
            IEnumerable<String> includeAssemblies,
            IEnumerable<String> excludeAssemblies,
            ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider,
            ISiteMapAssemblyService siteMapAssemblyService,
            IAttributeAssemblyProviderFactory attributeAssemblyProviderFactory,
            IMvcSiteMapNodeAttributeDefinitionProvider attributeNodeDefinitionProvider
            )
        {
            if (includeAssemblies == null)
                throw new ArgumentNullException("includeAssemblies");
            if (excludeAssemblies == null)
                throw new ArgumentNullException("excludeAssemblies");
            if (reservedAttributeNameProvider == null)
                throw new ArgumentNullException("reservedAttributeNameProvider");
            if (siteMapAssemblyService == null)
                throw new ArgumentNullException("siteMapAssemblyService");
            if (attributeAssemblyProviderFactory == null)
                throw new ArgumentNullException("attributeAssemblyProviderFactory");
            if (attributeNodeDefinitionProvider == null)
                throw new ArgumentNullException("attributeNodeDefinitionProvider");

            this.includeAssemblies = includeAssemblies;
            this.excludeAssemblies = excludeAssemblies;
            this.reservedAttributeNameProvider = reservedAttributeNameProvider;
            this.siteMapAssemblyService = siteMapAssemblyService;
            this.attributeAssemblyProviderFactory = attributeAssemblyProviderFactory;
            this.attributeNodeDefinitionProvider = attributeNodeDefinitionProvider;
        }
        protected readonly IEnumerable<string> includeAssemblies;
        protected readonly IEnumerable<string> excludeAssemblies;
        protected readonly ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider;
        protected readonly ISiteMapAssemblyService siteMapAssemblyService;
        protected readonly IMvcSiteMapNodeAttributeDefinitionProvider attributeNodeDefinitionProvider;
        protected readonly IAttributeAssemblyProviderFactory attributeAssemblyProviderFactory;


        #region ISiteMapBuilder Members
        

        /// <summary>
        /// Provides the base data on which the context-aware provider can generate a full tree.
        /// </summary>
        /// <param name="siteMap">The siteMap object to populate with the data.</param>
        /// <returns></returns>
        public virtual ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode)
        {
            var assemblyProvider = this.attributeAssemblyProviderFactory.Create(this.includeAssemblies, this.excludeAssemblies);
            var assemblies = assemblyProvider.GetAssemblies();
            var definitions = this.attributeNodeDefinitionProvider.GetMvcSiteMapNodeAttributeDefinitions(assemblies);
            rootNode = this.CreateNodesFromMvcSiteMapNodeAttributeDefinitions(siteMap, rootNode, definitions);

            // Done!
            return rootNode;
        }

        #endregion

        /// <summary>
        /// Creates the nodes from MVC site map node attribute definitions.
        /// </summary>
        /// <param name="definitions">The definitions.</param>
        protected virtual ISiteMapNode CreateNodesFromMvcSiteMapNodeAttributeDefinitions(ISiteMap siteMap, ISiteMapNode rootNode, IEnumerable<IMvcSiteMapNodeAttributeDefinition> definitions)
        {
            rootNode = GetRootNode(siteMap, rootNode, definitions);
            var sourceNodes = new List<ISiteMapNodeParentMap>();
            
            sourceNodes.AddRange(this.CreateNodesFromAttributeDefinitions(siteMap, definitions));
            sourceNodes.AddRange(this.CreateDynamicNodes(siteMap, sourceNodes));

            var orphans = this.siteMapAssemblyService.BuildHierarchy(siteMap, sourceNodes.ToArray());

            if (orphans.Count() > 0)
            {
                // We have orphaned nodes - throw an exception.
                var names = String.Join(Environment.NewLine + Environment.NewLine, orphans.Select(x => String.Format(Resources.Messages.SiteMapNodeFormatWithParentKey, x.ParentKey, x.Node.Controller, x.Node.Action, x.Node.Area, x.Node.Url, x.Node.Key, x.SourceName)));
                throw new MvcSiteMapException(String.Format(Resources.Messages.ReflectionSiteMapBuilderOrphanedNodes, this.siteMapAssemblyService.SiteMapCacheKey, names));
            }
            return rootNode;
        }

        protected virtual ISiteMapNode GetRootNode(ISiteMap siteMap, ISiteMapNode rootNode, IEnumerable<IMvcSiteMapNodeAttributeDefinition> definitions)
        {
            var emptyParentKeyCount = definitions.Where(t => string.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)).Count();

            ThrowIfRootNodeDefinedMultipleTimes(emptyParentKeyCount, definitions);
            ThrowIfRootNodeDefinedAndPassed(emptyParentKeyCount, rootNode, definitions);
            
            // Find root node
            if (rootNode == null && emptyParentKeyCount == 1)
            {
                var definition = definitions.Where(t => string.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)).Single();
                rootNode = this.CreateNodeFromAttributeDefinition(siteMap, definition);

                // Fixes #192 root node not added to sitemap
                if (rootNode != null && siteMap.FindSiteMapNodeFromKey(rootNode.Key) == null)
                {
                    // Add the root node to the sitemap
                    siteMap.AddNode(rootNode);
                }
            }
            return rootNode;
        }


        protected virtual void ThrowIfRootNodeDefinedMultipleTimes(int emptyParentKeyCount, IEnumerable<IMvcSiteMapNodeAttributeDefinition> definitions)
        {
            // Throw a sensible exception if the configuration has more than 1 empty parent key (#179).
            if (emptyParentKeyCount > 1)
            {
                var names = String.Join(Environment.NewLine, definitions.Where(t => String.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)).Select(x => "'" + x.SiteMapNodeAttribute.Key + "'"));
                throw new MvcSiteMapException(String.Format(Resources.Messages.ReflectionSiteMapBuilderRootKeyAmbiguous, this.siteMapAssemblyService.SiteMapCacheKey, names));
            }
        }

        protected virtual void ThrowIfRootNodeDefinedAndPassed(int emptyParentKeyCount, ISiteMapNode rootNode, IEnumerable<IMvcSiteMapNodeAttributeDefinition> definitions)
        {
            // Throw an error if we were passed a rootNode and there is also a rootnode configured on an attribute
            if (emptyParentKeyCount == 1 && rootNode != null)
            {
                var item = definitions.Where(t => string.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)).Single();
                throw new MvcSiteMapException(String.Format(Resources.Messages.ReflectionSiteMapBuilderRootKeyAmbiguousAcrossBuilders, 
                    this.siteMapAssemblyService.SiteMapCacheKey, rootNode.Key, item.SiteMapNodeAttribute.Key));
            }
        }

        protected virtual IList<ISiteMapNodeParentMap> CreateNodesFromAttributeDefinitions(ISiteMap siteMap, IEnumerable<IMvcSiteMapNodeAttributeDefinition> definitions)
        {
            var result = new List<ISiteMapNodeParentMap>();
            foreach (var definition in definitions.Where(t => !String.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)))
            {
                var node = this.CreateNodeFromAttributeDefinition(siteMap, definition);
                if (node != null)
                {
                    result.Add(this.siteMapAssemblyService.CreateSiteMapNodeParentMap(definition.SiteMapNodeAttribute.ParentKey, node, "MvcSiteMapNodeAttribute"));
                }
                else
                {
                    // Throw exception in the case where the definition didn't result in a node being created.
                    var attribute = definition.SiteMapNodeAttribute;
                    var nodeDescription = String.Format(Resources.Messages.SiteMapNodeFormatWithParentKey, attribute.ParentKey, "Unknown", "Unknown", attribute.AreaName, attribute.Url, attribute.Key, "MvcSiteMapNodeAttribute");
                    throw new MvcSiteMapException(String.Format(Resources.Messages.ReflectionSiteMapBuilderNodeCouldNotBeCreated, this.siteMapAssemblyService.SiteMapCacheKey, nodeDescription));
                }
            }
            return result;
        }

        protected virtual ISiteMapNode CreateNodeFromAttributeDefinition(ISiteMap siteMap, IMvcSiteMapNodeAttributeDefinition definition)
        {
            ISiteMapNode result = null;

            // Create node
            var actionNode = definition as MvcSiteMapNodeAttributeDefinitionForAction;
            if (actionNode != null)
            {
                // Create node for action
                result = GetSiteMapNodeFromMvcSiteMapNodeAttribute(
                    siteMap, actionNode.SiteMapNodeAttribute, actionNode.ControllerType, actionNode.ActionMethodInfo);
            }
            else
            {
                var controllerNode = definition as MvcSiteMapNodeAttributeDefinitionForController;
                if (controllerNode != null)
                {
                    // Create node for controller
                    result = GetSiteMapNodeFromMvcSiteMapNodeAttribute(
                        siteMap, controllerNode.SiteMapNodeAttribute, controllerNode.ControllerType, null);
                }
            }
            return result;
        }

        protected virtual IList<ISiteMapNodeParentMap> CreateDynamicNodes(ISiteMap siteMap, IList<ISiteMapNodeParentMap> sourceNodes)
        {
            var result = new List<ISiteMapNodeParentMap>();
            foreach (var sourceNode in sourceNodes.Where(x => x.Node.HasDynamicNodeProvider).ToArray())
            {
                result.AddRange(this.siteMapAssemblyService.BuildDynamicNodeParentMaps(siteMap, sourceNode.Node, sourceNode.ParentKey));

                // Remove the dynamic node from the sources - we are replacing its definition.
                sourceNodes.Remove(sourceNode);
            }
            return result;
        }

        /// <summary>
        /// Gets the site map node from MVC site map node attribute.
        /// </summary>
        /// <param name="siteMap">The site map.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="type">The type.</param>
        /// <param name="methodInfo">The method info.</param>
        /// <returns></returns>
        protected virtual ISiteMapNode GetSiteMapNodeFromMvcSiteMapNodeAttribute(ISiteMap siteMap, IMvcSiteMapNodeAttribute attribute, Type type, MethodInfo methodInfo)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (!String.IsNullOrEmpty(attribute.SiteMapCacheKey))
            {
                // Return null if the attribute doesn't apply to this cache key
                if (!this.siteMapAssemblyService.SiteMapCacheKey.Equals(attribute.SiteMapCacheKey))
                {
                    return null;
                }
            }

            if (methodInfo == null) // try to find Index action
            {
                var ms = type.FindMembers(MemberTypes.Method, BindingFlags.Instance | BindingFlags.Public,
                                          (mi, o) => mi != null && string.Equals(mi.Name, "Index"), null);
                foreach (MethodInfo m in ms.OfType<MethodInfo>())
                {
                    var pars = m.GetParameters();
                    if (pars.Length == 0)
                    {
                        methodInfo = m;
                        break;
                    }
                }
            }

            // Determine area (will only work if controller is defined as Assembly.<Area>.Controllers.HomeController)
            string area = "";
            if (!string.IsNullOrEmpty(attribute.AreaName))
            {
                area = attribute.AreaName;
            }
            if (string.IsNullOrEmpty(area))
            {
                var parts = type.Namespace.Split('.');
                area = parts[parts.Length - 2];

                var assemblyParts = type.Assembly.FullName.Split(',');

                if (type.Namespace == assemblyParts[0] + ".Controllers" || type.Namespace.StartsWith(area))
                {
                    // Is in default areaName...
                    area = "";
                }
            }

            // Determine controller and (index) action
            string controller = type.Name.Substring(0, type.Name.IndexOf("Controller"));
            string action = (methodInfo != null ? methodInfo.Name : null) ?? "Index";
            string httpMethod = "*"; 
            if (methodInfo != null)
            {
                // handle ActionNameAttribute
                var actionNameAttribute = methodInfo.GetCustomAttributes(typeof(ActionNameAttribute), true).FirstOrDefault() as ActionNameAttribute;
                if (actionNameAttribute != null)
                {
                    action = actionNameAttribute.Name;
                }

                // handle AcceptVerbsAttribute
                var acceptVerbsAttribute = methodInfo.GetCustomAttributes(typeof(AcceptVerbsAttribute), true).FirstOrDefault() as AcceptVerbsAttribute;
                if (acceptVerbsAttribute != null)
                {
                    httpMethod = string.Join(",", acceptVerbsAttribute.Verbs.ToArray());
                }
            }

            // Handle title and description
            var title = attribute.Title;
            var description = String.IsNullOrEmpty(attribute.Description) ? title : attribute.Description;

            // Handle implicit resources
            var implicitResourceKey = attribute.ResourceKey;

            // Generate key for node
            string key = this.siteMapAssemblyService.GenerateSiteMapNodeKey(
                null,
                attribute.Key,
                "",
                title,
                area,
                controller, action, httpMethod,
                attribute.Clickable);

            var siteMapNode = this.siteMapAssemblyService.CreateSiteMapNode(siteMap, key, implicitResourceKey);

            // Assign defaults
            siteMapNode.Title = title;
            siteMapNode.Description = description;
            AcquireAttributesFrom(attribute, siteMapNode.Attributes);
            AcquireRolesFrom(attribute, siteMapNode.Roles);
            siteMapNode.Clickable = attribute.Clickable;
            siteMapNode.VisibilityProvider = attribute.VisibilityProvider;
            siteMapNode.DynamicNodeProvider = attribute.DynamicNodeProvider;
            siteMapNode.ImageUrl = attribute.ImageUrl;
            siteMapNode.TargetFrame = attribute.TargetFrame;
            siteMapNode.HttpMethod = httpMethod;
            if (!string.IsNullOrEmpty(attribute.Url)) siteMapNode.Url = attribute.Url;
            siteMapNode.CacheResolvedUrl = attribute.CacheResolvedUrl;
            siteMapNode.CanonicalUrl = attribute.CanonicalUrl;
            siteMapNode.CanonicalKey = attribute.CanonicalKey;
            AcquireMetaRobotsValuesFrom(attribute, siteMapNode.MetaRobotsValues);
            siteMapNode.LastModifiedDate = attribute.LastModifiedDate;
            siteMapNode.ChangeFrequency = attribute.ChangeFrequency;
            siteMapNode.UpdatePriority = attribute.UpdatePriority;
            siteMapNode.Order = attribute.Order;

            // Handle route details
            siteMapNode.Route = attribute.Route;
            AcquireRouteValuesFrom(attribute, siteMapNode.RouteValues);
            AcquirePreservedRouteParametersFrom(attribute, siteMapNode.PreservedRouteParameters);
            siteMapNode.UrlResolver = attribute.UrlResolver;

            // Specified area, controller and action properties will override any 
            // provided in the attributes collection.
            if (!string.IsNullOrEmpty(area)) siteMapNode.RouteValues.Add("area", area);
            if (!string.IsNullOrEmpty(controller)) siteMapNode.RouteValues.Add("controller", controller);
            if (!string.IsNullOrEmpty(action)) siteMapNode.RouteValues.Add("action", action);

            // Handle MVC details

            // Add defaults for area
            if (!siteMapNode.RouteValues.ContainsKey("area"))
            {
                siteMapNode.RouteValues.Add("area", "");
            }

            return siteMapNode;
        }

        /// <summary>
        /// Acquires the attributes from a given <see cref="T:IMvcSiteMapNodeAttribute"/>
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        protected virtual void AcquireAttributesFrom(IMvcSiteMapNodeAttribute attribute, IDictionary<string, object> attributes)
        {
            foreach (var att in attribute.Attributes)
            {
                var attributeName = att.Key.ToString();
                var attributeValue = att.Value;

                if (reservedAttributeNameProvider.IsRegularAttribute(attributeName))
                {
                    attributes[attributeName] = attributeValue;
                }
            }
        }

        /// <summary>
        /// Acquires the route values from a given XElement.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        protected virtual void AcquireRouteValuesFrom(IMvcSiteMapNodeAttribute attribute, IRouteValueDictionary routeValues)
        {
            foreach (var att in attribute.Attributes)
            {
                var attributeName = att.Key.ToString();
                var attributeValue = att.Value;

                if (reservedAttributeNameProvider.IsRouteAttribute(attributeName))
                {
                    routeValues[attributeName] = attributeValue;
                }
            }
        }

        /// <summary>
        /// Acquires the roles list from a given <see cref="T:IMvcSiteMapNodeAttribute"/>
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="roles">The roles IList to populate.</param>
        protected virtual void AcquireRolesFrom(IMvcSiteMapNodeAttribute attribute, IList<string> roles)
        {
            if (attribute.Roles != null)
            {
                foreach (var role in attribute.Roles)
                {
                    roles.Add(role);
                }
            }
        }

        /// <summary>
        /// Acquires the meta robots values list from a given <see cref="T:IMvcSiteMapNodeAttribute"/>
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="metaRobotsValues">The meta robots values IList to populate.</param>
        protected virtual void AcquireMetaRobotsValuesFrom(IMvcSiteMapNodeAttribute attribute, IList<string> metaRobotsValues)
        {
            if (attribute.MetaRobotsValues != null)
            {
                foreach (var value in attribute.MetaRobotsValues)
                {
                    metaRobotsValues.Add(value);
                }
            }
        }

        /// <summary>
        /// Acquires the preserved route parameters list from a given <see cref="T:IMvcSiteMapNodeAttribute"/>
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="preservedRouteParameters">The preserved route parameters IList to populate.</param>
        protected virtual void AcquirePreservedRouteParametersFrom(IMvcSiteMapNodeAttribute attribute, IList<string> preservedRouteParameters)
        {
            var localParameters = (attribute.PreservedRouteParameters ?? "").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var parameter in localParameters)
            {
                preservedRouteParameters.Add(parameter);
            }
        }

    }
}
