

using System;
using System.Collections.Generic;

namespace MakeupShopApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public string PaymentMethod { get; set; }

        // Optional: Include order details for UI binding
        public List<OrderDetail> OrderDetails { get; set; }
    }
}

