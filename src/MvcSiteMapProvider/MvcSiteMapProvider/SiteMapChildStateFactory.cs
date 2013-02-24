using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Collections;

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

        public IDictionary<TKey, TValue> CreateGenericDictionary<TKey, TValue>()
        {
            return genericDictionaryFactory.Create<TKey, TValue>();
        }

        public ISiteMapNodeCollection CreateSiteMapNodeCollection()
        {
            return siteMapNodeCollectionFactory.Create();
        }

        public ISiteMapNodeCollection CreateLockableSiteMapNodeCollection(ISiteMap siteMap)
        {
            return siteMapNodeCollectionFactory.CreateLockable(siteMap);
        }

        public ISiteMapNodeCollection CreateReadOnlySiteMapNodeCollection(ISiteMapNodeCollection siteMapNodeCollection)
        {
            return siteMapNodeCollectionFactory.CreateReadOnly(siteMapNodeCollection);
        }

        public ISiteMapNodeCollection CreateEmptyReadOnlySiteMapNodeCollection()
        {
            return siteMapNodeCollectionFactory.CreateEmptyReadOnly();
        }

        #endregion
    }
}
