// -----------------------------------------------------------------------
// <copyright file="SiteMaps.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MvcSiteMapProvider.Core.Loader;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMaps
    {
        private static ISiteMapLoader loader;

        public static ISiteMapLoader Loader
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (loader != null)
                    throw new MvcSiteMapException(Resources.Messages.SiteMapLoaderAlreadySet);
                loader = value;
            }
        }

        public static ISiteMap Current
        {
            get { return GetSiteMap("default"); }
        }

        public static ISiteMap GetSiteMap(string siteMapKey, string builderSetName)
        {
            return loader.GetSiteMap(siteMapKey, builderSetName);
        }

        public static ISiteMap GetSiteMap(string builderSetName)
        {
            return loader.GetSiteMap(builderSetName);
        }
    }
}
