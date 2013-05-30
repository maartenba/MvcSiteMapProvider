namespace ExtensibleSiteMap.Tests
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
    using Video;

    [TestClass]
    public class Test
    {
        [TestMethod]
        public void VideoExtension()
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

            var rootNode = GetTestNodeTree();

            AddVideoData(rootNode);

            // A
            var videoSitemapExtension = new VideoSiteMapExtension();
            var extensions = new List<ISiteMapResultExtension>
            {
                videoSitemapExtension
            };

            var result = new TestExtensibleXmlSiteMapResult(0, rootNode, siteMapCacheKeys, baseUrl, siteMapUrlTemplate, siteMapLoader, extensions);
            result.ExecuteResult(context);

            var resultString = result.XDocument.ToString();
            Assert.IsFalse(string.IsNullOrWhiteSpace(resultString));
            Assert.AreEqual("random string", resultString);
        }

        private void AddVideoData(ExtensibleSiteMapNode root)
        {
            var videoData = new VideoSiteMapExtension.VideoData();

            var info = new VideoNodeInformation
            {
                ThumbnailUrl = "zomg.jpg",
                ContentUrl = "zomg.flv"
            };
            videoData.VideoNodeInformation.Add(info);

            var info2 = new VideoNodeInformation
            {
                ThumbnailUrl = "zomg2.jpg",
                ContentUrl = "zomg2.flv"
            };
            videoData.VideoNodeInformation.Add(info2);

            root.DataByExtensionKey.Add(VideoSiteMapExtension.ExtensionDataKey, videoData);
        }

        private ExtensibleSiteMapNode GetTestNodeTree()
        {
            var siteMap = Substitute.For<ISiteMap>();
            var pluginProvider = Substitute.For<ISiteMapNodePluginProvider>();
            var mvcContextFactory = Substitute.For<IMvcContextFactory>();
            var siteMapNodeChildStateFactory = Substitute.For<ISiteMapNodeChildStateFactory>();
            var localizationService = Substitute.For<ILocalizationService>();
            
            var root = new ExtensibleSiteMapNode(siteMap, "asdf", false, pluginProvider, mvcContextFactory, siteMapNodeChildStateFactory, localizationService, new UrlPath(mvcContextFactory))
            {
                Title = "test title",
                Description = "Well, the way they make shows is, they make one show. That show's called a pilot. Then they show that show to the people who make shows, and on the strength of that one show they decide if they're going to make more shows. Some pilots get picked and become television programs. Some don't, become nothing. She starred in one of the ones that became nothing.",
                Url = "zomg.htm"
            };

            return root;
        }
    }

    internal class TestExtensibleXmlSiteMapResult : ExtensibleXmlSiteMapResult
    {
        public TestExtensibleXmlSiteMapResult(int page, ISiteMapNode rootNode, IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate, ISiteMapLoader siteMapLoader, IEnumerable<ISiteMapResultExtension> extensions)
            : base(page, rootNode, siteMapCacheKeys, baseUrl, siteMapUrlTemplate, siteMapLoader, extensions)
        {
        }

        public XDocument XDocument { get; private set; }

        protected override void WriteXmlSitemapDocument(ControllerContext context, XDocument xmlSiteMap)
        {
            XDocument = xmlSiteMap;
        }
    }
}