using System;

namespace MvcSiteMapProvider.Core.Builder
{

    /// <summary>
    /// DefaultNodeKeyGenerator class.
    /// </summary>
    public class NodeKeyGenerator : INodeKeyGenerator
    {
        /// <summary>
        /// Generates the key.
        /// </summary>
        /// <param name="parentKey">The parent node's key</param>
        /// <param name="key">The key.</param>
        /// <param name="url">The URL.</param>
        /// <param name="title">The title.</param>
        /// <param name="area">The area.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="method">The method.</param>
        /// <param name="clickable">if set to <c>true</c> clickable.</param>
        /// <returns>
        /// A key represented as a <see cref="string" /> instance
        /// </returns>
        public string GenerateKey(string parentKey, string key, string url, string title, string area, string controller, string action, string method, bool clickable)
        {
            parentKey = string.IsNullOrEmpty(parentKey) ? "" : parentKey;
            url = string.IsNullOrEmpty(url) ? null : url;
            area = string.IsNullOrEmpty(area) ? null : area;

            return !string.IsNullOrEmpty(key) ? key :
                       (
                           parentKey
                           + (url ?? area ?? "")
                           + "_" + controller
                           + "_" + action
                           + "_" + method
                           + "_" + title
                           + "_" + (clickable ? "" : Guid.NewGuid().ToString())
                       );
        }
    }
}
