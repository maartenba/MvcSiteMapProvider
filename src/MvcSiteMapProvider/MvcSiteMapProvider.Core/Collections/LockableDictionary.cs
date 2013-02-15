using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Core;

namespace MvcSiteMapProvider.Core.Collections
{
    /// <summary>
    /// Dictionary that is aware of the ISiteMap interface and can be made read-only
    /// depending on the IsReadOnly property of ISiteMap.
    /// </summary>
    public class LockableDictionary<TKey, TValue>
        : ObservableDictionary<TKey, TValue>
    {
        public LockableDictionary(
            ISiteMap siteMap
            )
        {
            if (siteMap == null)
                throw new ArgumentNullException("siteMap");

            this.siteMap = siteMap;
        }

        protected readonly ISiteMap siteMap;

        public override void Add(KeyValuePair<TKey, TValue> item)
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            base.Add(item);
        }

        public override void Add(TKey key, TValue value)
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            base.Add(key, value);
        }

        public override void AddRange(IDictionary<TKey, TValue> items)
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            base.AddRange(items);
        }

        public override void Clear()
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            base.Clear();
        }

        protected override void Insert(TKey key, TValue value, bool add)
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            base.Insert(key, value, add);
        }

        public override bool IsReadOnly
        {
            get
            {
                return this.siteMap.IsReadOnly;
            }
        }

        public override bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            return base.Remove(item);
        }

        public override bool Remove(TKey key)
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            return base.Remove(key);
        }

        public override TValue this[TKey key]
        {
            get
            {
                return base[key];
            }
            set
            {
                if (this.siteMap.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
                }
                base[key] = value;
            }
        }
    }
}
