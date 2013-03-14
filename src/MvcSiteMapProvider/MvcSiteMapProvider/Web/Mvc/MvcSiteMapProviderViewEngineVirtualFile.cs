using System;
using System.Web.Hosting;
using System.IO;
using System.Reflection;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// MvcSiteMapProviderViewEngineVirtualFile class.
    /// </summary>
    internal class MvcSiteMapProviderViewEngineVirtualFile 
        : VirtualFile
    {
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        protected string FilePath { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcSiteMapProviderViewEngineVirtualFile"/> class.
        /// </summary>
        /// <param name="filePath">The filePath.</param>
        public MvcSiteMapProviderViewEngineVirtualFile(string filePath)
            : base(filePath)
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>A read-only stream to the virtual file.</returns>
        public override Stream Open()
        {
            return ReadResource(FilePath);
        }

        /// <summary>
        /// Reads the resource.
        /// </summary>
        /// <param name="embeddedFileName">Name of the embedded file.</param>
        /// <returns></returns>
        private static Stream ReadResource(string embeddedFileName)
        {
            string resourceFileName = Path.GetFileName(embeddedFileName);
            Assembly assembly = typeof(MvcSiteMapProviderViewEngineVirtualFile).Assembly;
            return assembly.GetManifestResourceStream("MvcSiteMapProvider.Web.Html.DisplayTemplates." + resourceFileName);
        }
    }
}
