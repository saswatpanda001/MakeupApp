using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeupShopApp.Services;
using MakeupShopApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MakeupShopApp.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        private readonly UserService _userService;

        public LoginPage()
        {
            InitializeComponent();
            _userService = new UserService();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string usernameInput = UsernameEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(usernameInput) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            try
            {
                // Fetch all users
                var users = await _userService.GetAllUsersAsync();

                // Check if the username or email matches and password is correct
                var user = users.FirstOrDefault(u =>
                    (u.Username.Equals(usernameInput, StringComparison.OrdinalIgnoreCase) &&
                    u.Password == password));

                if (user != null)
                {
                    await DisplayAlert("Login Successful", $"Welcome {user.Username}!", "OK");
                    SessionManager.LoggedInUser = user;

                    if (SessionManager.LoggedInUser != null)
                    {
                        await DisplayAlert("Login Successful", $"Welcome {SessionManager.LoggedInUser.Username}!", "OK");
                        await Navigation.PushAsync(new UserDashboard()); // Navigate to AboutPage or Dashboard
                    }

                   
                }
                else
                {
                    await DisplayAlert("Login Failed", "Invalid email or password.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }


        // Navigate to Login Page
        private async void OnSignUpTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
        }

        // Navigate to Login Page
        private async void OnForgotPasswordTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PasswordResetPage());
        }

    }
}