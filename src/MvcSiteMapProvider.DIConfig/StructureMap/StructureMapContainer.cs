using System;
using StructureMap;

namespace DI.StructureMap
{
    internal class StructureMapContainer
        : IDependencyInjectionContainer
    {
        private readonly IContainer container;

        public StructureMapContainer(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
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

        #endregion
    }
}