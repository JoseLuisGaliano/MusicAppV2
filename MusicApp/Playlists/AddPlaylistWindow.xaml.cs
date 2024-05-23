using System.Windows;
using MusicApp.Database.Models;

namespace MusicApp.Playlists
{
    public partial class AddPlaylistWindow : Window
    {
        private PlaylistLogic playlistLogic;

        public AddPlaylistWindow()
        {
            playlistLogic = new PlaylistLogic();
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            string playlistName = txtPlaylistName.Text;
            string playlistDescription = txtDescription.Text;
            playlistLogic.AddPlaylist(playlistName, playlistDescription);

            // Go back to playlist list
            PlaylistListWindow playlistList = new PlaylistListWindow();
            playlistList.Show();
            Close();
        }
    }
}

