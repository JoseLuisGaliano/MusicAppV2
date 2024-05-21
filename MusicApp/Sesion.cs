namespace MusicApp
{
    // Single object for the whole program that allows for any part of the program to access
    // information about the user without having to pass it through parameters all over the place
    public class Sesion
    {
        private static Sesion instance;
        public string Username { get; }

        // Private constructor to prevent instantiation from outside
        private Sesion(string username)
        {
            Username = username;
        }

        // Public static method to get the instance of Singleton class
        public static Sesion GetInstance(string username = "")
        {
            // Lazy initialization - instance is created only when needed
            if (instance == null)
            {
                instance = new Sesion(username);
            }
            return instance;
        }
    }
}
