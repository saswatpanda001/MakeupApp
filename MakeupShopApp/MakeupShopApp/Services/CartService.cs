using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MakeupShopApp.Models;

namespace MakeupShopApp.Services
{
    public class CartService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "http://10.0.2.2:57793/api/Carts"; // Adjust as needed

        public CartService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _client = new HttpClient(handler);
        }

        // ✅ Get All Carts
        public async Task<List<Cart>> GetAllCartsAsync()
        {
            try
            {
                var response = await _client.GetStringAsync(baseUrl);
                return JsonConvert.DeserializeObject<List<Cart>>(response) ?? new List<Cart>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Cart>();
            }
        }
        // ✅ Delete Cart
        public async Task<bool> DeleteCartAsync(int cartId)
        {
            try
            {
                var response = await _client.DeleteAsync($"{baseUrl}/{cartId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }





        // ✅ Add to Cart
        public async Task<bool> AddToCartAsync(Cart cart)
        {
            try
            {
                var json = JsonConvert.SerializeObject(cart);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(baseUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }



        // ✅ Get Cart by ID
        public async Task<Cart> GetCartByIdAsync(int cartId)
        {
            try
            {
                var response = await _client.GetAsync($"{baseUrl}/{cartId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Cart>(json);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        
        // ✅ Update Cart
        public async Task<bool> UpdateCartAsync(Cart cart)
        {
            try
            {
                var json = JsonConvert.SerializeObject(cart);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"{baseUrl}/{cart.CartId}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        
    }
}
