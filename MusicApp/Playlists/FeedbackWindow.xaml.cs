using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MusicApp.DataTypes;

namespace MusicApp.Playlists
{
    public partial class FeedbackWindow : Window
    {
        public ObservableCollection<string> PreviousComments { get; set; }
        private int songID;
        private int userRating = 0;

        public FeedbackWindow(Song song)
        {
            InitializeComponent();

            int songID = song.ID;
            this.songID = songID;
            PreviousComments = new ObservableCollection<string>();

            LoadSongDetails();
            LoadPreviousComments();
            DataContext = this;
        }

        private void LoadSongDetails()
        {
            // Load song from the database
            Song song = Database.DatabaseManager.LoadSong(songID);
            // Update GUI layout
            SongNameTextBlock.Text = song.SongTitle;
            ArtistNameTextBlock.Text = song.ArtistName;
            AlbumCoverImage.Source = new BitmapImage(new Uri(song.CoverImage, UriKind.Absolute));
        }

        private void LoadPreviousComments()
        {
            // Clear comment section
            PreviousComments.Clear();
            // Load comments from the database
            List<string> comments = Database.DatabaseManager.LoadComments(songID);
            // Update GUI list view
            comments.ForEach(x => PreviousComments.Add(x));
        }

        private void StarButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            var button = sender as Button;
            if (button != null)
            {
                userRating = Convert.ToInt32(button.Tag);
                UpdateStarRatingDisplay(userRating);
            }
        }

        private void UpdateStarRatingDisplay(int rating)
        {
            StackPanel starsPanel = FindName("StarsPanel") as StackPanel;
            if (starsPanel != null)
            {
                foreach (Button starButton in starsPanel.Children)
                {
                    int starRating = Convert.ToInt32(starButton.Tag);
                    starButton.Foreground = starRating <= rating ? Brushes.Yellow : Brushes.Gray;
                }
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            string comment = CommentTextBox.Text.Trim();
            bool success = PlaylistLogic.SubmitFeedback(songID, userRating, comment);
            if (success)
            {
                PreviousComments.Insert(0, comment); // Add the comment to the top of the list
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            var mainWindow = new PlaylistListWindow();
            mainWindow.Show();
            Close();
        }
    }
}