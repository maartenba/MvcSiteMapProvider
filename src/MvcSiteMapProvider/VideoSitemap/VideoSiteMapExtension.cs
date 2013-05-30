namespace ExtensibleSiteMap
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml.Linq;
    using VideoNode;
    
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
}