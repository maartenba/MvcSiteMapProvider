using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.Builder.ISiteMapBuilder"/> at runtime.
    /// </summary>
    public interface IXmlSiteMapBuilderFactory
    {
        ISiteMapBuilder Create(
            string xmlSiteMapFilePath,
            IEnumerable<string> attributesToIgnore);
    }
}
