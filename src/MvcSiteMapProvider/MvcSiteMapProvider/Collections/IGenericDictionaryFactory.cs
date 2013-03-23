﻿using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.Collections.IDictionary<TKey, TValue>"/> 
    /// at runtime.
    /// </summary>
    public interface IGenericDictionaryFactory
    {
        IDictionary<TKey, TValue> Create<TKey, TValue>();
    }
}
