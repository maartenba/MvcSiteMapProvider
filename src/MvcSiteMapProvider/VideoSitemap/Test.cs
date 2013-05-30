namespace VideoSitemap
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Xml.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MvcSiteMapProvider;
    using MvcSiteMapProvider.Globalization;
    using MvcSiteMapProvider.Loader;
    using MvcSiteMapProvider.Web;
    using MvcSiteMapProvider.Web.Mvc;
    using NSubstitute;
    using SiteMapNode;

    [TestClass]
    public class Test
    {
        [TestMethod]
        public void foo()
        {
            // A
            var siteMapCacheKeys = new List<string>();
            const string baseUrl = "http://foo.bar";
            string siteMapUrlTemplate = string.Empty;
            var siteMapLoader = Substitute.For<ISiteMapLoader>();

            var httpContext = Substitute.For<HttpContextBase>();
            var routeData = new RouteData();
            var controller = Substitute.For<ControllerBase>();

            var context = new ControllerContext(httpContext, routeData, controller);

            ISiteMapNode rootNode = GetTestNodeTree();

            // A
            var result = new TestVideoSitemapResult(0, rootNode, siteMapCacheKeys, baseUrl, siteMapUrlTemplate, siteMapLoader);
            result.ExecuteResult(context);

            var resultString = result.XDocument.ToString();
            Assert.IsFalse(string.IsNullOrWhiteSpace(resultString));
            Assert.AreEqual("blarg", resultString);
        }

        private ISiteMapNode GetTestNodeTree()
        {
            var root = GetEmptyNode("root");
            root.Title = "test title";
            root.Description = "Well, the way they make shows is, they make one show. That show's called a pilot. Then they show that show to the people who make shows, and on the strength of that one show they decide if they're going to make more shows. Some pilots get picked and become television programs. Some don't, become nothing. She starred in one of the ones that became nothing.";
            root.Url = "zomg.htm";

            var info = new VideoNodeInformation
            {
                ThumbnailUrl = "zomg.jpg",
                ContentUrl = "zomg.flv"
            };
            root.VideoNodeInformation.Add(info);

            var info2 = new VideoNodeInformation
            {
                ThumbnailUrl = "zomg2.jpg",
                ContentUrl = "zomg2.flv"
            };
            root.VideoNodeInformation.Add(info2);

            return root;
        }

        private VideoSiteMapNode GetEmptyNode(string key)
        {
            var siteMap = Substitute.For<ISiteMap>();
            var pluginProvider = Substitute.For<ISiteMapNodePluginProvider>();
            var mvcContextFactory = Substitute.For<IMvcContextFactory>();
            var siteMapNodeChildStateFactory = Substitute.For<ISiteMapNodeChildStateFactory>();
            var localizationService = Substitute.For<ILocalizationService>();
            var node = new VideoSiteMapNode(siteMap, key, false, pluginProvider, mvcContextFactory, siteMapNodeChildStateFactory, localizationService, new UrlPath(mvcContextFactory));
            return node;
        }
    }

    internal class TestVideoSitemapResult : VideoXmlSitemapResult
    {
        public TestVideoSitemapResult(int page, ISiteMapNode rootNode, IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate, ISiteMapLoader siteMapLoader)
            : base(page, rootNode, siteMapCacheKeys, baseUrl, siteMapUrlTemplate, siteMapLoader)
        {
        }

        public XDocument XDocument { get; private set; }

        protected override void WriteXmlSitemapDocument(ControllerContext context, XDocument xmlSiteMap)
        {
            XDocument = xmlSiteMap;
        }
    }
}
