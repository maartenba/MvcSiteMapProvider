using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Core.Web.Html.Models
{
    /// <summary>
    /// SiteMapPathHelperModel
    /// </summary>
    public class SiteMapPathHelperModel : IEnumerable<SiteMapNodeModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapPathHelperModel"/> class.
        /// </summary>
        public SiteMapPathHelperModel()
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
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }
    }
}
