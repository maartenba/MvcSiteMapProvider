using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for the fluent SiteMapNode factories.
    /// Calling Add() will initiate a fluent SiteMapNode underneath its current context (parent).
    /// </summary>
    public interface IFluentSiteMapNodeFactory
    {
        /// <summary>
        /// Start the build of a fluent SiteMapNode
        /// </summary>
        /// <returns></returns>
        IFluentSiteMapNodeBuilder Add();
    }
}
