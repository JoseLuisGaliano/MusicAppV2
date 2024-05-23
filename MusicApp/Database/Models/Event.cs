namespace MusicApp.Database.Models
{
    internal class Event
    {
        internal int EventID { get; set; }
        internal int ArtistID { get; set; }
        internal string EventName { get; set; }
        internal string EventDescription { get; set; }
        internal string EventLocation { get; set; }
        internal DateTime EventDate { get; set; }
        internal float EventTicketPrice { get; set; }
    }
}
