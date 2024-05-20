using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MusicApp.DataTypes;
using MusicApp.Search;

namespace MusicApp.Database
{
    public class DatabaseManager
    {
        // We will use the singleton design pattern to ensure only one instance of DatabaseManager exists
        private static DatabaseManager instance;
        private SearchLogic searchLogic;
        private const string ConnectionString = "Data Source=LAPTOP-85QOQ2U8;Initial Catalog = Search Item Database; Integrated Security = True";

        // Private constructor to prevent instantiation from outside
        private DatabaseManager()
        {
            searchLogic = new SearchLogic();
        }

        // Public static method to get the instance of Singleton class
        public static DatabaseManager GetInstance()
        {
            // Lazy initialization - instance is created only when needed
            if (instance == null)
            {
                instance = new DatabaseManager();
            }
            return instance;
        }

        // AUTHENTICATION
        public bool RegisterUser(string username, string password, string salt)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO [USER] (userName, profilePicture, subscriptionPlan, hashedPassword, salt) VALUES (@userName, @profilePicture, @subscriptionPlan, @hashedPassword, @salt);";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@userName", username);
                    command.Parameters.AddWithValue("@profilePicture", " ");
                    command.Parameters.AddWithValue("@subscriptionPlan", " ");
                    command.Parameters.AddWithValue("@hashedPassword", password);
                    command.Parameters.AddWithValue("@salt", salt);
                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while inserting values in the database: " + exception.Message);
                return false;
            }

            return true;
        }

        public (string, string) GetCredentials(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT hashedPassword, salt FROM [USER] WHERE userName = '" + username + "';";

                    SqlCommand command = new SqlCommand(query, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hashedPassword = reader.GetString(0);
                            string salt = reader.GetString(1);

                            return (hashedPassword, salt);
                        }
                        else
                        {
                            MessageBox.Show("No user found with the specified username.", "Warning", MessageBoxButton.OK);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while inserting values in the database: " + ex.Message);
            }

            return (" ", " ");
        }

        // SEARCH
        public List<Search.SearchResultItemControl> LoadUserSearchItems(List<Search.SearchResultItemControl> searchItems)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Specify query to execute
                    string query = "SELECT * FROM [USER];";

                    // Create a SqlDataAdapter to execute the query and retrieve data
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    // Fill a DataTable with the results of the query
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Convert each row into a search result item
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string image = row["profilePicture"].ToString();
                        string title = row["userName"].ToString();
                        string subtitle1 = "User";
                        searchItems.Add(searchLogic.AddSearchResult(image, title, subtitle1));
                    }

                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while retrieving items from the database: " + exception.Message);
            }
            return searchItems;
        }

        public List<Search.SearchResultItemControl> LoadArtistSearchItems(List<Search.SearchResultItemControl> searchItems)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Specify query to execute
                    string query = "SELECT * FROM ARTIST;";

                    // Create a SqlDataAdapter to execute the query and retrieve data
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    // Fill a DataTable with the results of the query
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Convert each row into a search result item
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string image = row["artistPicture"].ToString();
                        string title = row["artistName"].ToString();
                        string subtitle1 = "Artist";
                        searchItems.Add(searchLogic.AddSearchResult(image, title, subtitle1));
                    }

                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while retrieving items from the database: " + exception.Message);
            }
            return searchItems;
        }

        public List<Search.SearchResultItemControl> LoadSongSearchItems(List<Search.SearchResultItemControl> searchItems, string genreFilter = "")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Specify query to execute
                    string query;
                    if (genreFilter != " ")
                    {
                        query = "SELECT * FROM SONG WHERE album IN ( SELECT albumId FROM ALBUM WHERE genre = '" + genreFilter + "');";
                    }
                    else
                    {
                        query = "SELECT * FROM SONG;";
                    }

                    // Create a SqlDataAdapter to execute the query and retrieve data
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    // Fill a DataTable with the results of the query
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Convert each row into a search result item
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string title = row["songName"].ToString();

                        // Resolve the database references (foreign keys) to properly pass the data to AddSearchResult()
                        string getImageQuery = "SELECT albumPicture FROM ALBUM WHERE albumId = (SELECT album FROM SONG WHERE songName = '" + title + "');";
                        SqlCommand command = new SqlCommand(getImageQuery, connection);
                        string image = command.ExecuteScalar().ToString();

                        string getArtistQuery = "SELECT artistName FROM ARTIST WHERE artistId = ( SELECT albumArtist FROM ALBUM WHERE albumId = ( SELECT album FROM SONG WHERE songName = '" + title + "'));";
                        SqlCommand command1 = new SqlCommand(getArtistQuery, connection);
                        // Since we know the result of the select is a single element (one row and one column) we can use ExecuteScalar() to get that value
                        string subtitle1 = command1.ExecuteScalar().ToString();

                        string getDateQuery = "SELECT [date] FROM ALBUM WHERE albumId = (SELECT album FROM SONG WHERE songName = '" + title + "');";
                        SqlCommand command2 = new SqlCommand(getDateQuery, connection);
                        string subtitle2 = command2.ExecuteScalar().ToString();

                        string getGenreQuery = "SELECT genre FROM ALBUM WHERE albumId = (SELECT album FROM SONG WHERE songName = '" + title + "');";
                        SqlCommand command3 = new SqlCommand(getGenreQuery, connection);
                        string subtitle3 = command3.ExecuteScalar().ToString();

                        searchItems.Add(searchLogic.AddSearchResult(image, title, subtitle1, subtitle2, subtitle3));
                    }

                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while retrieving items from the database: " + exception.Message);
            }
            return searchItems;
        }

        public List<Search.SearchResultItemControl> LoadAlbumSearchItems(List<Search.SearchResultItemControl> searchItems, string genreFilter = "")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Specify query to execute
                    string query;
                    if (genreFilter != string.Empty)
                    {
                        query = "SELECT * FROM ALBUM WHERE genre = '" + genreFilter + "';";
                    }
                    else
                    {
                        query = "SELECT * FROM ALBUM;";
                    }

                    // Create a SqlDataAdapter to execute the query and retrieve data
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    // Fill a DataTable with the results of the query
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Convert each row into a search result item
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string image = row["albumPicture"].ToString();
                        string title = row["albumName"].ToString();
                        string subtitle2 = row["date"].ToString();
                        string subtitle3 = row["genre"].ToString();

                        // Resolve the database references (foreign keys) to properly pass the data to AddSearchResult()
                        string getArtistQuery = "SELECT artistName FROM ARTIST WHERE artistId = ( SELECT albumArtist FROM ALBUM WHERE albumName = '" + title + "');";
                        SqlCommand command = new SqlCommand(getArtistQuery, connection);
                        // Since we know the result of the select is a single element (one row and one column) we can use ExecuteScalar() to get that value
                        string subtitle1 = command.ExecuteScalar().ToString();

                        searchItems.Add(searchLogic.AddSearchResult(image, title, subtitle1, subtitle2, subtitle3));
                    }

                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while retrieving items from the database: " + exception.Message);
            }
            return searchItems;
        }

        public List<ArtistModel> LoadArtists()
        {
            List<ArtistModel> artists = new List<ArtistModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Build query
                    string query = "SELECT ArtistID, Name FROM Artists";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Execute query and build list
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        artists.Add(new ArtistModel
                        {
                            ArtistID = (int)reader["ArtistID"],
                            Name = reader["Name"].ToString()
                        });
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while loading artists: {ex.Message}");
            }

            return artists;
        }

        public List<EventModel> LoadArtistEvents(int artistId)
        {
            List<EventModel> events = new List<EventModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Build query
                    string query = $"SELECT Name, Location, DateTime FROM Events WHERE ArtistID = {artistId}";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Execute query and build list
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        events.Add(new EventModel
                        {
                            Name = reader["Name"].ToString(),
                            Location = reader["Location"].ToString(),
                            DateTime = Convert.ToDateTime(reader["DateTime"])
                        });
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while loading events: {ex.Message}");
            }

            return events;
        }

        public List<TicketType> GetTicketTypesForEvent(int eventId)
        {
            List<TicketType> ticketTypes = new List<TicketType>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Build query
                    string query = "SELECT DISTINCT TicketType, Price FROM Tickets WHERE EventID = @EventID";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Execute query and build list
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string type = reader["TicketType"].ToString();
                        decimal price = (decimal)reader["Price"];
                        ticketTypes.Add(new TicketType(type, price));
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while loading ticket types: {ex.Message}");
            }

            return ticketTypes;
        }

        // PLAYLIST/PLAYER
        public List<string> LoadPlaylist()
        {
            List<string> playlist = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Build query
                    string query = "SELECT FilePath FROM SongFiles";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Execute query and build list
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string songPath = reader.GetString(reader.GetOrdinal("FilePath"));
                        playlist.Add(songPath);
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while loading ticket types: {ex.Message}");
            }

            return playlist;
        }

        public List<Playlist> LoadAllPlaylists()
        {
            List<Playlist> playlists = new List<Playlist>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Build query
                    string query = "SELECT PlaylistID, Name, Description, CreationDate FROM Playlists";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Execute query and build list
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        playlists.Add(new Playlist
                        {
                            PlaylistID = Convert.ToInt32(reader["PlaylistID"]),
                            Name = reader["Name"].ToString(),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? string.Empty : reader["Description"].ToString(),
                            CreationDate = Convert.ToDateTime(reader["CreationDate"]),
                            IsLiked = false,  // Asumiendo estado inicial
                            IsFollowed = false// Asumiendo estado inicial
                        });
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while loading playlists: {ex.Message}");
            }

            return playlists;
        }

        public List<Playlist> SearchPlaylists(string searchQuery)
        {
            List<Playlist> playlists = new List<Playlist>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Build query
                    string query = "SELECT PlaylistID, Name, Description, CreationDate FROM Playlists WHERE LOWER(Name) LIKE @searchQuery";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@searchQuery", $"%{searchQuery}%");

                    // Execute query and build list
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        playlists.Add(new Playlist
                        {
                            PlaylistID = Convert.ToInt32(reader["PlaylistID"]),
                            Name = reader["Name"].ToString(),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? string.Empty : reader["Description"].ToString(),
                            CreationDate = Convert.ToDateTime(reader["CreationDate"]),
                            IsLiked = false,
                            IsFollowed = false
                        });
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while loading playlists: {ex.Message}");
            }

            return playlists;
        }

        public bool DeletePlaylist(string name)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Build query
                    string query = "DELETE FROM Playlists WHERE Name = @Name";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", name);

                    // Execute query
                    rowsAffected = command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while deleting a playlist: {ex.Message}");
            }

            return rowsAffected > 0;
        }

        public bool InsertPlaylist(string name, string description)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Build query
                    string query = "INSERT INTO Playlists (OwnerID, Name, Description, CreationDate) VALUES (@OwnerID, @Name, @Description, @CreationDate)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@OwnerID", 1); // 1 is an example owner
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@CreationDate", DateTime.Now);

                    // Execute query
                    rowsAffected = command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while adding a playlist: {ex.Message}");
            }

            return rowsAffected > 0;
        }

        public List<Song> LoadPlaylistSongs(int playlistID)
        {
            List<Song> songs = new List<Song>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Build query
                    string query = "SELECT s.Title, a.Name AS ArtistName FROM Songs s JOIN PlaylistSongs ps ON s.SongID = ps.SongID JOIN Artists a ON s.ArtistID = a.ArtistID WHERE ps.PlaylistID = @PlaylistID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PlaylistID", playlistID);

                    // Execute query and build list
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        songs.Add(new Song
                        {
                            SongTitle = reader["Title"].ToString(),
                            ArtistName = reader["ArtistName"].ToString()
                        });
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while loading songs: {ex.Message}");
            }

            return songs;
        }

        public bool InsertSongsIntoPlaylist(int playlistID, List<Song> songs)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Insert new songs into the playlist in the database
                    foreach (var song in songs)
                    {
                        string insertQuery = @"INSERT INTO PlaylistSongs (PlaylistID, SongID) 
                                               VALUES (@PlaylistID, (SELECT SongID FROM Songs WHERE Title = @SongTitle))";
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@PlaylistID", playlistID);
                            insertCommand.Parameters.AddWithValue("@SongTitle", song.SongTitle);
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    connection.Close();
                }

                MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool DeleteSongFromPlaylist(int playlistID, string songTitle)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Build query
                    string query = @"DELETE FROM PlaylistSongs WHERE PlaylistID = @PlaylistID AND SongID = (SELECT SongID FROM Songs WHERE Title = @SongTitle)";
                    SqlCommand deleteCommand = new SqlCommand(query, connection);
                    deleteCommand.Parameters.AddWithValue("@PlaylistID", playlistID);
                    deleteCommand.Parameters.AddWithValue("@SongTitle", songTitle);

                    // Execute query
                    deleteCommand.ExecuteNonQuery();

                    connection.Close();
                }

                MessageBox.Show("Song removed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing song: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // FEEDBACK
        public Song LoadSong(int songID)
        {
            Song loadedSong;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Build query
                    string query = @"
                    SELECT Songs.Title, Artists.Name, Albums.CoverArt
                    FROM Songs
                    INNER JOIN Artists ON Songs.ArtistID = Artists.ArtistID
                    INNER JOIN Albums ON Songs.AlbumID = Albums.AlbumID
                    WHERE Songs.SongID = @SongID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SongID", songID);

                    // Execute query
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Build Song object
                            loadedSong = new Song
                            {
                                SongTitle = reader.GetString(0),
                                ArtistName = reader.GetString(1),
                                CoverImage = reader.GetString(2)
                            };

                            return loadedSong;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading song details: " + ex.Message);
                    return null;
                }
            }
        }

        public List<string> LoadComments(int songID)
        {
            List<string> previousComments = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Build query
                    string query = "SELECT Comment, Username FROM Feedback INNER JOIN Users ON Feedback.UserID = Users.UserID WHERE SongID = @SongID ORDER BY DateAndTime DESC";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SongID", songID);

                    // Execute command and build list
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string comment = reader.GetString(0);
                            string username = reader.GetString(1);
                            previousComments.Add(username + ": " + comment);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading previous comments: " + ex.Message);
                }
            }

            return previousComments;
        }

        public bool InsertNewFeedback(int songID, int userRating, string comment)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Build query
                    string query = @"
                    INSERT INTO Feedback (UserID, SongID, Rating, Comment, DateAndTime)
                    VALUES (@UserID, @SongID, @Rating, @Comment, @DateAndTime)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserID", 1);
                    command.Parameters.AddWithValue("@SongID", songID);
                    command.Parameters.AddWithValue("@Rating", userRating);
                    command.Parameters.AddWithValue("@Comment", comment);
                    command.Parameters.AddWithValue("@DateAndTime", DateTime.Now);

                    // Execute query
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Your feedback has been submitted successfully.");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("It was not possible to submit your feedback.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error submitting feedback: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
