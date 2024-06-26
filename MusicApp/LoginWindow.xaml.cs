﻿using System.Windows;
using System.Windows.Controls;
using MusicApp.Authentication;
using MusicApp.Database;

namespace MusicApp
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginLogic loginLogic;

        public LoginWindow()
        {
            loginLogic = new LoginLogic();
            InitializeComponent();
        }

        public void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            if (!loginLogic.Login(username, password))
            {
                MessageBox.Show("Error while logging in. Please try again", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MainWindow menu = new MainWindow();
                menu.Show();
                Close();
            }
        }

        // API NOT WORKING
        /*
        public async void SignInButtonClick(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            bool signInSuccess = await loginLogic.SignIn(username, password);
            if (signInSuccess)
            {
                MessageBox.Show("Sign In successful!", "Sign In", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Error while signing in. Please try again", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        */

        public void SignInButtonClick(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            bool signInSuccess = loginLogic.SignIn(username, password);
            if (signInSuccess)
            {
                MessageBox.Show("Sign In successful!", "Sign In", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Error while signing in. Please try again", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
