using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Class that can be used to explicitly specify binding configuration by passing 
    /// <see cref="T:MvcSiteMapProvider.Web.IBinding"/> instances into the constructor.
    /// </summary>
    public class CustomBindingProvider
        : IBindingProvider
    {
        public CustomBindingProvider(
            IEnumerable<IBinding> bindings
            )
        {
            if (bindings == null)
                throw new ArgumentNullException("bindings");
            this.bindings = bindings;
        }

        protected readonly IEnumerable<IBinding> bindings;

        #region IBindingProvider Members

        public IEnumerable<IBinding> GetBindings()
        {
            return this.bindings;
        }

        #endregion
    }
}
