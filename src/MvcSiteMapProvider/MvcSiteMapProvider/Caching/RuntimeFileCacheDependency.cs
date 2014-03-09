#if !NET35
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// A wrapper class to create an IList of <see cref="System.Runtime.Caching.HostFileChangeMonitor"/> without creating
    /// a dependency on the System.Runtime.Caching library.
    /// </summary>
    public class RuntimeFileCacheDependency
        : ICacheDependency
    {
        public RuntimeFileCacheDependency(
            string fileName
            )
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            this.fileName = fileName;
        }

        protected readonly string fileName;

        #region ICacheDependency Members

        public virtual object Dependency
        {
            get 
            {
                var list = new List<ChangeMonitor>();
                list.Add(new HostFileChangeMonitor(new string[] { fileName }));
                return list; 
            }
        }

        #endregion
    }
}
#endif