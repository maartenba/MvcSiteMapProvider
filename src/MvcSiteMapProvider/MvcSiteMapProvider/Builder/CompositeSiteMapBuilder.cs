using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Used to chain several builders in succession. The builders will be processed in the same order
    /// as they are specified in the constructor.
    /// </summary>
    public class CompositeSiteMapBuilder : ISiteMapBuilder
    {
        public CompositeSiteMapBuilder(params ISiteMapBuilder[] siteMapBuilders)
        {
            if (siteMapBuilders == null)
                throw new ArgumentNullException("siteMapBuilders");

            this.siteMapBuilders = siteMapBuilders;
        }

        public readonly IEnumerable<ISiteMapBuilder> siteMapBuilders;



        #region ISiteMapBuilder Members

        public ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode)
        {
            ISiteMapNode result = rootNode;
            foreach (var builder in siteMapBuilders)
            {
                result = builder.BuildSiteMap(siteMap, result);
            }
            return result;
        }

        #endregion
    }
}
