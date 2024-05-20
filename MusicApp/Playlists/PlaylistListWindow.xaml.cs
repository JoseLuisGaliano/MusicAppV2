using System.Windows;
using System.Windows.Controls;
using MusicApp.Database;
using MusicApp.DataTypes;

namespace MusicApp.Playlists
{
    public partial class PlaylistListWindow : Window
    {
        private PlaylistLogic playlistLogic;

        public PlaylistListWindow()
        {
            playlistLogic = new PlaylistLogic();

            InitializeComponent();
            LoadPlaylistsFromDatabase();
        }

        private void LoadPlaylistsFromDatabase()
        {
            // Load all playlists from the database
            List<Playlist> playlists = DatabaseManager.GetInstance().LoadAllPlaylists();

            // Update GUI layout
            PlaylistResultsListBox.ItemsSource = playlists;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            // Search the playlist in the database using an sql query
            string searchQuery = SearchBar.Text.ToLower().Trim();
            List<Playlist> foundPlaylists = DatabaseManager.GetInstance().SearchPlaylists(searchQuery);

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
            bool deleted = playlistLogic.DeletePlaylist(selectedPlaylistName);
            if (deleted)
            {
                LoadPlaylistsFromDatabase(); // Refresh playlist list
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            var button = sender as Button;
            playlistLogic.ShowDetails(button);
        }
    }
}