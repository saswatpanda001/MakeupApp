using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeupShopApp.Models;
using MakeupShopApp.Services;
using MakeupShopApp.Views.Users;
using MakeupShopApp.Views.ManageProduct;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MakeupShopApp.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDashboard : ContentPage
    {
        public string WelcomeMessage { get; set; }
        public UserDashboard()
        {
            InitializeComponent();

            if (SessionManager.LoggedInUser != null)
            {
                WelcomeMessage = $"Welcome, {SessionManager.LoggedInUser.Username}!";
            }
            else
            {
                WelcomeMessage = "Welcome!";
            }

            BindingContext = this;
            NavigationPage.SetHasBackButton(this, false);


        }

        private async void OnProductClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageProduct.ProductListPage());
        }
        



        private async void OnOrdersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrdersPage());


        }

        private async void OnCartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageProduct.CartPage());
        }


        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PasswordResetPage());
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            SessionManager.LoggedInUser = null;
            await Navigation.PushAsync(new AboutPage());
        }
    }
}