using System;
using MvcSiteMapProvider.Web;

namespace MvcSiteMapProvider.Matching
{
    /// <summary>
    /// An abstract class containing the logic for comparing 2 IUrlKey instances.
    /// </summary>
    public abstract class UrlKeyBase
        : IUrlKey
    {
        public UrlKeyBase(
                IUrlPath urlPath
            )
        {
            if (urlPath == null)
                throw new ArgumentNullException("urlPath");

            this.urlPath = urlPath;
        }

        protected IUrlPath urlPath;
        protected string hostName;
        protected string rootRelativeUrl;

        public virtual string HostName { get { return this.hostName; } }

        public virtual string RootRelativeUrl { get { return this.rootRelativeUrl; } }

        protected virtual void SetUrlValues(string relativeOrAbsoluteUrl)
        {
            if (this.urlPath.IsAbsolutePhysicalPath(relativeOrAbsoluteUrl) || this.urlPath.IsAppRelativePath(relativeOrAbsoluteUrl))
            {
                this.rootRelativeUrl = this.urlPath.ResolveVirtualApplicationToRootRelativeUrl(relativeOrAbsoluteUrl);
            }
            else if (this.urlPath.IsAbsoluteUrl(relativeOrAbsoluteUrl))
            {
                var absoluteUri = new Uri(relativeOrAbsoluteUrl, UriKind.Absolute);

                // NOTE: this will cut off any fragments, but since they are not passed
                // to the server, this is desired.
                this.rootRelativeUrl = absoluteUri.PathAndQuery;
                this.hostName = absoluteUri.Host;
            }
            else
            {
                // We must assume we already have a relative root URL
                this.rootRelativeUrl = relativeOrAbsoluteUrl;
            }
        }

        // Source: http://stackoverflow.com/questions/70303/how-do-you-implement-gethashcode-for-structure-with-two-string#21604191
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 0;

                // String properties
                hashCode = (hashCode * 397) ^ (this.HostName != null ? this.HostName.GetHashCode() : string.Empty.GetHashCode());
                hashCode = (hashCode * 397) ^ (this.RootRelativeUrl != null ? this.RootRelativeUrl.GetHashCode() : string.Empty.GetHashCode());

                //// int properties
                //hashCode = (hashCode * 397) ^ intProperty;

                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (this == null)
            {
                return false;
            }
            IUrlKey objB = obj as IUrlKey;
            if (objB == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            if (!string.Equals(this.RootRelativeUrl, objB.RootRelativeUrl, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (!string.Equals(this.HostName, objB.HostName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return string.Format("[HostName: {0}, RootRelativeUrl: {1}]", this.HostName, this.RootRelativeUrl);
        }
    }
}
