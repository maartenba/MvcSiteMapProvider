using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for a set of services useful for creating SiteMap nodes.
    /// </summary>
    public interface ISiteMapNodeCreator
    {

        /// <summary>
        /// Creates an instance of <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeToParentRelation"/>.
        /// </summary>
        /// <param name="key">The key for the node. This cannot be changed.</param>
        /// <param name="parentKey">The key of the intended parent node.</param>
        /// <param name="sourceName">A string description of the provider. This value is shown in exception messages related to the node configuration.</param>
        /// <param name="implicitResourceKey">The implicit resource key for localization of the node's properties.</param>
        /// <returns>An <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeToParentRelation"/> instance.</returns>
        ISiteMapNodeToParentRelation CreateSiteMapNode(string key, string parentKey, string sourceName, string implicitResourceKey);

        /// <summary>
        /// Creates an instance of <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeToParentRelation"/>.
        /// </summary>
        /// <param name="key">The key for the node. This cannot be changed.</param>
        /// <param name="parentKey">The key of the intended parent node.</param>
        /// <param name="sourceName">A string description of the provider. This value is shown in exception messages related to the node configuration.</param>
        /// <param name="implicitResourceKey">The implicit resource key for localization of the node's properties.</param>
        /// <returns>An <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeToParentRelation"/> instance.</returns>
        ISiteMapNodeToParentRelation CreateDynamicSiteMapNode(string key, string parentKey, string sourceName, string implicitResourceKey);


        /// <summary>
        /// Generates a key based on the properties that are passed. The combination of values should be unique within the sitemap.
        /// </summary>
        /// <param name="parentKey">The key of the intended parent node.</param>
        /// <param name="key">The key (if any) of the current node.</param>
        /// <param name="url">The URL of the current node.</param>
        /// <param name="title">The title of the current node.</param>
        /// <param name="area">The area (if any) name of the current node.</param>
        /// <param name="controller">The controller name of the current node.</param>
        /// <param name="action">The action method name of the current node.</param>
        /// <param name="httpMethod">The HTTP method of the current node.</param>
        /// <param name="clickable">Whether or not the node should be clickable on the UI.</param>
        /// <returns>A key that applies to the current node.</returns>
        string GenerateSiteMapNodeKey(string parentKey, string key, string url, string title, string area, string controller, string action, string httpMethod, bool clickable);
    }
}
