using System.Windows;

namespace MusicApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoToSearch(object sender, RoutedEventArgs e)
        {
            Search.SearchWindow searchWindow = new Search.SearchWindow();
            searchWindow.Show();
            Close();
        }

        private void GoToChat(object sender, RoutedEventArgs e)
        {
            Chat.MVVM.View.ChatWindow chatWindow = new Chat.MVVM.View.ChatWindow();
            chatWindow.Show();
            Close();
        }

        private void GoToProfile(object sender, RoutedEventArgs e)
        {
            Profile.ProfileWindow profileWindow = new Profile.ProfileWindow();
            profileWindow.Show();
            Close();
        }

        private void GoToEvents(object sender, RoutedEventArgs e)
        {
            Events.Event eventWindow = new Events.Event();
            eventWindow.Show();
            Close();
        }

        private void GoToPlaylist(object sender, RoutedEventArgs eventArgs)
        {
            Playlists.PlaylistListWindow playlistListWindow = new Playlists.PlaylistListWindow();
            playlistListWindow.Show();
            Close();
        }
    }
}