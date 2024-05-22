using System.Collections.Generic;
using MusicApp.Database;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MusicApp.Profile
{
    public class Profile
    {
        private int id;
        private string biography;
        private List<string> savedSongs;
        private List<string> playlists;
        private DatabaseManager dbManager;
        private string email;

        public Profile(int id)
        {
            this.id = id;
            this.savedSongs = new List<string>();
            this.playlists = new List<string>();
            this.dbManager = DatabaseManager.GetInstance();
            this.email = dbManager.GetEmail(id);
            this.biography = dbManager.GetBiography(id);
        }
        public string GetBiography()
        {
            return biography;
        }
        public void SetBiography(string bio)
        {
            biography = bio;
            dbManager.SetBiography(id, bio);
        }
        public string GetEmail()
        {
            return email;
        }
        public void SetEmail(string email)
        {
            this.email = email;
            dbManager.SetEmail(id, email);
        }
        public List<string> GetSavedSongs()
        {
            return savedSongs;
        }

        public void AddSavedSong(string song)
        {
            savedSongs.Add(song);
            dbManager.AddSavedSong(id, song);
        }

        public void RemoveSavedSong(string song)
        {
            savedSongs.Remove(song);
            dbManager.RemoveSavedSong(id, song);
        }

        public List<string> GetPlaylists()
        {
            return playlists;
        }

        public void AddPlaylist(string playlist)
        {
            playlists.Add(playlist);
            dbManager.AddPlaylist(id, playlist);
        }

        public void RemovePlaylist(string playlist)
        {
            playlists.Remove(playlist);
            dbManager.RemovePlaylist(id, playlist);
        }
    }
}