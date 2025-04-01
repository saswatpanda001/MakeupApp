using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MakeupShopApp.Models;

namespace MakeupShopApp.Services
{
    public class ProductService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "http://10.0.2.2:57793/api/Products"; // Adjust port if needed

        public ProductService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _client = new HttpClient(handler);
        }

        // ✅ Get All Products
        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                var response = await _client.GetStringAsync(baseUrl);
                return JsonConvert.DeserializeObject<List<Product>>(response) ?? new List<Product>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Product>();
            }
        }

        // ✅ Get Product by ID
        public async Task<Product> GetProductByIdAsync(int productId)
        {
            try
            {
                var response = await _client.GetAsync($"{baseUrl}/{productId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Product>(json);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        // ✅ Create Product
        public async Task<bool> CreateProductAsync(Product product)
        {
            try
            {
                var json = JsonConvert.SerializeObject(product);
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

        // ✅ Update Product
        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                var json = JsonConvert.SerializeObject(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"{baseUrl}/{product.ProductId}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        // ✅ Delete Product
        public async Task<bool> DeleteProductAsync(int productId)
        {
            try
            {
                var response = await _client.DeleteAsync($"{baseUrl}/{productId}");
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
