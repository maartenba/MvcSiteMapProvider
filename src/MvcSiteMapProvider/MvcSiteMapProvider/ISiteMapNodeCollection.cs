using System;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using MvcSiteMapProvider.Collections;

namespace MvcSiteMapProvider
{
    public interface ISiteMapNodeCollection
        : IList<ISiteMapNode>, ICollection<ISiteMapNode>, IEnumerable<ISiteMapNode>
    {
        void AddRange(IEnumerable<ISiteMapNode> collection);
        //int RemoveAll(Predicate<ISiteMapNode> match);
        void RemoveRange(int index, int count);

        //int Add(ISiteMapNode value);
        //void AddRange(ISiteMapNode[] value);
        //void AddRange(ISiteMapNodeCollection value);
        //void Clear();
        //bool Contains(ISiteMapNode value);
        //void CopyTo(ISiteMapNode[] array, int index);
        //int Count { get; }
        //IEnumerator<ISiteMapNode> GetEnumerator();
        //IHierarchyData GetHierarchyData(object enumeratedItem);
        //int IndexOf(ISiteMapNode value);
        //void Insert(int index, ISiteMapNode value);
        //bool IsFixedSize { get; } // Don't need
        //bool IsReadOnly { get; }
        //bool IsSynchronized { get; } // Don't need
        //void Remove(ISiteMapNode value);
        //void RemoveAt(int index);
        //object SyncRoot { get; } // Don't need
        //ISiteMapNode this[int index] { get; set; }
    }
}
