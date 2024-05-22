namespace MusicAppWebAPI.Models
{
    public class Event
    {
        public int EventID { get; set; }
        public int ArtistID { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventLocation { get; set; }
        public DateTime EventDate { get; set; }
        public float EventTicketPrice { get; set; }
    }
}
