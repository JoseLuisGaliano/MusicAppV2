namespace MusicApp.Database.Models
{
    internal class Ticket
    {
        internal int TicketID { get; set; }
        internal int EventID { get; set; }
        internal int UserID { get; set; }
        internal DateTime PurchaseDate { get; set; }
        internal float TicketPrice { get; set; }
        internal string TicketType { get; set; }
    }
}
