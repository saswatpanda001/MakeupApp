using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MakeupShopApp.Models;

namespace MakeupShopApp.Services
{
    public class UserService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "http://10.0.2.2:57793/api/Users"; // Adjust based on your API URL

        public UserService()
        {
            // Disable SSL verification for local development (Not for production)
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _client = new HttpClient(handler);
        }

        
        // ✅ Update User
        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"{baseUrl}/{user.UserId}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        // ✅ Get All Users
        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var response = await _client.GetStringAsync(baseUrl);
                return JsonConvert.DeserializeObject<List<User>>(response) ?? new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                return new List<User>();
            }
        }

        // ✅ Get User by ID
        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                var response = await _client.GetAsync($"{baseUrl}/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(json);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by ID: {ex.Message}");
                return null;
            }
        }


        // ✅ Create User
        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(baseUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
                return false;
            }
        }




        // ✅ Delete User
        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                var response = await _client.DeleteAsync($"{baseUrl}/{userId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return false;
            }
        }
    }
}
