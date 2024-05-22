namespace MusicAppWebAPI.Models
{
    public class Ticket
    {
        public int TicketID { get; set; }
        public int EventID { get; set; }
        public int UserID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public float TicketPrice { get; set; }
        public string TicketType { get; set; }
    }
}
