using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MakeupShopApp.Models;

namespace MakeupShopApp.Services
{
    public class OrderDetailService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "http://10.0.2.2:57793/api/OrderDetails"; // Adjust URL as needed

        public OrderDetailService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _client = new HttpClient(handler);
        }

        public async Task<List<OrderDetail>> GetAllOrderDetailsAsync()
        {
            try
            {
                var response = await _client.GetStringAsync(baseUrl);
                return JsonConvert.DeserializeObject<List<OrderDetail>>(response) ?? new List<OrderDetail>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching order details: {ex.Message}");
                return new List<OrderDetail>();
            }
        }




        // ✅ Create a New Order Detail
        public async Task<bool> CreateOrderDetailAsync(OrderDetail orderDetail)
        {
            try
            {
                var json = JsonConvert.SerializeObject(orderDetail);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(baseUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order detail: {ex.Message}");
                return false;
            }
        }



       

        // ✅ Get Order Detail by ID
        public async Task<OrderDetail> GetOrderDetailByIdAsync(int orderDetailId)
        {
            try
            {
                var response = await _client.GetAsync($"{baseUrl}/{orderDetailId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<OrderDetail>(json);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching order detail by ID: {ex.Message}");
                return null;
            }
        }

        

        // ✅ Update Order Detail
        public async Task<bool> UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            try
            {
                var json = JsonConvert.SerializeObject(orderDetail);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"{baseUrl}/{orderDetail.OrderDetailId}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating order detail: {ex.Message}");
                return false;
            }
        }

        // ✅ Delete Order Detail
        public async Task<bool> DeleteOrderDetailAsync(int orderDetailId)
        {
            try
            {
                var response = await _client.DeleteAsync($"{baseUrl}/{orderDetailId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting order detail: {ex.Message}");
                return false;
            }
        }
    }
}
