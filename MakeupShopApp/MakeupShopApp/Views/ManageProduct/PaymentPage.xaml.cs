using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeupShopApp.Models;
using MakeupShopApp.Services;
using MakeupShopApp.Views.ManageProduct;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MakeupShopApp.Views.Users;

namespace MakeupShopApp.Views.ManageProduct
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {


        private readonly CartService _cartService = new CartService();
        private readonly ProductService _productService = new ProductService();
        private readonly OrderService _orderService = new OrderService();
        private readonly OrderDetailService _orderDetailService = new OrderDetailService();
        private decimal _totalPrice;

        public PaymentPage(decimal totalPrice)
        {
            InitializeComponent();
            _totalPrice = totalPrice;
            TotalLabel.Text = $"Total Amount: ${_totalPrice:F2}";
            NavigationPage.SetHasBackButton(this, false);
        }
        private async void OnHomeClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new UserDashboard());

        }

        private async void OnPlaceOrderClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
                string.IsNullOrWhiteSpace(AddressEditor.Text) ||
                string.IsNullOrWhiteSpace(PhoneEntry.Text) ||
                PaymentPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please fill all the fields and select a payment method.", "OK");
                return;
            }

            // Ensure user is logged in
            if (SessionManager.LoggedInUser == null)
            {
                await DisplayAlert("Error", "User not logged in. Please log in to continue.", "OK");
                return;
            }

            int userId = (int)SessionManager.LoggedInUser.UserId;
            


            // Fetch Cart Items
            var allCarts = await _cartService.GetAllCartsAsync();
            var userCart = allCarts.Where(c => c.UserId == userId).ToList();
            Console.WriteLine("success1");
            if (userCart.Count == 0)
            {
                await DisplayAlert("Error", "Your cart is empty.", "OK");
                return;
            }

            // Create Order
            var newOrder = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = _totalPrice,
                Status = "Pending",
                Name = NameEntry.Text,
                Address = AddressEditor.Text,
                Number = PhoneEntry.Text,
                PaymentMethod = PaymentPicker.SelectedItem.ToString(),


            };
            Console.WriteLine("success2");



            var orderId = await _orderService.CreateOrderAsync(newOrder);
            Console.WriteLine(orderId); 
            if (orderId == 0)
            {
                await DisplayAlert("Error", "Failed to create order.", "OK");
                return;
            }

            await DisplayAlert("Success", $"Order created successfully! Order ID: {orderId}", "OK");

   
            
            foreach (var cartItem in userCart)
            {
                var product = await _productService.GetProductByIdAsync(cartItem.ProductId);
                if (product == null)
                {
                    await DisplayAlert("Error", $"Product with ID {cartItem.ProductId} not found.", "OK");
                    return;
                }
                Console.WriteLine(orderId);
                Console.WriteLine(cartItem.ProductId);
                Console.WriteLine(cartItem.Quantity);
                Console.WriteLine(product.Price);

              


                var orderDetail = new OrderDetail
                {
                    OrderId = orderId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = product.Price
                };

                Console.WriteLine("success4");


                bool detailCreated = await _orderDetailService.CreateOrderDetailAsync(orderDetail);
                if (!detailCreated)
                {
                    await DisplayAlert("Error", "Failed to create order detail for a product.", "OK");
                    return;
                }
                product.Stock -= cartItem.Quantity;
                bool productQuantityUpdated = await _productService.UpdateProductAsync(product);
                Console.WriteLine(productQuantityUpdated);
                Console.WriteLine("success5");
            }

            foreach (var cartItem in userCart)
            {
                bool cartDeleted = await _cartService.DeleteCartAsync(cartItem.CartId);
                if (!cartDeleted)
                {
                    Console.WriteLine($"Failed to delete cart item with CartId: {cartItem.CartId}");
                }
            }

            Console.WriteLine("success6");








            await DisplayAlert("Success", $"Order placed successfully using {PaymentPicker.SelectedItem}!\nTotal Amount: ${_totalPrice:F2}", "OK");

            // Navigate back to the homepage or order summary
            await Navigation.PushAsync(new OrdersPage());
        }




        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartPage());
        }
    }
}