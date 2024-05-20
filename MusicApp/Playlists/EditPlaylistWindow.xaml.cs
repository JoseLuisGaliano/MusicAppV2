using System.Collections.ObjectModel;
using System.Windows;
using MusicApp.Database;
using MusicApp.DataTypes;
using MusicApp.Search;

namespace MusicApp.Playlists
{
    public partial class EditPlaylistWindow : Window
    {
        private int playlistID;
        private ObservableCollection<Song> playlistSongs = new ObservableCollection<Song>();
        private PlaylistLogic playlistLogic;

        public EditPlaylistWindow(int playlistID)
        {
            InitializeComponent();

            this.playlistID = playlistID;
            playlistLogic = new PlaylistLogic();
            LoadPlaylistData();
        }

        private void LoadPlaylistData()
        {
            // Load playlist songs metadata
            playlistSongs = new ObservableCollection<Song>(DatabaseManager.GetInstance().LoadPlaylistSongs(playlistID));
            // Bind the playlist songs to the ListView
            PlaylistListView.ItemsSource = playlistSongs;
        }

        private void AddSong_Click(object sender, RoutedEventArgs eventArgs)
        {
            playlistLogic.AddSongsToPlaylist(playlistID, playlistSongs.ToList());
        }

        private void RemoveSong_Click(object sender, RoutedEventArgs eventArgs)
        {
            var selectedSong = PlaylistListView.SelectedItem as Song;
            bool removed = playlistLogic.RemoveSongFromPlaylist(playlistID, selectedSong);
            // Remove the song from the playlist in the UI
            if (removed)
            {
                playlistSongs.Remove(selectedSong);
            }
        }
    }
}
