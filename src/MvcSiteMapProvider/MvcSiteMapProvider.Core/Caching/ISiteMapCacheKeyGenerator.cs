using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MvcSiteMapProvider.Core.Caching
{
    /// <summary>
    /// ISiteMapCacheKeyGenerator interface. This interface allows the sematics of when a new sitemap
    /// is generated vs when the sitemap is stored to be changed. Each unique sitemap key that is generated
    /// causes a new sitemap to be stored in the cache. This can be used by multi-tenant sites to control
    /// how incoming requests map to a specific sitemap.
    /// </summary>
    public interface ISiteMapCacheKeyGenerator
    {
        string GenerateKey();
    }
}
