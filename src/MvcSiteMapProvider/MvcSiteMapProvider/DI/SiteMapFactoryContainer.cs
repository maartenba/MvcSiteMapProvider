using System;
using System.Web.Mvc;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Collections;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Matching;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.Mvc.Filters;
using MvcSiteMapProvider.Web.Compilation;

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
            this.urlKeyFactory = new UrlKeyFactory(this.urlPath);
        }

        private readonly ConfigurationSettings settings;
        private readonly IMvcContextFactory mvcContextFactory;
        private readonly IRequestCache requestCache;
        private readonly IUrlPath urlPath;
        private readonly IUrlKeyFactory urlKeyFactory;

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
                    new ControllerDescriptorFactory(),
                    new ControllerBuilderAdapter(ControllerBuilder.Current),
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
                new SiteMapNodeCollectionFactory(),
                this.urlKeyFactory
                );
        }

        private IControllerTypeResolverFactory ResolveControllerTypeResolverFactory()
        {
            return new ControllerTypeResolverFactory(
                settings.ControllerTypeResolverAreaNamespacesToIgnore,
                new ControllerBuilderAdapter(ControllerBuilder.Current),
                new BuildManagerAdapter()
                );
        }
    }
}
