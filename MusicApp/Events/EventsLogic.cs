using System.Windows;
using MusicApp.Database;
using MusicApp.DataTypes;

namespace MusicApp.Events
{
    public static class EventsLogic
    {
        public static void GetTicketTypesForEvent(string location, DateTime date, int eventId)
        {
            // Get ticket types for the selected event from the database
            List<TicketType> ticketTypes = DatabaseManager.GetInstance().GetTicketTypesForEvent(eventId);

            // Correct the constructor call by passing all required arguments.
            Buy buyWindow = new Buy(location, date, eventId, ticketTypes);
            buyWindow.Show();
        }

        public static void PurchaseTickets(TicketType selectedTicket, int quantity, string location, DateTime date)
        {
            if (quantity <= 0)
            {
                MessageBox.Show("Please enter a valid number of tickets.");
                return;
            }

            Payment paymentWindow = new Payment(selectedTicket.Type, quantity, location, date);
            paymentWindow.Show();
        }

        public static void CompletePurchase(string eventLocation, DateTime eventDate, string ticketType, int quantity)
        {
            // TODO: Integrate payment method from the other group here
            CompletePurchase completePurchaseWindow = new CompletePurchase(eventLocation, eventDate, ticketType, quantity);
            completePurchaseWindow.Show();
        }
    }
}
