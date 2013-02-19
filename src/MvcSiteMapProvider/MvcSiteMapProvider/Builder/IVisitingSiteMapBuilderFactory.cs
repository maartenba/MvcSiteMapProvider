using System;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.Builder.ISiteMapBuilder"/> at runtime.
    /// </summary>
    public interface IVisitingSiteMapBuilderFactory
    {
        ISiteMapBuilder Create();
    }
}
