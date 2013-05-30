namespace VideoSitemap
{
    using System.Xml.Linq;

    public interface ISiteMapResultExtension
    {
        XNamespace NameSpace { get; }
        string ElementName { get; }
        void AddNodeContentToElement(IExtensibleSiteMapNode extensibleNode, XElement element);
    }
}