using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for newing up some items that are used for building fluent SiteMapNodes.
    /// </summary>
    public interface IFluentFactory
    {
        /// <summary>
        /// Create a IFluentSiteMapNodeFactory.
        /// </summary>
        /// <param name="result">This should an existing collection. All nodes created using Add() will be added to this collection.</param>
        /// <param name="siteMapNodeHelper">The ISiteMapNodeHelper that will internally create the ISiteMapNodes</param>
        /// <returns></returns>
        IFluentSiteMapNodeFactory CreateSiteMapNodeFactory(IList<IFluentSiteMapNodeBuilder> result, ISiteMapNodeHelper siteMapNodeHelper);

        /// <summary>
        /// Create a IFluentSiteMapNodeBuilder.
        /// </summary>
        /// <param name="siteMapNodeHelper">The ISiteMapNodeHelper that will internally create the ISiteMapNodes</param>
        /// <returns></returns>
        IFluentSiteMapNodeBuilder CreateSiteMapNodeBuilder(ISiteMapNodeHelper siteMapNodeHelper);
    }
}
