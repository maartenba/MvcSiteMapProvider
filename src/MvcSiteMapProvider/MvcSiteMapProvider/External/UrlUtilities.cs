#region Using directives

using System;
using System.Web;

#endregion

namespace MvcSiteMapProvider.External
{
    /// <summary>
    /// UrlUtilities class. See http://www.west-wind.com/Weblog/posts/154812.aspx for more information.
    /// </summary>
    public static class UrlUtilities
    {
        /// <summary>
        /// Returns a site relative HTTP path from a partial path starting out with a ~.
        /// Same syntax that ASP.Net internally supports but this method can be used
        /// outside of the Page framework.
        /// Works like Control.ResolveUrl including support for ~ syntax///
        /// but returns an absolute URL.
        /// </summary>
        /// <param name="originalUrl">Any Url including those starting with ~</param>
        /// <returns>Relative url</returns>
        public static string ResolveUrl(string originalUrl)
        {
            if (originalUrl == null)
            {
                return null;
            }

            // Absolute path - just return    
            if (originalUrl.IndexOf("://") != -1)
            {
                return originalUrl;
            }

            // Fix up image path for ~ root app dir directory    
            if (originalUrl.StartsWith("~"))
            {
                string newUrl = "";
                if (HttpContext.Current != null)
                {
                    newUrl = HttpContext.Current.Request.ApplicationPath +
                             originalUrl.Substring(1).Replace("//", "/");
                }
                else
                {
                    // Not context: assume current directory is the base directory   
                    throw new ArgumentException(Resources.Messages.RelativeUrlNotAllowed, "originalUrl");
                }

                // Just to be sure fix up any double slashes     
                return newUrl;
            }
            return originalUrl;
        }

        /// <summary>
        /// This method returns a fully qualified absolute server Url which includes
        /// the protocol, server, port in addition to the server relative Url.
        /// Works like Control.ResolveUrl including support for ~ syntax
        /// but returns an absolute URL.
        /// </summary>
        /// <param name="serverUrl">The server URL.</param>
        /// <param name="forceHttps">if true forces the url to use https</param>
        /// <returns>Fully qualified absolute server url.</returns>
        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            // Is it already an absolute Url?
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            // Start by fixing up the Url an Application relative Url
            string newUrl = ResolveUrl(serverUrl);

            // Due to URL rewriting, cloud computing (i.e. Azure)	
            // and web farms, etc., we have to be VERY careful about what	
            // we consider the incoming URL.  We want to see the URL as it would	
            // appear on the public-facing side of the hosting web site.	
            // HttpRequest.Url gives us the internal URL in a cloud environment,	
            // So we use a variable that (at least from what I can tell) gives us	
            // the public URL:
            Uri originalUri = null;
            if (HttpContext.Current.Request.Headers["Host"] != null)
            {
                string scheme = HttpContext.Current.Request.Headers["HTTP_X_FORWARDED_PROTO"] 
                    ?? HttpContext.Current.Request.Url.Scheme;
                originalUri = new Uri(scheme + Uri.SchemeDelimiter + HttpContext.Current.Request.Headers["Host"]);
            }
            else
            {
                originalUri = HttpContext.Current.Request.Url;
            }

            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                     "://" + originalUri.Authority + newUrl;

            if (newUrl.EndsWith("//"))
            {
                newUrl = newUrl.Substring(0, newUrl.Length - 2);
            }

            // Strip off the application root
            newUrl = new Uri(newUrl).GetLeftPart(UriPartial.Authority);

            return newUrl;
        }

        /// <summary>
        /// This method returns a fully qualified absolute server Url which includes
        /// the protocol, server, port in addition to the server relative Url.
        /// It work like Page.ResolveUrl, but adds these to the beginning.
        /// This method is useful for generating Urls for AJAX methods
        /// </summary>
        /// <param name="serverUrl">The server URL.</param>
        /// <returns>Fully qualified absolute server url.</returns>
        public static string ResolveServerUrl(string serverUrl)
        {
            return ResolveServerUrl(serverUrl, false);
        }
    }
}