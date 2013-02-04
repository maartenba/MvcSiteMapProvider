// -----------------------------------------------------------------------
// <copyright file="SiteMapNodeAttributeCollection.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using MvcSiteMapProvider.Core.Collections;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapNodeAttributeDictionary
        : ObservableDictionary<string, string>
    {

        public override void AddRange(IDictionary<string, string> items)
        {
            base.AddRange(items);
        }

        public override void Add(KeyValuePair<string, string> item)
        {
            base.Add(item);
        }

        public override void Add(string key, string value)
        {
            base.Add(key, value);
        }
    }
}
