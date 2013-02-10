using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Core.Collections
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGenericDictionaryFactory
    {
        IDictionary<TKey, TValue> Create<TKey, TValue>();
    }
}
