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
    /// ReflectionSiteMapBuilder class (copied from ReflectionSiteMapSource class). 
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
            INodeKeyGenerator nodeKeyGenerator,
            IDynamicNodeBuilder dynamicNodeBuilder,
            ISiteMapNodeFactory siteMapNodeFactory,
            ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator
            )
        {
            if (includeAssemblies == null)
                throw new ArgumentNullException("includeAssemblies");
            if (excludeAssemblies == null)
                throw new ArgumentNullException("excludeAssemblies");
            if (reservedAttributeNameProvider == null)
                throw new ArgumentNullException("reservedAttributeNameProvider");
            if (nodeKeyGenerator == null)
                throw new ArgumentNullException("nodeKeyGenerator");
            if (dynamicNodeBuilder == null)
                throw new ArgumentNullException("dynamicNodeBuilder");
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");
            if (siteMapCacheKeyGenerator == null)
                throw new ArgumentNullException("siteMapCacheKeyGenerator");

            this.includeAssemblies = includeAssemblies;
            this.excludeAssemblies = excludeAssemblies;
            this.reservedAttributeNameProvider = reservedAttributeNameProvider;
            this.nodeKeyGenerator = nodeKeyGenerator;
            this.dynamicNodeBuilder = dynamicNodeBuilder;
            this.siteMapNodeFactory = siteMapNodeFactory;
            this.siteMapCacheKeyGenerator = siteMapCacheKeyGenerator;

            // TODO: Inject in constructor or make factory
            this.assemblyProvider = new AttributeAssemblyProvider(this.includeAssemblies, this.excludeAssemblies);
            this.attributeNodeDefinitionProvider = new MvcSiteMapNodeAttributeDefinitionProvider();
        }
        protected readonly IEnumerable<string> includeAssemblies;
        protected readonly IEnumerable<string> excludeAssemblies;
        protected readonly ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider;
        protected readonly INodeKeyGenerator nodeKeyGenerator;
        protected readonly IDynamicNodeBuilder dynamicNodeBuilder;
        protected readonly ISiteMapNodeFactory siteMapNodeFactory;
        protected readonly ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator;
        protected readonly IAttributeAssemblyProvider assemblyProvider;
        protected readonly IMvcSiteMapNodeAttributeDefinitionProvider attributeNodeDefinitionProvider;


        protected string siteMapCacheKey;
        /// <summary>
        /// Gets the cache key for the current request and caches it, since this class should only be called 1 time per request.
        /// </summary>
        /// <remarks>
        /// Fixes #158 - this key should not be generated in the constructor because HttpContext cannot be accessed
        /// that early in the application lifecycle when run in IIS Integrated mode.
        /// </remarks>
        protected virtual string SiteMapCacheKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.siteMapCacheKey))
                {
                    this.siteMapCacheKey = siteMapCacheKeyGenerator.GenerateKey();
                }
                return this.siteMapCacheKey;
            }
        }

        #region ISiteMapBuilder Members
        

        /// <summary>
        /// Provides the base data on which the context-aware provider can generate a full tree.
        /// </summary>
        /// <param name="siteMap">The siteMap object to populate with the data.</param>
        /// <returns></returns>
        public virtual ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode)
        {
            var assemblies = this.assemblyProvider.GetAssemblies();
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
            var sourceNodes = CreateNodesFromAttributeDefinitions(siteMap, definitions);           
            var nodesAddedThisIteration = 0;
            do
            {
                var nodesAlreadyAdded = new List<string>();
                nodesAddedThisIteration = 0;
                foreach (var node in sourceNodes.OrderBy(x => x.Key.Order).ToArray())
                {
                    if (nodesAlreadyAdded.Contains(node.Key.Key))
                        continue;

                    var parentNode = siteMap.FindSiteMapNodeFromKey(node.Value);
                    if (parentNode != null)
                    {
                        this.AddAndTrackNode(siteMap, node, parentNode, sourceNodes, nodesAlreadyAdded);
                        nodesAddedThisIteration += 1;

                        // Add the rest of the tree branch below the current node
                        this.AddDescendantNodes(siteMap, node.Key, sourceNodes, nodesAlreadyAdded);
                    }
                }
            } while (nodesAddedThisIteration > 0 && sourceNodes.Count > 0);

            if (sourceNodes.Count > 0)
            {
                // We have orphaned nodes - throw an exception.
                var names = String.Join(Environment.NewLine, sourceNodes.Select(x => String.Format(Resources.Messages.SiteMapNodeFormatWithParentKey, x.Value, x.Key.Controller, x.Key.Action, x.Key.Url)));
                throw new MvcSiteMapException(String.Format(Resources.Messages.ReflectionSiteMapBuilderOrphanedNodes, this.SiteMapCacheKey, names));
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
                var item = definitions.Where(t => string.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)).Single();
                rootNode = this.CreateNodeFromAttributeDefinition(siteMap, item);

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
                throw new MvcSiteMapException(String.Format(Resources.Messages.ReflectionSiteMapBuilderRootKeyAmbiguous, this.SiteMapCacheKey, names));
            }
        }

        protected virtual void ThrowIfRootNodeDefinedAndPassed(int emptyParentKeyCount, ISiteMapNode rootNode, IEnumerable<IMvcSiteMapNodeAttributeDefinition> definitions)
        {
            // Throw an error if we were passed a rootNode and there is also a rootnode configured on an attribute
            if (emptyParentKeyCount == 1 && rootNode != null)
            {
                var item = definitions.Where(t => string.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)).Single();
                throw new MvcSiteMapException(String.Format(Resources.Messages.ReflectionSiteMapBuilderRootKeyAmbiguousAcrossBuilders, 
                    this.SiteMapCacheKey, rootNode.Key, item.SiteMapNodeAttribute.Key));
            }
        }

        protected virtual IList<KeyValuePair<ISiteMapNode, string>> CreateNodesFromAttributeDefinitions(ISiteMap siteMap, IEnumerable<IMvcSiteMapNodeAttributeDefinition> definitions)
        {
            var result = new List<KeyValuePair<ISiteMapNode, string>>();
            foreach (var definition in definitions.Where(t => !String.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)))
            {
                var node = this.CreateNodeFromAttributeDefinition(siteMap, definition);
                if (node != null)
                {
                    result.Add(new KeyValuePair<ISiteMapNode, string>(node, definition.SiteMapNodeAttribute.ParentKey));
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

        protected virtual void AddDescendantNodes(ISiteMap siteMap, ISiteMapNode currentNode, IList<KeyValuePair<ISiteMapNode, string>> sourceNodes, IList<string> nodesAlreadyAdded)
        {
            if (sourceNodes.Count == 0) return;
            var children = sourceNodes.Where(x => x.Value == currentNode.Key).OrderBy(x => x.Key.Order).ToArray();
            if (children.Count() == 0) return;
            foreach (var child in children)
            {
                if (sourceNodes.Count == 0) return;
                this.AddAndTrackNode(siteMap, child, currentNode, sourceNodes, nodesAlreadyAdded);
                
                if (sourceNodes.Count == 0) return;
                this.AddDescendantNodes(siteMap, child.Key, sourceNodes, nodesAlreadyAdded);
            }
        }

        protected virtual void AddAndTrackNode(ISiteMap siteMap, KeyValuePair<ISiteMapNode, string> node, ISiteMapNode parentNode, IList<KeyValuePair<ISiteMapNode, string>> sourceNodes, IList<string> nodesAlreadyAdded)
        {
            this.AddNode(siteMap, node.Key, parentNode);
            nodesAlreadyAdded.Add(node.Key.Key);
            sourceNodes.Remove(node);
        }

        protected virtual void AddNode(ISiteMap siteMap, ISiteMapNode node, ISiteMapNode parentNode)
        {
            if (!node.HasDynamicNodeProvider)
            {
                siteMap.AddNode(node, parentNode);
            }
            else
            {
                var dynamicNodes = dynamicNodeBuilder.BuildDynamicNodesFor(siteMap, node, parentNode);
                foreach (var dynamicNode in dynamicNodes)
                {
                    // Verify parent/child relation
                    if (dynamicNode.ParentNode == parentNode
                        && !siteMap.GetChildNodes(parentNode).Contains(dynamicNode))
                    {
                        siteMap.AddNode(dynamicNode, parentNode);
                    }
                }
            }
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
                if (!this.SiteMapCacheKey.Equals(attribute.SiteMapCacheKey))
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
            string key = nodeKeyGenerator.GenerateKey(
                null,
                attribute.Key,
                "",
                title,
                area,
                controller, action, httpMethod,
                attribute.Clickable);

            var siteMapNode = siteMapNodeFactory.Create(siteMap, key, implicitResourceKey);

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
