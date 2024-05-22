using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;
using MusicApp.DataTypes;
using MusicApp.Database.Models;

namespace MusicApp.Database
{
    public class DatabaseManagerAPI
    {
        private static DatabaseManagerAPI instance;
        private Search.SearchLogic searchLogic;
        private readonly HttpClient httpClient;

        private DatabaseManagerAPI()
        {
            searchLogic = new Search.SearchLogic();

            // Not ideal...
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;

            httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://localhost:5054") };
        }

        public static DatabaseManagerAPI GetInstance()
        {
            if (instance == null)
            {
                instance = new DatabaseManagerAPI();
            }
            return instance;
        }

        // AUTHENTICATION
        private async Task<int> ResolveUserID()
        {
            string username = Sesion.GetInstance().Username;
            var user = await httpClient.GetFromJsonAsync<Models.User>($"api/Users/{username}/credentials");
            return user?.UserID ?? -1;
        }

        public async Task BeginSession(string username)
        {
            await httpClient.PutAsync($"api/Users/{username}/begin-session", null);
        }

        public async Task EndSession(string username)
        {
            await httpClient.PutAsync($"api/Users/{username}/end-session", null);
        }

        public async Task<bool> RegisterUser(string username, string password, string salt)
        {
            var user = new Models.User
            {
                Username = username,
                HashedPassword = password,
                Salt = salt,
                Email = " ",
                DateJoined = DateTime.Today,
                IsActive = false,
                ProfilePicture = " ",
                SubscriptionPlan = " "
            };
            var response = await httpClient.PostAsJsonAsync("api/Users", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<(string, string)> GetCredentials(string username)
        {
            var response = await httpClient.GetAsync($"api/Users/{username}/credentials");
            if (response.IsSuccessStatusCode)
            {
                var credentials = await response.Content.ReadFromJsonAsync<(string HashedPassword, string Salt)>();
                return (credentials.HashedPassword, credentials.Salt);
            }
            else
            {
                return (" ", " ");
            }
        }
    }
}
