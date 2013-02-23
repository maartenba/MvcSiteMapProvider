using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using MvcSiteMapProvider.Collections;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// A specialized dictionary that contains the business logic for handling the attributes collection including
    /// localization of custom attributes.
    /// </summary>
    public class AttributeCollection
        : RequestCacheableDictionary<string, string>, IAttributeCollection
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

        public override void Add(string key, string value)
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            value = localizationService.ExtractExplicitResourceKey(key, value);
            base.Add(key, value);
        }

        public override void Clear()
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            foreach (var key in this.Keys)
            {
                localizationService.RemoveResourceKey(key);
            }
            base.Clear();
        }
        
        protected override void Insert(string key, string value, bool add)
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            value = localizationService.ExtractExplicitResourceKey(key, value);
            base.Insert(key, value, add);
        }

        public override bool Remove(KeyValuePair<string, string> item)
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            localizationService.RemoveResourceKey(item.Key);
            return base.Remove(item);
        }

        public override bool Remove(string key)
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapReadOnly));
            }
            localizationService.RemoveResourceKey(key);
            return base.Remove(key);
        }

        public override string this[string key]
        {
            get
            {
                return localizationService.GetResourceString(key, base[key], base.siteMap);
            }
            set
            {
                if (this.IsReadOnly)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.SiteMapNodeReadOnly, key));
                }
                base[key] = localizationService.ExtractExplicitResourceKey(key, value);
            }
        }

        public virtual bool MatchesRoute(IEnumerable<string> actionParameters, IDictionary<string, object> routeValues)
        {
            if (routeValues.Count > 0)
            {
                foreach (var pair in routeValues)
                {
                    if (this.ContainsKey(pair.Key) && !string.IsNullOrEmpty(this[pair.Key]))
                    {
                        if (this[pair.Key].ToLowerInvariant() == pair.Value.ToString().ToLowerInvariant())
                        {
                            continue;
                        }
                        else
                        {
                            // Is the current pair.Key a parameter on the action method?
                            if (!actionParameters.Contains(pair.Key, StringComparer.InvariantCultureIgnoreCase))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (pair.Value == null || string.IsNullOrEmpty(pair.Value.ToString()) || pair.Value == UrlParameter.Optional)
                        {
                            continue;
                        }
                        else if (pair.Key == "area")
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
