using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// ReflectionSiteMapNodeProvider class. 
    /// Builds a <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeToParentRelation"/> list based on a
    /// set of attributes within an assembly.
    /// </summary>
    public class ReflectionSiteMapNodeProvider
        : ISiteMapNodeProvider
    {
        public ReflectionSiteMapNodeProvider(
            IEnumerable<String> includeAssemblies,
            IEnumerable<String> excludeAssemblies,
            IAttributeAssemblyProviderFactory attributeAssemblyProviderFactory,
            IMvcSiteMapNodeAttributeDefinitionProvider attributeNodeDefinitionProvider
            )
        {
            if (includeAssemblies == null)
                throw new ArgumentNullException("includeAssemblies");
            if (excludeAssemblies == null)
                throw new ArgumentNullException("excludeAssemblies");
            if (attributeAssemblyProviderFactory == null)
                throw new ArgumentNullException("attributeAssemblyProviderFactory");
            if (attributeNodeDefinitionProvider == null)
                throw new ArgumentNullException("attributeNodeDefinitionProvider");

            this.includeAssemblies = includeAssemblies;
            this.excludeAssemblies = excludeAssemblies;
            this.attributeAssemblyProviderFactory = attributeAssemblyProviderFactory;
            this.attributeNodeDefinitionProvider = attributeNodeDefinitionProvider;
        }
        protected readonly IEnumerable<string> includeAssemblies;
        protected readonly IEnumerable<string> excludeAssemblies;
        protected readonly IMvcSiteMapNodeAttributeDefinitionProvider attributeNodeDefinitionProvider;
        protected readonly IAttributeAssemblyProviderFactory attributeAssemblyProviderFactory;
        protected const string SourceName = "MvcSiteMapNodeAttribute";

        #region ISiteMapNodeProvider Members

        public IEnumerable<ISiteMapNodeToParentRelation> GetSiteMapNodes(ISiteMapNodeHelper helper)
        {
            var result = new List<ISiteMapNodeToParentRelation>();

            var definitions = this.GetMvcSiteMapNodeAttributeDefinitions();
            result.AddRange(this.LoadSiteMapNodesNodesFromMvcSiteMapNodeAttributeDefinitions(definitions, helper));

            // Done!
            return result;
        }

        #endregion

        protected virtual IEnumerable<ISiteMapNodeToParentRelation> LoadSiteMapNodesNodesFromMvcSiteMapNodeAttributeDefinitions(
            IEnumerable<IMvcSiteMapNodeAttributeDefinition> definitions, ISiteMapNodeHelper helper)
        {
            var sourceNodes = new List<ISiteMapNodeToParentRelation>();

            sourceNodes.AddRange(this.CreateNodesFromAttributeDefinitions(definitions, helper));
            sourceNodes.AddRange(this.CreateDynamicNodes(sourceNodes, helper));

            return sourceNodes;
        }

        protected virtual IEnumerable<IMvcSiteMapNodeAttributeDefinition> GetMvcSiteMapNodeAttributeDefinitions()
        {
            var assemblyProvider = this.attributeAssemblyProviderFactory.Create(this.includeAssemblies, this.excludeAssemblies);
            var assemblies = assemblyProvider.GetAssemblies();
            var definitions = this.attributeNodeDefinitionProvider.GetMvcSiteMapNodeAttributeDefinitions(assemblies);
            return definitions;
        }

        protected virtual IList<ISiteMapNodeToParentRelation> CreateNodesFromAttributeDefinitions(
            IEnumerable<IMvcSiteMapNodeAttributeDefinition> definitions, ISiteMapNodeHelper helper)
        {
            var result = new List<ISiteMapNodeToParentRelation>();
            foreach (var definition in definitions)
            {
                var node = this.CreateNodeFromAttributeDefinition(definition, helper);

                // Note: A null node indicates that it doesn't apply to the current siteMapCacheKey
                if (node != null)
                {
                    result.Add(node);
                }
            }
            return result;
        }

        protected virtual ISiteMapNodeToParentRelation CreateNodeFromAttributeDefinition(IMvcSiteMapNodeAttributeDefinition definition, ISiteMapNodeHelper helper)
        {
            ISiteMapNodeToParentRelation result = null;

            // Create node
            var actionNode = definition as MvcSiteMapNodeAttributeDefinitionForAction;
            if (actionNode != null)
            {
                // Create node for action
                result = this.GetSiteMapNodeFromMvcSiteMapNodeAttribute(
                     actionNode.SiteMapNodeAttribute, actionNode.ControllerType, actionNode.ActionMethodInfo, helper);
            }
            else
            {
                var controllerNode = definition as MvcSiteMapNodeAttributeDefinitionForController;
                if (controllerNode != null)
                {
                    // Create node for controller
                    result = this.GetSiteMapNodeFromMvcSiteMapNodeAttribute(
                        controllerNode.SiteMapNodeAttribute, controllerNode.ControllerType, null, helper);
                }
            }
            return result;
        }

        protected virtual IList<ISiteMapNodeToParentRelation> CreateDynamicNodes(IList<ISiteMapNodeToParentRelation> sourceNodes, ISiteMapNodeHelper helper)
        {
            var result = new List<ISiteMapNodeToParentRelation>();
            foreach (var sourceNode in sourceNodes.Where(x => x.Node.HasDynamicNodeProvider).ToArray())
            {
                result.AddRange(helper.CreateDynamicNodes(sourceNode));

                // Remove the dynamic node from the sources - we are replacing its definition.
                sourceNodes.Remove(sourceNode);
            }
            return result;
        }

        /// <summary>
        /// Gets the site map node from MVC site map node attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="type">The type.</param>
        /// <param name="methodInfo">The method info.</param>
        /// <param name="helper">The node helper.</param>
        /// <returns></returns>
        protected virtual ISiteMapNodeToParentRelation GetSiteMapNodeFromMvcSiteMapNodeAttribute(
            IMvcSiteMapNodeAttribute attribute, Type type, MethodInfo methodInfo, ISiteMapNodeHelper helper)
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
                if (!helper.SiteMapCacheKey.Equals(attribute.SiteMapCacheKey))
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
            if (!String.IsNullOrEmpty(attribute.AreaName))
            {
                area = attribute.AreaName;
            }
            if (String.IsNullOrEmpty(area) && !String.IsNullOrEmpty(attribute.Area))
            {
                area = attribute.Area;
            }
            // Determine area (will only work if controller is defined as [<Anything>.]Areas.<Area>.Controllers.<AnyController>)
            if (String.IsNullOrEmpty(area))
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

            string httpMethod = String.IsNullOrEmpty(attribute.HttpMethod) ? HttpVerbs.Get.ToString().ToUpperInvariant() : attribute.HttpMethod.ToUpperInvariant();

            // Handle title and description
            var title = attribute.Title;
            var description = String.IsNullOrEmpty(attribute.Description) ? title : attribute.Description;

            // Handle implicit resources
            var implicitResourceKey = attribute.ResourceKey;

            // Generate key for node
            string key = helper.CreateNodeKey(
                attribute.ParentKey,
                attribute.Key,
                attribute.Url,
                title,
                area,
                controller, action, httpMethod,
                attribute.Clickable);

            var nodeParentMap = helper.CreateNode(key, attribute.ParentKey, SourceName, implicitResourceKey);
            var node = nodeParentMap.Node;

            // Assign defaults
            node.Title = title;
            node.Description = description;
            AcquireAttributesFrom(attribute, node.Attributes, helper);
            AcquireRolesFrom(attribute, node.Roles);
            node.Clickable = attribute.Clickable;
            node.VisibilityProvider = attribute.VisibilityProvider;
            node.DynamicNodeProvider = attribute.DynamicNodeProvider;
            node.ImageUrl = attribute.ImageUrl;
            node.TargetFrame = attribute.TargetFrame;
            node.HttpMethod = httpMethod;
            if (!string.IsNullOrEmpty(attribute.Url)) node.Url = attribute.Url;
            node.CacheResolvedUrl = attribute.CacheResolvedUrl;
            node.CanonicalUrl = attribute.CanonicalUrl;
            node.CanonicalKey = attribute.CanonicalKey;
            AcquireMetaRobotsValuesFrom(attribute, node.MetaRobotsValues);
            node.LastModifiedDate = attribute.LastModifiedDate;
            node.ChangeFrequency = attribute.ChangeFrequency;
            node.UpdatePriority = attribute.UpdatePriority;
            node.Order = attribute.Order;

            // Handle route details
            node.Route = attribute.Route;
            AcquireRouteValuesFrom(attribute, node.RouteValues, helper);
            AcquirePreservedRouteParametersFrom(attribute, node.PreservedRouteParameters);
            node.UrlResolver = attribute.UrlResolver;

            // Specified area, controller and action properties will override any 
            // provided in the attributes collection.
            if (!string.IsNullOrEmpty(area)) node.RouteValues.Add("area", area);
            if (!string.IsNullOrEmpty(controller)) node.RouteValues.Add("controller", controller);
            if (!string.IsNullOrEmpty(action)) node.RouteValues.Add("action", action);

            // Handle MVC details

            // Add defaults for area
            if (!node.RouteValues.ContainsKey("area"))
            {
                node.RouteValues.Add("area", "");
            }

            return nodeParentMap;
        }

        /// <summary>
        /// Acquires the attributes from a given <see cref="T:IMvcSiteMapNodeAttribute"/>
        /// </summary>
        /// <param name="attribute">The source attribute.</param>
        /// <param name="attributes">The attribute dictionary to populate.</param>
        /// <param name="helper">The node helper.</param>
        /// <returns></returns>
        protected virtual void AcquireAttributesFrom(IMvcSiteMapNodeAttribute attribute, IDictionary<string, object> attributes, ISiteMapNodeHelper helper)
        {
            foreach (var att in attribute.Attributes)
            {
                var attributeName = att.Key.ToString();
                var attributeValue = att.Value;

                if (helper.ReservedAttributeNames.IsRegularAttribute(attributeName))
                {
                    attributes[attributeName] = attributeValue;
                }
            }
        }

        /// <summary>
        /// Acquires the route values from a given XElement.
        /// </summary>
        /// <param name="attribute">The source attribute.</param>
        /// <param name="routeValues">The route value dictionary to populate.</param>
        /// <param name="helper">The node helper.</param>
        /// <returns></returns>
        protected virtual void AcquireRouteValuesFrom(IMvcSiteMapNodeAttribute attribute, IRouteValueDictionary routeValues, ISiteMapNodeHelper helper)
        {
            foreach (var att in attribute.Attributes)
            {
                var attributeName = att.Key.ToString();
                var attributeValue = att.Value;

                if (helper.ReservedAttributeNames.IsRouteAttribute(attributeName))
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