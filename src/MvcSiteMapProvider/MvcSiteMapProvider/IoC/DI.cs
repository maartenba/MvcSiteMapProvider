// -----------------------------------------------------------------------
// <copyright file="IoC.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Provides static access to a dependency injection container.
    /// </summary>
    public class DI
    {
        public static IResolver Container { get; set; }
    }
}
