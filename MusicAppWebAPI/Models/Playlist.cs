namespace MusicAppWebAPI.Models
{
    public class Playlist
    {
        public int PlaylistID { get; set; }
        public int OwnerID { get; set; }
        public string PlaylistName { get; set; }
        public string PlaylistDescription { get; set; }
        public DateTime PlaylistCreationDate { get; set; }
    }
}
