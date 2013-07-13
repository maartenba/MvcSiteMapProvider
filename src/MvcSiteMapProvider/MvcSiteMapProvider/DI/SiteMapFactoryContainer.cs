using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.Mvc.Filters;
using MvcSiteMapProvider.Web.Compilation;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Collections;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider.DI
{
    /// <summary>
    /// A specialized dependency injection container for resolving a <see cref="T:MvcSiteMapProvider.SiteMapFactory"/> instance.
    /// </summary>
    public class SiteMapFactoryContainer
    {
        public SiteMapFactoryContainer(
            ConfigurationSettings settings,
            IMvcContextFactory mvcContextFactory,
            IUrlPath urlPath)
        {
            this.settings = settings;
            this.mvcContextFactory = mvcContextFactory;
            this.requestCache = this.mvcContextFactory.GetRequestCache();
            this.urlPath = urlPath;
        }

        private readonly ConfigurationSettings settings;
        private readonly IMvcContextFactory mvcContextFactory;
        private readonly IRequestCache requestCache;
        private readonly IUrlPath urlPath;

        public ISiteMapFactory ResolveSiteMapFactory()
        {
            return new SiteMapFactory(
                this.ResolveSiteMapPluginProviderFactory(),
                new MvcResolverFactory(),
                this.mvcContextFactory,
                this.ResolveSiteMapChildStateFactory(),
                this.urlPath,
                this.ResolveControllerTypeResolverFactory(),
                new ActionMethodParameterResolverFactory(new ControllerDescriptorFactory())
                );
        }

        private ISiteMapPluginProviderFactory ResolveSiteMapPluginProviderFactory()
        {
            return new SiteMapPluginProviderFactory(
                this.ResolveAclModule()
                );
        }

        private IAclModule ResolveAclModule()
        {
            return new CompositeAclModule(
                new AuthorizeAttributeAclModule(
                    this.mvcContextFactory,
                    new ObjectCopier(),
                    new ControllerDescriptorFactory(),
                    new ControllerBuilderAdaptor(ControllerBuilder.Current),
                    new AuthorizeAttributeBuilder(), 
                    new GlobalFilterProvider()
),
                new XmlRolesAclModule(
                    this.mvcContextFactory
                    )
                );
        }

        private ISiteMapChildStateFactory ResolveSiteMapChildStateFactory()
        {
            return new SiteMapChildStateFactory(
                new GenericDictionaryFactory(),
                new SiteMapNodeCollectionFactory()
                );
        }

        private IControllerTypeResolverFactory ResolveControllerTypeResolverFactory()
        {
            return new ControllerTypeResolverFactory(
                settings.ControllerTypeResolverAreaNamespacesToIgnore,
                new ControllerBuilderAdaptor(ControllerBuilder.Current),
                new BuildManagerAdaptor()
                );
        }
    }
}
