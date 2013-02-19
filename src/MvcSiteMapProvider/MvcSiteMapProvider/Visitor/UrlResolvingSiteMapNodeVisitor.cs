using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Visitor
{
    /// <summary>
    /// Specialized <see cref="T:MvcSiteMapProvider.Visitor.ISiteMapNodeVisitor"/> class for resolving URLs
    /// during the build stage so they are aleady resolved before caching.
    /// </summary>
    public class UrlResolvingSiteMapNodeVisitor
        : ISiteMapNodeVisitor
    {
        #region ISiteMapNodeVisitor Members

        public void Execute(ISiteMapNode node)
        {
            node.ResolveUrl();
        }

        #endregion
    }
}
