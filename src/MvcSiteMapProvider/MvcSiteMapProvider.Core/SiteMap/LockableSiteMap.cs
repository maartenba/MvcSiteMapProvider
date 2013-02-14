using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider.Core.SiteMap.Builder;
using MvcSiteMapProvider.Core.Security;
using MvcSiteMapProvider.Core.Collections;
using MvcSiteMapProvider.Core.Web;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LockableSiteMap
        : SiteMap
    {
        public LockableSiteMap(
            ISiteMapBuilder siteMapBuilder,
            IHttpContextFactory httpContextFactory,
            IAclModule aclModule,
            ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory,
            IGenericDictionaryFactory genericDictionaryFactory
            ) 
            : base(siteMapBuilder, httpContextFactory, aclModule, siteMapNodeCollectionFactory, genericDictionaryFactory)
        {
        }

        public override void AddNode(ISiteMapNode node)
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
            }
            base.AddNode(node);
        }

        public override void AddNode(ISiteMapNode node, ISiteMapNode parentNode)
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
            }
            base.AddNode(node, parentNode);
        }

        public override void RemoveNode(ISiteMapNode node)
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
            }
            base.RemoveNode(node);
        }

        public override void Clear()
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
            }
            base.Clear();
        }

        public override void BuildSiteMap()
        {
            // Set the sitemap to read-write so we can populate it.
            this.IsReadOnly = false;
            base.BuildSiteMap();
            // Set the sitemap to read-only so the nodes cannot be inadvertantly modified by the UI layer.
            this.IsReadOnly = true;
        }


        public override bool EnableLocalization
        {
            get { return base.EnableLocalization; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
                }
                base.EnableLocalization = value;
            }
        }

        public override string ResourceKey
        {
            get { return base.ResourceKey; }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
                }
                base.ResourceKey = value;
            }
        }
    }
}
