namespace VideoSitemap
{
    using MvcSiteMapProvider.Web;
    using MvcSiteMapProvider.Web.Mvc;
    using NSubstitute;
    using Ninject.Modules;

    public class VideoSitemapNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IXmlSiteMapResultFactory>().To<VideoSitemapResultFactory>();
            Bind<IUrlPath>().To<UrlPath>();
            Bind<IMvcContextFactory>().ToMethod(ctx => Substitute.For<IMvcContextFactory>());
        }
    }
}
