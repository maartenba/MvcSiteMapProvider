namespace VideoSitemap.SiteMapNode
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using MvcSiteMapProvider;
    using MvcSiteMapProvider.Globalization;
    using MvcSiteMapProvider.Web;
    using MvcSiteMapProvider.Web.Mvc;

    public class VideoSiteMapNode : SiteMapNode
    {
        public VideoSiteMapNode(ISiteMap siteMap, string key, bool isDynamic, ISiteMapNodePluginProvider pluginProvider, IMvcContextFactory mvcContextFactory, ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory, ILocalizationService localizationService, IUrlPath urlPath) 
            : base(siteMap, key, isDynamic, pluginProvider, mvcContextFactory, siteMapNodeChildStateFactory, localizationService, urlPath)
        {
            RestrictedCountries = new List<string>();
            Prices = new Collection<VideoPrice>();
        }

        public string ThumbnailUrl { get; protected set; }
        public string ContentUrl { get; protected set; }

        public VideoPlayer Player { get; protected set; }
        
        public TimeSpan? Duration { get; protected set; }
        public DateTime? ExpirationDate { get; protected set; }
        public float? Rating { get; protected set; }
        public int? ViewCount { get; protected set; }
        public DateTime? PublicationDate { get; protected set; }
        
        public bool IsFamilyFriendly { get; protected set; }
        public bool? IsLiveStream { get; protected set; }
        public bool? RequiresSubscription { get; protected set; }

        public ICollection<string> Tags { get; protected set; }
        public string Category { get; protected set; }

        public ICollection<string> RestrictedCountries { get; protected set; }
        public RestrictionRelation? RestrictionRelation { get; protected set; }

        public string GalleryUrl { get; protected set; }
        public string GalleryTitle { get; protected set; }

        public ICollection<VideoPrice> Prices { get; protected set; }
        
        public VideoUploader Uploader { get; protected set; }

        public VideoPlatform Platform { get; protected set; }
    }
}