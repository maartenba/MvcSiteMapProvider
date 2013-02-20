using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Collections
{
    /// <summary>
    /// Generic dictionary that is aware of the request cache and when is in read-only
    /// mode will automatically switch to a writeable request-cached copy of the original dictionary
    /// during any write operation.
    /// </summary>
    public class RequestCacheableDictionary<TKey, TValue>
        : LockableDictionary<TKey, TValue>
    {
        public RequestCacheableDictionary(
            ISiteMap siteMap,
            IRequestCache requestCache
            )
            : base(siteMap)
        {
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.requestCache = requestCache;
        }

        protected readonly IRequestCache requestCache;
        protected readonly Guid instanceId = Guid.NewGuid();

        #region Write Operations

        public override void Add(KeyValuePair<TKey, TValue> item)
        {
            this.WriteOperationDictionary.Add(item);
        }

        public override void Add(TKey key, TValue value)
        {
            this.WriteOperationDictionary.Add(key, value);
        }

        public override void AddRange(IDictionary<TKey, TValue> items)
        {
            foreach (var item in items)
            {
                this.WriteOperationDictionary.Add(item);
            }
        }

        public override void Clear()
        {
            this.WriteOperationDictionary.Clear();
        }

        protected override void Insert(TKey key, TValue value, bool add)
        {
            if (key == null) throw new ArgumentNullException("key");

            TValue item;
            if (this.ReadOperationDictionary.TryGetValue(key, out item))
            {
                if (add) throw new ArgumentException(Resources.Messages.DictionaryAlreadyContainsKey);
                if (Equals(item, value)) return;
                this.WriteOperationDictionary[key] = value;

                OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, item));
            }
            else
            {
                this.WriteOperationDictionary[key] = value;

                OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
            }
        }

        public override bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.WriteOperationDictionary.Remove(item);
        }

        public override bool Remove(TKey key)
        {
            return this.WriteOperationDictionary.Remove(key);
        }

        #endregion

        public override TValue this[TKey key]
        {
            get { return this.ReadOperationDictionary[key]; }
            set { this.WriteOperationDictionary[key] = value; }
        }

        #region Read Operations

        public override bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.ReadOperationDictionary.Contains(item);
        }

        public override bool ContainsKey(TKey key)
        {
            return this.ReadOperationDictionary.ContainsKey(key);
        }

        public override void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.ReadOperationDictionary.CopyTo(array, arrayIndex);
        }

        public override int Count
        {
            get { return this.ReadOperationDictionary.Count; }
        }

        public override bool Equals(object obj)
        {
            return this.ReadOperationDictionary.Equals(obj);
        }

        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.ReadOperationDictionary.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return this.ReadOperationDictionary.GetHashCode();
        }

        public override ICollection<TKey> Keys
        {
            get { return this.ReadOperationDictionary.Keys; }
        }

        public override string ToString()
        {
            return this.ReadOperationDictionary.ToString();
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            return this.ReadOperationDictionary.TryGetValue(key, out value);
        }

        public override ICollection<TValue> Values
        {
            get { return this.ReadOperationDictionary.Values; }
        }

        #endregion


        /// <summary>
        /// Override this property and set it to false to disable all caching operations.
        /// </summary>
        protected virtual bool RequestCachingEnabled
        {
            get { return true; }
        }


        protected virtual string GetCacheKey()
        {
            return "__REQUEST_CACHEABLE_DICTIONARY_" + this.instanceId.ToString();
        }


        /// <summary>
        /// Gets a dictionary object that can be used to to perform a read operation.
        /// </summary>
        protected virtual IDictionary<TKey, TValue> ReadOperationDictionary
        {
            get
            {
                IDictionary<TKey, TValue> result = null;
                if (this.RequestCachingEnabled)
                {
                    var key = this.GetCacheKey();
                    result = this.requestCache.GetValue<IDictionary<TKey, TValue>>(key);
                    if (result == null)
                    {
                        // Request is not cached, return base dictionary
                        result = base.Dictionary;
                    }
                }
                else
                {
                    result = base.Dictionary;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets a dictionary object that can be used to perform a write operation.
        /// </summary>
        protected virtual IDictionary<TKey, TValue> WriteOperationDictionary
        {
            get
            {
                IDictionary<TKey, TValue> result = null;
                if (this.IsReadOnly && this.RequestCachingEnabled)
                {
                    var key = this.GetCacheKey();
                    result = this.requestCache.GetValue<IDictionary<TKey, TValue>>(key);
                    if (result == null)
                    {
                        // This is the first write operation request in read-only mode, 
                        // we need to create a new dictionary and cache it
                        // with a copy of the current values.
                        result = new Dictionary<TKey, TValue>();
                        base.CopyTo(result);
                        this.requestCache.SetValue<IDictionary<TKey, TValue>>(key, result);
                    }
                }
                else
                {
                    result = base.Dictionary;
                }
                return result;
            }
        }
    }
}
