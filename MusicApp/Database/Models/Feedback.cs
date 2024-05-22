namespace MusicApp.Database.Models
{
    public class Feedback
    {
        public int FeedbackID { get; set; }
        public int UserID { get; set; }
        public int SongID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime FeedbackDate { get; set; }
    }
}
