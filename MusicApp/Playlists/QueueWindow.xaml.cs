using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using MusicApp.DataTypes;

namespace MusicApp.Playlists
{
    public partial class QueueWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        public ObservableCollection<Song> Queue { get; set; }
        public string CurrentSongDisplay { get; set; }

        public QueueWindow()
        {
            InitializeComponent();

            Queue = new ObservableCollection<Song>();
            CurrentSongDisplay = "Not playing"; // Default text when no song is playing
            QueueListBox.ItemsSource = Queue;
            LoadQueueFromDatabase();
            DataContext = this;
        }

        // TODO: This method in general doesn't work and I don't really understand what they were trying to do...
        private void LoadQueueFromDatabase()
        {
            // Replace with your actual connection string
            string connectionString = @"Data Source=LAPTOP-85QOQ2U8;Initial Catalog=Lab5;Integrated Security=True;";
            int playlistId = 1; // Assume that there is a Playlist which acts as the queue

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = $"SELECT Songs.Title, Artists.Name FROM Songs INNER JOIN Artists ON Songs.ArtistID = Artists.ArtistID WHERE Songs.SongID IN (SELECT SongID FROM PlaylistSongs WHERE PlaylistID = {playlistId})";
                SqlCommand command = new SqlCommand(sqlQuery, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Song song = new Song
                        {
                            SongTitle = reader.GetString(0),
                            ArtistName = reader.GetString(1)
                        };
                        Queue.Add(song);
                    }

                    if (Queue.Count == 0)
                    {
                        CurrentSongDisplay = "There are no songs in the queue";
                    }
                    else
                    {
                        CurrentSongDisplay = $"Currently playing: {Queue[0].SongTitle} by {Queue[0].ArtistName}";
                    }
                }
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            var button = sender as Button;
            var song = button?.Tag as Song;
            if (song != null)
            {
                NowPlayingTextBlock.Text = $"Currently playing: {song.SongTitle} by {song.ArtistName}";
                // TODO: code to play the song
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            var button = sender as Button;
            var song = button?.Tag as Song;
            if (song != null)
            {
                // Replace with your actual connection string
                string connectionString = "Your Connection String Here";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    int playlistId = 1; // Your Playlist/queue ID
                    string sqlQuery = $"DELETE FROM PlaylistSongs WHERE PlaylistID = {playlistId} AND SongID = {song.SongID}";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.ExecuteNonQuery();
                }

                Queue.Remove(song);

                if (Queue.Count == 0)
                {
                    NowPlayingTextBlock.Text = "There are no songs in the queue";
                }
            }
        }

        private void AddFeedback_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs eventArgs)
        {
            var textBlock = sender as TextBlock;
            var song = textBlock?.DataContext as Song;
            if (song != null)
            {
                // Open feedback window for the song
                FeedbackWindow feedbackWindow = new FeedbackWindow(song);
                feedbackWindow.Show();
                this.Close();
            }
        }
    }
}