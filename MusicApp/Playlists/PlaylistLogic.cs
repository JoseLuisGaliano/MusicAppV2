using System.Windows;
using System.Windows.Controls;
using MusicApp.Database;
using MusicApp.DataTypes;

namespace MusicApp.Playlists
{
    public class PlaylistLogic
    {
        public PlaylistLogic()
        {
        }

        public bool DeletePlaylist(string selectedPlaylistName)
        {
            if (string.IsNullOrEmpty(selectedPlaylistName))
            {
                MessageBox.Show("Please select a playlist to delete.");
                return false;
            }

            var result = MessageBox.Show($"Are you sure you want to delete '{selectedPlaylistName}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                bool playlistDeleted = DatabaseManager.GetInstance().DeletePlaylist(selectedPlaylistName);
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

        public void ShowDetails(Button button)
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

        public bool DeletePlaylist(Button button)
        {
            if (button != null)
            {
                var playlist = button.DataContext as Playlist;
                if (playlist != null)
                {
                    return DatabaseManager.GetInstance().DeletePlaylist(playlist.Name);
                }
                else
                {
                    MessageBox.Show("Please select a playlist to delete");
                    return false;
                }
            }
            return false;
        }

        public bool AddPlaylist(string name, string description)
        {
            // Verify the input fields are not empty
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please fill in all the fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Add playlist to database
            bool added = DatabaseManager.GetInstance().InsertPlaylist(name, description);
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

        public bool AddSongsToPlaylist(int playlistID, List<Song> playlistSongs)
        {
            bool result = true;

            // Displays the search window and waits for the user to finish
            if (new Search.SearchWindow().ShowDialog() == true)
            {
                // TODO: Go to search window, select songs from there
                /*var selectedSongs = selectSongsWindow.SelectedSongs;
                foreach (var song in selectedSongs)
                {
                    playlistSongs.Add(song);
                    // Save changes
                    bool resultState = DatabaseManager.GetInstance().InsertSongsIntoPlaylist(playlistID, playlistSongs);
                    if (!resultState)
                    {
                        result = false;
                    }
                }*/
            }
            return result;
        }

        public bool RemoveSongFromPlaylist(int playlistID, Song selectedSong)
        {
            if (selectedSong != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to remove '{selectedSong.SongTitle}'?", "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    bool success = DatabaseManager.GetInstance().DeleteSongFromPlaylist(playlistID, selectedSong.SongTitle);
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

        public bool SubmitFeedback(int songID, int userRating, string comment)
        {
            if (string.IsNullOrEmpty(comment))
            {
                MessageBox.Show("Please enter a comment before submitting.", "Input Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            bool success = DatabaseManager.GetInstance().InsertNewFeedback(songID, userRating, comment);
            return success;
        }
    }
}
