
using System;
using System.Collections.Generic;
using System.Text;

namespace MakeupShopApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        // These will be used to store user-related data locally if needed
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Cart> Carts { get; set; } = new List<Cart>();
    }
}
