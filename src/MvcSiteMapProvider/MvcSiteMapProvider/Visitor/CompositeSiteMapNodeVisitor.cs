using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Visitor
{
    /// <summary>
    /// TODO: Update summary.
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
