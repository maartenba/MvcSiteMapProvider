using System;
using System.Collections;

namespace MvcSiteMapProvider.Web.Compilation
{
    /// <summary>
    /// Contract for wrapper for <see cref="T:System.Web.Compilation.BuildManager"/> class so it can be used as a test double.
    /// 
    /// Use http://encuestatron.googlecode.com/svn/trunk/src/MVC/test/SystemWebMvcTest/Mvc/Test/MockBuildManager.cs
    /// for making a mock.
    /// </summary>
    public interface IBuildManager
    {
        ICollection GetReferencedAssemblies();
    }
}
