namespace MusicAppWebAPI.Models
{
    public class Album
    {
        public int AlbumID { get; set; }
        public int ArtistID { get; set; }
        public string AlbumTitle { get; set; }
        public DateTime AlbumReleaseDate { get; set; }
        public string AlbumPicture { get; set; }
        public string AlbumGenre { get; set; }
    }
}
