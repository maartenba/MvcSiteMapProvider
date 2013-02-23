using System;
using System.Collections;
using System.Web.Compilation;

namespace MvcSiteMapProvider.Web.Compilation
{
    /// <summary>
    /// Adaptor class so test doubles can be injected for <see cref="T:System.Web.Compilation.BuildManager"/>.
    /// </summary>
    public class BuildManagerAdaptor
        : IBuildManager
    {

        #region IBuildManager Members

        public ICollection GetReferencedAssemblies()
        {
            return BuildManager.GetReferencedAssemblies();
        }

        #endregion
    }
}
