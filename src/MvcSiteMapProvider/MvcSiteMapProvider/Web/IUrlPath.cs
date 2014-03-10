using System;
using System.Web;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Contract for a class that contains logic for dealing with URLs.
    /// </summary>
    public interface IUrlPath
    {
        string AppDomainAppVirtualPath { get; }

        /// <summary>
        /// Combines multiple strings into a URL, fixing any problems with forward 
        /// and backslashes.
        /// </summary>
        /// <param name="uriParts">An array of strings to combine.</param>
        /// <returns>The combined URL.</returns>
        /// <remarks>Source: http://stackoverflow.com/questions/372865/path-combine-for-urls/6704287#6704287 </remarks>
        string CombineUrl(params string[] uriParts);

        string Combine(string basepath, string relative);
        bool IsAbsolutePhysicalPath(string path);
        bool IsAppRelativePath(string path);
        bool IsRooted(string basepath);
        string MakeVirtualPathAppAbsolute(string virtualPath);
        string MakeVirtualPathAppAbsolute(string virtualPath, string applicationPath);
        string UrlEncode(string url);
        string UrlDecode(string url);

        /// <summary>
        /// Determines if the URL is an absolute or relative URL.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <returns><b>true</b> if the URL is absolute; otherwise <b>false</b>.</returns>
        bool IsAbsoluteUrl(string url);

        /// <summary>
        /// Determines if a URL is not part of the current application or web site.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <param name="httpContext">The HTTP context representing the request.</param>
        /// <returns><b>true</b> if the URL is not part of the virtual application or is on a different host name; otherwise <b>false</b>.</returns>
        bool IsExternalUrl(string url, HttpContextBase httpContext);

        /// <summary>
        /// Determines if the host name matches the public facing host name.
        /// </summary>
        /// <param name="hostName">The host name.</param>
        /// <param name="httpContext">The HTTP context representing the request.</param>
        /// <returns><b>true</b> if the host name matches that of the public URL; otherwise <b>false</b>.</returns>
        bool IsPublicHostName(string hostName, HttpContextBase httpContext);

        /// <summary>
        /// Resolves the URL to an absolute URL using the HTTP protocol.
        /// Absolute URLs will be passed through unchanged.
        /// </summary>
        /// <param name="url">
        /// Any Url including those starting with "/", "~", or protocol.
        /// </param>
        /// <returns>The absolute URL.</returns>
        string MakeUrlAbsolute(string url);

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
        string MakeUrlAbsolute(string baseUrl, string url);

        /// <summary>
        /// Resolves a URL that starts with a "~" into a URL that starts with the virtual
        /// application path. For example ~/MySite/ will resolve to /VirtualApplication/MySite/.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <returns>The resolved URL.</returns>
        string ResolveVirtualApplicationToRootRelativeUrl(string url);

        /// <summary>
        /// Resolves a URL, similar to how it would on Control.ResolveUrl() in ASP.NET.
        /// If the URL begins with a "/", it will be resolved to the web root. If the 
        /// URL begins with a "~", it will be resolved to the virtual application root.
        /// Absolute URLs will be passed through unchanged.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <returns>The resolved URL.</returns>
        string ResolveUrl(string url);

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
        string ResolveUrl(string url, string protocol);

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
        string ResolveUrl(string url, string protocol, string hostName);

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
        /// <param name="httpContext">The HTTP context representing the context of the request.</param>
        /// <returns>The resolved URL.</returns>
        string ResolveUrl(string url, string protocol, string hostName, HttpContextBase httpContext);

                /// <summary>
        /// Resolves a URL, similar to how it would on Control.ResolveUrl() in ASP.NET.
        /// If the URL begins with a "/", it will be resolved to the web root. If the 
        /// URL begins with a "~", it will be resolved to the virtual application root.
        /// Absolute URLs will be passed through unchanged.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <returns>The resolved URL.</returns>
        string ResolveContentUrl(string url);

        /// <summary>
        /// Resolves a URL, similar to how it would on Control.ResolveUrl() in ASP.NET.
        /// If the URL begins with a "/", it will be resolved to the web root. If the 
        /// URL begins with a "~", it will be resolved to the virtual application root.
        /// Absolute URLs will be passed through unchanged. Uses the protocol of the request.
        /// </summary>
        /// <param name="url">Any Url including those starting with "/", "~", or protocol.</param>
        /// <param name="protocol">The protocol such as http, https, or ftp. Defaults to protocol of the
        /// request to prevent errors when resolving content under https.</param>
        /// <returns>The resolved URL.</returns>
        string ResolveContentUrl(string url, string protocol);

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
        string ResolveContentUrl(string url, string protocol, string hostName);

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
        string ResolveContentUrl(string url, string protocol, string hostName, HttpContextBase httpContext);

        /// <summary>
        /// Gets the public facing URL for the given incoming HTTP request.
        /// </summary>
        /// <param name="httpContext">The HTTP context representing the context of the request.</param>
        /// <returns>The URI that the outside world used to create this request.</returns>
        /// <remarks>Source: http://stackoverflow.com/questions/7795910/how-do-i-get-url-action-to-use-the-right-port-number#11888846 </remarks>
        Uri GetPublicFacingUrl(HttpContextBase httpContext);

        [Obsolete(@"Use MakeUrlAbsolute(string) instead. Example: This method will be removed in version 5.")]
        string MakeRelativeUrlAbsolute(string url);

        [Obsolete(@"Use MakeUrlAbsolute(string, string) instead. Example: This method will be removed in version 5.")]
        string ResolveServerUrl(string serverUrl, bool forceHttps);

        [Obsolete(@"Use MakeUrlAbsolute(string) instead. Example: This method will be removed in version 5.")]
        string ResolveServerUrl(string serverUrl);
    }
}
