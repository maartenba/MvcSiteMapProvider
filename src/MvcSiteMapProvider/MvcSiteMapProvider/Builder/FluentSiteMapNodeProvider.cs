using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// FluentSiteMapNodeProvider class is a ISiteMapNodeProvider that builds nodes in a fluent fashion.
    /// </summary>
    public abstract class FluentSiteMapNodeProvider
        : ISiteMapNodeProvider
    {
        private readonly IFluentFactory _fluentFactory;

        public FluentSiteMapNodeProvider(IFluentFactory fluentFactory)
        {
            if(fluentFactory == null)
                throw new ArgumentNullException("fluentFactory");

            _fluentFactory = fluentFactory;
        }

        [Flags]
        protected enum NodesToProcess
        {
            StandardNodes = 1,
            DynamicNodes = 2,
            All = StandardNodes | DynamicNodes
        }

        public abstract void BuildSitemapNodes(IFluentSiteMapNodeFactory fluentSiteMapNodeFactory); 

        public abstract bool UseNestedDynamicNodeRecursion { get; }

        #region ISiteMapNodeProvider Members

        public IEnumerable<ISiteMapNodeToParentRelation> GetSiteMapNodes(ISiteMapNodeHelper helper)
        {
            var builders = new List<IFluentSiteMapNodeBuilder>();
            var menuItemFactory = _fluentFactory.CreateSiteMapNodeFactory(builders, helper);

            BuildSitemapNodes(menuItemFactory);

            var result = new List<ISiteMapNodeToParentRelation>();
            foreach (var builder in builders)
            {
                var rootNode = builder.CreateNode(helper, null);
                result.Add(rootNode);
                result.AddRange(ProcessBuilders(rootNode.Node, builder.Children, NodesToProcess.All, helper));
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Recursively processes our XML document, parsing our siteMapNodes and dynamicNode(s).
        /// </summary>
        /// <param name="parentNode">The parent node to process.</param>
        /// <param name="childrenBuilders">The correspoding parent XML element.</param>
        /// <param name="processFlags">Flags to indicate which nodes to process.</param>
        /// <param name="helper">The node helper.</param>
        protected virtual IList<ISiteMapNodeToParentRelation> ProcessBuilders(ISiteMapNode parentNode, IList<IFluentSiteMapNodeBuilder> childrenBuilders, NodesToProcess processFlags, ISiteMapNodeHelper helper)
        {
            var result = new List<ISiteMapNodeToParentRelation>();
            bool processStandardNodes = (processFlags & NodesToProcess.StandardNodes) == NodesToProcess.StandardNodes;
            bool processDynamicNodes = (processFlags & NodesToProcess.DynamicNodes) == NodesToProcess.DynamicNodes;

            foreach (var node in childrenBuilders)
            {
                var child = node.CreateNode(helper, parentNode);

                if (processStandardNodes && !child.Node.HasDynamicNodeProvider)
                {
                    result.Add(child);

                    // Continue recursively processing the XML file.
                    result.AddRange(ProcessBuilders(child.Node, node.Children, processFlags, helper));
                }
                else if (processDynamicNodes && child.Node.HasDynamicNodeProvider)
                {
                    // We pass in the parent node key as the default parent because the dynamic node (child) is never added to the sitemap.
                    var dynamicNodes = helper.CreateDynamicNodes(child, parentNode.Key);

                    foreach (var dynamicNode in dynamicNodes)
                    {
                        result.Add(dynamicNode);

                        if (!this.UseNestedDynamicNodeRecursion)
                        {
                            // Recursively add non-dynamic childs for every dynamic node
                            result.AddRange(ProcessBuilders(dynamicNode.Node, node.Children, NodesToProcess.StandardNodes, helper));
                        }
                        else
                        {
                            // Recursively process both dynamic nodes and static nodes.
                            // This is to allow V3 recursion behavior for those who depended on it - it is not a feature.
                            result.AddRange(ProcessBuilders(dynamicNode.Node, node.Children, NodesToProcess.All, helper));
                        }
                    }

                    if (!this.UseNestedDynamicNodeRecursion)
                    {
                        // Process the next nested dynamic node provider. We pass in the parent node as the default 
                        // parent because the dynamic node definition node (child) is never added to the sitemap.
                        result.AddRange(ProcessBuilders(parentNode, node.Children, NodesToProcess.DynamicNodes, helper));
                    }
                    else
                    {
                        // Continue recursively processing the XML file.
                        // Can't figure out why this is here, but this is the way it worked in V3 and if
                        // anyone depends on the broken recursive behavior, they probably also depend on this.
                        result.AddRange(ProcessBuilders(child.Node, node.Children, processFlags, helper));
                    }
                }
            }
            return result;
        }
    }
}
