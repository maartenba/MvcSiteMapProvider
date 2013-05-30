namespace ExtensibleSiteMap.VideoNode
{
    using System.Collections.Generic;

    public class VideoPlatform
    {
        private readonly IEnumerable<VideoPlatformTarget> _targets;
        private readonly RestrictionRelation _relation;

        public VideoPlatform(IEnumerable<VideoPlatformTarget> targets, RestrictionRelation relation)
        {
            _targets = targets;
            _relation = relation;
        }

        public IEnumerable<VideoPlatformTarget> Targets
        {
            get { return _targets; }
        }

        public RestrictionRelation Relation
        {
            get { return _relation; }
        }
    }

    public enum VideoPlatformTarget
    {
        Mobile, Television, Web
    }
}