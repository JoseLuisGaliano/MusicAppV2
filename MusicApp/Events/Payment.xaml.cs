using System.Windows;

namespace MusicApp.Events
{
    public partial class Payment : Window
    {
        private string ticketType;
        private int quantity;
        private string eventLocation;
        private DateTime eventDate;

        public Payment(string ticketType, int quantity, string eventLocation, DateTime eventDate)
        {
            InitializeComponent();
            this.ticketType = ticketType;
            this.quantity = quantity;
            this.eventLocation = eventLocation;
            this.eventDate = eventDate;
            LoadEventData();
        }

        private void LoadEventData()
        {
            // Load up GUI layout
            eventLocationDateTextBlock.Text = $"{eventLocation} - {eventDate.ToString("MMMM dd, yyyy")}";
        }

        private void ConfirmPurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the CompletePurchase window with event details and close this one
            EventsLogic.CompletePurchase(eventLocation, eventDate, ticketType, quantity);
            Close();
        }
    }
}




