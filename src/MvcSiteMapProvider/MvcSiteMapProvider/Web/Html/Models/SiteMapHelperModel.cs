using System.Collections;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Web.Html.Models
{
    /// <summary>
    /// SiteMapHelperModel
    /// </summary>
    public class SiteMapHelperModel 
        : IEnumerable<SiteMapNodeModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapHelperModel"/> class.
        /// </summary>
        public SiteMapHelperModel()
        {
            Nodes = new List<SiteMapNodeModel>();
        }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>The nodes.</value>
        public List<SiteMapNodeModel> Nodes { get; set; }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<SiteMapNodeModel> GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }
    }
}
