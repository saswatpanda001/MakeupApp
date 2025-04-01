using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeupShopApp.Models;
using MakeupShopApp.Services;
using MakeupShopApp.Views.ManageProduct;
using MakeupShopApp.Views.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MakeupShopApp.Views.ManageProduct
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailPage : ContentPage
    {

        private readonly Product _product;
        private readonly CartService _cartService = new CartService();

        public ProductDetailPage(Product product)
        {
            InitializeComponent();
            _product = product;
            BindingContext = _product;
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnHomeClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new UserDashboard());
        }



        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            if (SessionManager.LoggedInUser == null)
            {
                await DisplayAlert("Error", "Please log in to add products to your cart.", "OK");
                return;
            }

            if (!int.TryParse(QuantityEntry.Text, out int quantity) || quantity <= 0)
            {
                await DisplayAlert("Invalid Quantity", "Please enter a valid quantity.", "OK");
                return;
            }

            if (quantity > _product.Stock)
            {
                await DisplayAlert("Insufficient Stock", $"Only {_product.Stock} items available.", "OK");
                return;
            }

            var cartItem = new Cart
            {
                UserId = SessionManager.LoggedInUser.UserId,
                ProductId = _product.ProductId,
                Quantity = quantity,
              
            };

            bool isAdded = await _cartService.AddToCartAsync(cartItem);

            if (isAdded)
            {
                await DisplayAlert("Success", $"{_product.ProductName} added to your cart.", "OK");
                await Navigation.PushAsync(new CartPage());
            }
            else
            {
                await DisplayAlert("Error", "Failed to add product to cart. Please try again.", "OK");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ManageProduct.ProductListPage());
        }

        private async void OnCartClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ManageProduct.CartPage());
        }

      



    }
}