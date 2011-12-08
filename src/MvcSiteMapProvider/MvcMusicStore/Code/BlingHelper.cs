using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider.Web.Html;

namespace MvcMusicStore
{
    /// <summary>
    /// BlingHelper class
    /// </summary>
    public static class BlingHelper
    {
        /// <summary>
        /// Bling!
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns>Node bling.</returns>
        public static string Bling(this MvcSiteMapHtmlHelper helper)
        {
            var node = SiteMap.CurrentNode;
            if (node != null)
            {
                return node["bling"] ?? "";
            }
            return "";
        }
    }
}