﻿#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Extensibility;

#endregion

namespace MvcSiteMapProvider
{
    /// <summary>
    /// DefaultControllerTypeResolver class
    /// </summary>
    public class DefaultControllerTypeResolver
        : IControllerTypeResolver
    {
        /// <summary>
        /// Gets or sets the cache.
        /// </summary>
        /// <value>The cache.</value>
        protected Dictionary<string, Type> Cache { get; private set; }

        /// <summary>
        /// Gets or sets the assembly cache.
        /// </summary>
        /// <value>The assembly cache.</value>
        protected Dictionary<string, ILookup<string, Type>> AssemblyCache { get; private set; }

        private readonly object synclock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultControllerTypeResolver"/> class.
        /// </summary>
        public DefaultControllerTypeResolver()
        {
            Cache = new Dictionary<string, Type>();
        }

        #region IControllerTypeResolver Members

        /// <summary>
        /// Resolves the type of the controller.
        /// </summary>
        /// <param name="areaName">Name of the area.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns>Controller type</returns>
        public Type ResolveControllerType(string areaName, string controllerName)
        {
            // Is the type cached?
            string cacheKey = areaName + "_" + controllerName;
            if (Cache.ContainsKey(cacheKey))
            {
                return Cache[cacheKey];
            }

            // Find controller details
            IEnumerable<string> areaNamespaces = FindNamespacesForArea(areaName, RouteTable.Routes);
            string area = areaName;
            string controller = controllerName;

            // Find controller type
            Type controllerType;
            HashSet<string> namespaces = null;
            if (areaNamespaces != null)
            {
                namespaces = new HashSet<string>(areaNamespaces, StringComparer.OrdinalIgnoreCase);
                if (string.IsNullOrEmpty(areaName))
    			{
					namespaces = new HashSet<string>(namespaces.Union(ControllerBuilder.Current.DefaultNamespaces, StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);
				}
            }
            else if (ControllerBuilder.Current.DefaultNamespaces.Count > 0)
            {
                namespaces = ControllerBuilder.Current.DefaultNamespaces;
            }
            controllerType = GetControllerTypeWithinNamespaces(area, controller, namespaces);

            // Cache the result
            if (!Cache.ContainsKey(cacheKey))
            {
                lock (this)
                {
                    if (!Cache.ContainsKey(cacheKey))
                    {
                        Cache.Add(cacheKey, controllerType);
                    }
                }
            }

            // Return
            return controllerType;
        }

        #endregion

        /// <summary>
        /// Finds the namespaces for area.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="routes">The routes.</param>
        /// <returns>
        /// A namespaces for area represented as a <see cref="string"/> instance
        /// </returns>
        protected virtual IEnumerable<string> FindNamespacesForArea(string area, RouteCollection routes)
        {
            var namespacesForArea = new List<string>();
            var namespacesCommon = new List<string>();

            foreach (var route in routes.OfType<Route>().Where(r => r.DataTokens != null && r.DataTokens["Namespaces"] != null))
    		{
				// search for area-based namespaces
				if (route.DataTokens["area"] != null && route.DataTokens["area"].ToString().Equals(area, StringComparison.OrdinalIgnoreCase))
					namespacesForArea.AddRange((IEnumerable<string>)route.DataTokens["Namespaces"]);
				else if (route.DataTokens["area"] == null)
					namespacesCommon.AddRange((IEnumerable<string>)route.DataTokens["Namespaces"]);
			}

            if (namespacesForArea.Count > 0)
            {
                return namespacesForArea;
            }
            else if (namespacesCommon.Count > 0)
            {
                return namespacesCommon;
            }

            return null;
        }

        /// <summary>
        /// Inits the assembly cache.
        /// </summary>
        private void InitAssemblyCache()
        {
            if (AssemblyCache == null)
            {
                lock (synclock)
                {
                    if (AssemblyCache == null)
                    {
                        List<Type> controllerTypes = GetListOfControllerTypes();
                        var groupedByName = controllerTypes.GroupBy(
                            t => t.Name.Substring(0, t.Name.Length - "Controller".Length),
                            StringComparer.OrdinalIgnoreCase);
                        AssemblyCache = groupedByName.ToDictionary(
                            g => g.Key,
                            g => g.ToLookup(t => t.Namespace ?? String.Empty, StringComparer.OrdinalIgnoreCase),
                            StringComparer.OrdinalIgnoreCase);
                    }
                }
            }

        }

        /// <summary>
        /// Gets the list of controller types.
        /// </summary>
        /// <returns></returns>
        protected virtual List<Type> GetListOfControllerTypes()
        {
            IEnumerable<Type> typesSoFar = Type.EmptyTypes;
            ICollection assemblies = System.Web.Compilation.BuildManager.GetReferencedAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] typesInAsm;
                try
                {
                    typesInAsm = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    typesInAsm = ex.Types;
                }
                typesSoFar = typesSoFar.Concat(typesInAsm);
            }
            return typesSoFar.Where(t => t != null && t.IsClass && t.IsPublic && !t.IsAbstract && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) && typeof(IController).IsAssignableFrom(t)).ToList();
        }

        /// <summary>
        /// Determines whether namespace matches the specified requested namespace.
        /// </summary>
        /// <param name="requestedNamespace">The requested namespace.</param>
        /// <param name="targetNamespace">The target namespace.</param>
        /// <returns>
        /// 	<c>true</c> if is namespace matches the specified requested namespace; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsNamespaceMatch(string requestedNamespace, string targetNamespace)
        {
            // degenerate cases
            if (requestedNamespace == null)
            {
                return false;
            }
            else if (requestedNamespace.Length == 0)
            {
                return true;
            }

            if (!requestedNamespace.EndsWith(".*", StringComparison.OrdinalIgnoreCase))
            {
                // looking for exact namespace match
                return String.Equals(requestedNamespace, targetNamespace, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // looking for exact or sub-namespace match
                requestedNamespace = requestedNamespace.Substring(0, requestedNamespace.Length - ".*".Length);
                if (!targetNamespace.StartsWith(requestedNamespace, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (requestedNamespace.Length == targetNamespace.Length)
                {
                    // exact match
                    return true;
                }
                else if (targetNamespace[requestedNamespace.Length] == '.')
                {
                    // good prefix match, e.g. requestedNamespace = "Foo.Bar" and targetNamespace = "Foo.Bar.Baz"
                    return true;
                }
                else
                {
                    // bad prefix match, e.g. requestedNamespace = "Foo.Bar" and targetNamespace = "Foo.Bar2"
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the controller type within namespaces.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="namespaces">The namespaces.</param>
        /// <returns>
        /// A controller type within namespaces represented as a <see cref="Type"/> instance 
        /// </returns>
        protected virtual Type GetControllerTypeWithinNamespaces(string area, string controller, HashSet<string> namespaces)
        {
            if (string.IsNullOrEmpty(controller) || controller == "")
                return null;

            InitAssemblyCache();

            HashSet<Type> matchingTypes = new HashSet<Type>();
            ILookup<string, Type> nsLookup;
            if (AssemblyCache.TryGetValue(controller, out nsLookup))
            {
                // this friendly name was located in the cache, now cycle through namespaces
                if (namespaces != null)
                {
                    foreach (string requestedNamespace in namespaces)
                    {
                        foreach (var targetNamespaceGrouping in nsLookup)
                        {
                            if (IsNamespaceMatch(requestedNamespace, targetNamespaceGrouping.Key))
                            {
                                matchingTypes.UnionWith(targetNamespaceGrouping);
                            }
                        }
                    }
                }
                else
                {
                    // if the namespaces parameter is null, search *every* namespace
                    foreach (var nsGroup in nsLookup)
                    {
                        matchingTypes.UnionWith(nsGroup);
                    }
                }
            }

            if (matchingTypes.Count == 1)
            {
                return matchingTypes.First();
            }
            else if (matchingTypes.Count > 1)
            {
                throw new MvcSiteMapProvider.AmbiguousControllerException(string.Format("Found multiple controllers:{0}", controller));
            }
            return null;
        }
    }
}
