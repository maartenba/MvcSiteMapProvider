using System;
using System.Collections.Specialized;
using System.Globalization;

namespace MvcSiteMapProvider.Globalization
{
    /// <summary>
    /// AttributeResourceKeyParser class. This class is used to parse resource information from 
    /// attributes in XML files (or other sources). This information can later be used to lookup localized versions of the text
    /// to insert in place of the resource string.
    /// </summary>
    public class ExplicitResourceKeyParser
        : IExplicitResourceKeyParser
    {
        #region IExplicitResourceKeyParser Members

        public void HandleResourceAttribute(string attributeName, ref string value, ref NameValueCollection explicitResourceKeys)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string resourceString;
                var trimmedText = value.TrimStart(new[] { ' ' });
                if (((trimmedText.Length > 10)) && trimmedText.ToLower(CultureInfo.InvariantCulture).StartsWith("$resources:", StringComparison.Ordinal))
                {
                    resourceString = trimmedText.Substring(11);
                    var index = resourceString.IndexOf(',');
                    string resourceLocation = resourceString.Substring(0, index);
                    string resourceName = resourceString.Substring(index + 1);
                    var length = resourceName.IndexOf(',');
                    if (length != -1)
                    {
                        value = resourceName.Substring(length + 1);
                        resourceName = resourceName.Substring(0, length);
                    }
                    else
                    {
                        // Fixes #339, return string datatype (rather than null) so custom 
                        // attributes can still be identified as string vs another datatype.
                        value = string.Empty;
                    }
                    if (explicitResourceKeys == null)
                    {
                        explicitResourceKeys = new NameValueCollection();
                    }
                    explicitResourceKeys.Add(attributeName, resourceLocation.Trim());
                    explicitResourceKeys.Add(attributeName, resourceName.Trim());
                }
            }
        }

        #endregion
    }
}
