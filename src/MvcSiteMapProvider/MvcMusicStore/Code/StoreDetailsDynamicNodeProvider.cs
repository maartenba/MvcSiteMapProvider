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
        /// <returns>
        /// A dynamic node collection represented.
        /// </returns>
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection()
        {
            // Create a node for each album
            foreach (var album in storeDB.Albums.Include("Genre"))
            {
                DynamicNode node = new DynamicNode();
                node.Title = album.Title;
                node.ParentKey = "Genre_" + album.Genre.Name;
                node.RouteValues.Add("id", album.AlbumId);

                if (album.Title.Contains("Hit") || album.Title.Contains("Best"))
                {
                    node.Attributes.Add("bling", "<span style=\"color: Red;\">hot!</span>");
                }

                yield return node; 
            }
        }
    }
}