using System.Windows;

namespace MusicApp.Playlists
{
    public partial class AddPlaylistWindow : Window
    {
        public AddPlaylistWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            string playlistName = txtPlaylistName.Text;
            string playlistDescription = txtDescription.Text;
            PlaylistLogic.AddPlaylist(playlistName, playlistDescription);
        }
    }
}

