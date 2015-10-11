using System;
using System.IO;
using System.Xml.Linq;

namespace MvcSiteMapProvider.Xml
{
    /// <summary>
    /// Provides an XDocument instance based on an XML file source.
    /// </summary>
    public class FileXmlSource
        : IXmlSource
    {
        /// <summary>
        /// Creates a new instance of FileXmlSource.
        /// </summary>
        /// <param name="fileName">The absolute path to the Xml file.</param>
        public FileXmlSource(
            string fileName
            )
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            this.fileName = fileName;
        }

        protected readonly string fileName;

        #region IXmlSource Members

        public XDocument GetXml()
        {
            XDocument result = null;
            if (File.Exists(this.fileName))
            {
                result = XDocument.Load(this.fileName);
            }
            else
            {
                throw new FileNotFoundException(string.Format(Resources.Messages.XmlFileNotFound, this.fileName), this.fileName);
            }
            return result;
        }

        #endregion
    }
}
