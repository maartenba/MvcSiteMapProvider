using System;
using System.Web.Mvc;
using System.Collections.Generic;
using MvcSiteMapProvider.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor.Configuration;
using Castle.Windsor;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;

namespace DI.Windsor.Installers
{
    public class MvcInstaller
        : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Fix for controllers - need to ensure they are transient or http scoped or
            // there will be problems when using dependency injection.
            container.Register(Classes.FromThisAssembly()
                .BasedOn<IController>()
                .LifestyleTransient());

            container.Register(Classes.FromAssembly(typeof(XmlSiteMapController).Assembly)
                .BasedOn<IController>()
                .LifestyleTransient());
        }

        #endregion
    }
}
