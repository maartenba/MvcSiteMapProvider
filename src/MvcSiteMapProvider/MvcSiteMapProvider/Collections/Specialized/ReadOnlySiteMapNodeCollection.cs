using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// A specialized collection that provides a read-only wrapper for a <see cref="T:MvcSiteMapProvider.ISiteMapNodeCollection"/>.
    /// </summary>
    public class ReadOnlySiteMapNodeCollection
            : ISiteMapNodeCollection
    {
        public ReadOnlySiteMapNodeCollection(
            ISiteMapNodeCollection siteMapNodeCollection
            )
        {
            if (siteMapNodeCollection == null)
                throw new ArgumentNullException("siteMapNodeCollection");

            this.siteMapNodeCollection = siteMapNodeCollection;
        }

        private readonly ISiteMapNodeCollection siteMapNodeCollection;

        #region ISiteMapNodeCollection Members

        public void AddRange(IEnumerable<ISiteMapNode> collection)
        {
            throw new NotSupportedException(Resources.Messages.CollectionReadOnly);
        }

        public void RemoveRange(int index, int count)
        {
            throw new NotSupportedException(Resources.Messages.CollectionReadOnly);
        }

        #endregion

        #region IList<ISiteMapNode> Members

        public int IndexOf(ISiteMapNode item)
        {
            return this.siteMapNodeCollection.IndexOf(item);
        }

        public void Insert(int index, ISiteMapNode item)
        {
            throw new NotSupportedException(Resources.Messages.CollectionReadOnly);
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException(Resources.Messages.CollectionReadOnly);
        }

        public ISiteMapNode this[int index]
        {
            get
            {
                return this.siteMapNodeCollection[index];
            }
            set
            {
                throw new NotSupportedException(Resources.Messages.CollectionReadOnly);
            }
        }

        #endregion

        #region ICollection<ISiteMapNode> Members

        public void Add(ISiteMapNode item)
        {
            throw new NotSupportedException(Resources.Messages.CollectionReadOnly);
        }

        public void Clear()
        {
            throw new NotSupportedException(Resources.Messages.CollectionReadOnly);
        }

        public bool Contains(ISiteMapNode item)
        {
            return this.siteMapNodeCollection.Contains(item);
        }

        public void CopyTo(ISiteMapNode[] array, int arrayIndex)
        {
            this.siteMapNodeCollection.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.siteMapNodeCollection.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(ISiteMapNode item)
        {
            throw new NotSupportedException(Resources.Messages.CollectionReadOnly);
        }

        #endregion

        #region IEnumerable<ISiteMapNode> Members

        public IEnumerator<ISiteMapNode> GetEnumerator()
        {
            return this.siteMapNodeCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.siteMapNodeCollection.GetEnumerator();
        }

        #endregion
    }
}
