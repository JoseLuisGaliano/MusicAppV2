namespace MusicApp.Database.Models
{
    internal class Album
    {
        internal int AlbumID { get; set; }
        internal int ArtistID { get; set; }
        internal string AlbumTitle { get; set; }
        internal DateTime AlbumReleaseDate { get; set; }
        internal string AlbumPicture { get; set; }
        internal string AlbumGenre { get; set; }
    }
}
