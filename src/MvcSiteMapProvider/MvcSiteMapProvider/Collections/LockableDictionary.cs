using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections
{
    /// <summary>
    /// Generic dictionary that is aware of the ISiteMap interface and can be made read-only
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
            this.ThrowIfReadOnly();
            base.Add(item);
        }

        public override void Add(TKey key, TValue value)
        {
            this.ThrowIfReadOnly();
            base.Add(key, value);
        }

        public override void AddRange(IDictionary<TKey, TValue> items)
        {
            this.ThrowIfReadOnly();
            base.AddRange(items);
        }

        public override void Clear()
        {
            this.ThrowIfReadOnly();
            base.Clear();
        }

        protected override void Insert(TKey key, TValue value, bool add)
        {
            this.ThrowIfReadOnly();
            base.Insert(key, value, add);
        }

        public override bool IsReadOnly
        {
            get { return this.siteMap.IsReadOnly; }
        }

        public override bool Remove(KeyValuePair<TKey, TValue> item)
        {
            this.ThrowIfReadOnly();
            return base.Remove(item);
        }

        public override bool Remove(TKey key)
        {
            this.ThrowIfReadOnly();
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
                this.ThrowIfReadOnly();
                base[key] = value;
            }
        }

        public virtual void CopyTo(IDictionary<TKey, TValue> destination)
        {
            foreach (var item in this.Dictionary)
            {
                var keyIsPointer = item.Key.GetType().IsPointer;
                var valueIsPointer = item.Value.GetType().IsPointer;
                if (!keyIsPointer && !valueIsPointer)
                {
                    destination.Add(new KeyValuePair<TKey, TValue>(item.Key, item.Value));
                }
                else
                {
                    throw new NotSupportedException(Resources.Messages.CopyOperationDoesNotSupportReferenceTypes);
                }
            }
        }

        protected virtual void ThrowIfReadOnly()
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(string.Format(Resources.Messages.SiteMapReadOnly));
            }
        }
    }
}
