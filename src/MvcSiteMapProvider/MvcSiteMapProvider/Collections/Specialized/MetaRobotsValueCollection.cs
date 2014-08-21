using System;
using System.Collections.Generic;
using MvcSiteMapProvider.DI;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// A specialized collection for managing the business rules for handling the
    /// allowed values in the meta robots tag.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///     <listheader>
    ///         <term>Allowed Values</term>
    ///     </listheader>
    ///     <item>
    ///         <term>index</term>
    ///         <description>
    ///             Allow search engine robots to index the page, you don't have to add this to your pages, as it's the default.
    ///             May not be used in conjunction with noindex or none.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>noindex</term>
    ///         <description>
    ///             Disallow search engines from showing this page in their results.
    ///             May not be used in conjunction with index or none.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>follow</term>
    ///         <description>
    ///             Tells the search engine robots to follow the links on the page, whether it can index it or not.
    ///             May not be used in conjunction with nofollow or none.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>nofollow</term>
    ///         <description>
    ///             Tells the search engine robots to not follow any links on the page at all.
    ///             May not be used in conjunction with follow or none.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>none</term>
    ///         <description>
    ///             This is a shortcut for 'noindex,nofollow', or basically saying to search engines: don't do anything with this page at all.
    ///             May not be used in conjunction with index, noindex, follow, or nofollow.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>noarchive</term>
    ///         <description>
    ///             Prevents the search engines from showing a cached copy of this page.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>nocache</term>
    ///         <description>
    ///             Same as noarchive, but only used by MSN/Live/Bing.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>nosnippet</term>
    ///         <description>
    ///             Prevents the search engines from showing a snippet of this page in the search results and prevents them from caching the page.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>nopreview</term>
    ///         <description>
    ///             Same as nosnippet, but only used by Bing.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>noodp</term>
    ///         <description>
    ///             Blocks search engines from using the description for this page in DMOZ (aka ODP) as the snippet for your page in the search results.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>noydir</term>
    ///         <description>
    ///             Blocks Yahoo! from using the description for this page in the Yahoo! directory as the snippet for your page in the search results. 
    ///             No other search engines use the Yahoo! directory for this purpose, so they don’t support the tag.
    ///         </description>
    ///     </item>
    /// </list>
    /// Any use of a single value more than one time will be ignored.
    /// </remarks>
    [ExcludeFromAutoRegistration]
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

        /// <summary>
        /// Splits a string with the given separator characters and adds each element to the collection as a new meta robots value. Duplicates will be ignored.
        /// </summary>
        /// <param name="stringToSplit">The meta robots string to split.</param>
        /// <param name="separator">An array of Unicode characters that delimit the substrings in this string, an empty array that contains no delimiters, or <b>null</b>.</param>
        public void AddRange(string stringToSplit, char[] separator)
        {
            if (!string.IsNullOrEmpty(stringToSplit))
            {
                var parameters = stringToSplit.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (var parameter in parameters)
                {
                    this.Add(parameter.Trim());
                }
            }
        }

        /// <summary>
        /// Adds each element of a <see cref="T:System.Collections.Generic.IEnumerable{string}"/> to the collection as a new meta robots value. Duplicates will be ignored.
        /// </summary>
        /// <param name="collection">The <see cref="T:System.Collections.Generic.IEnumerable{string}"/> containing the values to add, or <b>null</b>.</param>
        public override void AddRange(IEnumerable<string> collection)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    this.Add(item);
                }
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
            if (this.HasDefaultValue) return string.Empty;
            string result = string.Empty;
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
                        throw new ArgumentException(string.Format(Resources.Messages.MetaRobotsValueIndexAmbiguous, "noindex"));
                    }
                    if (this.Contains("none"))
                    {
                        throw new ArgumentException(string.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "none"));
                    }
                    break;
                case "noindex":
                    if (this.Contains("index"))
                    {
                        throw new ArgumentException(string.Format(Resources.Messages.MetaRobotsValueIndexAmbiguous, "index"));
                    }
                    if (this.Contains("none"))
                    {
                        throw new ArgumentException(string.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "none"));
                    }
                    break;
                case "follow":
                    if (this.Contains("nofollow"))
                    {
                        throw new ArgumentException(string.Format(Resources.Messages.MetaRobotsValueFollowAmbiguous, "nofollow"));
                    }
                    if (this.Contains("none"))
                    {
                        throw new ArgumentException(string.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "none"));
                    }
                    break;
                case "nofollow":
                    if (this.Contains("follow"))
                    {
                        throw new ArgumentException(string.Format(Resources.Messages.MetaRobotsValueFollowAmbiguous, "follow"));
                    }
                    if (this.Contains("none"))
                    {
                        throw new ArgumentException(string.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "none"));
                    }
                    break;
                case "none":
                    if (this.Contains("index"))
                    {
                        throw new ArgumentException(string.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "index"));
                    }
                    if (this.Contains("noindex"))
                    {
                        throw new ArgumentException(string.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "noindex"));
                    }
                    if (this.Contains("follow"))
                    {
                        throw new ArgumentException(string.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "follow"));
                    }
                    if (this.Contains("nofollow"))
                    {
                        throw new ArgumentException(string.Format(Resources.Messages.MetaRobotsValueNoneAmbiguous, "nofollow"));
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
                    throw new ArgumentException(string.Format(Resources.Messages.MetaRobotsValueUnrecognized, item));

                // For information on these values, see http://yoast.com/articles/robots-meta-tags/
            }
        }
    }
}
