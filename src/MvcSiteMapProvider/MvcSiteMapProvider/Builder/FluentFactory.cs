using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Linq;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// The default implementation of IFluentFactory
    /// </summary>
    public class FluentFactory
        : IFluentFactory
    {
        /// <summary>
        /// Create a IFluentSiteMapNodeFactory.
        /// </summary>
        /// <param name="result">This should an existing collection. All nodes created using Add() will be added to this collection.</param>
        /// <param name="siteMapNodeHelper">The ISiteMapNodeHelper that will internally create the ISiteMapNodes</param>
        /// <returns></returns>
        public IFluentSiteMapNodeFactory CreateSiteMapNodeFactory(IList<IFluentSiteMapNodeBuilder> result, ISiteMapNodeHelper siteMapNodeHelper)
        {
            return new FluentSiteMapNodeFactory(this, result, siteMapNodeHelper);
        }

        /// <summary>
        /// Create a IFluentSiteMapNodeBuilder.
        /// </summary>
        /// <param name="siteMapNodeHelper">The site map node helper.</param>
        /// <returns></returns>
        public IFluentSiteMapNodeBuilder CreateSiteMapNodeBuilder(ISiteMapNodeHelper siteMapNodeHelper)
        {
            return new FluentSiteMapNodeBuilder(this, siteMapNodeHelper);
        }
    }
}
