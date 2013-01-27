// -----------------------------------------------------------------------
// <copyright file="INodeLocalizer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.Globalization
{
    using System;
    using System.Collections.Specialized;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INodeLocalizer
    {
        void HandleResourceAttribute(string attributeName, ref string text, ref NameValueCollection collection);
    }
}
