using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider.Web.Script.Serialization
{
    /// <summary>
    /// Specialized class to deserialize JSON into a <see cref="T:System.Collections.Generic.IDictionary{string, object}"/>. The 
    /// value is request cached so if the string has been encountered before in the current request it will not be deserialized again.
    /// </summary>
    public class JsonToDictionaryDeserializer : MvcSiteMapProvider.Web.Script.Serialization.IJsonToDictionaryDeserializer
    {
        public JsonToDictionaryDeserializer(
            IJavaScriptSerializer javaScriptSerializer,
            IMvcContextFactory mvcContextFactory
            )
        {
            if (javaScriptSerializer == null)
                throw new ArgumentNullException("javaScriptSerializer");
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");

            this.javaScriptSerializer = javaScriptSerializer;
            this.requestCache = mvcContextFactory.GetRequestCache();
        }

        protected readonly IJavaScriptSerializer javaScriptSerializer;
        protected readonly IRequestCache requestCache;

        public virtual IDictionary<string, object> Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return new Dictionary<string, object>();
            }
            
            var key = "__JsonToDictionaryDeserializer_" + json;
            var result = this.requestCache.GetValue<IDictionary<string, object>>(key);
            if (result == null)
            {
                result = this.DeserializeJson(json);
                this.requestCache.SetValue<IDictionary<string, object>>(key, result);
            }

            return result;
        }

        protected virtual IDictionary<string, object> DeserializeJson(string json)
        {
            try
            {
                return this.javaScriptSerializer.Deserialize<Dictionary<string, object>>(json);
            }
            catch (Exception ex)
            {
                throw new MvcSiteMapException(string.Format(Resources.Messages.JsonToDictionaryDeserializerJsonInvalid, json, ex.Message), ex);
            }
        }
    }
}
