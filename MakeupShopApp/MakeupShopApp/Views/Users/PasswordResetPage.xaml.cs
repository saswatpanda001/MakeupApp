using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeupShopApp.Models;
using MakeupShopApp.Views.Users;
using MakeupShopApp.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MakeupShopApp.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordResetPage : ContentPage
    {


        private readonly UserService _userService;

        public PasswordResetPage()
        {
            InitializeComponent();
            _userService = new UserService();
            NavigationPage.SetHasBackButton(this, false);
        }

        // Handle Reset Password Click
        private async void OnResetPasswordClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();
            string email = EmailEntry.Text?.Trim();
            string newPassword = NewPasswordEntry.Text?.Trim();
            string confirmPassword = ConfirmPasswordEntry.Text?.Trim();

            // ✅ Validation
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Error", "All fields are required.", "OK");
                return;
            }

            if (newPassword != confirmPassword)
            {
                await DisplayAlert("Error", "New password and Confirm password do not match.", "OK");
                return;
            }

            try
            {
                // ✅ Check if User Exists
                var users = await _userService.GetAllUsersAsync();
                var user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                                                      u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    await DisplayAlert("Error", "User not found with the provided username and email.", "OK");
                    return;
                }

                // ✅ Update the Password
                user.Password = newPassword;
                bool isUpdated = await _userService.UpdateUserAsync(user);

                if (isUpdated)
                {
                    await DisplayAlert("Success", "Password has been reset successfully.", "OK");
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    await DisplayAlert("Error", "Failed to reset password. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        // Navigate to Login Page
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Users.LoginPage());
        }


    }
}