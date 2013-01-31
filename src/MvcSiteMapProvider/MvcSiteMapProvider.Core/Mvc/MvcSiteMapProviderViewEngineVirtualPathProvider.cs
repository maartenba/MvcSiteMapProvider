using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.IO;
using System.Web;
using System.Reflection;

namespace MvcSiteMapProvider.Core.Mvc
{
    /// <summary>
    /// MvcSiteMapProviderViewEnginePathProvider class
    /// </summary>
    internal class MvcSiteMapProviderViewEngineVirtualPathProvider
        : VirtualPathProvider
    {
        /// <summary>
        /// Check if a path exists in the virtual file system.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>true if the path exists in the virtual file system; otherwise, false.</returns>
        private bool PathExists(string path)
        {
            if (path.Contains("__MVCSITEMAPPROVIDER") && !path.Contains(".."))
            {
                string resourceFileName = Path.GetFileName(path);
                Assembly assembly = typeof(MvcSiteMapProviderViewEngineVirtualFile).Assembly;
                return assembly.GetManifestResourceStream("MvcSiteMapProvider.Web.Html.DisplayTemplates." + resourceFileName) != null;
            }
            return false;
        }

        /// <summary>
        /// Gets a value that indicates whether a file exists in the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        /// true if the file exists in the virtual file system; otherwise, false.
        /// </returns>
        public override bool FileExists(string virtualPath)
        {
            if (PathExists(virtualPath))
            {
                return true;
            }
            return base.FileExists(virtualPath);
        }

        /// <summary>
        /// Gets a virtual file from the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        /// A descendent of the <see cref="T:System.Web.Hosting.VirtualFile"/> class that represents a file in the virtual file system.
        /// </returns>
        public override VirtualFile GetFile(string virtualPath)
        {
            if (PathExists(virtualPath))
            {
                return new MvcSiteMapProviderViewEngineVirtualFile(virtualPath);
            }
            return base.GetFile(virtualPath);
        }

        /// <summary>
        /// Creates a cache dependency based on the specified virtual paths.
        /// </summary>
        /// <param name="virtualPath">The path to the primary virtual resource.</param>
        /// <param name="virtualPathDependencies">An array of paths to other resources required by the primary virtual resource.</param>
        /// <param name="utcStart">The UTC time at which the virtual resources were read.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Caching.CacheDependency"/> object for the specified virtual resources.
        /// </returns>
        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return null;
        }
    }
}
