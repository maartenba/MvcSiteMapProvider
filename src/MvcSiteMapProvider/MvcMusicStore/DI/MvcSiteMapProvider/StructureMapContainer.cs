//using System;
//using System.Collections.Generic;
//using System.Linq;
//using StructureMap;

//namespace MvcMusicStore.DI.MvcSiteMapProvider
//{
//    public class StructureMapContainer
//        : IDependencyInjectionContainer
//    {
//        private readonly IContainer container;

//        public StructureMapContainer(IContainer container)
//        {
//            if (container == null)
//                throw new ArgumentNullException("container");
//            this.container = container;
//        }

//        #region IDependencyInjectionContainer Members

//        public object GetInstance(Type type)
//        {
//            if (type == null)
//            {
//                return null;
//            }

//            try
//            {
//                return type.IsAbstract || type.IsInterface
//                       ? this.container.TryGetInstance(type)
//                       : this.container.GetInstance(type);
//            }
//            catch (StructureMapException ex)
//            {
//                string message = ex.Message + "\n" + container.WhatDoIHave();
//                throw new Exception(message);
//            }
//        }

//        public IEnumerable<object> GetAllInstances(Type type)
//        {
//            try
//            {
//                return this.container.GetAllInstances<object>()
//                    .Where(s => s.GetType() == type);
//            }
//            catch (StructureMapException ex)
//            {
//                string message = ex.Message + "\n" + container.WhatDoIHave();
//                throw new Exception(message);
//            }
//        }

//        #endregion
//    }
//}