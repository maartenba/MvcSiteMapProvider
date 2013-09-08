using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Xml;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for abstract factory to assist with the creation of XmlSiteMapBuilder for DI containers 
    /// that don't support injection of a partial list of constructor parameters.
    /// </summary>
    public interface IXmlSiteMapBuilderFactory
    {
        ISiteMapBuilder Create(IXmlSource xmlSource);
    }
}
