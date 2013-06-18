using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace DI.Autofac
{
    public class AutofacDependencyInjectionContainer
        : IDependencyInjectionContainer
    {
        public AutofacDependencyInjectionContainer(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            this.container = container;
        }
        private readonly IContainer container;

        public object GetInstance(Type type)
        {
            if (type == null)
            {
                return null;
            }

            return this.container.Resolve(type);
        }

        public object TryGetInstance(Type type)
        {
            if (type == null)
            {
                return null;
            }

            return this.container.ResolveOptional(type);
        }

        public IEnumerable<object> GetAllInstances(Type type)
        {
            Type lookupType = typeof(IEnumerable<>).MakeGenericType(new Type[] { type });
            object obj = this.container.Resolve(lookupType);
            return (IEnumerable<object>)obj;
        }

        public void Release(object instance)
        {
            // Do nothing
        }

    }
}