using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MusicApp.Player
{
    public partial class PlayerWindow : Window
    {
        private PlayerLogic playerLogic = new PlayerLogic();
        private DispatcherTimer timer = new DispatcherTimer();

        public PlayerWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerEvent;
        }

        public class MediaFile
        {
            public string FilePath { get; set; }
            public string FileName { get; set; }
        }

        private void LoadFolderEvent(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Stop();
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var filteredFiles = Directory.GetFiles(dialog.SelectedPath, "*.*")
                    .Where(file => file.ToLower().EndsWith("webm") ||
                                   file.ToLower().EndsWith("mp4") ||
                                   file.ToLower().EndsWith("wmv") ||
                                   file.ToLower().EndsWith("mkv") ||
                                   file.ToLower().EndsWith("mp3") ||
                                   file.ToLower().EndsWith("avi")).ToList();
                playerLogic.LoadPlaylist(filteredFiles);
                LoadPlayList(filteredFiles);
            }
        }
        private void LoadPlayList(List<string> files)
        {
            PlayList.Items.Clear();
            foreach (string filePath in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                PlayList.Items.Add(new MediaFile { FilePath = filePath, FileName = fileName });
            }

            if (files.Count > 0)
            {
                fileName.Content = "Files Found: " + files.Count;
                PlayList.SelectedIndex = 0;
                playerLogic.StartReproduction(files[0]);
                ShowFileName();
            }
            else
            {
                MessageBox.Show("No Audio Files Found in this folder");
            }
        }
        private void PlayList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PlayList.SelectedIndex != -1)
            {
                var selectedMediaFile = PlayList.SelectedItem as MediaFile;
                if (selectedMediaFile != null)
                {
                    playerLogic.StartReproduction(selectedMediaFile.FilePath);
                    ShowFileName();
                }
            }
        }

        private void AudioPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            GoToNextSong();
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            AudioPlayer.Play();
            timer.Stop();
        }

        private void ShowFileName()
        {
            string currentSong = playerLogic.GetCurrentSong();
            if (!string.IsNullOrEmpty(currentSong))
            {
                string file = Path.GetFileName(currentSong);
                fileName.Content = "Currently Playing: " + file;
            }
        }

        private void PreviousSong_Click(object sender, RoutedEventArgs e)
        {
            playerLogic.GoToPreviousSong();
            PlayList.SelectedIndex = playerLogic.GetCurrentSongIndex();
            ShowFileName();
        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            playerLogic.PlaySong();
            ShowFileName();
        }

        private void NextSong_Click(object sender, RoutedEventArgs e)
        {
            GoToNextSong();
        }

        private void GoToNextSong()
        {
            playerLogic.GoToNextSong();
            PlayList.SelectedIndex = playerLogic.GetCurrentSongIndex();
            ShowFileName();
        }
    }
}