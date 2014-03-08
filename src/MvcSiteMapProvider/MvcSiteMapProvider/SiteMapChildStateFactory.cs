using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Collections;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Matching;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Abstract factory for creating new instances of types required by the <see cref="T:MvcSiteMapProvider.SiteMap"/>
    /// at runtime.
    /// </summary>
    public class SiteMapChildStateFactory
        : ISiteMapChildStateFactory
    {
        public SiteMapChildStateFactory(
            IGenericDictionaryFactory genericDictionaryFactory,
            ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory,
            IUrlKeyFactory urlKeyFactory
            )
        {
            if (genericDictionaryFactory == null)
                throw new ArgumentNullException("genericDictionaryFactory");
            if (siteMapNodeCollectionFactory == null)
                throw new ArgumentNullException("siteMapNodeCollectionFactory");
            if (urlKeyFactory == null)
                throw new ArgumentNullException("urlKeyFactory");

            this.genericDictionaryFactory = genericDictionaryFactory;
            this.siteMapNodeCollectionFactory = siteMapNodeCollectionFactory;
            this.urlKeyFactory = urlKeyFactory;
        }

        protected readonly IGenericDictionaryFactory genericDictionaryFactory;
        protected readonly ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory;
        protected readonly IUrlKeyFactory urlKeyFactory;

        #region ISiteMapChildStateFactory Members

        public virtual IDictionary<ISiteMapNode, ISiteMapNodeCollection> CreateChildNodeCollectionDictionary()
        {
            return genericDictionaryFactory.Create<ISiteMapNode, ISiteMapNodeCollection>();
        }

        public virtual IDictionary<string, ISiteMapNode> CreateKeyDictionary()
        {
            return genericDictionaryFactory.Create<string, ISiteMapNode>();
        }

        public virtual IDictionary<ISiteMapNode, ISiteMapNode> CreateParentNodeDictionary()
        {
            return genericDictionaryFactory.Create<ISiteMapNode, ISiteMapNode>();
        }

        public virtual IDictionary<IUrlKey, ISiteMapNode> CreateUrlDictionary()
        {
            return genericDictionaryFactory.Create<IUrlKey, ISiteMapNode>();
        }

        public virtual IUrlKey CreateUrlKey(ISiteMapNode node)
        {
            return this.urlKeyFactory.Create(node);
        }

        public virtual IUrlKey CreateUrlKey(string relativeOrAbsoluteUrl, string hostName)
        {
            return this.urlKeyFactory.Create(relativeOrAbsoluteUrl, hostName);
        }

        public virtual ISiteMapNodeCollection CreateSiteMapNodeCollection()
        {
            return siteMapNodeCollectionFactory.Create();
        }

        public virtual ISiteMapNodeCollection CreateLockableSiteMapNodeCollection(ISiteMap siteMap)
        {
            return siteMapNodeCollectionFactory.CreateLockable(siteMap);
        }

        public virtual ISiteMapNodeCollection CreateReadOnlySiteMapNodeCollection(ISiteMapNodeCollection siteMapNodeCollection)
        {
            return siteMapNodeCollectionFactory.CreateReadOnly(siteMapNodeCollection);
        }

        public virtual ISiteMapNodeCollection CreateEmptyReadOnlySiteMapNodeCollection()
        {
            return siteMapNodeCollectionFactory.CreateEmptyReadOnly();
        }

        #endregion
    }
}
