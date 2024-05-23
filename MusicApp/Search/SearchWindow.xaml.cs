using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;
using MusicApp.DataTypes;

namespace MusicApp.Search
{
    public partial class SearchWindow : Window
    {
        private SearchLogic searchLogic;
        public List<Song> SelectedSongs { get; }

        public SearchWindow()
        {
            searchLogic = new SearchLogic();
            InitializeComponent();
        }

        private void SearchButton(object sender, RoutedEventArgs e)
        {
            // Clear results area for the next search
            searchResultsStackPanel.Children.Clear();

            // Get search results (given the inputted values in the GUI)
            int filter = filterComboBox.SelectedIndex;
            string genre = genreInput.Text;
            string keywords = searchInput.Text;
            int sorter = sortComboBox.SelectedIndex;
            List<SearchResultItemControl> searchResults = searchLogic.GetSearchResults(filter, genre, keywords, sorter);

            // Show results
            DisplaySearchResults(searchResults);
        }

        public void DisplaySearchResults(List<SearchResultItemControl> searchResults)
        {
            // Add SearchResultItemControl items to the searchResultsPanel
            foreach (SearchResultItemControl item in searchResults)
            {
                searchResultsStackPanel.Children.Add(item);
            }
            // Trigger layout update
            searchResultsStackPanel.UpdateLayout();
        }

        // Small event handler for hiding the genre input bar as long as the genre filter is not selected
        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (genreInput != null)
            {
                if (filterComboBox.SelectedIndex == 4)
                {
                    genreInput.Visibility = Visibility.Visible;
                }
                else
                {
                    genreInput.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow menu = new MainWindow();
            menu.Show();
            Close();
        }
    }
}
