using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcSiteMapProvider.Extensibility;

namespace MvcSiteMapProvider
{
    class ItemFactory
    {
        public static object CreateInstance(Type type)
        {
#if !NET35
            return DependencyResolver.Current.GetService(type);
#else
            return  Activator.CreateInstance(type);
#endif
        }
    }
}
