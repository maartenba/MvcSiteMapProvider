using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Visitor
{
    /// <summary>
    /// TODO: Update summary.
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
