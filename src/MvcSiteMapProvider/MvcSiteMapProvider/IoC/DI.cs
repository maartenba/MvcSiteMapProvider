using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.IoC
{
    /// <summary>
    /// Provides static access to a dependency injection container.
    /// </summary>
    public class DI
    {
        public static IResolver Container { get; set; }
    }
}
