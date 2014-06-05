using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Web.Script.Serialization;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// ReflectionSiteMapBuilder class (copied from ReflectionSiteMapSource class). 
    /// Builds a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> tree based on a
    /// set of attributes within an assembly.
    /// </summary>
    [Obsolete("ReflectionSiteMapBuilder has been deprecated and will be removed in version 5. Use ReflectionSiteMapNodeProvider in conjunction with SiteMapBuilder instead.")]
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
        /// that early in the application life-cycle when run in IIS Integrated mode.
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
            var emptyParentKeyCount = definitions.Where(t => string.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)).Count();

            // Throw a sensible exception if the configuration has more than 1 empty parent key (#179).
            if (emptyParentKeyCount > 1)
            {
                throw new MvcSiteMapException(Resources.Messages.ReflectionSiteMapBuilderRootKeyAmbiguous);
            }

            // Find root node
            if (parentNode == null)
            {
                if (emptyParentKeyCount == 1)
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

            // Fixes #192 root node not added to sitemap
            if (siteMap.FindSiteMapNodeFromKey(parentNode.Key) == null)
            {
                // Add the root node to the sitemap
                siteMap.AddNode(parentNode);
            }

            // Create nodes
            foreach (var assemblyNode in definitions.Where(t => !string.IsNullOrEmpty(t.SiteMapNodeAttribute.ParentKey)))
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
                                if (dynamicNode.ParentNode.Equals(parentNode)
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
                                if (dynamicNode.ParentNode.Equals(parentNode)
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

            if (!string.IsNullOrEmpty(attribute.SiteMapCacheKey))
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

            string area = "";
            if (!string.IsNullOrEmpty(attribute.AreaName))
            {
                area = attribute.AreaName;
            }
            if (string.IsNullOrEmpty(area) && !string.IsNullOrEmpty(attribute.Area))
            {
                area = attribute.Area;
            }
            // Determine area (will only work if controller is defined as [<Anything>.]Areas.<Area>.Controllers.<AnyController>)
            if (string.IsNullOrEmpty(area))
            {
                var m = Regex.Match(type.Namespace, @"(?:[^\.]+\.|\s+|^)Areas\.(?<areaName>[^\.]+)\.Controllers");
                if (m.Success)
                {
                    area = m.Groups["areaName"].Value;
                }
            }

            // Determine controller and (index) action
            string controller = type.Name.Substring(0, type.Name.IndexOf("Controller"));
            string action = (methodInfo != null ? methodInfo.Name : null) ?? "Index";
            if (methodInfo != null)
            {
                // handle ActionNameAttribute
                var actionNameAttribute = methodInfo.GetCustomAttributes(typeof(ActionNameAttribute), true).FirstOrDefault() as ActionNameAttribute;
                if (actionNameAttribute != null)
                {
                    action = actionNameAttribute.Name;
                }
            }

            string httpMethod = string.IsNullOrEmpty(attribute.HttpMethod) ? HttpVerbs.Get.ToString().ToUpperInvariant() : attribute.HttpMethod.ToUpperInvariant();

            // Handle title
            var title = attribute.Title;

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
            siteMapNode.Description = attribute.Description;
            siteMapNode.Attributes.AddRange(attribute.Attributes, false);
            siteMapNode.Roles.AddRange(attribute.Roles);
            siteMapNode.Clickable = attribute.Clickable;
            siteMapNode.VisibilityProvider = attribute.VisibilityProvider;
            siteMapNode.DynamicNodeProvider = attribute.DynamicNodeProvider;
            siteMapNode.ImageUrl = attribute.ImageUrl;
            siteMapNode.ImageUrlProtocol = attribute.ImageUrlProtocol;
            siteMapNode.ImageUrlHostName = attribute.ImageUrlHostName;
            siteMapNode.TargetFrame = attribute.TargetFrame;
            siteMapNode.HttpMethod = httpMethod;
            if (!string.IsNullOrEmpty(attribute.Url)) siteMapNode.Url = attribute.Url;
            siteMapNode.CacheResolvedUrl = attribute.CacheResolvedUrl;
            siteMapNode.IncludeAmbientValuesInUrl = attribute.IncludeAmbientValuesInUrl;
            siteMapNode.Protocol = attribute.Protocol;
            siteMapNode.HostName = attribute.HostName;
            siteMapNode.CanonicalKey = attribute.CanonicalKey;
            siteMapNode.CanonicalUrl = attribute.CanonicalUrl;
            siteMapNode.CanonicalUrlProtocol = attribute.CanonicalUrlProtocol;
            siteMapNode.CanonicalUrlHostName = attribute.CanonicalUrlHostName;
            siteMapNode.MetaRobotsValues.AddRange(attribute.MetaRobotsValues);
            siteMapNode.LastModifiedDate = string.IsNullOrEmpty(attribute.LastModifiedDate) ? DateTime.MinValue : DateTime.Parse(attribute.LastModifiedDate, CultureInfo.InvariantCulture);
            siteMapNode.ChangeFrequency = attribute.ChangeFrequency;
            siteMapNode.UpdatePriority = attribute.UpdatePriority;
            siteMapNode.Order = attribute.Order;

            // Handle route details
            siteMapNode.Route = attribute.Route;
            siteMapNode.RouteValues.AddRange(attribute.Attributes, false);
            siteMapNode.PreservedRouteParameters.AddRange(attribute.PreservedRouteParameters, new[] { ',', ';' });
            siteMapNode.UrlResolver = attribute.UrlResolver;
            
            // Specified area, controller and action properties will override any 
            // provided in the attributes collection.
            if (!string.IsNullOrEmpty(area)) siteMapNode.RouteValues.Add("area", area);
            if (!string.IsNullOrEmpty(controller)) siteMapNode.RouteValues.Add("controller", controller);
            if (!string.IsNullOrEmpty(action)) siteMapNode.RouteValues.Add("action", action);

            return siteMapNode;
        }
    }
}