#region Using directives

using System.Xml.Linq;

#endregion

namespace MvcSiteMapProvider
{
    /// <summary>
    /// XElementExtensions class
    /// </summary>
    public static class XElementExtensions
    {
        /// <summary>
        /// Given an XElement and an attribute key, will either return an empty string if its value is
        /// null or the actual value.
        /// </summary>
        /// <param name="node">The current node</param>
        /// <param name="attributeName">The attribe to get the value for.</param>
        /// <returns>Empty string or attribute value</returns>
        public static string GetAttributeValue(this XElement node, string attributeName)
        {
            var attribute = node.Attribute(attributeName);
            return attribute != null ? attribute.Value : string.Empty;
        }
    }
}
