using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcMusicStore.Models;
using MvcSiteMapProvider;

namespace MvcMusicStore.Code
{
    /// <summary>
    /// StoreBrowseDynamicNodeProvider class
    /// </summary>
    public class StoreBrowseDynamicNodeProvider
        : DynamicNodeProviderBase
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        /// <summary>
        /// Gets the dynamic node collection.
        /// </summary>
        /// <param name="node">The current node.</param>
        /// <returns>
        /// A dynamic node collection represented as a <see cref="IEnumerable&lt;MvcSiteMapProvider.Extensibility.DynamicNode&gt;"/> instance 
        /// </returns>
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            // Create a node for each genre
            foreach (var genre in storeDB.Genres)
            {
                DynamicNode dynamicNode = new DynamicNode("Genre_" + genre.Name, genre.Name);
                dynamicNode.RouteValues.Add("genre", genre.Name);

                yield return dynamicNode; 
            }
        }
    }
}