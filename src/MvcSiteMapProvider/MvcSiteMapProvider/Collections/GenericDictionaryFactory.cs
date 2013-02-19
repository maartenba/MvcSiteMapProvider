using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Collections
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:System.Collections.Generic.Dictionary<TKey, TValue>"/>
    /// at runtime.
    /// </summary>
    public class GenericDictionaryFactory
        : IGenericDictionaryFactory
    {

        #region IGenericDictionaryFactory Members

        public virtual IDictionary<TKey, TValue> Create<TKey, TValue>()
        {
            return new Dictionary<TKey, TValue>();
        }

        #endregion
    }
}
