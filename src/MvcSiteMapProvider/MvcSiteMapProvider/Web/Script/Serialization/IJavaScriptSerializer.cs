using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;

namespace MvcSiteMapProvider.Web.Script.Serialization
{
    /// <summary>
    /// Contract for <see cref="T:System.Web.Script.Serialization.JavaScriptSerializer"/> wrapper class.
    /// </summary>
    public interface IJavaScriptSerializer
    {
#if !NET35
        object ConvertToType(object obj, Type targetType);
        object Deserialize(string input, Type targetType);
#endif
        T Deserialize<T>(string input);
        object DeserializeObject(string input);
        void RegisterConverters(IEnumerable<JavaScriptConverter> converters);
        string Serialize(object obj);
        void Serialize(object obj, StringBuilder output);
    }
}
