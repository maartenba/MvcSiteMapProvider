using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGenericDictionaryFactory
    {
        IDictionary<TKey, TValue> Create<TKey, TValue>();
    }
}
