using System;
using System.Xml;
using System.Xml.Schema;

namespace MvcSiteMapProvider.Xml
{
    /// <summary>
    /// Validates an XML file against an XSD schema. Throws an exception if it fails.
    /// </summary>
    public class SiteMapXmlValidator 
        : ISiteMapXmlValidator
    {
        public void ValidateXml(string xmlPath)
        {
            const string resourceNamespace = "MvcSiteMapProvider.Xml";
            const string resourceFileName = "MvcSiteMapSchema.xsd";

            var xsdPath = resourceNamespace + "." + resourceFileName;
            var xsdStream = this.GetType().Assembly.GetManifestResourceStream(xsdPath);
            using (XmlReader xsd = XmlReader.Create(xsdStream))
            {
                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add(null, xsd);

                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                xmlReaderSettings.ValidationType = ValidationType.Schema;
                xmlReaderSettings.Schemas.Add(schema);
                //xmlReaderSettings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

                using (XmlReader xmlReader = XmlReader.Create(xmlPath, xmlReaderSettings))
                {
                    try
                    {
                        while (xmlReader.Read()) ;
                    }
                    catch (Exception ex)
                    {
                        throw new MvcSiteMapException(string.Format(Resources.Messages.XmlValidationFailed, xmlPath), ex);
                    }
                }
            }
        }
    }
}
