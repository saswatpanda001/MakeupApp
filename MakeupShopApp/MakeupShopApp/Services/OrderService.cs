using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MakeupShopApp.Models;

namespace MakeupShopApp.Services
{
    public class OrderService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "http://10.0.2.2:57793/api/Orders"; // Adjust as needed

        public OrderService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _client = new HttpClient(handler);
        }

        // ✅ Get All Orders
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            try
            {
                var response = await _client.GetStringAsync(baseUrl);
                return JsonConvert.DeserializeObject<List<Order>>(response) ?? new List<Order>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching orders: {ex.Message}");
                return new List<Order>();
            }
        }



        public async Task<int> CreateOrderAsync(Order order)
        {
            try
            {
                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(baseUrl, content);


                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    int orderId = JsonConvert.DeserializeObject<int>(responseContent); // Deserialize as an int
                    return orderId;
                }

                return 0; // Return 0 if the order creation failed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
                return 0; // Return 0 in case of an exception
            }
        }





      

        // ✅ Get Order by ID
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var response = await _client.GetAsync($"{baseUrl}/{orderId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Order>(json);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching order by ID: {ex.Message}");
                return null;
            }
        }

        // ✅ Create New Order
        
        // ✅ Update Order
        public async Task<bool> UpdateOrderAsync(Order order)
        {
            try
            {
                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"{baseUrl}/{order.OrderId}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating order: {ex.Message}");
                return false;
            }
        }

        // ✅ Delete Order
        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            try
            {
                var response = await _client.DeleteAsync($"{baseUrl}/{orderId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting order: {ex.Message}");
                return false;
            }
        }
    }
}
