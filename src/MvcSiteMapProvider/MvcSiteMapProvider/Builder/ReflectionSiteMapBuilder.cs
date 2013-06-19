using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Collections.Specialized;

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
        }
        protected readonly IEnumerable<string> includeAssemblies;
        protected readonly IEnumerable<string> excludeAssemblies;
        protected readonly ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider;
        protected readonly INodeKeyGenerator nodeKeyGenerator;
        protected readonly IDynamicNodeBuilder dynamicNodeBuilder;
        protected readonly ISiteMapNodeFactory siteMapNodeFactory;
        protected readonly ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator;


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


        /// <summary>
        /// Provides the base data on which the context-aware provider can generate a full tree.
        /// </summary>
        /// <param name="siteMap">The siteMap object to populate with the data.</param>
        /// <returns></returns>
        public virtual ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode)
        {
            // List of assemblies
            IEnumerable<Assembly> assemblies;
            if (includeAssemblies.Any())
            {
                // An include list is given
                assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => includeAssemblies.Contains(new AssemblyName(a.FullName).Name));
            }
            else
            {
                // An exclude list is given
                assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.FullName.StartsWith("mscorlib")
                                && !a.FullName.StartsWith("System")
                                && !a.FullName.StartsWith("Microsoft")
                                && !a.FullName.StartsWith("WebDev")
                                && !a.FullName.StartsWith("SMDiagnostics")
                                && !a.FullName.StartsWith("Anonymously")
                                && !a.FullName.StartsWith("App_")
                                && !excludeAssemblies.Contains(new AssemblyName(a.FullName).Name));
            }

            foreach (Assembly assembly in assemblies)
            {
                // http://stackoverflow.com/questions/1423733/how-to-tell-if-a-net-assembly-is-dynamic
                if (!(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder)
                    && assembly.ManifestModule.GetType().Namespace != "System.Reflection.Emit")
                {
                    rootNode = ProcessNodesInAssembly(siteMap, assembly, rootNode);
                }
            }

            // Done!
            return rootNode;
        }

        /// <summary>
        /// Processes the nodes in assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        protected virtual ISiteMapNode ProcessNodesInAssembly(ISiteMap siteMap, Assembly assembly, ISiteMapNode parentNode)
        {
            // Create a list of all nodes defined in the assembly
            var assemblyNodes = new List<IMvcSiteMapNodeAttributeDefinition>();

            // Retrieve types
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types;
            }

            // Add all types
            foreach (Type type in types)
            {
                var attributes = type.GetCustomAttributes(typeof(IMvcSiteMapNodeAttribute), true) as IMvcSiteMapNodeAttribute[];
                foreach (var attribute in attributes)
                {
                    assemblyNodes.Add(new MvcSiteMapNodeAttributeDefinitionForController
                    {
                        SiteMapNodeAttribute = attribute,
                        ControllerType = type
                    });
                }
                

                // Add their methods
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.GetCustomAttributes(typeof(IMvcSiteMapNodeAttribute), true).Any());

                foreach (var method in methods)
                {
                    attributes = method.GetCustomAttributes(typeof(IMvcSiteMapNodeAttribute), false) as IMvcSiteMapNodeAttribute[];
                    foreach (var attribute in attributes)
                    {
                        assemblyNodes.Add(new MvcSiteMapNodeAttributeDefinitionForAction
                        {
                            SiteMapNodeAttribute = attribute,
                            ControllerType = type,
                            ActionMethodInfo = method
                        });
                    }
                }
            }

            // Create nodes from MVC site map node attribute definitions
            return CreateNodesFromMvcSiteMapNodeAttributeDefinitions(siteMap, parentNode, assemblyNodes.OrderBy(n => n.SiteMapNodeAttribute.Order));
        }

        /// <summary>
        /// Creates the nodes from MVC site map node attribute definitions.
        /// </summary>
        /// <param name="definitions">The definitions.</param>
        protected virtual ISiteMapNode CreateNodesFromMvcSiteMapNodeAttributeDefinitions(ISiteMap siteMap, ISiteMapNode parentNode, IEnumerable<IMvcSiteMapNodeAttributeDefinition> definitions)
        {
            // A dictionary of nodes to process later (node, parentKey)
            var nodesToProcessLater = new Dictionary<ISiteMapNode, string>();

            // Find root node
            if (parentNode == null)
            {
                if (definitions.Where(t => string.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)).Count() == 1)
                {
                    ISiteMapNode attributedRootNode = null;

                    var item = definitions.Where(t => string.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)).Single();

                    var actionNode = item as MvcSiteMapNodeAttributeDefinitionForAction;
                    if (actionNode != null)
                    {
                        // Create node for action
                        attributedRootNode = GetSiteMapNodeFromMvcSiteMapNodeAttribute(
                            siteMap, actionNode.SiteMapNodeAttribute, actionNode.ControllerType, actionNode.ActionMethodInfo);
                    }
                    else
                    {
                        var controllerNode = item as MvcSiteMapNodeAttributeDefinitionForController;
                        if (controllerNode != null)
                        {
                            // Create node for controller
                            attributedRootNode = GetSiteMapNodeFromMvcSiteMapNodeAttribute(
                                siteMap, controllerNode.SiteMapNodeAttribute, controllerNode.ControllerType, null);
                        }
                    }

                    if (attributedRootNode.Attributes.ContainsKey("parentKey"))
                    {
                        attributedRootNode.Attributes.Remove("parentKey");
                    }
                    parentNode = attributedRootNode;
                }
            }

            // Create nodes
            foreach (var assemblyNode in definitions.Where(t => !String.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)))
            {
                ISiteMapNode nodeToAdd = null;

                // Create node
                var actionNode = assemblyNode as MvcSiteMapNodeAttributeDefinitionForAction;
                if (actionNode != null)
                {
                    // Create node for action
                    nodeToAdd = GetSiteMapNodeFromMvcSiteMapNodeAttribute(
                        siteMap, actionNode.SiteMapNodeAttribute, actionNode.ControllerType, actionNode.ActionMethodInfo);
                }
                else
                {
                    var controllerNode = assemblyNode as MvcSiteMapNodeAttributeDefinitionForController;
                    if (controllerNode != null)
                    {
                        // Create node for controller
                        nodeToAdd = GetSiteMapNodeFromMvcSiteMapNodeAttribute(
                            siteMap, controllerNode.SiteMapNodeAttribute, controllerNode.ControllerType, null);
                    }
                }

                // Add node
                if (nodeToAdd != null)
                {
                    if (string.IsNullOrEmpty(assemblyNode.SiteMapNodeAttribute.ParentKey))
                    {
                        throw new MvcSiteMapException(string.Format(Resources.Messages.NoParentKeyDefined, nodeToAdd.Controller, nodeToAdd.Action));
                    }

                    var parentForNode = parentNode != null ? siteMap.FindSiteMapNodeFromKey(assemblyNode.SiteMapNodeAttribute.ParentKey) : null;

                    if (parentForNode != null)
                    {
                        if (nodeToAdd.HasDynamicNodeProvider)
                        {
                            var dynamicNodesForChildNode = dynamicNodeBuilder.BuildDynamicNodesFor(siteMap, nodeToAdd, parentForNode);
                            foreach (var dynamicNode in dynamicNodesForChildNode)
                            {
                                // Verify parent/child relation
                                if (dynamicNode.ParentNode == parentNode
                                    && !siteMap.GetChildNodes(parentNode).Contains(dynamicNode))
                                {
                                    siteMap.AddNode(dynamicNode, parentNode);
                                }
                            }
                        }
                        else
                        {
                            siteMap.AddNode(nodeToAdd, parentForNode);
                        }
                    }
                    else
                    {
                        nodesToProcessLater.Add(nodeToAdd, assemblyNode.SiteMapNodeAttribute.ParentKey);
                    }
                }
            }

            // Process list of nodes that did not have a parent defined.
            // If this does not succeed at this time, parent will default to root node.
            if (parentNode != null)
            {
                foreach (var nodeToAdd in nodesToProcessLater)
                {
                    var parentForNode = siteMap.FindSiteMapNodeFromKey(nodeToAdd.Value);
                    if (parentForNode == null)
                    {
                        var temp = nodesToProcessLater.Keys.Where(t => t.Key == nodeToAdd.Value).FirstOrDefault();
                        if (temp != null)
                        {
                            parentNode = temp;
                        }
                    }
                    if (parentForNode != null)
                    {
                        if (nodeToAdd.Key.HasDynamicNodeProvider)
                        {
                            var dynamicNodesForChildNode = dynamicNodeBuilder.BuildDynamicNodesFor(siteMap, nodeToAdd.Key, parentForNode);
                            foreach (var dynamicNode in dynamicNodesForChildNode)
                            {
                                // Verify parent/child relation
                                if (dynamicNode.ParentNode == parentNode
                                    && !siteMap.GetChildNodes(parentNode).Contains(dynamicNode))
                                {
                                    siteMap.AddNode(dynamicNode, parentNode);
                                }
                            }
                        }
                        else
                        {
                            siteMap.AddNode(nodeToAdd.Key, parentForNode);
                        }
                    }
                }
            }

            return parentNode;
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
            var description = attribute.Description;

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
        protected virtual void AcquireRouteValuesFrom(IMvcSiteMapNodeAttribute attribute, IRouteValueCollection routeValues)
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
