using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Used to chain several <see cref="T:MvcSiteMapProvider.Web.IBindingProvider"/> instances in succession. 
    /// The providers will be processed in the same order as they are specified in the constructor.
    /// </summary>
    public class CompositeBindingProvider
        : IBindingProvider
    {
        public CompositeBindingProvider(params IBindingProvider[] bindingProviders)
        {
            if (bindingProviders == null)
                throw new ArgumentNullException("bindingProviders");
            this.bindingProviders = bindingProviders;
        }
        protected readonly IEnumerable<IBindingProvider> bindingProviders;

        #region IBindingProvider Members

        public IEnumerable<IBinding> GetBindings()
        {
            var result = new List<IBinding>();
            foreach (var provider in this.bindingProviders)
            {
                result.AddRange(provider.GetBindings());
            }
            return result;
        }

        #endregion
    }
}