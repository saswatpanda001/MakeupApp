using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeupShopApp.Models;
using MakeupShopApp.Services;
using MakeupShopApp.Views.Users;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MakeupShopApp.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {

        private readonly UserService _userService;

        public SignupPage()
        {
            InitializeComponent();
            _userService = new UserService();
            NavigationPage.SetHasBackButton(this, false);
        }

        // Handle Signup Button Click
        private async void OnSignupClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();
            string email = EmailEntry.Text?.Trim();
            string phone = PhoneEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();
            string confirmPassword = ConfirmPasswordEntry.Text?.Trim();

            // ✅ Validation
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Error", "All fields are required.", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            try
            {
                // ✅ Check Email and Phone Uniqueness
                var existingUsers = await _userService.GetAllUsersAsync();
                if (existingUsers.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                {
                    await DisplayAlert("Error", "Email already exists. Please use another email.", "OK");
                    return;
                }

                if (existingUsers.Any(u => u.Phone == phone))
                {
                    await DisplayAlert("Error", "Phone number already exists.", "OK");
                    return;
                }

                if (existingUsers.Any(u => u.Username == username))
                {
                    await DisplayAlert("Error", "Username already exists.", "OK");
                    return;
                }


                // ✅ Create a New User
                var newUser = new User
                {
                    Username = username,
                    Email = email,
                    Phone = phone,
                    Password = password
                };

                bool isSuccess = await _userService.CreateUserAsync(newUser);

                if (isSuccess)
                {
                    await DisplayAlert("Success", "Account created successfully!", "OK");
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    await DisplayAlert("Error", "Failed to create account. Please try again.", "OK");
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