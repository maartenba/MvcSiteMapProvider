// -----------------------------------------------------------------------
// <copyright file="NodeLocalizer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.Globalization
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;

    /// <summary>
    /// NodeLocalizer class.
    /// </summary>
    public class NodeLocalizer : INodeLocalizer
    {
        #region INodeLocalizer Members

        /// <summary>
        /// Handle resource attribute
        /// </summary>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="text">Text</param>
        /// <param name="collection">NameValueCollection to be used for localization</param>
        public void HandleResourceAttribute(string attributeName, ref string text, ref NameValueCollection collection)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string tempStr1;
                var tempStr2 = text.TrimStart(new[] { ' ' });
                if (((tempStr2.Length > 10)) && tempStr2.ToLower(CultureInfo.InvariantCulture).StartsWith("$resources:", StringComparison.Ordinal))
                {
                    tempStr1 = tempStr2.Substring(11);
                    string tempStr3;
                    string tempStr4;
                    var index = tempStr1.IndexOf(',');
                    tempStr3 = tempStr1.Substring(0, index);
                    tempStr4 = tempStr1.Substring(index + 1);
                    var length = tempStr4.IndexOf(',');
                    if (length != -1)
                    {
                        text = tempStr4.Substring(length + 1);
                        tempStr4 = tempStr4.Substring(0, length);
                    }
                    else
                    {
                        text = null;
                    }
                    if (collection == null)
                    {
                        collection = new NameValueCollection();
                    }
                    collection.Add(attributeName, tempStr3.Trim());
                    collection.Add(attributeName, tempStr4.Trim());
                }
            }
        }

        #endregion

    }
}
