using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MusicApp.DataTypes
{
    public class Song : INotifyPropertyChanged
    {
        private string title;
        private string artist;
        private string filePath;
        private string artistID;
        private string songID;
        private string coverImagePath;
        private int id;

        public event PropertyChangedEventHandler PropertyChanged;

        public string SongTitle
        {
            get => title;
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged(nameof(SongTitle));
                }
            }
        }

        public string ArtistName
        {
            get => artist;
            set
            {
                if (artist != value)
                {
                    artist = value;
                    OnPropertyChanged(nameof(ArtistName));
                }
            }
        }

        public string FilePath
        {
            get => filePath;
            set
            {
                if (filePath != value)
                {
                    filePath = value;
                    OnPropertyChanged(nameof(FilePath));
                }
            }
        }

        public string ArtistID
        {
            get => artistID;
            set
            {
                if (artistID != value)
                {
                    artistID = value;
                    OnPropertyChanged(nameof(ArtistID));
                }
            }
        }

        public string SongID
        {
            get => songID;
            set
            {
                if (songID != value)
                {
                    songID = value;
                    OnPropertyChanged(nameof(SongID));
                }
            }
        }

        public string CoverImage
        {
            get => coverImagePath;
            set
            {
                if (coverImagePath != value)
                {
                    coverImagePath = value;
                    OnPropertyChanged(nameof(CoverImage));
                }
            }
        }

        public int ID
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(ID));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
