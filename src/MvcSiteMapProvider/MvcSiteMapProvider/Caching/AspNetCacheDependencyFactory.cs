using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// A wrapper abstract factory to provide concrete instances of the <see cref="T:System.Web.Caching.CacheDependency"/> at runtime
    /// without creating a dependency on the System.Web library. If a <see cref="T:System.Web.Caching.SqlCacheDependeny"/> is required, simply
    /// inherit this class and override the CreateCacheDependency() method.
    /// </summary>
    public class AspNetCacheDependencyFactory
        : ICacheDependencyFactory
    {
        public AspNetCacheDependencyFactory(
            IEnumerable<string> dependencyFileNames
            )
        {
            if (dependencyFileNames == null)
                throw new ArgumentNullException("dependencyFileNames");

            this.dependencyFileNames = dependencyFileNames;
        }

        protected readonly IEnumerable<string> dependencyFileNames;

        #region ICacheDependencyFactory Members

        public virtual ICacheDependency Create()
        {
            return new AspNetCacheDependency(this.CreateCacheDependency());
        }

        #endregion

        protected virtual CacheDependency CreateCacheDependency()
        {
            CacheDependency result = null;
            var count = this.dependencyFileNames.Count();
            if (count > 0)
            {
                if (count == 1)
                {
                    result = new CacheDependency(this.dependencyFileNames.First());
                }
                else
                {
                    var list = new AggregateCacheDependency();
                    foreach (var fileName in this.dependencyFileNames)
                    {
                        list.Add(new CacheDependency(fileName));
                    }
                    result = list;
                }
            }
            return result;
        }
    }
}
