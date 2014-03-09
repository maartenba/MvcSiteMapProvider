using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Reflection;

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
            var xsdStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(xsdPath);
            XmlReader xsd = new XmlTextReader(xsdStream);
            try
            {
                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add(null, xsd);

                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                xmlReaderSettings.ValidationType = ValidationType.Schema;
                xmlReaderSettings.Schemas.Add(schema);
                //xmlReaderSettings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

                XmlReader xmlReader = XmlReader.Create(xmlPath, xmlReaderSettings);
                try
                {
                    while (xmlReader.Read());
                }
                catch (Exception ex)
                {
                    throw new MvcSiteMapException(string.Format(Resources.Messages.XmlValidationFailed, xmlPath), ex);
                }
                finally
                {
                    xmlReader.Close();
                }
            }
            finally
            {
                xsd.Close();
            }
        }
    }
}
