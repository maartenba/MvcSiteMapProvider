#if !NET35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// A wrapper class to create an IList of <see cref="System.Runtime.Caching.ChangeMonitor"/> without creating
    /// a dependency on the System.Runtime.Caching library.
    /// </summary>
    public class RuntimeCompositeCacheDependency
        : ICacheDependency
    {
        public RuntimeCompositeCacheDependency(
            params ICacheDependency[] cacheDependencies
            )
        {
            if (cacheDependencies == null)
                throw new ArgumentNullException("cacheDependencies");
            this.cacheDependencies = cacheDependencies;
        }

        protected readonly ICacheDependency[] cacheDependencies;

        #region ICacheDependency Members

        public object Dependency
        {
            get
            {
                if (this.cacheDependencies.Count() > 0)
                {
                    var list = new List<ChangeMonitor>();
                    foreach (var item in this.cacheDependencies)
                    {
                        var changeMonitorList = (IList<ChangeMonitor>)item.Dependency;
                        if (changeMonitorList != null)
                        {
                            foreach (var changeMonitor in changeMonitorList)
                            {
                                list.Add(changeMonitor);
                            }
                        }
                    }
                    return list;
                }
                return null;
            }
        }

        #endregion
    }
}
#endif