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
    using MvcSiteMapProvider.Core.Globalization;

    // TODO: It is probably not necessary to inherit from ObservableDictionary. Need to find another Dictionary to inherit from.

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AttributeCollection
        : ObservableDictionary<string, string>
    {

        public AttributeCollection(
            ISiteMap siteMap,
            ILocalizationService localizationService
            )
        {
            if (siteMap == null)
                throw new ArgumentNullException("siteMap");
            if (localizationService == null)
                throw new ArgumentNullException("localizationService");

            this.siteMap = siteMap;
            this.localizationService = localizationService;
        }

        protected readonly ISiteMap siteMap;
        protected readonly ILocalizationService localizationService;

        public override void Add(string key, string value)
        {
            value = localizationService.ExtractExplicitResourceKey(key, value);
            base.Add(key, value);
        }

        public override string this[string key]
        {
            get
            {
                return localizationService.GetResourceString(key, base[key], siteMap);
            }
            set
            {
                base[key] = localizationService.ExtractExplicitResourceKey(key, value);
            }
        }

    }
}
