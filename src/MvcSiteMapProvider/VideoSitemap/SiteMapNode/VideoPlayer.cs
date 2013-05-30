namespace VideoSitemap.SiteMapNode
{
    public class VideoPlayer
    {
        public VideoPlayer(string url, bool allowEmbed, string autoPlay)
        {
            Url = url;
            AllowEmbed = allowEmbed;
            AutoPlay = autoPlay;
        }

        public string Url { get; private set; }
        public bool AllowEmbed { get; private set; }
        public string AutoPlay { get; private set; }
    }
}