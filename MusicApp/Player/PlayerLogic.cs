using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace MusicApp.Player
{
    internal class PlayerLogic
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private List<string> playlist = new List<string>(); // List of the file paths of the songs in the playlist
        private int currentSongIndex = 0;
        private bool isCurrentlyPlaying = false;

        internal void LoadPlaylist(List<string> files)
        {
            playlist = files;
        }

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
                currentSongIndex = playlist.IndexOf(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing song: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        internal string GetCurrentSong()
        {
            if (currentSongIndex >= 0 && currentSongIndex < playlist.Count)
            {
                return playlist[currentSongIndex];
            }
            return null;
        }

        internal int GetCurrentSongIndex()
        {
            return currentSongIndex;
        }
    }
}