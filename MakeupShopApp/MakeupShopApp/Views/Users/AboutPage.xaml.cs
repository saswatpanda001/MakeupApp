using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MakeupShopApp.Views.Users;
using MakeupShopApp.Views.Others;


namespace MakeupShopApp.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }
       

        private void OnLoginClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Users.LoginPage());
        }

        private void OnSignUpClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AboutPage());
        }


        private void OnAboutClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Others.AboutUsPage());
        }

        private void OnContactClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Others.ContactUs());
        }
    }
}
