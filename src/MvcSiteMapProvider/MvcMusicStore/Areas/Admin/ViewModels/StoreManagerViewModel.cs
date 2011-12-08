using System.Collections.Generic;
using MvcMusicStore.Models;

namespace MvcMusicStore.Areas.Admin.ViewModels
{
    public class StoreManagerViewModel
    {
        public Album Album { get; set; }
        public List<Artist> Artists { get; set; }
        public List<Genre> Genres { get; set; }
    }
}