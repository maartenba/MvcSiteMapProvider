// -----------------------------------------------------------------------
// <copyright file="ISiteMapCacheKeyBuilder.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapCacheKeyGenerator
    {
        string GenerateKey(HttpContext context);
    }
}
