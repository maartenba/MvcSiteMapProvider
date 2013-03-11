using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcMusicStore.Models;
using System.Web.Caching;
using MvcSiteMapProvider;

namespace MvcMusicStore.Code
{
    /// <summary>
    /// StoreDetailsDynamicNodeProvider class
    /// </summary>
    public class StoreDetailsDynamicNodeProvider
        : DynamicNodeProviderBase
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        /// <summary>
        /// Gets the dynamic node collection.
        /// </summary>
        /// <param name="node">The current node.</param>
        /// <returns>
        /// A dynamic node collection represented.
        /// </returns>
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            // Create a node for each album
            foreach (var album in storeDB.Albums.Include("Genre"))
            {
                DynamicNode dynamicNode = new DynamicNode();
                dynamicNode.Title = album.Title;
                dynamicNode.ParentKey = "Genre_" + album.Genre.Name;
                dynamicNode.RouteValues.Add("id", album.AlbumId);

                if (album.Title.Contains("Hit") || album.Title.Contains("Best"))
                {
                    dynamicNode.Attributes.Add("bling", "<span style=\"color: Red;\">hot!</span>");
                }

                yield return dynamicNode; 
            }
        }
    }
}