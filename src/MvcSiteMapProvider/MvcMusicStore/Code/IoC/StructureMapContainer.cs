using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;

namespace MvcMusicStore.Code.IoC
{
    public class StructureMapContainer 
        : IDependencyInjectionContainer
    {
        private readonly StructureMap.IContainer container;

        public StructureMapContainer(StructureMap.IContainer container)
        {
            this.container = container;
        }

        #region IDependencyInjectionContainer Members

        public object Resolve(Type type)
        {
            try
            {
                return container.GetInstance(type);
            }
            catch (StructureMapException ex)
            {
                string message = ex.Message + "\n" + container.WhatDoIHave();
                throw new Exception(message);
            }
        }

        public T Resolve<T>()
        {
            try
            {
                return container.GetInstance<T>();
            }
            catch (StructureMapException ex)
            {
                string message = ex.Message + "\n" + container.WhatDoIHave();
                throw new Exception(message);
            }
        }

        #endregion
    }
}