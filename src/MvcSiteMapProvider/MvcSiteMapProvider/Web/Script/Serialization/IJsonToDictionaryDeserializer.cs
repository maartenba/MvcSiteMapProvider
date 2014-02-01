using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Web.Script.Serialization
{
    /// <summary>
    /// Contract for specialized JSON deserializer.
    /// </summary>
    public interface IJsonToDictionaryDeserializer
    {
        IDictionary<string, object> Deserialize(string json);
    }
}
