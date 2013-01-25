// -----------------------------------------------------------------------
// <copyright file="UrlPath.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UrlPath
    {
        public static string MakeVirtualPathAppAbsolute(string virtualPath)
        {
            //var path = HttpRuntime.AppDomainAppVirtualPath;
            //return MakeVirtualPathAppAbsolute(virtualPath, HttpRuntime.AppDomainAppVirtualPathString);
            return MakeVirtualPathAppAbsolute(virtualPath, HttpRuntime.AppDomainAppVirtualPath);
        }

        public static string MakeVirtualPathAppAbsolute(string virtualPath, string applicationPath)
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

        public static bool IsAppRelativePath(string path)
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

        public static bool IsRooted(string basepath)
        {
            if (!string.IsNullOrEmpty(basepath) && (basepath[0] != '/'))
            {
                return (basepath[0] == '\\');
            }
            return true;
        }

 


    }
}
