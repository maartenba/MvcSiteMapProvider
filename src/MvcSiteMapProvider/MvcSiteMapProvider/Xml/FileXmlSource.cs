using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web.Hosting;
using System.IO;

namespace MvcSiteMapProvider.Xml
{
    /// <summary>
    /// Provides an XDocument instance based on an XML file source.
    /// </summary>
    public class FileXmlSource
        : IXmlSource
    {
        public FileXmlSource(
            string xmlFileName
            )
        {
            if (String.IsNullOrEmpty(xmlFileName))
                throw new ArgumentNullException("xmlFilePath");

            this.xmlFileName = xmlFileName;
        }

        protected readonly string xmlFileName;

        #region IXmlSource Members

        public XDocument GetXml()
        {
            XDocument result = null;
            var siteMapFileAbsolute = HostingEnvironment.MapPath(this.xmlFileName);
            if (File.Exists(siteMapFileAbsolute))
            {
                result = XDocument.Load(siteMapFileAbsolute);
            }
            else
            {
                throw new FileNotFoundException(string.Format(Resources.Messages.XmlFileNotFound, this.xmlFileName), siteMapFileAbsolute);
            }
            return result;
        }

        #endregion
    }
}
