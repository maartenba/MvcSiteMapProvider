using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using MvcSiteMapProvider.DI.Bootstrap;

namespace $rootnamespace$.DI.MvcSiteMapProvider
{
    public class AutofacContainer
        : IDependencyInjectionContainer
    {
        private readonly IContainer container;

        public AutofacContainer(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            this.container = container;
        }

        #region IDependencyInjectionContainer Members

        public object GetInstance(Type type)
        {
            if (type == null)
            {
                return null;
            }

            try
            {
                return this.container.Resolve(type);
            }
            catch (DependencyResolutionException ex)
            {
                string message = ex.Message;
                throw new Exception(message);
            }
        }

        public IEnumerable<object> GetAllInstances(Type type)
        {
            try
            {
                Type lookupType = typeof(IEnumerable<>).MakeGenericType(new Type[] { type });
                object obj = this.container.Resolve(lookupType);
                return (IEnumerable<object>)obj;
            }
            catch (DependencyResolutionException ex)
            {
                string message = ex.Message;
                throw new Exception(message);
            }
        }

        #endregion
    }
}