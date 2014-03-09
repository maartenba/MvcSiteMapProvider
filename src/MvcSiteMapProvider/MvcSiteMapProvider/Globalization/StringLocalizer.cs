using System;
using System.Collections.Specialized;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider.Globalization
{
    /// <summary>
    /// Contains methods to deal with localization of strings to the current culture.
    /// </summary>
    public class StringLocalizer 
        : IStringLocalizer
    {
        public StringLocalizer(
            IMvcContextFactory mvcContextFactory
            )
        {
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");
            this.mvcContextFactory = mvcContextFactory;
        }

        protected readonly IMvcContextFactory mvcContextFactory;
        
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
                var httpContext = mvcContextFactory.CreateHttpContext();
                try
                {
                    globalResourceObject = httpContext.GetGlobalResourceObject(classKey, implicitResourceKey + "." + attributeName) as string;
                }
                catch
                {
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
                var httpContext = mvcContextFactory.CreateHttpContext();
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
                    throw new InvalidOperationException(string.Format(Resources.Messages.ResourceNotFoundWithClassAndKey, values[0], values[1]));
                }
            }
            return globalResourceObject;
        }
    }
}
