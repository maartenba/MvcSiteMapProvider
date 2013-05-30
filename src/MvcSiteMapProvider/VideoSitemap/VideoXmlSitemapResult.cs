namespace VideoSitemap
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using MvcSiteMapProvider;
    using MvcSiteMapProvider.Loader;
    using MvcSiteMapProvider.Web.Mvc;
    using SiteMapNode;

    public class VideoXmlSitemapResult : XmlSiteMapResult
    {
        private const string DateTimeFormat = "yyyy'-'MM'-'dd";

        public VideoXmlSitemapResult(int page, ISiteMapNode rootNode, IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate, ISiteMapLoader siteMapLoader)
            : base(page, rootNode, siteMapCacheKeys, baseUrl, siteMapUrlTemplate, siteMapLoader)
        {
            VideoNs = "http://www.google.com/schemas/sitemap-video/1.1";
            VideoNsName = "video";
        }

        protected XNamespace VideoNs { get; set; }
        protected string VideoNsName { get; set; }

        protected override void AddAttributesToUrlElement(ISiteMapNode siteMapNode, System.Xml.Linq.XElement urlElement)
        {
            base.AddAttributesToUrlElement(siteMapNode, urlElement);

            var videoNode = siteMapNode as VideoSiteMapNode;

            if (videoNode != null)
            {
                foreach (var info in videoNode.VideoNodeInformation)
                {
                    var videoElement = new XElement(VideoNs + "video");
                    AddAttributesToVideoElement(info, videoElement);
                    urlElement.Add(videoElement);
                }
            }
        }

        protected override XElement GetRootElement()
        {
            var root = base.GetRootElement();
            root.SetAttributeValue(XNamespace.Xmlns + VideoNsName, VideoNs);
            return root;
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
                var playerElement = new XElement(VideoNs + "player_loc");
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
                    var restriction = new XElement(VideoNs + "restriction", country);
                    restriction.SetAttributeValue("relationship", relationshipValue);
                    e.Add(restriction);
                }
            }

            if (!string.IsNullOrWhiteSpace(videoInformation.GalleryUrl))
            {
                var galleryElement = new XElement(VideoNs + "gallery_loc", videoInformation.GalleryUrl);
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
                    var priceElement = new XElement(VideoNs + "price", price.Price);
                    if (!string.IsNullOrWhiteSpace(price.Currency))
                    {
                        priceElement.SetAttributeValue("currency", price.Currency);
                    }

                    if (price.PurchaseType != PurchaseType.Undefined)
                    {
                        priceElement.SetAttributeValue(VideoNs + "type", price.PurchaseType == PurchaseType.Own ? "own" : "rent");
                    }

                    if (price.Resolution != PurchaseResolution.Undefined)
                    {
                        priceElement.SetAttributeValue(VideoNs + "resolution", price.Resolution == PurchaseResolution.HD ? "HD" : "SD");
                    }
                    e.Add(priceElement);
                }
            }

            if (videoInformation.Uploader != null)
            {
                var uploaderElement = new XElement(VideoNs + "uploader", videoInformation.Uploader.Name);
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
                var platformElement = new XElement(VideoNs + "platform", platform.Targets);
                platformElement.SetAttributeValue("relationship", platform.Relation == RestrictionRelation.NotThese ? "deny" : "allow");
                e.Add(platformElement);
            }
        }

        private void Add(XElement target, string name, string value)
        {
            target.Add(new XElement(VideoNs + name, value));
        }
    }

    internal static class IntExtensions
    {
        public static TimeSpan Hours(this int theNumber)
        {
            return TimeSpan.FromHours(theNumber);
        }
    }
}