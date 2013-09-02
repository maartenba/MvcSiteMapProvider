using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// A specialized dictionary that contains the business logic for handling the attributes collection including
    /// localization of custom attributes.
    /// </summary>
    public class AttributeDictionary
        : CacheableDictionary<string, object>, IAttributeDictionary
    {
        public AttributeDictionary(
            ISiteMap siteMap,
            ILocalizationService localizationService,
            ICache cache
            )
            : base(siteMap, cache)
        {
            if (localizationService == null)
                throw new ArgumentNullException("localizationService");

            this.localizationService = localizationService;
        }

        protected readonly ILocalizationService localizationService;

        protected override string GetCacheKey()
        {
            return "__ATTRIBUTE_DICTIONARY_" + this.instanceId.ToString();
        }

        public override void Add(string key, object value)
        {
            if (value.GetType().Equals(typeof(string)))
                value = localizationService.ExtractExplicitResourceKey(key, value.ToString());
            base.Add(key, value);
        }

        public override void Clear()
        {
            base.Clear();
            if (!this.IsReadOnly)
            {
                foreach (var key in this.Keys)
                {
                    localizationService.RemoveResourceKey(key);
                }
            }
        }
        
        protected override void Insert(string key, object value, bool add)
        {
            if (value.GetType().Equals(typeof(string)))
                value = localizationService.ExtractExplicitResourceKey(key, value.ToString());
            base.Insert(key, value, add);
        }

        public override bool Remove(KeyValuePair<string, object> item)
        {
            var removed = base.Remove(item);
            if (removed && !this.IsReadOnly)
                localizationService.RemoveResourceKey(item.Key);
            return removed;
        }

        public override bool Remove(string key)
        {
            var removed = base.Remove(key);
            if (removed && !this.IsReadOnly)
                localizationService.RemoveResourceKey(key);
            return removed;
        }

        public override object this[string key]
        {
            get
            {
                var value = base[key];
                if (value.GetType().Equals(typeof(string)))
                {
                    return localizationService.GetResourceString(key, value.ToString(), base.siteMap);
                }
                else
                {
                    return value;
                }
            }
            set
            {
                if (value.GetType().Equals(typeof(string)))
                {
                    base[key] = localizationService.ExtractExplicitResourceKey(key, value.ToString());
                }
                else
                {
                    base[key] = value;
                }
            }
        }
    }
}
