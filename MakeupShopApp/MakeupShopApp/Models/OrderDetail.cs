using System;
using System.Collections.Generic;
using System.Text;

namespace MakeupShopApp.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Optional: For displaying product details
        public Product Product { get; set; }
    }
}

