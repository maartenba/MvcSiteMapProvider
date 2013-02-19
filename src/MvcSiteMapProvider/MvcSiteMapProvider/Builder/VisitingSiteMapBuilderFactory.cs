using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Visitor;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.Builder.VisitingSiteMapBuilder"/>
    /// at runtime.
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
