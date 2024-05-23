namespace MusicApp.Database.Models
{
    internal class Feedback
    {
        internal int FeedbackID { get; set; }
        internal int UserID { get; set; }
        internal int SongID { get; set; }
        internal int Rating { get; set; }
        internal string Comment { get; set; }
        internal DateTime FeedbackDate { get; set; }
    }
}
