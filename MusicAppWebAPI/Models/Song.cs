namespace MusicAppWebAPI.Models
{
    public class Song
    {
        public int SongID { get; set; }
        public string SongTitle { get; set; }
        public int ArtistID { get; set; }
        public int AlbumID { get; set; }
        public string SongGenre { get; set; }
        public TimeSpan SongDuration { get; set; }
        public DateTime SongReleaseDate { get; set; }
    }
}
