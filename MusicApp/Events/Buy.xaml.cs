using System.Windows;
using System.Windows.Controls;
using MusicApp.Database;
using MusicApp.DataTypes;

namespace MusicApp.Events
{
    public partial class Buy : Window
    {
        private string location;
        private DateTime eventDate;
        private int eventId;
        private List<TicketType> ticketTypes;

        public Buy(string location, DateTime eventDate, int eventId, List<TicketType> ticketTypes)
        {
            InitializeComponent();
            this.location = location;
            this.eventDate = eventDate;
            this.eventId = eventId;
            this.ticketTypes = ticketTypes;
            LoadEventData();
        }

        private void LoadEventData()
        {
            // Set location and date text blocks
            locationTextBlock.Text = $"Location: {location}";
            dateTextBlock.Text = $"Date: {eventDate.ToString("MMMM dd, yyyy")}";

            // Set the ticket types
            ticketTypeComboBox.ItemsSource = ticketTypes;
            ticketTypeComboBox.DisplayMemberPath = "DisplayInfo";
            ticketTypeComboBox.SelectedIndex = 0;
        }

        private void TicketsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs eventArgs)
        {
            if (ticketTypeComboBox.SelectedItem is TicketType selectedTicket)
            {
                DatabaseManager.GetInstance().GetTicketTypesForEvent(selectedTicket.EventID);
            }
        }

        private void PurchaseButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            if (ticketTypeComboBox.SelectedItem is TicketType selectedTicketType)
            {
                int.TryParse(quantityTextBox.Text, out int quantity);
                // Go to purchase window and close this one
                EventsLogic.PurchaseTickets(selectedTicketType, quantity, location, eventDate);
                Close();
            }
            else
            {
                MessageBox.Show("Please select a ticket type.");
            }
        }
    }
}