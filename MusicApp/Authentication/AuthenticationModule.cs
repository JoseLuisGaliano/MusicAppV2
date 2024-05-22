using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MusicApp.Database;

namespace MusicApp.Authentication
{
    public class AuthenticationModule
    {
        public AuthenticationModule()
        {
        }

        // API NOT WORKING YET
        /*
        public async Task<bool> RegisterUser(string username, string password)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(password, salt);
            bool success = await DatabaseManagerAPI.GetInstance().RegisterUser(username, hashedPassword, salt);
            return success;
        }
        */

        public bool RegisterUser(string username, string password)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(password, salt);
            bool success = DatabaseManager.GetInstance().RegisterUser(username, hashedPassword, salt);
            return success;
        }

        public bool AuthenticateUser(string username, string password)
        {
            (string, string) credentials = DatabaseManager.GetInstance().GetCredentials(username);
            string hashedPassword = credentials.Item1;
            string salt = credentials.Item2;
            return hashedPassword == HashPassword(password, salt);
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] saltBytes = Convert.FromBase64String(salt);
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[saltBytes.Length + passwordBytes.Length];

                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                Buffer.BlockCopy(saltBytes, 0, saltedPassword, passwordBytes.Length, saltBytes.Length);

                byte[] hashedBytes = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
