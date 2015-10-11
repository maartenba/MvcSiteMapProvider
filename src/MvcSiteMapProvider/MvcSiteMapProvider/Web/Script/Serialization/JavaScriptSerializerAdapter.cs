using System.Web.Script.Serialization;

namespace MvcSiteMapProvider.Web.Script.Serialization
{
    /// <summary>
    /// Adapter for the <see cref="T:System.Web.Script.Serialization.JavaScriptSerializer"/> class so a test double can be passed between methods.
    /// </summary>
    public class JavaScriptSerializerAdapter
        : JavaScriptSerializer, IJavaScriptSerializer
    {
    }
}
