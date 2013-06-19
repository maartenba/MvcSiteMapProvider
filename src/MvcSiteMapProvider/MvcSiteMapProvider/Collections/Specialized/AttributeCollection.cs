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
    public class AttributeCollection
        : RequestCacheableDictionary<string, object>, IAttributeCollection
    {
        public AttributeCollection(
            ISiteMap siteMap,
            ILocalizationService localizationService,
            IRequestCache requestCache
            )
            : base(siteMap, requestCache)
        {
            if (localizationService == null)
                throw new ArgumentNullException("localizationService");

            this.localizationService = localizationService;
        }

        protected readonly ILocalizationService localizationService;

        public override void Add(string key, object value)
        {
            ThrowIfReadOnly();
            if (value.GetType().Equals(typeof(string)))
                value = localizationService.ExtractExplicitResourceKey(key, value.ToString());
            base.Add(key, value);
        }

        public override void Clear()
        {
            ThrowIfReadOnly();
            foreach (var key in this.Keys)
            {
                localizationService.RemoveResourceKey(key);
            }
            base.Clear();
        }
        
        protected override void Insert(string key, object value, bool add)
        {
            ThrowIfReadOnly();
            if (value.GetType().Equals(typeof(string)))
                value = localizationService.ExtractExplicitResourceKey(key, value.ToString());
            base.Insert(key, value, add);
        }

        public override bool Remove(KeyValuePair<string, object> item)
        {
            ThrowIfReadOnly();
            localizationService.RemoveResourceKey(item.Key);
            return base.Remove(item);
        }

        public override bool Remove(string key)
        {
            ThrowIfReadOnly();
            localizationService.RemoveResourceKey(key);
            return base.Remove(key);
        }

        public override object this[string key]
        {
            get
            {
                var value = base[key];
                if (value.GetType().Equals(typeof(string)))
                {
                    return localizationService.GetResourceString(key, base[key].ToString(), base.siteMap);
                }
                else
                {
                    return value;
                }
            }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, key));
                }
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
