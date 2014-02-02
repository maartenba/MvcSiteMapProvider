using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Collections;
using MvcSiteMapProvider.Collections.Specialized;

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
            ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory
            )
        {
            if (genericDictionaryFactory == null)
                throw new ArgumentNullException("genericDictionaryFactory");
            if (siteMapNodeCollectionFactory == null)
                throw new ArgumentNullException("siteMapNodeCollectionFactory");

            this.genericDictionaryFactory = genericDictionaryFactory;
            this.siteMapNodeCollectionFactory = siteMapNodeCollectionFactory;
        }

        protected readonly IGenericDictionaryFactory genericDictionaryFactory;
        protected readonly ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory;

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

        public virtual IDictionary<string, ISiteMapNode> CreateUrlDictionary()
        {
            return genericDictionaryFactory.Create<string, ISiteMapNode>(StringComparer.OrdinalIgnoreCase);
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
