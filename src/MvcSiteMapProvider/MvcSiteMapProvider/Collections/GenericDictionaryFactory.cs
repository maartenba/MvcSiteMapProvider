using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Collections
{
    /// <summary>
    /// TODO: Update summary.
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
