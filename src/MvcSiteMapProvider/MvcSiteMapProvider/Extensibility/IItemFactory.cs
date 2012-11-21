using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Extensibility
{
    /// <summary>
    /// Factory for creating VisibilityProvider instances
    /// </summary>
    public interface IItemFactory
    {
        /// <summary>
        /// Used to instantiate objects
        /// </summary>
        /// <param name="type">The type to instantiate</param>
        /// <returns></returns>
        object CreateInstance(Type type);
    }
}
