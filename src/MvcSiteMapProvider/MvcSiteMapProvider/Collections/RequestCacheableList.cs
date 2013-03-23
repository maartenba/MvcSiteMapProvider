
// This class has infinite recursion problems. This was avoided in the RequestCacheableDictionary class because its base
// class stores the dictionary in a wrapped instance that has protected access. Unfortunately, with List<T> that isn't the case.
// So for this to work, its base class needs to wrap a List<T> and override all members of List<T> or it needs to inherit
// from a base class that does same and exposes the internal list at least at the protected level.

//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using MvcSiteMapProvider;
//using MvcSiteMapProvider.Caching;

//namespace MvcSiteMapProvider.Collections
//{
//    /// <summary>
//    /// Generic list that is aware of the request cache and when is in read-only
//    /// mode will automatically switch to a writeable request-cached copy of the original list
//    /// during any write operation.
//    /// </summary>
//    public class RequestCacheableList<T>
//        : LockableList<T>
//    {
//        public RequestCacheableList(
//            ISiteMap siteMap,
//            IRequestCache requestCache
//            )
//            : base(siteMap)
//        {
//            if (requestCache == null)
//                throw new ArgumentNullException("requestCache");

//            this.requestCache = requestCache;
//        }

//        protected readonly IRequestCache requestCache;
//        protected readonly Guid instanceId = Guid.NewGuid();

//        #region Write Operations

//        public override void Add(T item)
//        {
//            this.WriteOperationList.Add(item);
//        }

//        public override void AddRange(IEnumerable<T> collection)
//        {
//            this.WriteOperationList.AddRange(collection);
//        }

//        public override void Clear()
//        {
//            this.WriteOperationList.Clear();
//        }

//        public override void Insert(int index, T item)
//        {
//            this.WriteOperationList.Insert(index, item);
//        }

//        public override void InsertRange(int index, IEnumerable<T> collection)
//        {
//            this.WriteOperationList.InsertRange(index, collection);
//        }

//        public override bool Remove(T item)
//        {
//            return this.WriteOperationList.Remove(item);
//        }

//        public override int RemoveAll(Predicate<T> match)
//        {
//            return this.WriteOperationList.RemoveAll(match);
//        }

//        public override void RemoveAt(int index)
//        {
//            this.WriteOperationList.RemoveAt(index);
//        }

//        public override void RemoveRange(int index, int count)
//        {
//            this.WriteOperationList.RemoveRange(index, count);
//        }

//        public override void Reverse()
//        {
//            this.WriteOperationList.Reverse();
//        }

//        public override void Reverse(int index, int count)
//        {
//            this.WriteOperationList.Reverse(index, count);
//        }

//        public override void Sort()
//        {
//            this.WriteOperationList.Sort();
//        }

//        public override void Sort(Comparison<T> comparison)
//        {
//            this.WriteOperationList.Sort(comparison);
//        }

//        public override void Sort(IComparer<T> comparer)
//        {
//            this.WriteOperationList.Sort(comparer);
//        }

//        public override void Sort(int index, int count, IComparer<T> comparer)
//        {
//            this.WriteOperationList.Sort(index, count, comparer);
//        }

//        public override void TrimExcess()
//        {
//            this.WriteOperationList.TrimExcess();
//        }

//        #endregion

//        #region Read Operations

//        new public virtual ReadOnlyCollection<T> AsReadOnly()
//        {
//            return this.ReadOperationList.AsReadOnly();
//        }

//        new public virtual int BinarySearch(T item)
//        {
//            return this.ReadOperationList.BinarySearch(item);
//        }

//        new public virtual int BinarySearch(T item, IComparer<T> comparer)
//        {
//            return this.ReadOperationList.BinarySearch(item, comparer);
//        }

//        new public virtual int BinarySearch(int index, int count, T item, IComparer<T> comparer)
//        {
//            return this.ReadOperationList.BinarySearch(index, count, item, comparer);
//        }

//        new public virtual int Capacity
//        {
//            get { return this.ReadOperationList.Capacity; }
//        }

//        new public virtual bool Contains(T item)
//        {
//            return this.ReadOperationList.Contains(item);
//        }

//        new public virtual bool Contains(T value, IEqualityComparer<T> comparer)
//        {
//            return this.ReadOperationList.Contains(value, comparer);
//        }

//        new public virtual List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
//        {
//            return this.ReadOperationList.ConvertAll<TOutput>(converter);
//        }

//        //public override void CopyTo(IList<T> destination)
//        //{
//        //    this.ReadOperationList.CopyTo(destination);
//        //}

//        new public virtual int Count
//        {
//            get { return this.ReadOperationList.Count; }
//        }

//        public override bool Equals(object obj)
//        {
//            return this.ReadOperationList.Equals(obj);
//        }

//        new public virtual bool Exists(Predicate<T> match)
//        {
//            return this.ReadOperationList.Exists(match);
//        }

//        new public virtual T Find(Predicate<T> match)
//        {
//            return this.ReadOperationList.Find(match);
//        }

//        new public virtual List<T> FindAll(Predicate<T> match)
//        {
//            return this.ReadOperationList.FindAll(match);
//        }

//        new public virtual int FindIndex(Predicate<T> match)
//        {
//            return this.ReadOperationList.FindIndex(match);
//        }

//        new public virtual int FindIndex(int startIndex, Predicate<T> match)
//        {
//            return this.ReadOperationList.FindIndex(startIndex, match);
//        }

//        new public virtual int FindIndex(int startIndex, int count, Predicate<T> match)
//        {
//            return this.ReadOperationList.FindIndex(startIndex, count, match);
//        }

//        new public virtual T FindLast(Predicate<T> match)
//        {
//            return this.ReadOperationList.FindLast(match);
//        }

//        new public virtual int FindLastIndex(Predicate<T> match)
//        {
//            return this.ReadOperationList.FindLastIndex(match);
//        }

//        new public virtual int FindLastIndex(int startIndex, Predicate<T> match)
//        {
//            return this.ReadOperationList.FindLastIndex(startIndex, match);
//        }

//        new public virtual int FindLastIndex(int startIndex, int count, Predicate<T> match)
//        {
//            return this.ReadOperationList.FindLastIndex(startIndex, count, match);
//        }

//        new public virtual void ForEach(Action<T> action)
//        {
//            this.ReadOperationList.ForEach(action);
//        }

//        new public virtual Enumerator GetEnumerator()
//        {
//            return this.ReadOperationList.GetEnumerator();
//        }

//        public override int GetHashCode()
//        {
//            return this.ReadOperationList.GetHashCode();
//        }

//        new public virtual List<T> GetRange(int index, int count)
//        {
//            return this.ReadOperationList.GetRange(index, count);
//        }

//        new public virtual Type GetType()
//        {
//            return this.ReadOperationList.GetType();
//        }

//        new public virtual int IndexOf(T item)
//        {
//            return this.ReadOperationList.IndexOf(item);
//        }

//        new public virtual int IndexOf(T item, int index)
//        {
//            return this.ReadOperationList.IndexOf(item, index);
//        }

//        new public virtual int IndexOf(T item, int index, int count)
//        {
//            return this.ReadOperationList.IndexOf(item, index, count);
//        }

//        //public override bool IsReadOnly
//        //{
//        //    get { return this.ReadOperationList.IsReadOnly; }
//        //}

//        new public virtual int LastIndexOf(T item)
//        {
//            return this.ReadOperationList.LastIndexOf(item);
//        }

//        new public virtual int LastIndexOf(T item, int index)
//        {
//            return this.ReadOperationList.LastIndexOf(item, index);
//        }

//        new public virtual int LastIndexOf(T item, int index, int count)
//        {
//            return this.ReadOperationList.LastIndexOf(item, index, count);
//        }

//        new public virtual T[] ToArray()
//        {
//            return this.ReadOperationList.ToArray();
//        }

//        public override string ToString()
//        {
//            return this.ReadOperationList.ToString();
//        }

//        new public virtual bool TrueForAll(Predicate<T> match)
//        {
//            return this.ReadOperationList.TrueForAll(match);
//        }

//        #endregion


//        /// <summary>
//        /// Override this property and set it to false to disable all caching operations.
//        /// </summary>
//        protected virtual bool RequestCachingEnabled
//        {
//            get { return true; }
//        }


//        protected virtual string GetCacheKey()
//        {
//            return "__REQUEST_CACHEABLE_LIST_" + this.instanceId.ToString();
//        }


//        /// <summary>
//        /// Gets a list object that can be used to to perform a read operation.
//        /// </summary>
//        protected virtual LockableList<T> ReadOperationList
//        {
//            get
//            {
//                LockableList<T> result = null;
//                if (this.RequestCachingEnabled)
//                {
//                    var key = this.GetCacheKey();
//                    result = this.requestCache.GetValue<LockableList<T>>(key);
//                    if (result == null)
//                    {
//                        // Request is not cached, return base list
//                        result = base.List;
//                    }
//                }
//                else
//                {
//                    result = base.List;
//                }
//                //result = base.List;
//                return result;
//            }
//        }

//        /// <summary>
//        /// Gets a list object that can be used to perform a write operation.
//        /// </summary>
//        protected virtual LockableList<T> WriteOperationList
//        {
//            get
//            {
//                LockableList<T> result = null;
//                if (this.IsReadOnly && this.RequestCachingEnabled)
//                {
//                    var key = this.GetCacheKey();
//                    result = this.requestCache.GetValue<LockableList<T>>(key);
//                    if (result == null)
//                    {
//                        // This is the first write operation request in read-only mode, 
//                        // we need to create a new dictionary and cache it
//                        // with a copy of the current values.
//                        result = new LockableList<T>(this.siteMap);
//                        base.CopyTo(result);
//                        this.requestCache.SetValue<LockableList<T>>(key, result);
//                    }
//                }
//                else
//                {
//                    result = base.List;
//                }
//                //result = base;
//                return result;
//            }
//        }


//    }
//}
