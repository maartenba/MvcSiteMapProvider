using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Web;


namespace MvcSiteMapProvider.Globalization
{
    /// <summary>
    /// Contains methods to deal with localization of strings to the current culture.
    /// </summary>
    public class StringLocalizer 
        : IStringLocalizer
    {
        public StringLocalizer(
            IHttpContextFactory httpContextFactory
            )
        {
            if (httpContextFactory == null)
                throw new ArgumentNullException("httpContextFactory");
            this.httpContextFactory = httpContextFactory;
        }

        protected readonly IHttpContextFactory httpContextFactory;
        
        /// <summary>
        /// Gets the localized text for the supplied attributeName.
        /// </summary>
        /// <param name="attributeName">The name of the attribute (as if it were in the original XML file).</param>
        /// <param name="value">The current object's value of the attribute.</param>
        /// <param name="enableLocalization">True if localization has been enabled, otherwise false.</param>
        /// <param name="classKey">The resource key from the ISiteMap class.</param>
        /// <param name="implicitResourceKey">The implicit resource key.</param>
        /// <param name="explicitResourceKeys">A <see cref="T:System.Collections.Specialized.NameValueCollection"/> containing the explicit resource keys.</param>
        /// <returns></returns>
        public virtual string GetResourceString(string attributeName, string value, bool enableLocalization, string classKey, string implicitResourceKey, NameValueCollection explicitResourceKeys)
        {
            if (enableLocalization)
            {
                string resourceString = this.GetImplicitResourceString(attributeName, implicitResourceKey, classKey);
                if (resourceString != null)
                {
                    return resourceString;
                }
                resourceString = this.GetExplicitResourceString(attributeName, value, true, explicitResourceKeys);
                if (resourceString != null)
                {
                    return resourceString;
                }
            }
            if (value != null)
            {
                return value;
            }
            return string.Empty;
        }

        protected virtual string GetImplicitResourceString(string attributeName, string implicitResourceKey, string classKey)
        {
            if (attributeName == null)
            {
                throw new ArgumentNullException("attributeName");
            }
            string globalResourceObject = null;
            if (!string.IsNullOrEmpty(implicitResourceKey))
            {
                var httpContext = httpContextFactory.Create();
                try
                {
                    globalResourceObject = httpContext.GetGlobalResourceObject(classKey, implicitResourceKey + "." + attributeName) as string;
                }
                catch
                {
                    // TODO: it would be nice if we had a way to log or otherwise send this error to the developer for inspection.
                }
            }
            return globalResourceObject;
        }

        protected virtual string GetExplicitResourceString(string attributeName, string defaultValue, bool throwIfNotFound, NameValueCollection explicitResourceKeys)
        {
            if (attributeName == null)
            {
                throw new ArgumentNullException("attributeName");
            }
            string globalResourceObject = null;
            if (explicitResourceKeys != null)
            {
                string[] values = explicitResourceKeys.GetValues(attributeName);
                if ((values == null) || (values.Length <= 1))
                {
                    return globalResourceObject;
                }
                var httpContext = httpContextFactory.Create();
                try
                {
                    globalResourceObject = httpContext.GetGlobalResourceObject(values[0], values[1]) as string;
                }
                catch (System.Resources.MissingManifestResourceException)
                {
                    if (defaultValue != null)
                    {
                        return defaultValue;
                    }
                }
                if ((globalResourceObject == null) && throwIfNotFound)
                {
                    throw new InvalidOperationException(String.Format(Resources.Messages.ResourceNotFoundWithClassAndKey, values[0], values[1]));
                }
            }
            return globalResourceObject;
        }
    }
}
