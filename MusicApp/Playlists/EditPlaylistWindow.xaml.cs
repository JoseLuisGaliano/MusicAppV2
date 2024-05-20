using System.Collections.ObjectModel;
using System.Windows;
using MusicApp.DataTypes;

namespace MusicApp.Playlists
{
    public partial class EditPlaylistWindow : Window
    {
        private int playlistID;
        private ObservableCollection<Song> playlistSongs = new ObservableCollection<Song>();

        public EditPlaylistWindow(int playlistID)
        {
            InitializeComponent();

            this.playlistID = playlistID;
            LoadPlaylistData();
        }

        private void LoadPlaylistData()
        {
            // Load playlist songs metadata
            playlistSongs = new ObservableCollection<Song>(Database.DatabaseManager.LoadPlaylistSongs(playlistID));
            // Bind the playlist songs to the ListView
            PlaylistListView.ItemsSource = playlistSongs;
        }

        private void AddSong_Click(object sender, RoutedEventArgs eventArgs)
        {
            PlaylistLogic.AddSongsToPlaylist(playlistID, playlistSongs.ToList());
        }

        private void RemoveSong_Click(object sender, RoutedEventArgs eventArgs)
        {
            var selectedSong = PlaylistListView.SelectedItem as Song;
            bool removed = PlaylistLogic.RemoveSongFromPlaylist(playlistID, selectedSong);
            // Remove the song from the playlist in the UI
            if (removed)
            {
                playlistSongs.Remove(selectedSong);
            }
        }
    }
}
