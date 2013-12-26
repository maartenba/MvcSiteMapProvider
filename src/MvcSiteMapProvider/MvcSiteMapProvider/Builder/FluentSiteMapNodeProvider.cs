using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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

        public abstract void BuildSitemapNodes(IFluentSiteMapNodeFactory fluentSiteMapNodeFactory); 

        #region ISiteMapNodeProvider Members

        public IEnumerable<ISiteMapNodeToParentRelation> GetSiteMapNodes(ISiteMapNodeHelper helper)
        {
            var builders = new List<IFluentSiteMapNodeBuilder>();
            var menuItemFactory = _fluentFactory.CreateSiteMapNodeFactory(builders, helper);

            BuildSitemapNodes(menuItemFactory);

            var nodes = new List<ISiteMapNodeToParentRelation>();
            foreach (var builder in builders)
                RecursivelyBuildNodes(helper, null, builder, nodes);
            return nodes;
        }

        #endregion

        private void RecursivelyBuildNodes(ISiteMapNodeHelper helper, ISiteMapNodeToParentRelation parent, IFluentSiteMapNodeBuilder builder, List<ISiteMapNodeToParentRelation> nodes)
        {
            var node = builder.CreateNode(helper, parent != null ? parent.Node : null);
            nodes.Add(node);
            if (builder.Children != null)
                foreach (var child in builder.Children)
                    RecursivelyBuildNodes(helper, node, child, nodes);
        }
    }
}
