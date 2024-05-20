using System.Windows;
using MusicApp.DataTypes;

namespace MusicApp.Playlists
{
    public partial class PlaylistDetailsWindow : Window
    {
        private int playlistID;

        public PlaylistDetailsWindow(int playlistID)
        {
            InitializeComponent();

            this.playlistID = playlistID;
            LoadSongsFromDatabase();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            QueueWindow queueWindow = new QueueWindow();
            queueWindow.Show();
            Close();
        }
        private void EditPlaylistButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            EditPlaylistWindow editPlaylistWindow = new EditPlaylistWindow(playlistID);
            editPlaylistWindow.Show();
            Close();
        }
        private void LoadSongsFromDatabase()
        {
            // Load songs from this playlist from the database
            List<Song> songs = Database.DatabaseManager.LoadPlaylistSongs(playlistID);
            // Update GUI layout
            SongsListBox.ItemsSource = songs;
        }
    }
}

