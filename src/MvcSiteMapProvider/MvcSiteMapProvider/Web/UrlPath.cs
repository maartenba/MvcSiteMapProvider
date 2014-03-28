using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Contains methods for working with URLs.
    /// </summary>
    public class UrlPath 
        : IUrlPath
    {
        public UrlPath(
            IMvcContextFactory mvcContextFactory,
            IBindingProvider bindingProvider
            )
        {
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");
            if (bindingProvider == null)
                throw new ArgumentNullException("bindingProvider");

            this.mvcContextFactory = mvcContextFactory;
            this.bindingProvider = bindingProvider;
        }

        protected readonly IMvcContextFactory mvcContextFactory;
        protected readonly IBindingProvider bindingProvider;

        protected virtual HttpContextBase HttpContext
        {
            get { return this.mvcContextFactory.CreateHttpContext(); }
        }

        public string AppDomainAppVirtualPath
        {
            get { return this.HttpContext.Request.ApplicationPath; }
        }

        public string MakeVirtualPathAppAbsolute(string virtualPath)
        {
            return MakeVirtualPathAppAbsolute(virtualPath, this.AppDomainAppVirtualPath);
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

        /// <summary>
        /// Combines multiple strings into a URL, fixing any problems with forward 
        /// and backslashes.
        /// </summary>
        /// <param name="uriParts">An array of strings to combine.</param>
        /// <returns>The combined URL.</returns>
        /// <remarks>Source: http://stackoverflow.com/questions/372865/path-combine-for-urls/6704287#6704287 </remarks>
        public string CombineUrl(params string[] uriParts)
        {
            string uri = string.Empty;
            if (uriParts != null && uriParts.Length > 0)
            {
                char[] trims = new char[] { '\\', '/' };
                uri = (uriParts[0] ?? string.Empty).TrimEnd(trims);
                for (int i = 1; i < uriParts.Length; i++)
                {
                    uri = string.Format("{0}/{1}", uri.TrimEnd(trims), (uriParts[i] ?? string.Empty).TrimStart(trims));
                }
            }
            return uri;
        }

        public string Combine(string basepath, string relative)
        {
            return Combine(this.AppDomainAppVirtualPath, basepath, relative);
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

        public string UrlDecode(string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        /// <summary>
        /// Determines if the URL is an absolute or relative URL.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <returns><b>true</b> if the URL is absolute; otherwise <b>false</b>.</returns>
        public bool IsAbsoluteUrl(string url)
        {
            // Optimization: Return false early if there is no scheme delimiter in the string
            // prefixed by at least 1 character.
            if (!(url.IndexOf(Uri.SchemeDelimiter) > 0))
                return false;

            // There must be at least 1 word character before the scheme delimiter.
            // This ensures we don't return true for query strings that contain absolute URLs.
            return Regex.IsMatch(url, @"^\w+://", RegexOptions.Compiled);
        }

        /// <summary>
        /// Determines if a URL is not part of the current application or web site.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <param name="httpContext">The HTTP context representing the request.</param>
        /// <returns><b>true</b> if the URL is not part of the virtual application or is on a different host name; otherwise <b>false</b>.</returns>
        public bool IsExternalUrl(string url, HttpContextBase httpContext)
        {
            if (!this.IsAbsoluteUrl(url))
                return false;

            Uri uri = null;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                var publicFacingUrl = this.GetPublicFacingUrl(httpContext);
                var isDifferentHost = !uri.Host.ToLowerInvariant().Equals(publicFacingUrl.Host.ToLowerInvariant());
                var isDifferentVirtualApplication = !uri.AbsolutePath.StartsWith(httpContext.Request.ApplicationPath, StringComparison.InvariantCultureIgnoreCase);

                return (isDifferentHost || isDifferentVirtualApplication);
            }

            return false;
        }

        /// <summary>
        /// Determines if the host name matches the public facing host name.
        /// </summary>
        /// <param name="hostName">The host name.</param>
        /// <param name="httpContext">The HTTP context representing the request.</param>
        /// <returns><b>true</b> if the host name matches that of the public URL; otherwise <b>false</b>.</returns>
        public bool IsPublicHostName(string hostName, HttpContextBase httpContext)
        {
            var publicFacingUrl = this.GetPublicFacingUrl(httpContext);
            if (string.Equals(publicFacingUrl.Host, hostName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resolves the URL to an absolute URL using the HTTP protocol.
        /// Absolute URLs will be passed through unchanged.
        /// </summary>
        /// <param name="url">
        /// Any Url including those starting with "/", "~", or protocol.
        /// </param>
        /// <returns>The absolute URL.</returns>
        public string MakeUrlAbsolute(string url)
        {
            if (this.IsAbsoluteUrl(url))
                return url;

            return this.ResolveUrl(url, Uri.UriSchemeHttp);
        }

        /// <summary>
        /// Resolves the URL and combines it with the specified base URL to
        /// make an absolute URL. Absolute URLs will be passed through unchanged.
        /// </summary>
        /// <param name="baseUrl">An absolute URL beginning with protocol.</param>
        /// <param name="url">
        /// Any Url including those starting with "/", "~", or protocol. 
        /// If an absolute URL is provided in this field, the baseUrl will be ignored.
        /// </param>
        /// <returns>The absolute URL.</returns>
        public string MakeUrlAbsolute(string baseUrl, string url)
        {
            if (this.IsAbsoluteUrl(url))
                return url;

            return this.CombineUrl(baseUrl, this.ResolveUrl(url));
        }

        /// <summary>
        /// Resolves a URL that starts with a "~" into a URL that starts with the virtual
        /// application path. For example ~/MySite/ will resolve to /VirtualApplication/MySite/.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <returns>The resolved URL.</returns>
        public string ResolveVirtualApplicationToRootRelativeUrl(string url)
        {
            if (this.IsAbsoluteUrl(url) || this.IsAbsolutePhysicalPath(url))
                return url;

            return this.MakeVirtualPathAppAbsolute(this.Combine(this.AppDomainAppVirtualPath, url));
        }

        /// <summary>
        /// Resolves a URL, similar to how it would on Control.ResolveUrl() in ASP.NET.
        /// If the URL begins with a "/", it will be resolved to the web root. If the 
        /// URL begins with a "~", it will be resolved to the virtual application root.
        /// Absolute URLs will be passed through unchanged.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <returns>The resolved URL.</returns>
        public string ResolveUrl(string url)
        {
            return this.GenerateUrl(url, null /* protocol */, null /* hostName */, true /* defaultToHttp */, this.HttpContext);
        }

        /// <summary>
        /// Resolves a URL, similar to how it would on Control.ResolveUrl() in ASP.NET.
        /// If the URL begins with a "/", it will be resolved to the web root. If the 
        /// URL begins with a "~", it will be resolved to the virtual application root.
        /// Absolute URLs will be passed through unchanged.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <param name="protocol">The protocol such as http, https, or ftp. Defaults to http 
        /// protocol if null or empty string. To use the protocol of the current request, use *.</param>
        /// <returns>The resolved URL.</returns>
        public string ResolveUrl(string url, string protocol)
        {
            return this.GenerateUrl(url, protocol, null /* hostName */, true /* defaultToHttp */, this.HttpContext);
        }

        /// <summary>
        /// Resolves a URL, similar to how it would on Control.ResolveUrl() in ASP.NET.
        /// If the URL begins with a "/", it will be resolved to the web root. If the 
        /// URL begins with a "~", it will be resolved to the virtual application root.
        /// Absolute URLs will be passed through unchanged.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <param name="protocol">The protocol such as http, https, or ftp. Defaults to http 
        /// protocol if null or empty string. To use the protocol of the current request, use *.</param>
        /// <param name="hostName">The host name such as www.somewhere.com.</param>
        /// <returns>The resolved URL.</returns>
        public string ResolveUrl(string url, string protocol, string hostName)
        {
            return this.GenerateUrl(url, protocol, hostName, true /* defaultToHttp */, this.HttpContext);
        }

        /// <summary>
        /// Resolves a URL, similar to how it would on Control.ResolveUrl() in ASP.NET.
        /// If the URL begins with a "/", it will be resolved to the web root. If the 
        /// URL begins with a "~", it will be resolved to the virtual application root.
        /// Absolute URLs will be passed through unchanged. Uses the protocol of the request.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <param name="protocol">The protocol such as http, https, or ftp. Defaults to http 
        /// protocol if null or empty string. To use the protocol of the current request, use *.</param>
        /// <param name="hostName">The host name such as www.somewhere.com.</param>
        /// <param name="httpContext">The HTTP context representing the context of the request.</param>
        /// <returns>The resolved URL.</returns>
        public string ResolveUrl(string url, string protocol, string hostName, HttpContextBase httpContext)
        {
            return this.GenerateUrl(url, protocol, hostName, true /* defaultToHttp */, httpContext);
        }

        /// <summary>
        /// Resolves a URL, similar to how it would on Control.ResolveUrl() in ASP.NET.
        /// If the URL begins with a "/", it will be resolved to the web root. If the 
        /// URL begins with a "~", it will be resolved to the virtual application root.
        /// Absolute URLs will be passed through unchanged.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <returns>The resolved URL.</returns>
        public string ResolveContentUrl(string url)
        {
            return this.GenerateUrl(url, null /* protocol */, null /* hostName */, false /* defaultToHttp */, this.HttpContext);
        }

        /// <summary>
        /// Resolves a URL, similar to how it would on Control.ResolveUrl() in ASP.NET.
        /// If the URL begins with a "/", it will be resolved to the web root. If the 
        /// URL begins with a "~", it will be resolved to the virtual application root.
        /// Absolute URLs will be passed through unchanged.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <param name="protocol">The protocol such as http, https, or ftp. Defaults to protocol of the
        /// request to prevent errors when resolving content under https.</param>
        /// <returns>The resolved URL.</returns>
        public string ResolveContentUrl(string url, string protocol)
        {
            return this.GenerateUrl(url, protocol, null /* hostName */, false /* defaultToHttp */, this.HttpContext);
        }

        /// <summary>
        /// Resolves a URL, similar to how it would on Control.ResolveUrl() in ASP.NET.
        /// If the URL begins with a "/", it will be resolved to the web root. If the 
        /// URL begins with a "~", it will be resolved to the virtual application root.
        /// Absolute URLs will be passed through unchanged. 
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <param name="protocol">The protocol such as http, https, or ftp. Defaults to protocol of the
        /// request to prevent errors when resolving content under https.</param>
        /// <param name="hostName">The host name such as www.somewhere.com.</param>
        /// <returns>The resolved URL.</returns>
        public string ResolveContentUrl(string url, string protocol, string hostName)
        {
            return this.GenerateUrl(url, protocol, hostName, false /* defaultToHttp */, this.HttpContext);
        }

        /// <summary>
        /// Resolves a URL, similar to how it would on Control.ResolveUrl() in ASP.NET.
        /// If the URL begins with a "/", it will be resolved to the web root. If the 
        /// URL begins with a "~", it will be resolved to the virtual application root.
        /// Absolute URLs will be passed through unchanged.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <param name="protocol">The protocol such as http, https, or ftp. Defaults to protocol of the
        /// request to prevent errors when resolving content under https.</param>
        /// <param name="hostName">The host name such as www.somewhere.com.</param>
        /// <param name="httpContext">The HTTP context representing the context of the request.</param>
        /// <returns>The resolved URL.</returns>
        public string ResolveContentUrl(string url, string protocol, string hostName, HttpContextBase httpContext)
        {
            return this.GenerateUrl(url, protocol, hostName, false /* defaultToHttp */, httpContext);
        }

        /// <summary>
        /// Resolves a URL, similar to how it would on Control.ResolveUrl() in ASP.NET.
        /// If the URL begins with a "/", it will be resolved to the web root. If the 
        /// URL begins with a "~", it will be resolved to the virtual application root.
        /// Absolute URLs will be passed through unchanged.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <param name="protocol">The protocol such as http, https, or ftp. 
        /// To use the protocol of the current request, use *.</param>
        /// <param name="hostName">The host name such as www.somewhere.com.</param>
        /// <param name="defaultToHttp">
        /// <b>true</b> to default the protocol to http if it is null or empty string; 
        /// <b>false</b> to default the protocol to that of the current request.
        /// </param>
        /// <param name="httpContext">The HTTP context representing the context of the request.</param>
        /// <returns>The resolved URL.</returns>
        protected virtual string GenerateUrl(string url, string protocol, string hostName, bool defaultToHttp, HttpContextBase httpContext)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            if (!this.IsAbsoluteUrl(url))
            {
                url = this.ResolveVirtualApplicationToRootRelativeUrl(url);

                if (!string.IsNullOrEmpty(protocol) || !string.IsNullOrEmpty(hostName))
                {
                    Uri requestUrl = this.GetPublicFacingUrl(httpContext);
                    bool isWildcardProtocol = (protocol == "*");
                    string defaultProtocol = (isWildcardProtocol || !defaultToHttp) ? requestUrl.Scheme : Uri.UriSchemeHttp;

                    // Get the protocol and hostName
                    protocol = (!string.IsNullOrEmpty(protocol) && !isWildcardProtocol) ? protocol : defaultProtocol;
                    hostName = !string.IsNullOrEmpty(hostName) ? hostName : requestUrl.Host;

                    // Get the port
                    string port = this.GetPortString(protocol, hostName, requestUrl);

                    url = protocol + Uri.SchemeDelimiter + hostName + port + url;
                }
            }

            return url;
        }

        protected virtual string GetPortString(string protocol, string hostName, Uri requestUrl)
        {
            int port = 80;
            bool isProtocolMatch = string.Equals(protocol, requestUrl.Scheme, StringComparison.OrdinalIgnoreCase);
            bool isHostMatch = string.Equals(hostName, requestUrl.Host, StringComparison.OrdinalIgnoreCase);
            bool isDevelopmentEnvironment = (this.IsVisualStudioDevelopmentServer && isHostMatch);

            if ((isProtocolMatch && isHostMatch) || isDevelopmentEnvironment)
            {
                port = requestUrl.Port;
            }
            else
            {
                // Attempt to get the port bindings from the binding provider.
                bool succeeded = false;
                var bindings = this.bindingProvider.GetBindings();
                if (bindings != null)
                {
                    // Match the protocol
                    var protocolBindings = bindings.Where(x => string.Equals(x.Protocol, protocol, StringComparison.OrdinalIgnoreCase)).ToList();
                    if (protocolBindings.Count > 0)
                    {
                        // Favor an exact match
                        var binding = protocolBindings.Where(x => string.Equals(x.HostName, hostName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (binding == null)
                        {
                            // Try using a wildcard *
                            binding = protocolBindings.Where(x => x.HostName == "*").FirstOrDefault();
                        }
                        if (binding != null)
                        {
                            port = binding.Port;
                            succeeded = true;
                        }
                    }
                }

                if (!succeeded)
                {
                    // Binding not found - use default port.
                    return string.Empty;
                }
            }

            return this.IsDefaultPort(port, protocol) ? string.Empty : (":" + Convert.ToString(port, CultureInfo.InvariantCulture));
        }

        protected virtual bool IsDefaultPort(int port, string protocol)
        {
            return new Uri(protocol + Uri.SchemeDelimiter + "unknownhost:" + 
                Convert.ToString(port, CultureInfo.InvariantCulture) + "/").IsDefaultPort;
        }

        protected virtual bool IsVisualStudioDevelopmentServer
        {
            get { return string.IsNullOrEmpty(this.HttpContext.Request.ServerVariables["SERVER_SOFTWARE"]); }
        }

        /// <summary>
        /// Gets the public facing URL for the given incoming HTTP request.
        /// </summary>
        /// <param name="httpContext">The HTTP context representing the context of the request.</param>
        /// <returns>The URI that the outside world used to create this request.</returns>
        /// <remarks>Source: http://stackoverflow.com/questions/7795910/how-do-i-get-url-action-to-use-the-right-port-number#11888846 </remarks>
        public Uri GetPublicFacingUrl(HttpContextBase httpContext)
        {
            var serverVariables = httpContext.Request.ServerVariables;
            var request = httpContext.Request;

            // Due to URL rewriting, cloud computing (i.e. Azure)
            // and web farms, etc., we have to be VERY careful about what
            // we consider the incoming URL.  We want to see the URL as it would
            // appear on the public-facing side of the hosting web site.
            // HttpRequest.Url gives us the internal URL in a cloud environment,
            // So we use a variable that (at least from what I can tell) gives us
            // the public URL:
            if (serverVariables["HTTP_HOST"] != null)
            {
                string scheme = serverVariables["HTTP_X_FORWARDED_PROTO"] ?? request.Url.Scheme;
                Uri hostAndPort = new Uri(scheme + Uri.SchemeDelimiter + serverVariables["HTTP_HOST"]);
                UriBuilder publicRequestUri = new UriBuilder(request.Url);
                publicRequestUri.Scheme = scheme;
                publicRequestUri.Host = hostAndPort.Host;
                publicRequestUri.Port = hostAndPort.Port; // CC missing Uri.Port contract that's on UriBuilder.Port
                return publicRequestUri.Uri;
            }
            // Failover to the method that works for non-web farm environments.
            // We use Request.Url for the full path to the server, and modify it
            // with Request.RawUrl to capture both the cookieless session "directory" if it exists
            // and the original path in case URL rewriting is going on.  We don't want to be
            // fooled by URL rewriting because we're comparing the actual URL with what's in
            // the return_to parameter in some cases.
            // Response.ApplyAppPathModifier(builder.Path) would have worked for the cookieless
            // session, but not the URL rewriting problem.
            return new Uri(request.Url, request.RawUrl);
        }

        [Obsolete(@"Use MakeUrlAbsolute(string) instead. Example: This method will be removed in version 5.")]
        public string MakeRelativeUrlAbsolute(string url)
        {
            if (!IsAbsolutePhysicalPath(url))
            {
                url = MakeVirtualPathAppAbsolute(Combine(AppDomainAppVirtualPath, url));
            }
            var basePath = ResolveServerUrl("~/", false);
            return basePath + url;
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
        [Obsolete(@"Use MakeUrlAbsolute(string, string) instead. Example: This method will be removed in version 5.")]
        public string ResolveServerUrl(string serverUrl, bool forceHttps)
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
            var httpContext = mvcContextFactory.CreateHttpContext();

            if (httpContext.Request.Headers["Host"] != null)
            {
                string scheme = httpContext.Request.Headers["HTTP_X_FORWARDED_PROTO"]
                    ?? httpContext.Request.Url.Scheme;
                originalUri = new Uri(scheme + Uri.SchemeDelimiter + httpContext.Request.Headers["Host"]);
            }
            else
            {
                originalUri = httpContext.Request.Url;
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
        [Obsolete(@"Use MakeUrlAbsolute(string) instead. Example: This method will be removed in version 5.")]
        public string ResolveServerUrl(string serverUrl)
        {
            return ResolveServerUrl(serverUrl, false);
        }
    }
}
