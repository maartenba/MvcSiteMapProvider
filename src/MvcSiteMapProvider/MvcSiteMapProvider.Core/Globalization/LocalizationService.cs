using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Core.SiteMap;

namespace MvcSiteMapProvider.Core.Globalization
{
    /// <summary>
    /// Provides services to extract meta-keys and to later use the keys to localize text into different cultures. 
    /// </summary>
    public class LocalizationService 
        : ILocalizationService
    {
        public LocalizationService(
            string implicitResourceKey,
            IExplicitResourceKeyParser explicitResourceKeyParser,
            IStringLocalizer stringLocalizer
            )
        {
            if (explicitResourceKeyParser == null)
                throw new ArgumentNullException("explicitResourceKeyParser");
            if (stringLocalizer == null)
                throw new ArgumentNullException("stringLocalizer");

            this.ResourceKey = implicitResourceKey;
            this.explicitResourceKeyParser = explicitResourceKeyParser;
            this.stringLocalizer = stringLocalizer;
        }

        protected readonly IExplicitResourceKeyParser explicitResourceKeyParser;
        protected readonly IStringLocalizer stringLocalizer;
        protected NameValueCollection explicitResourceKeys = new NameValueCollection();

        /// <summary>
        /// Gets or sets the implicit resource key (optional).
        /// </summary>
        /// <value>The implicit resource key.</value>
        public string ResourceKey { get; protected set; }

        /// <summary>
        /// Extracts any meta information that is used to lookup the resource key and stores it for later use by the <see cref="GetResourceString"/> method.
        /// </summary>
        /// <param name="attributeName">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        /// <returns>The new value after the evaluation.</returns>
        public virtual string ExtractExplicitResourceKey(string attributeName, string value)
        {
            var currentValue = value;
            explicitResourceKeyParser.HandleResourceAttribute(attributeName, ref currentValue, ref explicitResourceKeys);
            return currentValue;
        }

        public virtual void RemoveResourceKey(string attributeName)
        {
            if (explicitResourceKeys.AllKeys.Contains(attributeName))
            {
                explicitResourceKeys.Remove(attributeName);
            }
        }

        public virtual string GetResourceString(string attributeName, string value, ISiteMap siteMap)
        {
            return stringLocalizer.GetResourceString(
                attributeName, value, siteMap.EnableLocalization, siteMap.ResourceKey, this.ResourceKey, this.explicitResourceKeys);
        }
    }
}
