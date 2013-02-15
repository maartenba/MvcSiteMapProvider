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
            //var node = SiteMap.CurrentNode;
            var node = MvcSiteMapProvider.SiteMaps.Current.CurrentNode;
            if (node != null)
            {
                if (node.Attributes.ContainsKey("bling"))
                    return node.Attributes["bling"];
                else
                    return string.Empty;

                //return node["bling"] ?? "";
            }
            return "";
        }
    }
}