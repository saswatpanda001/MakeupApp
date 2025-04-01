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
    public partial class OrdersPage : ContentPage
    {

        private readonly OrderService _orderService = new OrderService();
        private int _userId = SessionManager.LoggedInUser.UserId; // Assuming SessionManager handles login

        public OrdersPage()
        {
            InitializeComponent();
            LoadOrders();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void LoadOrders()
        {

            var allOrders = await _orderService.GetAllOrdersAsync();            
            var userOrders = allOrders.Where(o => o.UserId == _userId).ToList();
            OrdersListView.ItemsSource = userOrders;

        }
        private async void OnHomeClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new UserDashboard());
        }


        private async void OnOrderSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Order selectedOrder)
            {
                await Navigation.PushAsync(new OrderDetailsPage(selectedOrder.OrderId));
            }
        }



       
     


    }
}