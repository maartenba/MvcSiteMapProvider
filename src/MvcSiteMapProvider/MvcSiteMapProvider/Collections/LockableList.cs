using System;
using System.Collections.Generic;
using System.Linq;
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
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            base.Add(item);
        }

        new public virtual void AddRange(IEnumerable<T> collection)
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            base.AddRange(collection);
        }

        new public virtual void Clear()
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            base.Clear();
        }

        new public virtual void Insert(int index, T item)
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            base.Insert(index, item);
        }

        new public virtual void InsertRange(int index, IEnumerable<T> collection)
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            base.InsertRange(index, collection);
        }

        public virtual bool IsReadOnly
        {
            get { return this.siteMap.IsReadOnly; }
        }

        new public virtual bool Remove(T item)
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            return base.Remove(item);
        }

        new public virtual int RemoveAll(Predicate<T> match)
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            return base.RemoveAll(match);
        }

        new public virtual void RemoveAt(int index)
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            base.RemoveAt(index);
        }

        new public virtual void RemoveRange(int index, int count)
        {
            if (this.siteMap.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            base.RemoveRange(index, count);
        }

        public void CopyTo(IList<T> destination)
        {
            foreach (var item in this)
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

    }
}
