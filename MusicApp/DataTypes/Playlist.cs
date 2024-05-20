using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.DataTypes
{
    public class Playlist
    {
        public int PlaylistID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsLiked { get; set; }
        public bool IsFollowed { get; set; }
    }
}
