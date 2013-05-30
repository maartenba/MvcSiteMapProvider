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
        }

        protected XNamespace VideoNs { get; set; }

        protected override void AddAttributesToUrlElement(ISiteMapNode siteMapNode, System.Xml.Linq.XElement urlElement)
        {
            base.AddAttributesToUrlElement(siteMapNode, urlElement);

            var videoNode = siteMapNode as VideoSiteMapNode;

            if (videoNode != null)
            {
                var videoElement = new XElement(VideoNs + "video");
                AddAttributesToVideoElement(videoNode, videoElement);
                urlElement.Add(videoElement);
            }
        }

        private void AddAttributesToVideoElement(VideoSiteMapNode videoNode, XElement e)
        {
            if (!string.IsNullOrWhiteSpace(videoNode.ThumbnailUrl))
            {
                Add(e, "thumbnail_loc", videoNode.ThumbnailUrl);
            }

            if (!string.IsNullOrWhiteSpace(videoNode.ContentUrl))
            {
                Add(e, "content_loc", videoNode.ContentUrl);
            }

            var videoPlayer = videoNode.Player;
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

            if (videoNode.Duration.HasValue)
            {
                var duration = videoNode.Duration.Value.TotalSeconds;
                if (duration > 0 && duration <= 8.Hours().TotalSeconds)
                {
                    Add(e, "duration", duration.ToString());
                }
            }

            if (videoNode.ExpirationDate.HasValue)
            {
                Add(e, "expiration_date", videoNode.ExpirationDate.Value.ToString(DateTimeFormat));
            }

            if (videoNode.Rating.HasValue)
            {
                var rating = videoNode.Rating.Value;
                if (0 <= rating && rating <= 5.0)
                {
                    Add(e, "rating", rating.ToString());
                }
            }

            if (videoNode.ViewCount.HasValue)
            {
                Add(e, "view_count", videoNode.ViewCount.Value.ToString());
            }

            if (videoNode.PublicationDate.HasValue)
            {
                Add(e, "publication_date", videoNode.PublicationDate.Value.ToString(DateTimeFormat));
            }

            if (!videoNode.IsFamilyFriendly)
            {
                Add(e, "family_friendly", "No");
            }

            if (videoNode.IsLiveStream.HasValue)
            {
                Add(e, "live", videoNode.IsLiveStream.Value ? "yes" : "no");
            }

            if (videoNode.RequiresSubscription.HasValue)
            {
                Add(e, "requires_subscription", videoNode.RequiresSubscription.Value ? "yes" : "no");
            }

            if (videoNode.Tags.Any())
            {
                foreach (var tag in videoNode.Tags)
                {
                    Add(e, "tag", tag);
                }
            }

            if (!string.IsNullOrWhiteSpace(videoNode.Category))
            {
                Add(e, "category", videoNode.Category);
            }


            if (videoNode.RestrictedCountries.Any())
            {
                string relationshipValue = videoNode.RestrictionRelation == RestrictionRelation.NotThese ? "deny" : "allow";

                foreach (var country in videoNode.RestrictedCountries)
                {
                    var restriction = new XElement(VideoNs + "restriction", country);
                    restriction.SetAttributeValue("relationship", relationshipValue);
                    e.Add(restriction);
                }
            }

            if (!string.IsNullOrWhiteSpace(videoNode.GalleryUrl))
            {
                var galleryElement = new XElement(VideoNs + "gallery_loc", videoNode.GalleryUrl);
                if (!string.IsNullOrWhiteSpace(videoNode.GalleryTitle))
                {
                    galleryElement.SetAttributeValue("title", videoNode.GalleryTitle);
                }
                e.Add(galleryElement);
            }

            if (videoNode.Prices.Any())
            {
                foreach (var price in videoNode.Prices)
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

            if (videoNode.Uploader != null)
            {
                var uploaderElement = new XElement(VideoNs + "uploader", videoNode.Uploader.Name);
                string location = videoNode.Uploader.Location;
                if (!string.IsNullOrWhiteSpace(location))
                {
                    uploaderElement.SetAttributeValue("info", location);
                }
                e.Add(uploaderElement);
            }

            var platform = videoNode.Platform;
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