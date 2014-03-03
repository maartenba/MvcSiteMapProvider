using System;
using System.Globalization;
using System.Threading;
using MvcSiteMapProvider.DI;

namespace MvcSiteMapProvider.Globalization
{
    /// <summary>
    /// Allows switching the current thread to a new culture in a using block that will automatically 
    /// return the culture to its previous state upon completion.
    /// </summary>
    [ExcludeFromAutoRegistration]
    public class CultureContext
        : ICultureContext
    {
        public CultureContext(
            CultureInfo culture,
            CultureInfo uiCulture
            )
        {
            if (culture == null)
                throw new ArgumentNullException("culture");
            if (uiCulture == null)
                throw new ArgumentNullException("uiCulture");

            this.currentThread = Thread.CurrentThread;

            // Record the current culture settings so they can be restored later.
            this.originalCulture = this.currentThread.CurrentCulture;
            this.originalUICulture = this.currentThread.CurrentUICulture;

            // Set both the culture and UI culture for this context.
            this.currentThread.CurrentCulture = culture;
            this.currentThread.CurrentUICulture = uiCulture;
        }

        private readonly Thread currentThread;
        private readonly CultureInfo originalCulture;
        private readonly CultureInfo originalUICulture;

        public CultureInfo OriginalCulture
        {
            get { return this.originalCulture; }
        }

        public CultureInfo OriginalUICulture
        {
            get { return this.originalUICulture; }
        }

        public void Dispose()
        {
            // Restore the culture to the way it was before the constructor was called.
            this.currentThread.CurrentCulture = this.originalCulture;
            this.currentThread.CurrentUICulture = this.originalUICulture;
        }
    }
}
