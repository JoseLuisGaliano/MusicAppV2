namespace MusicApp.Database.Models
{
    internal class Playlist
    {
        internal int PlaylistID { get; set; }
        internal int OwnerID { get; set; }
        internal string PlaylistName { get; set; }
        internal string PlaylistDescription { get; set; }
        internal DateTime PlaylistCreationDate { get; set; }
    }
}
