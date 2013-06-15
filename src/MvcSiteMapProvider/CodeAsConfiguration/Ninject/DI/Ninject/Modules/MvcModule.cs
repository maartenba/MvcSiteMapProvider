using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web.Mvc;
using Ninject;
using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace DI.Ninject.Modules
{
    public class MvcModule
        : NinjectModule
    {
        public override void Load()
        {
            var currentAssembly = typeof(MvcModule).Assembly;

            // Fix for controllers - need to ensure they are transient or http scoped or
            // there will be problems when using dependency injection.
            //http://code-inside.de/blog/2011/01/18/fix-a-single-instance-of-controller-foocontroller-cannot-be-used-to-handle-multiple-requests-mvc3/
            this.Kernel.Bind(scanner => scanner.From(new Assembly[] { currentAssembly, typeof(SiteMaps).Assembly })
                   .Select(IsControllerType)
                   .BindDefaultInterface()
                   .Configure(binding => binding.InTransientScope())
                   );
        }

        private bool IsControllerType(Type type)
        {
            return type.IsClass && type.GetInterfaces().Any(intface => intface.GetType().Equals(typeof(IController)));
        }
    }
}
