// -----------------------------------------------------------------------
// <copyright file="IDynamicNodeBuilder.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IDynamicNodeBuilder
    {
        //bool HasDynamicNodes(ISiteMapNode node);
        IEnumerable<ISiteMapNode> BuildDynamicNodesFor(ISiteMap siteMap, ISiteMapNode node, ISiteMapNode parentNode);
    }
}
