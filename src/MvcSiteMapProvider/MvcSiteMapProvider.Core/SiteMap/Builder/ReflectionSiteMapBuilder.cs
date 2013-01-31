// -----------------------------------------------------------------------
// <copyright file="ReflectionSiteMapBuilder.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web;
    using MvcSiteMapProvider.Core.Globalization;

    /// <summary>
    /// ReflectionSiteMapBuilder class (copied from ReflectionSiteMapSource class).
    /// </summary>
    public class ReflectionSiteMapBuilder : ISiteMapBuilder
    {
        public ReflectionSiteMapBuilder(
            IEnumerable<String> includeAssemblies,
            IEnumerable<String> excludeAssemblies,
            INodeKeyGenerator nodeKeyGenerator,
            INodeLocalizer nodeLocalizer,
            IDynamicNodeBuilder dynamicNodeBuilder,
            ISiteMapNodeFactory siteMapNodeFactory
            )
        {
            if (includeAssemblies == null)
                throw new ArgumentNullException("includeAssemblies");
            if (excludeAssemblies == null)
                throw new ArgumentNullException("excludeAssemblies");
            if (nodeKeyGenerator == null)
                throw new ArgumentNullException("nodeKeyGenerator");
            if (nodeLocalizer == null)
                throw new ArgumentNullException("nodeLocalizer");
            if (dynamicNodeBuilder == null)
                throw new ArgumentNullException("dynamicNodeBuilder");
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");

            this.IncludeAssemblies = includeAssemblies;
            this.ExcludeAssemblies = excludeAssemblies;
            this.nodeKeyGenerator = nodeKeyGenerator;
            this.nodeLocalizer = nodeLocalizer;
            this.dynamicNodeBuilder = dynamicNodeBuilder;
            this.siteMapNodeFactory = siteMapNodeFactory;
        }

        //protected readonly ISiteMapNodeVisibilityProvider siteMapNodeVisibilityProvider;
        protected readonly INodeKeyGenerator nodeKeyGenerator;
        protected readonly INodeLocalizer nodeLocalizer;
        protected readonly IDynamicNodeBuilder dynamicNodeBuilder;
        protected readonly ISiteMapNodeFactory siteMapNodeFactory;


        /// <summary>
        /// Gets or sets the include assemblies.
        /// </summary>
        /// <value>
        /// The include assemblies.
        /// </value>
        protected IEnumerable<string> IncludeAssemblies { get; set; }

        /// <summary>
        /// Gets or sets the exclude assemblies.
        /// </summary>
        /// <value>
        /// The exclude assemblies.
        /// </value>
        protected IEnumerable<string> ExcludeAssemblies { get; set; }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="ReflectionSiteMapSource" /> class.
        ///// </summary>
        ///// <param name="providerName">Name of the provider.</param>
        //public ReflectionSiteMapSource(string providerName)
        //    : this(providerName, null, null)
        //{  
        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="ReflectionSiteMapSource" /> class.
        ///// </summary>
        ///// <param name="providerName">Name of the provider.</param>
        ///// <param name="includeAssemblies">The include assemblies.</param>
        ///// <param name="excludeAssemblies">The exclude assemblies.</param>
        //public ReflectionSiteMapSource(string providerName, string includeAssemblies, string excludeAssemblies)
        //{
        //    ProviderName = providerName;
        //    if (!string.IsNullOrEmpty(includeAssemblies))
        //    {
        //        IncludeAssemblies = includeAssemblies.Split(new[] {',', ';'}, StringSplitOptions.RemoveEmptyEntries).ToList();
        //    }
        //    else
        //    {
        //        IncludeAssemblies = new List<string>();
        //    }
        //    if (!string.IsNullOrEmpty(excludeAssemblies))
        //    {
        //        ExcludeAssemblies = excludeAssemblies.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //    }
        //    else
        //    {
        //        ExcludeAssemblies = new List<string>();
        //    }
        //}

        /// <summary>
        /// Provides the base data on which the context-aware provider can generate a full tree.
        /// </summary>
        /// <param name="siteMap">The siteMap object to populate with the data.</param>
        /// <returns></returns>
        public virtual ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode)
        {
            // List of assemblies
            IEnumerable<Assembly> assemblies;
            if (IncludeAssemblies.Any())
            {
                // An include list is given
                assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => IncludeAssemblies.Contains(new AssemblyName(a.FullName).Name));
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
                                && !ExcludeAssemblies.Contains(new AssemblyName(a.FullName).Name));
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
            List<IMvcSiteMapNodeAttributeDefinition> assemblyNodes = new List<IMvcSiteMapNodeAttributeDefinition>();

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
                try
                {
                    var attribute = type.GetCustomAttributes(typeof(IMvcSiteMapNodeAttribute), true).FirstOrDefault() as IMvcSiteMapNodeAttribute;
                    if (attribute != null)
                    {
                        assemblyNodes.Add(new MvcSiteMapNodeAttributeDefinitionForController
                        {
                            SiteMapNodeAttribute = attribute,
                            ControllerType = type
                        });
                    }
                }
                catch
                {
                }

                // Add their methods
                try
                {
                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(x => x.GetCustomAttributes(typeof(IMvcSiteMapNodeAttribute), true).Any());

                    foreach (var method in methods)
                    {
                        var attributes = (IMvcSiteMapNodeAttribute[])method.GetCustomAttributes(typeof(IMvcSiteMapNodeAttribute), false);
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
                catch
                {
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
            Dictionary<ISiteMapNode, string> nodesToProcessLater
                = new Dictionary<ISiteMapNode, string>();

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

                    //var parentForNode = parentNode != null ? parentNode.FindForKey(assemblyNode.SiteMapNodeAttribute.ParentKey) : null;
                    var parentForNode = parentNode != null ? siteMap.FindSiteMapNodeFromKey(assemblyNode.SiteMapNodeAttribute.ParentKey) : null;

                    if (parentForNode != null)
                    {
                        nodeToAdd.ParentNode = parentForNode;

                        //if (dynamicNodeBuilder.HasDynamicNodes(nodeToAdd))
                        if (nodeToAdd.HasDynamicNodeProvider)
                        {
                            var dynamicNodesForChildNode = dynamicNodeBuilder.BuildDynamicNodesFor(siteMap, nodeToAdd, parentForNode);
                            foreach (var dynamicNode in dynamicNodesForChildNode)
                            {
                                // Verify parent/child relation
                                //if (dynamicNode.ParentNode == parentNode
                                //    && parentNode.ChildNodes.All(n => n != dynamicNode))
                                if (dynamicNode.ParentNode == parentNode
                                    && !siteMap.GetChildNodes(parentNode).Contains(dynamicNode))
                                {
                                    parentNode.ChildNodes.Add(dynamicNode);
                                }
                            }
                        }
                        else
                        {
                            parentNode.ChildNodes.Add(nodeToAdd);
                        }
                    }
                    else
                    {
                        nodesToProcessLater.Add(nodeToAdd, assemblyNode.SiteMapNodeAttribute.ParentKey);
                    }
                }
            }


            //// Process list of nodes that did not have a parent defined.
            //// If this does not succeed at this time, parent will default to root node.
            //foreach (var nodeToAdd in nodesToProcessLater)
            //{
            //    var parentNode = siteMap.FindSiteMapNodeFromKey(nodeToAdd.Value);
            //    if (parentNode == null)
            //    {
            //        var temp = nodesToProcessLater.Keys.FirstOrDefault(t => t.Key == nodeToAdd.Value);
            //        if (temp != null)
            //        {
            //            parentNode = temp;
            //        }
            //    }
            //    parentNode = parentNode ?? root;

            //    if (parentNode != null)
            //    {
            //        if (!HasDynamicNodes(nodeToAdd.Key))
            //        {
            //            AddNode(nodeToAdd.Key, parentNode);
            //        }
            //        else
            //        {
            //            AddDynamicNodesFor(nodeToAdd.Key, parentNode);
            //        }
            //    }
            //}

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
                        nodeToAdd.Key.ParentNode = parentForNode;

                        //if (dynamicNodeBuilder.HasDynamicNodes(nodeToAdd.Key))
                        if (nodeToAdd.Key.HasDynamicNodeProvider)
                        {
                            var dynamicNodesForChildNode = dynamicNodeBuilder.BuildDynamicNodesFor(siteMap, nodeToAdd.Key, parentForNode);
                            foreach (var dynamicNode in dynamicNodesForChildNode)
                            {
                                // Verify parent/child relation
                                //if (dynamicNode.ParentNode == parentNode
                                //    && parentNode.ChildNodes.All(n => n != dynamicNode))
                                if (dynamicNode.ParentNode == parentNode
                                    && !siteMap.GetChildNodes(parentNode).Contains(dynamicNode))
                                {
                                    parentNode.ChildNodes.Add(dynamicNode);
                                }
                            }
                        }
                        else
                        {
                            parentNode.ChildNodes.Add(nodeToAdd.Key);
                        }
                    }
                }
            }

            return parentNode;
        }

        /// <summary>
        /// Gets the site map node from MVC site map node attribute.
        /// </summary>
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
                    httpMethod = string.Join(",", acceptVerbsAttribute.Verbs);
                }
            }

            // Generate key for node
            string key = nodeKeyGenerator.GenerateKey(
                null,
                attribute.Key,
                "",
                attribute.Title,
                area,
                controller, action, httpMethod,
                attribute.Clickable);

            // Handle title and description globalization
            var explicitResourceKeys = new NameValueCollection();
            var title = attribute.Title;
            var description = attribute.Description;
            nodeLocalizer.HandleResourceAttribute("title", ref title, ref explicitResourceKeys);
            nodeLocalizer.HandleResourceAttribute("description", ref description, ref explicitResourceKeys);

            // Handle implicit resources
            var implicitResourceKey = attribute.ResourceKey;
            if (!string.IsNullOrEmpty(implicitResourceKey))
            {
                title = null;
                description = null;
            }

            // Assign defaults
            //var siteMapNode = new MvcSiteMapNode();
            var siteMapNode = siteMapNodeFactory.Create(siteMap, key, implicitResourceKey);
            //siteMapNode.Key = key;
            siteMapNode.Title = title;
            siteMapNode.Description = description;
            //siteMapNode.ResourceKey = implicitResourceKey;
            siteMapNode.Roles = attribute.Roles;
            siteMapNode.Clickable = attribute.Clickable;

            // TODO: Change semantics here so the providers cannot be overridden after node creation.
            siteMapNode.VisibilityProvider = attribute.VisibilityProvider;
            siteMapNode.UrlResolver = attribute.UrlResolver;
            siteMapNode.DynamicNodeProvider = attribute.DynamicNodeProvider;
            siteMapNode.ImageUrl = attribute.ImageUrl;
            siteMapNode.TargetFrame = attribute.TargetFrame;
            siteMapNode.HttpMethod = httpMethod;

            if (!siteMapNode.Clickable)
            {
                siteMapNode.Url = "";
            }

            siteMapNode.LastModifiedDate = attribute.LastModifiedDate;
            siteMapNode.ChangeFrequency = attribute.ChangeFrequency;
            siteMapNode.UpdatePriority = attribute.UpdatePriority;

            // Handle route details
            siteMapNode.Route = attribute.Route;

            // Create a route data dictionary
            IDictionary<string, object> routeValues = new Dictionary<string, object>();
            routeValues.Add("area", area);
            routeValues.Add("controller", controller);
            routeValues.Add("action", action);

            // Add route values to sitemap node
            siteMapNode.RouteValues = routeValues;
            siteMapNode.PreservedRouteParameters = (attribute.PreservedRouteParameters ?? "").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            // Handle MVC details
            siteMapNode.Area = area;
            siteMapNode.Controller = controller;
            siteMapNode.Action = action;

            // Add defaults for area
            if (!siteMapNode.RouteValues.ContainsKey("area"))
            {
                siteMapNode.Attributes["area"] = "";
                siteMapNode.RouteValues.Add("area", "");
            }

            return siteMapNode;
        }

    }
}
