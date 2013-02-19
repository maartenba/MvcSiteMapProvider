using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Visitor;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VisitingSiteMapBuilderFactory
        : IVisitingSiteMapBuilderFactory
    {
        public VisitingSiteMapBuilderFactory(
            ISiteMapNodeVisitor siteMapNodeVisitor
            )
        {
            if (siteMapNodeVisitor == null)
                throw new ArgumentNullException("siteMapNodeVisitor");
            this.siteMapNodeVisitor = siteMapNodeVisitor;
        }

        protected readonly ISiteMapNodeVisitor siteMapNodeVisitor;

        #region IVisitingSiteMapBuilderFactory Members

        public virtual ISiteMapBuilder Create()
        {
            return new VisitingSiteMapBuilder(siteMapNodeVisitor);
        }

        #endregion
    }
}
