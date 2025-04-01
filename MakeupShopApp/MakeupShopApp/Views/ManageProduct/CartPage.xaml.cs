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
    public partial class CartPage : ContentPage
    {


        private readonly CartService _cartService = new CartService();
        private readonly ProductService _productService = new ProductService();
        public decimal totalprice_cart = 0;



        public CartPage()
        {
            InitializeComponent();
            LoadCartItems();
            NavigationPage.SetHasBackButton(this, false);
        }


        private async void OnHomeClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new UserDashboard());
        }


        private async void LoadCartItems()
        {
            if (SessionManager.LoggedInUser == null)
            {
                await DisplayAlert("Error", "Please log in to view your cart.", "OK");
                return;
            }

            try
            {
                var allCarts = await _cartService.GetAllCartsAsync();
                var userCarts = allCarts.Where(c => c.UserId == SessionManager.LoggedInUser.UserId).ToList();

                if (!userCarts.Any())
                {
                    TotalLabel.Text = "Your cart is empty.";
                    CartListView.ItemsSource = null;
                    return;
                }

                // Get product details
                foreach (var cart in userCarts)
                {
                    if (cart.Product == null)
                    {
                        cart.Product = await _productService.GetProductByIdAsync(cart.ProductId);
                    }
                }

                CartListView.ItemsSource = userCarts;
                TotalLabel.Text = $"Total: ₹{userCarts.Sum(c => c.Product.Price * c.Quantity):F2}";
                totalprice_cart = userCarts.Sum(c => c.Product.Price * c.Quantity);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load cart items: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int cartId)
            {
                bool confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this item?", "Yes", "No");

                if (!confirm) return;

                var success = await _cartService.DeleteCartAsync(cartId);
                if (success)
                {
                    await DisplayAlert("Success", "Item deleted from cart.", "OK");
                    LoadCartItems(); // Refresh cart
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete item.", "OK");
                }
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageProduct.ProductListPage());
        }

        private async void OnOrderClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageProduct.PaymentPage(totalprice_cart));
        }




    }
}