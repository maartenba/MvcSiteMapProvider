using System;
using System.Collections.Generic;
using MvcSiteMapProvider;

namespace MvcSiteMapProvider.Collections
{
    /// <summary>
    /// Generic list that is aware of the ISiteMap interface and can be made read-only
    /// depending on the IsReadOnly property of ISiteMap.
    /// </summary>
    public class LockableList<T>
        : List<T>
    {
        public LockableList(ISiteMap siteMap)
        {
            if (siteMap == null)
                throw new ArgumentNullException("siteMap");

            this.siteMap = siteMap;
        }

        protected readonly ISiteMap siteMap;

        /// <summary>
        /// Adds an object to the end of the <see cref="T:LockableList"/>
        /// </summary>
        /// <param name="item">The item to add to the list.</param>
        new public virtual void Add(T item)
        {
            this.ThrowIfReadOnly();
            base.Add(item);
        }

        new public virtual void AddRange(IEnumerable<T> collection)
        {
            this.ThrowIfReadOnly();
            base.AddRange(collection);
        }

        new public virtual void Clear()
        {
            this.ThrowIfReadOnly();
            base.Clear();
        }

        new public virtual void Insert(int index, T item)
        {
            this.ThrowIfReadOnly();
            base.Insert(index, item);
        }

        new public virtual void InsertRange(int index, IEnumerable<T> collection)
        {
            this.ThrowIfReadOnly();
            base.InsertRange(index, collection);
        }

        public virtual bool IsReadOnly
        {
            get { return this.siteMap.IsReadOnly; }
        }

        new public virtual bool Remove(T item)
        {
            this.ThrowIfReadOnly();
            return base.Remove(item);
        }

        new public virtual int RemoveAll(Predicate<T> match)
        {
            this.ThrowIfReadOnly();
            return base.RemoveAll(match);
        }

        new public virtual void RemoveAt(int index)
        {
            this.ThrowIfReadOnly();
            base.RemoveAt(index);
        }

        new public virtual void RemoveRange(int index, int count)
        {
            this.ThrowIfReadOnly();
            base.RemoveRange(index, count);
        }

        new public virtual void Reverse()
        {
            this.ThrowIfReadOnly();
            base.Reverse();
        }

        new public virtual void Reverse(int index, int count)
        {
            this.ThrowIfReadOnly();
            base.Reverse(index, count);
        }

        new public virtual void Sort()
        {
            this.ThrowIfReadOnly();
            base.Sort();
        }

        new public virtual void Sort(Comparison<T> comparison)
        {
            this.ThrowIfReadOnly();
            base.Sort(comparison);
        }

        new public virtual void Sort(IComparer<T> comparer)
        {
            this.ThrowIfReadOnly();
            base.Sort(comparer);
        }

        new public virtual void Sort(int index, int count, IComparer<T> comparer)
        {
            this.ThrowIfReadOnly();
            base.Sort(index, count, comparer);
        }

        new public virtual void TrimExcess()
        {
            this.ThrowIfReadOnly();
            base.TrimExcess();
        }


        // Property access to internal list
        protected LockableList<T> List
        {
            get { return this; }
        }

        public virtual void CopyTo(IList<T> destination)
        {
            foreach (var item in this.List)
            {
                if (!item.GetType().IsPointer)
                {
                    destination.Add(item);
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
