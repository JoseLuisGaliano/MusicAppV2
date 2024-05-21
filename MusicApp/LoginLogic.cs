using System.Windows;
using MusicApp.Authentication;
using MusicApp.Database;

namespace MusicApp
{
    internal class LoginLogic
    {
        private AuthenticationModule authModule;

        public LoginLogic()
        {
            authModule = new AuthenticationModule();
        }

        public bool Login(string username, string password)
        {
            if (authModule.AuthenticateUser(username, password))
            {
                // Begin the sesion
                DatabaseManager.GetInstance().BeginSesion(username);

                // Create the sesion object for user info to be accessible
                Sesion sesion = Sesion.GetInstance(username);

                /* HOW DO WE DETERMINE THE END POINT OF THE PROGRAM??? PROBABLY FIXED BY DOING THE WINDOW FLOW
                // When the program ends, we update the sesion status in the database
                DatabaseManager.GetInstance().EndSesion(username);
                */

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SignIn(string username, string password)
        {
            return authModule.RegisterUser(username, password);
        }
    }
}
