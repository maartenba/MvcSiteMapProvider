namespace VideoSitemap
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MvcSiteMapProvider.Web.Mvc;
    using Ninject;

    [TestClass]
    public class Test
    {
        [TestMethod]
        public void foo()
        {
            var kernel = SetUpNinjectKernel();
            var ctrlr = kernel.Get<XmlSiteMapController>();
            var result = ctrlr.Index(0);

            Assert.IsNotNull(result);
        }

        private IKernel SetUpNinjectKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(typeof(VideoSitemapNinjectModule).Assembly);
            return kernel;
        }
    }
}
