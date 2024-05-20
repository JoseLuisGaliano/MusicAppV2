using System.IO;
using System.Windows;
using System.Windows.Media;
using MusicApp.Database;
using MusicApp.DataTypes;

namespace MusicApp.Player
{
    internal class PlayerLogic
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private List<string> playlist = new List<string>(); // List of the file paths of the songs in the playlist
        private int currentSongIndex = 1;
        private bool isCurrentlyPlaying = false;

        internal void StartReproduction(string songPath)
        {
            // Verify if the song path is valid, otherwise the player will not be able to start
            if (!string.IsNullOrEmpty(songPath))
            {
                LoadSong(songPath);
            }
            else
            {
                MessageBox.Show("File path is not provided or invalid.");
            }
        }

        private void LoadSong(string filePath)
        {
            try
            {
                mediaPlayer.Open(new Uri(filePath, UriKind.Absolute));
                mediaPlayer.Play();
                isCurrentlyPlaying = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing song: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // TODO: Integrate loading playlists into the player
        private void InitializePlaylist()
        {
            playlist = DatabaseManager.GetInstance().LoadPlaylist();
        }

        internal void GoToPreviousSong()
        {
            if (isCurrentlyPlaying)
            {
                mediaPlayer.Stop(); // Stop reproduction before loading previous song
                isCurrentlyPlaying = false;
            }

            // Decrement index (managing initial song case)
            currentSongIndex--;
            if (currentSongIndex < 0)
            {
                currentSongIndex = playlist.Count - 1; // If we reach the initial song, we loop back to the final song
            }

            // Load previous song
            LoadSong(playlist[currentSongIndex]);
        }

        internal void PlaySong()
        {
            if (isCurrentlyPlaying)
            {
                mediaPlayer.Pause();
                isCurrentlyPlaying = false;
            }
            else
            {
                mediaPlayer.Play();
                isCurrentlyPlaying = true;
            }
        }

        internal void GoToNextSong()
        {
            if (isCurrentlyPlaying)
            {
                mediaPlayer.Stop(); // Stop reproduction before loading next song
                isCurrentlyPlaying = false;
            }

            // Increment index (managing final song case)
            currentSongIndex++;
            if (currentSongIndex >= playlist.Count)
            {
                currentSongIndex = 0; // If we reach the final song, we loop back to the initial song
            }

            // Load next song
            LoadSong(playlist[currentSongIndex]);
        }

        internal void DownloadSong(Song selectedSong)
        {
            if (selectedSong != null)
            {
                // Create text file with song data
                string songData = $"Title: {selectedSong.SongTitle}\nArtist: {selectedSong.ArtistName}\n";

                // Path to which the song file will be saved
                string filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{selectedSong.SongTitle}.txt";

                try
                {
                    // Write song data into text file
                    File.WriteAllText(filePath, songData);

                    MessageBox.Show("Canción descargada exitosamente.", "Descarga Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al descargar la canción: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccione una canción de la lista antes de descargar.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
