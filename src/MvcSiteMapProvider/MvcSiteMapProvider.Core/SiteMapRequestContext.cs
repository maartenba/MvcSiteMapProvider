// -----------------------------------------------------------------------
// <copyright file="SiteMapRequestContext.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Provides data to uniquely identify a sitemap within a mulit-culture and multi-tenant site.
    /// </summary>
    public class SiteMapRequestContext
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public int LocaleId { get; set; }
    }
}
