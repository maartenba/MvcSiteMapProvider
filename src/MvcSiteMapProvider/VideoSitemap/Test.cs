namespace VideoSitemap
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
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
            var videoSitemapExtension = new VideoSiteMapExtension();
            var extensions = new List<ISiteMapResultExtension>
            {
                videoSitemapExtension
            };

            var result = new TestExtensibleXmlSiteMapResult(0, rootNode, siteMapCacheKeys, baseUrl, siteMapUrlTemplate, siteMapLoader, extensions);
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

            return root;
        }

        private ExtensibleSiteMapNode GetEmptyNode(string key)
        {
            var siteMap = Substitute.For<ISiteMap>();
            var pluginProvider = Substitute.For<ISiteMapNodePluginProvider>();
            var mvcContextFactory = Substitute.For<IMvcContextFactory>();
            var siteMapNodeChildStateFactory = Substitute.For<ISiteMapNodeChildStateFactory>();
            var localizationService = Substitute.For<ILocalizationService>();
            var node = new ExtensibleSiteMapNode(siteMap, key, false, pluginProvider, mvcContextFactory, siteMapNodeChildStateFactory, localizationService, new UrlPath(mvcContextFactory));
            return node;
        }
    }

    public class VideoSiteMapExtension : ISiteMapResultExtension
    {
        private const string DateTimeFormat = "yyyy'-'MM'-'dd";

        public const string ExtensionDataKey = "video"; 
        
        public XNamespace NameSpace { get { return "http://www.google.com/schemas/sitemap-video/1.1"; } }
        public string ElementName { get { return "video"; } }
        
        public void AddNodeContentToElement(IExtensibleSiteMapNode extensibleNode, XElement element)
        {
            object data;
            if (extensibleNode.DataByExtensionKey.TryGetValue(ExtensionDataKey, out data))
            {
                var videoData = data as VideoData;
                if (videoData == null)
                {
                    throw new InvalidOperationException(string.Format("Site map node had video information. Expected related data to be of type '{0}', but got object of type '{1}'", typeof(VideoData), data.GetType()));
                }

                foreach (var info in videoData.VideoNodeInformation)
                {
                    var videoElement = new XElement(NameSpace + "video");
                    AddAttributesToVideoElement(info, videoElement);
                    element.Add(videoElement);
                }
            }
        }
        
        private void AddAttributesToVideoElement(VideoNodeInformation videoInformation, XElement e)
        {
            if (!string.IsNullOrWhiteSpace(videoInformation.ThumbnailUrl))
            {
                Add(e, "thumbnail_loc", videoInformation.ThumbnailUrl);
            }

            if (!string.IsNullOrWhiteSpace(videoInformation.ContentUrl))
            {
                Add(e, "content_loc", videoInformation.ContentUrl);
            }

            var videoPlayer = videoInformation.Player;
            if (videoPlayer != null)
            {
                var playerElement = new XElement(NameSpace + "player_loc");
                playerElement.SetAttributeValue("allow_embed", videoPlayer.AllowEmbed ? "Yes" : "No");
                if (!string.IsNullOrWhiteSpace(videoPlayer.AutoPlay))
                {
                    playerElement.SetAttributeValue("autoplay", videoPlayer.AutoPlay);
                }
                e.Add(playerElement);
            }

            if (videoInformation.Duration.HasValue)
            {
                var duration = videoInformation.Duration.Value.TotalSeconds;
                if (duration > 0 && duration <= 8.Hours().TotalSeconds)
                {
                    Add(e, "duration", duration.ToString());
                }
            }

            if (videoInformation.ExpirationDate.HasValue)
            {
                Add(e, "expiration_date", videoInformation.ExpirationDate.Value.ToString(DateTimeFormat));
            }

            if (videoInformation.Rating.HasValue)
            {
                var rating = videoInformation.Rating.Value;
                if (0 <= rating && rating <= 5.0)
                {
                    Add(e, "rating", rating.ToString());
                }
            }

            if (videoInformation.ViewCount.HasValue)
            {
                Add(e, "view_count", videoInformation.ViewCount.Value.ToString());
            }

            if (videoInformation.PublicationDate.HasValue)
            {
                Add(e, "publication_date", videoInformation.PublicationDate.Value.ToString(DateTimeFormat));
            }

            if (!videoInformation.IsFamilyFriendly)
            {
                Add(e, "family_friendly", "No");
            }

            if (videoInformation.IsLiveStream.HasValue)
            {
                Add(e, "live", videoInformation.IsLiveStream.Value ? "yes" : "no");
            }

            if (videoInformation.RequiresSubscription.HasValue)
            {
                Add(e, "requires_subscription", videoInformation.RequiresSubscription.Value ? "yes" : "no");
            }

            if (videoInformation.Tags.Any())
            {
                foreach (var tag in videoInformation.Tags)
                {
                    Add(e, "tag", tag);
                }
            }

            if (!string.IsNullOrWhiteSpace(videoInformation.Category))
            {
                Add(e, "category", videoInformation.Category);
            }


            if (videoInformation.RestrictedCountries.Any())
            {
                string relationshipValue = videoInformation.RestrictionRelation == RestrictionRelation.NotThese ? "deny" : "allow";

                foreach (var country in videoInformation.RestrictedCountries)
                {
                    var restriction = new XElement(NameSpace + "restriction", country);
                    restriction.SetAttributeValue("relationship", relationshipValue);
                    e.Add(restriction);
                }
            }

            if (!string.IsNullOrWhiteSpace(videoInformation.GalleryUrl))
            {
                var galleryElement = new XElement(NameSpace + "gallery_loc", videoInformation.GalleryUrl);
                if (!string.IsNullOrWhiteSpace(videoInformation.GalleryTitle))
                {
                    galleryElement.SetAttributeValue("title", videoInformation.GalleryTitle);
                }
                e.Add(galleryElement);
            }

            if (videoInformation.Prices.Any())
            {
                foreach (var price in videoInformation.Prices)
                {
                    var priceElement = new XElement(NameSpace + "price", price.Price);
                    if (!string.IsNullOrWhiteSpace(price.Currency))
                    {
                        priceElement.SetAttributeValue("currency", price.Currency);
                    }

                    if (price.PurchaseType != PurchaseType.Undefined)
                    {
                        priceElement.SetAttributeValue(NameSpace + "type", price.PurchaseType == PurchaseType.Own ? "own" : "rent");
                    }

                    if (price.Resolution != PurchaseResolution.Undefined)
                    {
                        priceElement.SetAttributeValue(NameSpace + "resolution", price.Resolution == PurchaseResolution.HD ? "HD" : "SD");
                    }
                    e.Add(priceElement);
                }
            }

            if (videoInformation.Uploader != null)
            {
                var uploaderElement = new XElement(NameSpace + "uploader", videoInformation.Uploader.Name);
                string location = videoInformation.Uploader.Location;
                if (!string.IsNullOrWhiteSpace(location))
                {
                    uploaderElement.SetAttributeValue("info", location);
                }
                e.Add(uploaderElement);
            }

            var platform = videoInformation.Platform;
            if (platform != null)
            {
                var platformElement = new XElement(NameSpace + "platform", platform.Targets);
                platformElement.SetAttributeValue("relationship", platform.Relation == RestrictionRelation.NotThese ? "deny" : "allow");
                e.Add(platformElement);
            }
        }

        private void Add(XElement target, string name, string value)
        {
            target.Add(new XElement(NameSpace + name, value));
        }

        public class VideoData
        {
            public VideoData()
            {
                VideoNodeInformation = new Collection<VideoNodeInformation>();
            }

            public ICollection<VideoNodeInformation> VideoNodeInformation { get; set; }
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