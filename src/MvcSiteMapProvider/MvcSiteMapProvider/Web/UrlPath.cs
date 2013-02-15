using System;
using System.Collections;
using System.Text;
using System.Web;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UrlPath 
        : IUrlPath
    {
        public string AppDomainAppVirtualPath
        {
            get { return HttpRuntime.AppDomainAppVirtualPath; }
        }

        public string MakeVirtualPathAppAbsolute(string virtualPath)
        {
            return MakeVirtualPathAppAbsolute(virtualPath, HttpRuntime.AppDomainAppVirtualPath);
        }

        public string MakeVirtualPathAppAbsolute(string virtualPath, string applicationPath)
        {
            if ((virtualPath.Length == 1) && (virtualPath[0] == '~'))
            {
                return applicationPath;
            }
            if (((virtualPath.Length >= 2) && (virtualPath[0] == '~')) && ((virtualPath[1] == '/') || (virtualPath[1] == '\\')))
            {
                if (applicationPath.Length > 1)
                {
                    return (applicationPath + virtualPath.Substring(2));
                }
                return ("/" + virtualPath.Substring(2));
            }
            if (!IsRooted(virtualPath))
            {
                throw new ArgumentOutOfRangeException("virtualPath");
            }
            return virtualPath;
        }

        public bool IsAppRelativePath(string path)
        {
            if (path == null)
            {
                return false;
            }
            int length = path.Length;
            if (length == 0)
            {
                return false;
            }
            if (path[0] != '~')
            {
                return false;
            }
            if ((length != 1) && (path[1] != '\\'))
            {
                return (path[1] == '/');
            }
            return true;
        }

        public bool IsRooted(string basepath)
        {
            if (!string.IsNullOrEmpty(basepath) && (basepath[0] != '/'))
            {
                return (basepath[0] == '\\');
            }
            return true;
        }

        public bool IsAbsolutePhysicalPath(string path)
        {
            if ((path == null) || (path.Length < 3))
            {
                return false;
            }
            return (((path[1] == ':') && IsDirectorySeparatorChar(path[2])) || IsUncSharePath(path));
        }

        private bool IsDirectorySeparatorChar(char ch)
        {
            if (ch != '\\')
            {
                return (ch == '/');
            }
            return true;
        }

        private bool IsUncSharePath(string path)
        {
            return (((path.Length > 2) && IsDirectorySeparatorChar(path[0])) && IsDirectorySeparatorChar(path[1]));
        }

        public string Combine(string basepath, string relative)
        {
            return Combine(HttpRuntime.AppDomainAppVirtualPath, basepath, relative);
        }

        private string Combine(string appPath, string basepath, string relative)
        {
            string str;
            if (string.IsNullOrEmpty(relative))
            {
                throw new ArgumentNullException("relative");
            }
            if (string.IsNullOrEmpty(basepath))
            {
                throw new ArgumentNullException("basepath");
            }
            if ((basepath[0] == '~') && (basepath.Length == 1))
            {
                basepath = "~/";
            }
            else
            {
                int num = basepath.LastIndexOf('/');
                if (num < (basepath.Length - 1))
                {
                    basepath = basepath.Substring(0, num + 1);
                }
            }
            CheckValidVirtualPath(relative);
            if (IsRooted(relative))
            {
                str = relative;
            }
            else
            {
                if ((relative.Length == 1) && (relative[0] == '~'))
                {
                    return appPath;
                }
                if (IsAppRelativePath(relative))
                {
                    if (appPath.Length > 1)
                    {
                        str = appPath + "/" + relative.Substring(2);
                    }
                    else
                    {
                        str = "/" + relative.Substring(2);
                    }
                }
                else
                {
                    str = SimpleCombine(basepath, relative);
                }
            }
            return Reduce(str);
        }

        private string SimpleCombine(string basepath, string relative)
        {
            if (HasTrailingSlash(basepath))
            {
                return (basepath + relative);
            }
            return (basepath + "/" + relative);
        }

        private bool HasTrailingSlash(string virtualPath)
        {
            return (virtualPath[virtualPath.Length - 1] == '/');
        }

        private void CheckValidVirtualPath(string path)
        {
            if (IsAbsolutePhysicalPath(path))
            {
                throw new HttpException(string.Format(Resources.Messages.PhysicalPathNotAllowed, path));
            }
            int index = path.IndexOf('?');
            if (index >= 0)
            {
                path = path.Substring(0, index);
            }
            if (HasScheme(path))
            {
                throw new HttpException(string.Format(Resources.Messages.InvalidVirtualPath, path));
            }
        }

        private bool HasScheme(string virtualPath)
        {
            int index = virtualPath.IndexOf(':');
            if (index == -1)
            {
                return false;
            }
            int num2 = virtualPath.IndexOf('/');
            if (num2 != -1)
            {
                return (index < num2);
            }
            return true;
        }

        private string Reduce(string path)
        {
            string str = null;
            if (path != null)
            {
                int index = path.IndexOf('?');
                if (index >= 0)
                {
                    str = path.Substring(index);
                    path = path.Substring(0, index);
                }
            }
            path = FixVirtualPathSlashes(path);
            path = ReduceVirtualPath(path);
            if (str == null)
            {
                return path;
            }
            return (path + str);
        }

        private string ReduceVirtualPath(string path)
        {
            int length = path.Length;
            int startIndex = 0;
            while (true)
            {
                startIndex = path.IndexOf('.', startIndex);
                if (startIndex < 0)
                {
                    return path;
                }
                if (((startIndex == 0) || (path[startIndex - 1] == '/')) && ((((startIndex + 1) == length) || (path[startIndex + 1] == '/')) || ((path[startIndex + 1] == '.') && (((startIndex + 2) == length) || (path[startIndex + 2] == '/')))))
                {
                    break;
                }
                startIndex++;
            }
            ArrayList list = new ArrayList();
            StringBuilder builder = new StringBuilder();
            startIndex = 0;
            do
            {
                int num3 = startIndex;
                startIndex = path.IndexOf('/', num3 + 1);
                if (startIndex < 0)
                {
                    startIndex = length;
                }
                if ((((startIndex - num3) <= 3) && ((startIndex < 1) || (path[startIndex - 1] == '.'))) && (((num3 + 1) >= length) || (path[num3 + 1] == '.')))
                {
                    if ((startIndex - num3) == 3)
                    {
                        if (list.Count == 0)
                        {
                            throw new HttpException(Resources.Messages.CannotExitUpTopDirectory);
                        }
                        if ((list.Count == 1) && IsAppRelativePath(path))
                        {
                            return ReduceVirtualPath(MakeVirtualPathAppAbsolute(path));
                        }
                        builder.Length = (int)list[list.Count - 1];
                        list.RemoveRange(list.Count - 1, 1);
                    }
                }
                else
                {
                    list.Add(builder.Length);
                    builder.Append(path, num3, startIndex - num3);
                }
            }
            while (startIndex != length);
            string str = builder.ToString();
            if (str.Length != 0)
            {
                return str;
            }
            if ((length > 0) && (path[0] == '/'))
            {
                return "/";
            }
            return ".";
        }

        private string FixVirtualPathSlashes(string virtualPath)
        {
            virtualPath = virtualPath.Replace('\\', '/');
            while (true)
            {
                string str = virtualPath.Replace("//", "/");
                if (str == virtualPath)
                {
                    return virtualPath;
                }
                virtualPath = str;
            }
        }

        public string UrlEncode(string url)
        {
            return HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// Returns a site relative HTTP path from a partial path starting out with a ~.
        /// Same syntax that ASP.Net internally supports but this method can be used
        /// outside of the Page framework.
        /// Works like Control.ResolveUrl including support for ~ syntax///
        /// but returns an absolute URL.
        /// </summary>
        /// <remarks>See http://www.west-wind.com/Weblog/posts/154812.aspx for more information.</remarks>
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
        /// <remarks>See http://www.west-wind.com/Weblog/posts/154812.aspx for more information.</remarks>
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
        /// <remarks>See http://www.west-wind.com/Weblog/posts/154812.aspx for more information.</remarks>
        /// <param name="serverUrl">The server URL.</param>
        /// <returns>Fully qualified absolute server url.</returns>
        public static string ResolveServerUrl(string serverUrl)
        {
            return ResolveServerUrl(serverUrl, false);
        }
    }
}
