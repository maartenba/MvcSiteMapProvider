using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:System.Collections.Generic.Dictionary<TKey, TValue>"/>
    /// at runtime.
    /// </summary>
    public class GenericIgnoreCaseDictionaryFactory
        : IGenericDictionaryFactory
    {

        #region IGenericDictionaryFactory Members

        public virtual IDictionary<TKey, TValue> Create<TKey, TValue>()
        {
            if (typeof(TKey) == typeof(string))
                return (IDictionary<TKey, TValue>)new Dictionary<string, TValue>(StringComparer.OrdinalIgnoreCase);
            return new Dictionary<TKey, TValue>();
        }

        #endregion
    }
}
