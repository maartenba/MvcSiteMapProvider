using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
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
                    string resourceLocation;
                    string resourceName;
                    var index = resourceString.IndexOf(',');
                    resourceLocation = resourceString.Substring(0, index);
                    resourceName = resourceString.Substring(index + 1);
                    var length = resourceName.IndexOf(',');
                    if (length != -1)
                    {
                        value = resourceName.Substring(length + 1);
                        resourceName = resourceName.Substring(0, length);
                    }
                    else
                    {
                        value = null;
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
