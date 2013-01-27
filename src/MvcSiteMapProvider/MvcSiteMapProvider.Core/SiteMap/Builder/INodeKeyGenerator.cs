namespace MvcSiteMapProvider.Core.SiteMap.Builder
{
    /// <summary>
    /// INodeKeyGenerator contract
    /// </summary>
    public interface INodeKeyGenerator
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
       /// <param name="httpMethod">The HTTP method.</param>
       /// <param name="clickable">if set to <c>true</c> [clickable].</param>
       /// <returns>
       /// A key represented as a <see cref="string"/> instance 
       /// </returns>
       string GenerateKey(string parentKey, string key, string url, string title, string area, string controller, string action, string httpMethod, bool clickable);
    }
}