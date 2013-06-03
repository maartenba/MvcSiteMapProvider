namespace ExtensibleSiteMap.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MvcSiteMapProvider;
    using MvcSiteMapProvider.Globalization;
    using MvcSiteMapProvider.Loader;
    using MvcSiteMapProvider.Web;
    using MvcSiteMapProvider.Web.Mvc;
    using NSubstitute;
    using Video;

    [TestClass]
    public class When_creating_extensing_sitemap_with_video_data
    {
        private const string BaseUrl = "http://foo.bar";

        private const string VideoSchemaNamespace = "http://www.google.com/schemas/sitemap-video/1.1";
        private const string VideoSchemaUrl = "http://www.google.com/schemas/sitemap-video/1.1/sitemap-video.xsd";
        
        private const string SitemapSchemaNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private const string SitemapSchemaUrl = "http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd";

        [TestMethod]
        public void Basic_videos_get_written_correctly()
        {
            // A
            var siteMapCacheKeys = new List<string>();
            string siteMapUrlTemplate = string.Empty;
            var siteMapLoader = Substitute.For<ISiteMapLoader>();
            var context = FakeControllerContext();
            
            // A
            var rootNode = GetTestNodeTree();
            AddBasicVideoData(rootNode);

            var videoSitemapExtension = new VideoSiteMapExtension();
            var extensions = new List<ISiteMapResultExtension>
            {
                videoSitemapExtension
            };

            var result = new TestExtensibleXmlSiteMapResult(0, rootNode, siteMapCacheKeys, BaseUrl, siteMapUrlTemplate, siteMapLoader, extensions);
            result.ExecuteResult(context);
            var xDocument = result.XDocument;
            
            SchemaValidateResultDocument(xDocument);
        }

        [TestMethod]
        public void Complete_video_gets_written_correctly()
        {
            // A
            var siteMapCacheKeys = new List<string>();
            string siteMapUrlTemplate = string.Empty;
            var siteMapLoader = Substitute.For<ISiteMapLoader>();
            var context = FakeControllerContext();

            // A
            var rootNode = GetTestNodeTree();
            AddFullVideoData(rootNode);

            var videoSitemapExtension = new VideoSiteMapExtension();
            var extensions = new List<ISiteMapResultExtension>
            {
                videoSitemapExtension
            };

            var result = new TestExtensibleXmlSiteMapResult(0, rootNode, siteMapCacheKeys, BaseUrl, siteMapUrlTemplate, siteMapLoader, extensions);
            result.ExecuteResult(context);
            var xDocument = result.XDocument;

            SchemaValidateResultDocument(xDocument);

            Console.WriteLine(xDocument.ToString());
        }

        private static ControllerContext FakeControllerContext()
        {
            var httpContext = Substitute.For<HttpContextBase>();
            var routeData = new RouteData();
            var controller = Substitute.For<ControllerBase>();

            var context = new ControllerContext(httpContext, routeData, controller);
            return context;
        }

        private static void SchemaValidateResultDocument(XDocument xDocument)
        {
            var schemas = new XmlSchemaSet();
            schemas.Add(VideoSchemaNamespace, VideoSchemaUrl);
            schemas.Add(SitemapSchemaNamespace, SitemapSchemaUrl);

            var eventArgs = new List<ValidationEventArgs>();
            ValidationEventHandler handler = (sender, args) => eventArgs.Add(args);
            xDocument.Validate(schemas, handler);

            if (eventArgs.Any())
            {
                foreach (var eventArg in eventArgs)
                {
                    Console.WriteLine("{0}: {1}", eventArg.Severity, eventArg.Message);
                }

                Console.WriteLine(xDocument.ToString());

                Assert.Fail("Error in schema validation. See console output.");
            }
        }

        private void AddBasicVideoData(ExtensibleSiteMapNode rootNode)
        {
            var videoData = new VideoSiteMapExtension.VideoData();

            var info = new VideoNodeInformation
            {
                ThumbnailUrl = "zomg.jpg",
                Title = "test title",
                Description = "Well, the way they make shows is, they make one show. That show's called a pilot. Then they show that show to the people who make shows, and on the strength of that one show they decide if they're going to make more shows. Some pilots get picked and become television programs. Some don't, become nothing. She starred in one of the ones that became nothing.",
                ContentUrl = "zomg.flv",
            };
            videoData.VideoNodeInformation.Add(info);

            var info2 = new VideoNodeInformation
            {
                ThumbnailUrl = "zomg2.jpg",
                Title = "test title",
                Description = "Well, the way they make shows is, they make one show. That show's called a pilot. Then they show that show to the people who make shows, and on the strength of that one show they decide if they're going to make more shows. Some pilots get picked and become television programs. Some don't, become nothing. She starred in one of the ones that became nothing.",
                ContentUrl = "zomg2.flv"
            };
            videoData.VideoNodeInformation.Add(info2);

            rootNode.DataByExtensionKey.Add(VideoSiteMapExtension.ExtensionDataKey, videoData);
        }

        private void AddFullVideoData(ExtensibleSiteMapNode rootNode)
        {
            var videoData = new VideoSiteMapExtension.VideoData();

            var info = new VideoNodeInformation
            {
                ThumbnailUrl = "zomg.jpg",
                Title = "test title",
                Description = "Well, the way they make shows is, they make one show. That show's called a pilot. Then they show that show to the people who make shows, and on the strength of that one show they decide if they're going to make more shows. Some pilots get picked and become television programs. Some don't, become nothing. She starred in one of the ones that became nothing.",
                ContentUrl = "zomg.flv",
                Category = "test category",
                Duration = TimeSpan.FromSeconds(678),
                ExpirationDate = DateTime.Now.AddDays(150),
                GalleryTitle = "gallery for test video",
                GalleryUrl = "videolist.html",
                IsFamilyFriendly = false,
                IsLiveStream = true,
                Platform = new VideoPlatform(new List<VideoPlatformTarget> { VideoPlatformTarget.Mobile, VideoPlatformTarget.Television }, RestrictionRelation.NotThese),
                Player = new VideoPlayer("player.url", true, "autoplay=testparm"),
                Prices = new List<VideoPrice> { new VideoPrice("1.55", "USD", PurchaseType.Rent, PurchaseResolution.SD), new VideoPrice("9821345.222", "BLA", PurchaseType.Undefined, PurchaseResolution.Undefined) },
                PublicationDate = DateTime.Now,
                Rating = 4.99f,
                RequiresSubscription = true,
                RestrictedCountries = new List<string> { "de", "uk", "us" },
                RestrictionRelation = RestrictionRelation.OnlyThese,
                Tags = new List<string> { "tag!", "tag2", "tag4"},
                Uploader = new VideoUploader("test uploader", "uploader.htm"),
                ViewCount = 12345
            };
            videoData.VideoNodeInformation.Add(info);
            
            rootNode.DataByExtensionKey.Add(VideoSiteMapExtension.ExtensionDataKey, videoData);
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
                Title = "a test title",
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