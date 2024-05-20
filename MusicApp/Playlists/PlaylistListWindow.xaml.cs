using System.Windows;
using System.Windows.Controls;
using MusicApp.DataTypes;

namespace MusicApp.Playlists
{
    public partial class PlaylistListWindow : Window
    {
        public PlaylistListWindow()
        {
            InitializeComponent();
            LoadPlaylistsFromDatabase();
        }

        private void LoadPlaylistsFromDatabase()
        {
            // Load all playlists from the database
            List<Playlist> playlists = Database.DatabaseManager.LoadAllPlaylists();

            // Update GUI layout
            PlaylistResultsListBox.ItemsSource = playlists;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            // Search the playlist in the database using an sql query
            string searchQuery = SearchBar.Text.ToLower().Trim();
            List<Playlist> foundPlaylists = Database.DatabaseManager.SearchPlaylists(searchQuery);

            // Update GUI layout
            PlaylistResultsListBox.ItemsSource = foundPlaylists;
        }

        private void AddPlaylistButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            // Create window to add playlists
            AddPlaylistWindow addPlaylistWindow = new AddPlaylistWindow();
            addPlaylistWindow.Show();
            Close();
        }

        private void DeletePlaylistButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            var selectedPlaylistName = PlaylistResultsListBox.SelectedItem as string;
            bool deleted = PlaylistLogic.DeletePlaylist(selectedPlaylistName);
            if (deleted)
            {
                LoadPlaylistsFromDatabase(); // Refresh playlist list
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            var button = sender as Button;
            PlaylistLogic.ShowDetails(button);
        }
    }
}