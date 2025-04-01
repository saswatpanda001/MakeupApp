using System;
using System.Collections.Generic;
using System.Text;


namespace MakeupShopApp.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Optional: Include Product details for displaying in the UI
        public Product Product { get; set; }
        public User User { get; set; }

    }
}

