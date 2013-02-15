using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider.Core.Web;

namespace MvcSiteMapProvider.Core.Caching
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RequestCache 
        : IRequestCache
    {
        public RequestCache(
            IHttpContextFactory httpContextFactory
            )
        {
            if (httpContextFactory == null)
                throw new ArgumentNullException("httpContextFactory");

            this.httpContextFactory = httpContextFactory;
        }

        private readonly IHttpContextFactory httpContextFactory;

        protected HttpContextBase Context
        {
            get 
            { 
                return this.httpContextFactory.Create();
            }
        }

        public virtual T GetValue<T>(string key)
        {
            if (this.Context.Items.Contains(key))
            {
                return (T)this.Context.Items[key];
            }
            return default(T);
        }

        public virtual void SetValue<T>(string key, T value)
        {
            this.Context.Items[key] = value;
        }
    }
}
