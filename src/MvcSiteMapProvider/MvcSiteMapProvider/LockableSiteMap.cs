using System;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Provides the means to make the <see cref="T:MvcSiteMapProvider.SiteMap"/> instance read-only so it cannot be 
    /// inadvertently altered while it is in the cache. 
    /// </summary>
    public class LockableSiteMap
        : SiteMap
    {
        public LockableSiteMap(
            ISiteMapPluginProvider pluginProvider,
            IMvcContextFactory mvcContextFactory,
            ISiteMapChildStateFactory siteMapChildStateFactory,
            IUrlPath urlPath,
            ISiteMapSettings siteMapSettings
            )
            : base(pluginProvider, mvcContextFactory, siteMapChildStateFactory, urlPath, siteMapSettings)
        {
        }

        public override void AddNode(ISiteMapNode node)
        {
            this.ThrowIfReadOnly();
            base.AddNode(node);
        }

        public override void AddNode(ISiteMapNode node, ISiteMapNode parentNode)
        {
            this.ThrowIfReadOnly();
            base.AddNode(node, parentNode);
        }

        public override void RemoveNode(ISiteMapNode node)
        {
            this.ThrowIfReadOnly();
            base.RemoveNode(node);
        }

        public override void Clear()
        {
            // Set the sitemap to read-write so we can destroy it.
            this.IsReadOnly = false;
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

        public override string ResourceKey
        {
            get { return base.ResourceKey; }
            set
            {
                this.ThrowIfReadOnly();
                base.ResourceKey = value;
            }
        }

        protected virtual void ThrowIfReadOnly()
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(Resources.Messages.SiteMapReadOnly);
            }
        }
    }
}
