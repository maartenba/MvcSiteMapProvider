using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Class for tracking the relationship between node instances to their parent nodes 
    /// before they are added to the SiteMap.
    /// </summary>
    public class SiteMapNodeParentMap
        : ISiteMapNodeParentMap
    {
        public SiteMapNodeParentMap(
            string parentKey,
            ISiteMapNode node,
            string sourceName
            )
        {
            if (node == null)
                throw new ArgumentNullException("node");

            this.parentKey = parentKey;
            this.node = node;
            this.sourceName = sourceName;
        }
        protected readonly string parentKey;
        protected readonly ISiteMapNode node;
        protected readonly string sourceName;

        #region ISiteMapNodeParentMap Members

        public virtual string ParentKey
        {
            get { return this.parentKey; }
        }

        public virtual ISiteMapNode Node
        {
            get { return this.node; }
        }

        public virtual string SourceName
        {
            get { return this.sourceName; }
        }

        #endregion
    }
}
