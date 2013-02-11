using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcSiteMapProvider.Core.RequestCache
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RequestCache 
        : IRequestCache
    {
        public RequestCache(
            HttpContext context
            )
        {
            if (context == null)
                throw new ArgumentNullException("context");

            this.context = context;
        }

        private readonly HttpContext context;

        public virtual T GetValue<T>(string key)
        {
            return (T)this.context.Items[key];
        }

        public virtual void SetValue<T>(string key, T value)
        {
            this.context.Items[key] = value;
        }
    }
}
