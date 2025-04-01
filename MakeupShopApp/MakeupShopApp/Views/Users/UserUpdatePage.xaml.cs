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
    public partial class UserUpdatePage : ContentPage
    {

        private readonly UserService _userService = new UserService();
        public User User { get; set; }

        public UserUpdatePage()
        {
            InitializeComponent();

            if (SessionManager.LoggedInUser != null)
            {
                User = new User
                {
                    UserId = SessionManager.LoggedInUser.UserId,
                    Username = SessionManager.LoggedInUser.Username,
                    Email = SessionManager.LoggedInUser.Email,
                    Phone = SessionManager.LoggedInUser.Phone,
                    Address = SessionManager.LoggedInUser.Address
                };
                BindingContext = this;
            }
            else
            {
                DisplayAlert("Error", "User not found. Please log in again.", "OK");
                Navigation.PopAsync();
            }
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            bool isConfirmed = await DisplayAlert("Confirm", "Do you want to update your profile?", "Yes", "No");
            if (!isConfirmed)
                return;

            bool isUpdated = await _userService.UpdateUserAsync(User);

            if (isUpdated)
            {
                SessionManager.LoggedInUser.Email = User.Email;
                SessionManager.LoggedInUser.Phone = User.Phone;
                SessionManager.LoggedInUser.Address = User.Address;

                await DisplayAlert("Success", "Profile updated successfully!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to update profile. Please try again.", "OK");
            }
        }
    }
}