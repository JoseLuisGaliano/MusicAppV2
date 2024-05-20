using System.Windows;
using System.Windows.Controls;
using MusicApp.DataTypes;

namespace MusicApp.Events
{
    public partial class Event : Window
    {
        public Event()
        {
            InitializeComponent();
            LoadArtists();
        }

        private void LoadArtists()
        {
            // Load artists from the database into a list
            List<ArtistModel> artists = Database.DatabaseManager.LoadArtists();

            // Update layout with fetched artists
            ArtistsComboBox.ItemsSource = artists;
        }

        private void ArtistsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArtistsComboBox.SelectedItem is ArtistModel selectedArtist)
            {
                LoadEventsForSelectedArtist(selectedArtist.ArtistID);
            }
        }

        private void LoadEventsForSelectedArtist(int artistId)
        {
            // Load events for selected artist from the database
            List<EventModel> events = Database.DatabaseManager.LoadArtistEvents(artistId);

            // Update layout with fetched events
            EventsListBox.ItemsSource = events;
        }

        private void BuyTickets_Click(object sender, RoutedEventArgs eventArgs)
        {
            // Assuming that you have selected an event to pass to the Buy window.
            if (EventsListBox.SelectedItem is EventModel selectedEvent)
            {
                // Get ticket types and go to ticket window
                EventsLogic.GetTicketTypesForEvent(selectedEvent.Location, selectedEvent.DateTime, selectedEvent.EventID);
                // Close events window
                Close();
            }
            else
            {
                MessageBox.Show("Please select an event first.");
            }
        }
    }
}

