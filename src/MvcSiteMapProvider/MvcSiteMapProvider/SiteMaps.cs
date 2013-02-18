using System;
using MvcSiteMapProvider.Loader;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// This class is the static entry point where the presentation layer can request a sitemap by calling Current or passing a siteMapCacheKey.
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
            get { return GetSiteMap(); }
        }

        public static ISiteMap GetSiteMap(string siteMapCacheKey)
        {
            return loader.GetSiteMap(siteMapCacheKey);
        }

        public static ISiteMap GetSiteMap()
        {
            return loader.GetSiteMap();
        }
    }
}
