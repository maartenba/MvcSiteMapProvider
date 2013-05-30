namespace ExtensibleSiteMap.VideoNode
{
    public class VideoUploader
    {
        private readonly string _name;
        private readonly string _location;

        public VideoUploader(string name, string location)
        {
            _name = name;
            _location = location;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Location
        {
            get { return _location; }
        }
    }
}