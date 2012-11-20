using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Extensibility;

namespace MvcSiteMapProvider
{
    class DefaultItemFactory:IItemFactory
    {
        public object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
