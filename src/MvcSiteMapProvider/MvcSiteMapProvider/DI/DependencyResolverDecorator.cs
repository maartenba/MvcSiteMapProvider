#if !MVC2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider.DI
{
    /// <summary>
    /// An implementation of <see cref="T:System.Web.Mvc.IDependencyResolver"/> that wraps another instance of 
    /// <see cref="T:System.Web.Mvc.IDependencyResolver"/> so they can be used in conjunction with each other.
    /// </summary>
    public class DependencyResolverDecorator
        : IDependencyResolver
    {
        public DependencyResolverDecorator(
            IDependencyResolver dependencyResolver,
            ConfigurationSettings settings
            )
        {
            if (dependencyResolver == null)
                throw new ArgumentNullException("dependencyResolver");
            if (settings == null)
                throw new ArgumentNullException("settings");
            this.innerDependencyResolver = dependencyResolver;
            this.settings = settings;
        }

        private readonly IDependencyResolver innerDependencyResolver;
        private readonly ConfigurationSettings settings;

        #region IDependencyResolver Members

        public object GetService(Type serviceType)
        {
            if (serviceType != null && serviceType.Equals(typeof(XmlSiteMapController)))
            {
                var xmlSiteMapResultFactoryContainer = new XmlSiteMapResultFactoryContainer(this.settings);
                return new XmlSiteMapController(xmlSiteMapResultFactoryContainer.ResolveXmlSiteMapResultFactory());
            }

            return innerDependencyResolver.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType != null && serviceType.Equals(typeof(XmlSiteMapController)))
            {
                var xmlSiteMapResultFactoryContainer = new XmlSiteMapResultFactoryContainer(this.settings);
                return new List<object>() { new XmlSiteMapController(xmlSiteMapResultFactoryContainer.ResolveXmlSiteMapResultFactory()) };
            }

            return innerDependencyResolver.GetServices(serviceType);
        }

        #endregion
    }
}
#endif