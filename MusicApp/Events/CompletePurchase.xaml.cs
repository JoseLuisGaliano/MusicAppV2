using System.Windows;

namespace MusicApp.Events
{
    public partial class CompletePurchase : Window
    {
        private string eventLocation;
        private DateTime eventDate;
        private string ticketType;
        private int quantity;
        public CompletePurchase(string eventLocation, DateTime eventDate, string ticketType, int quantity)
        {
            InitializeComponent();
            this.eventLocation = eventLocation;
            this.eventDate = eventDate;
            this.ticketType = ticketType;
            this.quantity = quantity;
        }

        private void BackToEventsButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            // The events window is already open
            Close();
        }
    }
}

