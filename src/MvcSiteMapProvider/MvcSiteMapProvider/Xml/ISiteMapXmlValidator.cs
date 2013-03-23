using System;

namespace MvcSiteMapProvider.Xml
{
    /// <summary>
    /// Contract to provide an instance that can validate an XML file against an embedded XSD schema.
    /// </summary>
    public interface ISiteMapXmlValidator
    {
        void ValidateXml(string xmlPath);
    }
}
