using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MusicApp.DataTypes;

namespace MusicApp.Database
{
    public class DatabaseManager
    {
        // We will use the singleton design pattern to ensure only one instance of DatabaseManager exists
        private static DatabaseManager instance;
        private Search.SearchLogic searchLogic;
        private const string ConnectionString = "Data Source=LAPTOP-85QOQ2U8;Initial Catalog = MusicAppDB; Integrated Security = True";

        // Private constructor to prevent instantiation from outside
        private DatabaseManager()
        {
            searchLogic = new Search.SearchLogic();
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
        private int ResolveUserID()
        {
            string username = Sesion.GetInstance().Username;
            int id = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Build query
                    string query = "SELECT userID FROM [USER] WHERE username = " + username + ";";

                    // Execute query
                    SqlCommand command = new SqlCommand(query, connection);
                    // Since we know the result of the select is a single element (one row and one column) we can use ExecuteScalar() to get that value
                    id = (int)command.ExecuteScalar(); // We know it is an int

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while retrieving userID from the database: " + ex.Message);
            }

            return id;
        }

        public void BeginSesion(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Build query
                    string query = "UPDATE [USER] SET isActive = 1 WHERE username = " + username + ";";

                    // Execute query
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Warning: user sesion not properly updated in the database. Error while beginning user sesion: " + ex.Message);
            }
        }

        public void EndSesion(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Build query
                    string query = "UPDATE [USER] SET isActive = 0 WHERE username = " + username + ";";

                    // Execute query
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Warning: user sesion not properly updated in the database. Error while ending user sesion: " + ex.Message);
            }
        }

        public bool RegisterUser(string username, string password, string salt)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO [USER] (username, email, dateJoined, isActive, profilePicture, subscriptionPlan, hashedPassword, salt) VALUES (@username, @email, @dateJoined, @isActive, @profilePicture, @subscriptionPlan, @hashedPassword, @salt);";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@email", " ");
                    command.Parameters.AddWithValue("@dateJoined", DateTime.Today);
                    command.Parameters.AddWithValue("@isActive", 0);
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
                        string title = row["username"].ToString();
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
                        query = "SELECT * FROM SONG WHERE albumID IN ( SELECT albumID FROM ALBUM WHERE albumGenre = '" + genreFilter + "');";
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
                        string title = row["songTitle"].ToString();

                        // Resolve the database references (foreign keys) to properly pass the data to AddSearchResult()
                        string getImageQuery = "SELECT albumPicture FROM ALBUM WHERE albumID = (SELECT albumID FROM SONG WHERE songTitle = '" + title + "');";
                        SqlCommand command = new SqlCommand(getImageQuery, connection);
                        string image = command.ExecuteScalar().ToString();

                        string getArtistQuery = "SELECT artistName FROM ARTIST WHERE artistID = ( SELECT albumArtist FROM ALBUM WHERE albumID = ( SELECT albumID FROM SONG WHERE songTitle = '" + title + "'));";
                        SqlCommand command1 = new SqlCommand(getArtistQuery, connection);
                        // Since we know the result of the select is a single element (one row and one column) we can use ExecuteScalar() to get that value
                        string subtitle1 = command1.ExecuteScalar().ToString();

                        string getDateQuery = "SELECT albumReleaseDate FROM ALBUM WHERE albumID = (SELECT albumID FROM SONG WHERE songTitle = '" + title + "');";
                        SqlCommand command2 = new SqlCommand(getDateQuery, connection);
                        string subtitle2 = command2.ExecuteScalar().ToString();

                        string getGenreQuery = "SELECT albumGenre FROM ALBUM WHERE albumID = (SELECT albumID FROM SONG WHERE songTitle = '" + title + "');";
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
                        query = "SELECT * FROM ALBUM WHERE albumGenre = '" + genreFilter + "';";
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
                        string title = row["albumTitle"].ToString();
                        string subtitle2 = row["albumReleaseDate"].ToString();
                        string subtitle3 = row["albumGenre"].ToString();

                        // Resolve the database references (foreign keys) to properly pass the data to AddSearchResult()
                        string getArtistQuery = "SELECT artistName FROM ARTIST WHERE artistID = ( SELECT albumArtist FROM ALBUM WHERE albumTitle = '" + title + "');";
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
                    string query = "SELECT artistID, Name FROM ARTIST";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Execute query and build list
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        artists.Add(new ArtistModel
                        {
                            ArtistID = (int)reader["ArtistID"],
                            Name = reader["artistName"].ToString()
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
                    string query = $"SELECT eventName, eventLocation, eventDate FROM [EVENT] WHERE artistID = {artistId}";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Execute query and build list
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        events.Add(new EventModel
                        {
                            Name = reader["eventName"].ToString(),
                            Location = reader["eventLocation"].ToString(),
                            DateTime = Convert.ToDateTime(reader["eventDate"])
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
                    string query = "SELECT playlistID, playlistName, playlistDescription, playlistCreationDate FROM PLAYLIST";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Execute query and build list
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        playlists.Add(new Playlist
                        {
                            PlaylistID = Convert.ToInt32(reader["playlistID"]),
                            Name = reader["playlistName"].ToString(),
                            Description = reader.IsDBNull(reader.GetOrdinal("playlistDescription")) ? string.Empty : reader["playlistDescription"].ToString(),
                            CreationDate = Convert.ToDateTime(reader["playlistCreationDate"]),
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
                    string query = "SELECT playlistID, playlistName, playlistDescription, playlistCreationDate FROM PLAYLIST WHERE LOWER(playlistName) LIKE @searchQuery";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@searchQuery", $"%{searchQuery}%");

                    // Execute query and build list
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        playlists.Add(new Playlist
                        {
                            PlaylistID = Convert.ToInt32(reader["playlistID"]),
                            Name = reader["Name"].ToString(),
                            Description = reader.IsDBNull(reader.GetOrdinal("playlistDescription")) ? string.Empty : reader["playlistDescription"].ToString(),
                            CreationDate = Convert.ToDateTime(reader["playlistCreationDate"]),
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
                    string query = "DELETE FROM PLAYLIST WHERE playlistName = @playlistName";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@playlistName", name);

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
                    string query = "INSERT INTO Playlists (ownerID, playlistName, playlistDescription, playlistCreationDate) VALUES (@ownerID, @playlistName, @playlistDescription, @playlistCreationDate)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ownerID", ResolveUserID());
                    command.Parameters.AddWithValue("@playlistName", name);
                    command.Parameters.AddWithValue("@playlistDescription", description);
                    command.Parameters.AddWithValue("@playlistCreationDate", DateTime.Today);

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
                    string query = "SELECT songTitle, a.artistName AS artistName FROM SONG s JOIN PLAYLISTSONG ps ON s.songID = ps.songID JOIN ARTIST a ON s.artistID = a.artistID WHERE ps.playlistID = @playlistID;";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@playlistID", playlistID);

                    // Execute query and build list
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        songs.Add(new Song
                        {
                            SongTitle = reader["songTitle"].ToString(),
                            ArtistName = reader["artistName"].ToString()
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
                        string insertQuery = @"INSERT INTO PLAYLISTSONG (playlistID, songID) 
                                               VALUES (@playlistID, (SELECT songID FROM SONG WHERE songTitle = @songTitle))";
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@playlistID", playlistID);
                            insertCommand.Parameters.AddWithValue("@songTitle", song.SongTitle);
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
                    string query = @"DELETE FROM playlistSongs WHERE playlistID = @playlistID AND songID = (SELECT songID FROM SONG WHERE songTitle = @songTitle)";
                    SqlCommand deleteCommand = new SqlCommand(query, connection);
                    deleteCommand.Parameters.AddWithValue("@playlistID", playlistID);
                    deleteCommand.Parameters.AddWithValue("@songTitle", songTitle);

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
                    SELECT SONG.songTitle, ARTIST.artistName, ALBUM.albumPicture
                    FROM SONG
                    INNER JOIN ARTIST ON SONG.artistID = ARTIST.artistID
                    INNER JOIN ALBUM ON SONG.albumID = ALBUM.albumID
                    WHERE SONG.songID = @songID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@songID", songID);

                    // Execute query
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Build Song object
                            loadedSong = new Song
                            {
                                SongTitle = reader["songTitle"].ToString(),
                                ArtistName = reader["artistName"].ToString(),
                                CoverImage = reader["albumPicture"].ToString()
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
                    string query = "SELECT Comment, Username FROM FEEDBACK INNER JOIN [USER] ON FEEDBACK.userID = [USER].userID WHERE songID = @songID ORDER BY feedbackDate DESC";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@songID", songID);

                    // Execute command and build list
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string comment = reader["Comment"].ToString();
                            string username = reader["Username"].ToString();
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
                    INSERT INTO FEEDBACK (userID, songID, rating, Comment, feedbackDate)
                    VALUES (@userID, @songID, @rating, @Comment, @feedbackDate)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@userID", ResolveUserID());
                    command.Parameters.AddWithValue("@songID", songID);
                    command.Parameters.AddWithValue("@rating", userRating);
                    command.Parameters.AddWithValue("@Comment", comment);
                    command.Parameters.AddWithValue("@feedbackDate", DateTime.Now);

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
