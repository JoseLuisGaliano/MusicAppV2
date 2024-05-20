using System.Windows;
using System.Windows.Controls;
using MusicApp.DataTypes;

namespace MusicApp.Playlists
{
    public static class PlaylistLogic
    {
        public static bool DeletePlaylist(string selectedPlaylistName)
        {
            if (string.IsNullOrEmpty(selectedPlaylistName))
            {
                MessageBox.Show("Please select a playlist to delete.");
                return false;
            }

            var result = MessageBox.Show($"Are you sure you want to delete '{selectedPlaylistName}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                bool playlistDeleted = Database.DatabaseManager.DeletePlaylist(selectedPlaylistName);
                if (playlistDeleted)
                {
                    MessageBox.Show("Playlist deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Playlist not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return false;
        }

        public static void ShowDetails(Button button)
        {
            if (button != null)
            {
                var playlist = button.DataContext as Playlist;
                if (playlist != null)
                {
                    var detailsWindow = new PlaylistDetailsWindow(playlist.PlaylistID);
                    detailsWindow.Show();
                }
                else
                {
                    MessageBox.Show("Playlist details cound not be fetched");
                }
            }
        }

        public static bool AddPlaylist(string name, string description)
        {
            // Verify the input fields are not empty
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please fill in all the fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Add playlist to database
            bool added = Database.DatabaseManager.InsertPlaylist(name, description);
            if (added)
            {
                MessageBox.Show("Playlist added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            else
            {
                MessageBox.Show($"Failed to add playlist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool AddSongsToPlaylist(int playlistID, List<Song> playlistSongs)
        {
            var selectSongsWindow = new SelectSongsWindow();
            bool result = true;

            // Displays the window and waits for the user to finish
            if (selectSongsWindow.ShowDialog() == true)
            {
                var selectedSongs = selectSongsWindow.SelectedSongs;
                foreach (var song in selectedSongs)
                {
                    playlistSongs.Add(song);
                    // Save changes
                    bool resultState = Database.DatabaseManager.InsertSongsIntoPlaylist(playlistID, playlistSongs);
                    if (!resultState)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        public static bool RemoveSongFromPlaylist(int playlistID, Song selectedSong)
        {
            if (selectedSong != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to remove '{selectedSong.SongTitle}'?", "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    bool success = Database.DatabaseManager.DeleteSongFromPlaylist(playlistID, selectedSong.SongTitle);
                    return success;
                }
                return false;
            }
            else
            {
                MessageBox.Show("Please select a song to remove.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }

        public static bool SubmitFeedback(int songID, int userRating, string comment)
        {
            if (string.IsNullOrEmpty(comment))
            {
                MessageBox.Show("Please enter a comment before submitting.", "Input Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            bool success = Database.DatabaseManager.InsertNewFeedback(songID, userRating, comment);
            return success;
        }
    }
}
