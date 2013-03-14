using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Visitor
{
    /// <summary>
    /// Used to chain several <see cref="T:MvcSiteMapProvider.Visitor.ISiteMapNodeVisitor"/> instances in succession. 
    /// The visitors will be processed in the same order as they are specified in the constructor.
    /// </summary>
    public class CompositeSiteMapNodeVisitor
        : ISiteMapNodeVisitor
    {
        public CompositeSiteMapNodeVisitor(params ISiteMapNodeVisitor[] siteMapNodeVisitors)
        {
            if (siteMapNodeVisitors == null)
                throw new ArgumentNullException("siteMapNodeVisitors");
            this.siteMapNodeVisitors = siteMapNodeVisitors;
        }

        protected readonly IEnumerable<ISiteMapNodeVisitor> siteMapNodeVisitors;


        #region ISiteMapNodeVisitor Members

        public void Execute(ISiteMapNode node)
        {
            foreach (var visitor in this.siteMapNodeVisitors)
            {
                visitor.Execute(node);
            }
        }

        #endregion
    }
}
