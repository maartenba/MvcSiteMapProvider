using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;

namespace DI.StructureMap
{
    public class StructureMapDependencyInjectionContainer
        : IDependencyInjectionContainer
    {
        public StructureMapDependencyInjectionContainer(IContainer container)
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

            try
            {
                return type.IsAbstract || type.IsInterface
                       ? this.container.TryGetInstance(type)
                       : this.container.GetInstance(type);
            }
            catch (StructureMapException ex)
            {
                string message = ex.Message + "\n" + container.WhatDoIHave();
                throw new Exception(message);
            }
        }

        public object TryGetInstance(Type type)
        {
            if (type == null)
            {
                return null;
            }

            try
            {
                return type.IsAbstract || type.IsInterface
                       ? this.container.TryGetInstance(type)
                       : this.container.GetInstance(type);
            }
            catch (StructureMapException ex)
            {
                string message = ex.Message + "\n" + container.WhatDoIHave();
                throw new Exception(message);
            }
        }

        public IEnumerable<object> GetAllInstances(Type type)
        {
            try
            {
                return this.container.GetAllInstances<object>()
                    .Where(s => s.GetType() == type);
            }
            catch (StructureMapException ex)
            {
                string message = ex.Message + "\n" + container.WhatDoIHave();
                throw new Exception(message);
            }
        }

        public void Release(object instance)
        {
            // Do nothing
        }
    }
}