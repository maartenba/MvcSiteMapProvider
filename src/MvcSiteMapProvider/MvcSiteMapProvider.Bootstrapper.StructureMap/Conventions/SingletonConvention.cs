﻿using System;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace MvcSiteMapProvider.Bootstrapper.StructureMap.Conventions
{
    public class SingletonConvention
        : IRegistrationConvention
    {
        #region IRegistrationConvention Members

        public void Process(Type type, Registry registry)
        {
            registry.For(type).Singleton();
        }

        #endregion
    }
}