using MvcSiteMapProvider.Builder;
using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Used to chain several <see cref="T:MvcSiteMapProvider.ISiteMapNodeProvider"/> instances in succession. 
    /// The providers will be processed in the same order as they are specified in the constructor.
    /// </summary>
    public class CompositeSiteMapNodeProvider
        : ISiteMapNodeProvider
    {
        public CompositeSiteMapNodeProvider(params ISiteMapNodeProvider[] siteMapNodeProviders)
        {
            if (siteMapNodeProviders == null)
                throw new ArgumentNullException("siteMapNodeProviders");
            this.siteMapNodeProviders = siteMapNodeProviders;
        }
        protected readonly IEnumerable<ISiteMapNodeProvider> siteMapNodeProviders;

        #region ISiteMapNodeProvider Members

        public IEnumerable<ISiteMapNodeToParentRelation> GetSiteMapNodes(ISiteMapNodeHelper helper)
        {
            var result = new List<ISiteMapNodeToParentRelation>();
            foreach (var provider in this.siteMapNodeProviders)
            {
                result.AddRange(provider.GetSiteMapNodes(helper));
            }
            return result;
        }

        #endregion
    }
}
