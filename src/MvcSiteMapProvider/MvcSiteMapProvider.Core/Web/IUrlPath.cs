using System;

namespace MvcSiteMapProvider.Core.Web
{
    public interface IUrlPath
    {
        string AppDomainAppVirtualPath { get; }
        string Combine(string basepath, string relative);
        bool IsAbsolutePhysicalPath(string path);
        bool IsAppRelativePath(string path);
        bool IsRooted(string basepath);
        string MakeVirtualPathAppAbsolute(string virtualPath);
        string MakeVirtualPathAppAbsolute(string virtualPath, string applicationPath);
        string UrlEncode(string url);
    }
}
