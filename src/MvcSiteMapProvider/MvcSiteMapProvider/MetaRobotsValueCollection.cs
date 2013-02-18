using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Collections;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MetaRobotsValueCollection
        : LockableList<string>, IMetaRobotsValueCollection
    {
        public MetaRobotsValueCollection(
            ISiteMap siteMap
            )
            : base(siteMap)
        {
        }

        public override void Add(string item)
        {
            item = item.ToLowerInvariant();
            this.ValidateValue(item);
            // skip duplicates
            if (!this.Contains(item))
            {
                base.Add(item);
            }
        }

        public override void AddRange(IEnumerable<string> collection)
        {
            foreach (var item in collection)
            {
                this.Add(item);
            }
        }

        public override void Insert(int index, string item)
        {
            item = item.ToLowerInvariant();
            this.ValidateValue(item);
            // skip duplicates
            if (!this.Contains(item))
            {
                base.Insert(index, item);
            }
        }

        public override void InsertRange(int index, IEnumerable<string> collection)
        {
            foreach (var item in collection)
            {
                this.Insert(index, item);
                index += 2;
            }
        }

        public virtual string GetMetaRobotsContentString()
        {
            if (this.HasDefaultValue) return String.Empty;
            string result = String.Empty;
            bool first = true;
            foreach (var item in this)
            {
                if (first)
                    first = false;
                else
                    result += ",";
                result += item;
            }
            return result;
        }

        public virtual bool HasNoIndexAndNoFollow
        {
            get
            {
                if (this.Contains("none"))
                    return true;
                if (this.Contains("noindex") && this.Contains("nofollow"))
                    return true;
                return false;
            }
        }

        protected virtual bool HasDefaultValue
        {
            get
            {
                return (this.Count == 2 && this.Contains("index") && this.Contains("follow")) ||
                    (this.Count == 1 && (this[0].Equals("index") || this[0].Equals("follow")));

            }
        }

        protected virtual void ValidateValue(string item)
        {
            switch (item)
            {
                case "index":
                    if (this.Contains("noindex"))
                    {
                        throw new ArgumentException(String.Format(Resources.Messages.MetaRobotsValueIndexAmbiguous, "noindex"));
                    }
                    if (this.Contains("none"))
                    {
                        throw new ArgumentException(String.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "none"));
                    }
                    break;
                case "noindex":
                    if (this.Contains("index"))
                    {
                        throw new ArgumentException(String.Format(Resources.Messages.MetaRobotsValueIndexAmbiguous, "index"));
                    }
                    if (this.Contains("none"))
                    {
                        throw new ArgumentException(String.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "none"));
                    }
                    break;
                case "follow":
                    if (this.Contains("nofollow"))
                    {
                        throw new ArgumentException(String.Format(Resources.Messages.MetaRobotsValueFollowAmbiguous, "nofollow"));
                    }
                    if (this.Contains("none"))
                    {
                        throw new ArgumentException(String.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "none"));
                    }
                    break;
                case "nofollow":
                    if (this.Contains("follow"))
                    {
                        throw new ArgumentException(String.Format(Resources.Messages.MetaRobotsValueFollowAmbiguous, "follow"));
                    }
                    if (this.Contains("none"))
                    {
                        throw new ArgumentException(String.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "none"));
                    }
                    break;
                case "none":
                    if (this.Contains("index"))
                    {
                        throw new ArgumentException(String.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "index"));
                    }
                    if (this.Contains("noindex"))
                    {
                        throw new ArgumentException(String.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "noindex"));
                    }
                    if (this.Contains("follow"))
                    {
                        throw new ArgumentException(String.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "follow"));
                    }
                    if (this.Contains("nofollow"))
                    {
                        throw new ArgumentException(String.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "nofollow"));
                    }
                    break;
                case "noarchive":
                case "nocache":
                case "nosnippet":
                case "nopreview":
                case "noodp":
                case "noydir":
                    // Do nothing - these are valid
                    break;
                default:
                    throw new ArgumentException(String.Format(Resources.Messages.MetaRobotsValueUnrecognized, item));

                // For information on these values, see http://yoast.com/articles/robots-meta-tags/
            }
        }
    }
}
