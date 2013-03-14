using System;

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
