using System;

namespace MvcSiteMapProvider.Globalization
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.Globalization.LocalizationService"/>
    /// at runtime.
    /// </summary>
    public class LocalizationServiceFactory
        : ILocalizationServiceFactory
    {
        public LocalizationServiceFactory(
            IExplicitResourceKeyParser explicitResourceKeyParser,
            IStringLocalizer stringLocalizer
            )
        {
            if (explicitResourceKeyParser == null)
                throw new ArgumentNullException("explicitResourceKeyParser");
            if (stringLocalizer == null)
                throw new ArgumentNullException("stringLocalizer");

            this.explicitResourceKeyParser = explicitResourceKeyParser;
            this.stringLocalizer = stringLocalizer;
        }

        protected readonly IExplicitResourceKeyParser explicitResourceKeyParser;
        protected readonly IStringLocalizer stringLocalizer;

        #region ILocalizationServiceFactory Members

        public ILocalizationService Create(string implicitResourceKey)
        {
            return new LocalizationService(implicitResourceKey, explicitResourceKeyParser, stringLocalizer);
        }

        #endregion
    }
}
