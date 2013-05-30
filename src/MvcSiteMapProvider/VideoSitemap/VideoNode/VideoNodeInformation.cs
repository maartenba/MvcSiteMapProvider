namespace ExtensibleSiteMap.VideoNode
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class VideoNodeInformation
    {
        public VideoNodeInformation()
        {
            RestrictedCountries = new Collection<string>();
            Prices = new Collection<VideoPrice>();
            Tags = new Collection<string>();
            IsFamilyFriendly = true;
        }

        public string ThumbnailUrl { get; set; }
        public string ContentUrl { get; set; }
        public VideoPlayer Player { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public float? Rating { get; set; }
        public int? ViewCount { get; set; }
        public DateTime? PublicationDate { get; set; }
        public bool IsFamilyFriendly { get; set; }
        public bool? IsLiveStream { get; set; }
        public bool? RequiresSubscription { get; set; }
        public ICollection<string> Tags { get; set; }
        public string Category { get; set; }
        public ICollection<string> RestrictedCountries { get; set; }
        public RestrictionRelation? RestrictionRelation { get; set; }
        public string GalleryUrl { get; set; }
        public string GalleryTitle { get; set; }
        public ICollection<VideoPrice> Prices { get; set; }
        public VideoUploader Uploader { get; set; }
        public VideoPlatform Platform { get; set; }
    }
}