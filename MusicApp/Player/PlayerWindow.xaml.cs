using System.Windows;
using MusicApp.DataTypes;

namespace MusicApp.Player
{
	public partial class PlayerWindow : Window
	{
		private PlayerLogic player;

        public PlayerWindow(string songPath)
        {
            InitializeComponent();

			// Create logic object for this player instance
			player = new PlayerLogic();

			// Check if reproduction can begin or not
            player.StartReproduction(songPath);
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs eventArgs)
		{
			player.GoToPreviousSong();
		}

		// Manejador de eventos para el botón "Play"
		private void PlayButton_Click(object sender, RoutedEventArgs eventArgs)
		{
			player.PlaySong();

			// Update GUI accordingly
			if (PlayButton.Content == "Play")
			{
				PlayButton.Content = "Pause";
			}
			else
			{
				PlayButton.Content = "Play";
			}
		}

		// Manejador de eventos para el botón "Next" (Siguiente)
		private void NextButton_Click(object sender, RoutedEventArgs eventArgs)
		{
			player.GoToNextSong();
		}

		private void QueueButton_Click(object sender, RoutedEventArgs eventArgs)
		{
			Playlists.QueueWindow queueWindow = new Playlists.QueueWindow();
			queueWindow.Show();
		}

		private void DownloadButton_Click(object sender, RoutedEventArgs eventArgs)
		{
            var selectedSong = QueueListBox.SelectedItem as Song;
			player.DownloadSong(selectedSong);
		}
	}
}