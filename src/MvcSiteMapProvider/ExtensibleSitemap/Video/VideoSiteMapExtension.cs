namespace ExtensibleSiteMap.Video
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using MvcSiteMapProvider;

    public class VideoSiteMapExtension : ISiteMapResultExtension
    {
        public class VideoData
        {
            public VideoData()
            {
                VideoNodeInformation = new Collection<VideoNodeInformation>();
            }

            public ICollection<VideoNodeInformation> VideoNodeInformation { get; set; }
        }

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
            var errorMessageByProperty = ValidateVideoNodeInformation(videoInformation);
            if (errorMessageByProperty.Any())
            {
                var details = errorMessageByProperty.Aggregate(
                    new StringBuilder(),
                    (sb, kvp) => sb.AppendFormat("{0}: {1}, ", kvp.Key, kvp.Value),
                    sb => sb.ToString().Substring(0, sb.Length - 2)
                );

                throw new InvalidOperationException("Video node information is invalid: " + details);
            }

            AddRequiredAttributes(videoInformation, e);
            AddOptionalAttributes(videoInformation, e);
        }

        private void AddRequiredAttributes(VideoNodeInformation videoInformation, XElement e)
        {
            // Required. Guaranteed to be there through validation in AddAttributesToVideoElement
            Add(e, "thumbnail_loc", videoInformation.ThumbnailUrl);
            Add(e, "title", videoInformation.Title);
            Add(e, "description", videoInformation.Description);

            // At least one required
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
        }

        private void AddOptionalAttributes(VideoNodeInformation videoInformation, XElement e)
        {
            // Optional
            if (videoInformation.Duration.HasValue)
            {
                Add(e, "duration", videoInformation.Duration.Value.TotalSeconds.ToString());
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

            if (!videoInformation.IsFamilyFriendly)
            {
                Add(e, "family_friendly", "No");
            }

            AddRestriction(videoInformation, e);
            AddGallery(videoInformation, e);
            AddPrices(videoInformation, e);

            if (videoInformation.RequiresSubscription.HasValue)
            {
                Add(e, "requires_subscription", videoInformation.RequiresSubscription.Value ? "yes" : "no");
            }

            AddUploader(videoInformation, e);
            AddPlatform(videoInformation, e);

            if (videoInformation.IsLiveStream.HasValue)
            {
                Add(e, "live", videoInformation.IsLiveStream.Value ? "yes" : "no");
            }
        }

        private void AddPlatform(VideoNodeInformation videoInformation, XElement e)
        {
            var platform = videoInformation.Platform;
            if (platform != null)
            {
                string targets = platform.Targets.Aggregate(string.Empty, (acc, target) => acc + " " + ConvertPlatformTargetString(target), s => s.Substring(1));
                var platformElement = new XElement(NameSpace + "platform", targets);
                platformElement.SetAttributeValue("relationship", platform.Relation == RestrictionRelation.NotThese ? "deny" : "allow");
                e.Add(platformElement);
            }
        }

        private void AddUploader(VideoNodeInformation videoInformation, XElement e)
        {
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
        }

        private void AddPrices(VideoNodeInformation videoInformation, XElement e)
        {
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
                        priceElement.SetAttributeValue("type", price.PurchaseType == PurchaseType.Own ? "own" : "rent");
                    }

                    if (price.Resolution != PurchaseResolution.Undefined)
                    {
                        priceElement.SetAttributeValue("resolution", price.Resolution == PurchaseResolution.HD ? "HD" : "SD");
                    }
                    e.Add(priceElement);
                }
            }
        }

        private void AddGallery(VideoNodeInformation videoInformation, XElement e)
        {
            if (!string.IsNullOrWhiteSpace(videoInformation.GalleryUrl))
            {
                var galleryElement = new XElement(NameSpace + "gallery_loc", videoInformation.GalleryUrl);
                if (!string.IsNullOrWhiteSpace(videoInformation.GalleryTitle))
                {
                    galleryElement.SetAttributeValue("title", videoInformation.GalleryTitle);
                }
                e.Add(galleryElement);
            }
        }

        private void AddRestriction(VideoNodeInformation videoInformation, XElement e)
        {
            if (videoInformation.RestrictedCountries.Any())
            {
                string relationshipValue = videoInformation.RestrictionRelation == RestrictionRelation.NotThese ? "deny" : "allow";
                var countries = videoInformation.RestrictedCountries.Aggregate(
                    string.Empty,
                    (acc, s) => acc + " " + s.ToUpperInvariant(),
                    s => s.Substring(1)
                    );

                var restriction = new XElement(NameSpace + "restriction", countries);
                restriction.SetAttributeValue("relationship", relationshipValue);

                e.Add(restriction);
            }
        }

        private string ConvertPlatformTargetString(VideoPlatformTarget target)
        {
            switch (target)
            {
                case VideoPlatformTarget.Mobile:
                    return "mobile";
                case VideoPlatformTarget.Television:
                    return "tv";
                case VideoPlatformTarget.Web:
                    return "web";
                default: throw new ArgumentException("Unexpected enum member for VideoPlatformTarget: " + target.ToString());
            }
        }

        private IDictionary<string, string> ValidateVideoNodeInformation(VideoNodeInformation vi)
        {
            var result = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(vi.ThumbnailUrl))
            {
                result.Add("ThumbnailUrl", "Required");
            }

            if (string.IsNullOrWhiteSpace(vi.Title))
            {
                result.Add("Title", "Required");
            }

            if (string.IsNullOrWhiteSpace(vi.Description))
            {
                result.Add("Description", "Required");
            }

            if (vi.Duration.HasValue)
            {
                var duration = vi.Duration.Value.TotalSeconds;
                if (duration <= 0 || duration > 28800) // 8 hours
                {
                    result.Add("Duration", "Must be non-negative and shorter than 28800 seconds");
                }
            }

            if (vi.Rating.HasValue)
            {
                var rating = vi.Rating.Value;
                if (rating < 0 || rating > 5)
                {
                    result.Add("Rating", "Must be in interval [0,5].");
                }
            }

            if (vi.ViewCount < 0)
            {
                result.Add("ViewCount", "Must be non-negative");
            }

            if (vi.Tags.Count > 32)
            {
                result.Add("Tags", "At most 32 tags are permitted");
            }

            return result;
        }

        private void Add(XElement target, string name, string value)
        {
            target.Add(new XElement(NameSpace + name, value));
        }
    }
}