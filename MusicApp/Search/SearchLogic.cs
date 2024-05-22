using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MusicApp.Database;

namespace MusicApp.Search
{
    public class SearchLogic
    {
        private Sorters sortingModule;
        public SearchLogic()
        {
            sortingModule = new Sorters();
        }

        public List<SearchResultItemControl> GetSearchResults(int filter, string genre, string keywords, int sorter)
        {
            // Filter search
            List<SearchResultItemControl> searchItems = FilterSearch(filter, genre);

            // Search algorithm
            List<SearchResultItemControl> searchResults = FuzzyMatchingSearch(keywords, searchItems);

            // Sort results
            searchResults = SortSearchResults(searchResults, sorter, filter);

            return searchResults;
        }

        private List<SearchResultItemControl> FilterSearch(int filter, string genre)
        {
            List<SearchResultItemControl> searchItems = new List<SearchResultItemControl>();

            // Determine which table(s) to search through according to the selected filter
            switch (filter)
            {
                case 0:
                    searchItems = DatabaseManager.GetInstance().LoadSongSearchItems(searchItems);
                    searchItems = DatabaseManager.GetInstance().LoadArtistSearchItems(searchItems);
                    searchItems = DatabaseManager.GetInstance().LoadAlbumSearchItems(searchItems);
                    searchItems = DatabaseManager.GetInstance().LoadUserSearchItems(searchItems);
                    break;
                case 1:
                    searchItems = DatabaseManager.GetInstance().LoadSongSearchItems(searchItems);
                    break;
                case 2:
                    searchItems = DatabaseManager.GetInstance().LoadArtistSearchItems(searchItems);
                    break;
                case 3:
                    searchItems = DatabaseManager.GetInstance().LoadAlbumSearchItems(searchItems);
                    break;
                case 4:
                    searchItems = DatabaseManager.GetInstance().LoadSongSearchItems(searchItems, genre);
                    searchItems = DatabaseManager.GetInstance().LoadAlbumSearchItems(searchItems, genre);
                    break;
                case 5:
                    searchItems = DatabaseManager.GetInstance().LoadUserSearchItems(searchItems);
                    break;
            }

            return searchItems;
        }

        private List<SearchResultItemControl> FuzzyMatchingSearch(string keywords, List<SearchResultItemControl> searchItems)
        {
            List<SearchResultItemControl> matches = new List<SearchResultItemControl>();
            foreach (SearchResultItemControl item in searchItems)
            {
                // TODO
                string itemTitle = item.title.Name;

                // Allow for an edit distance of 2, without case sensitivity
                LevenshteinDistance fuzzyMatchAlgorithm = new LevenshteinDistance();
                if (fuzzyMatchAlgorithm.IsFuzzyMatch(keywords.ToLower(), itemTitle.ToLower(), 2))
                {
                    matches.Add(item);
                }
            }

            return matches;
        }

        private List<SearchResultItemControl> SortSearchResults(List<SearchResultItemControl> searchResults, int sorter, int filter)
        {
            List<SearchResultItemControl> sortedResults = new List<SearchResultItemControl>();
            // Determine the sorting algorithm to use and call it
            switch (sorter)
            {
                case 0:
                    // sortedResults = SortByRelevance(searchResults);
                    sortedResults = searchResults; // PROVISIONAL
                    break;
                case 1:
                    // sortedResults = SortByPopularity(searchResults);
                    sortedResults = searchResults; // PROVISIONAL
                    break;
                case 2:
                    if (filter == 0 || filter == 2 || filter == 5)
                    {
                        MessageBox.Show("Cannot sort artists or users by date. Please select either another sorter or filter", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        sortedResults = searchResults;
                    }
                    else
                    {
                        sortedResults = sortingModule.NumericalQuickSort(searchResults);
                    }
                    break;
                case 3:
                    sortedResults = sortingModule.AlphabeticalQuickSort(searchResults);
                    break;
            }
            return sortedResults;
        }

        // This is here since it correponds to the search function and not the interactions with the database,
        // but it is actually used in the database manager
        public SearchResultItemControl AddSearchResult(string imagePath, string title, string subTitle1 = "", string subTitle2 = "", string subTitle3 = "")
        {
            // Create a result item control
            SearchResultItemControl resultItem = new SearchResultItemControl();

            // Set fields
            resultItem.SetImage(imagePath);
            resultItem.SetTitle(title);
            resultItem.SetSubTitle1(subTitle1);
            resultItem.SetSubTitle2(subTitle2);
            resultItem.SetSubTitle3(subTitle3);

            return resultItem;
        }
    }
}
