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
    public partial class OrderDetailsPage : ContentPage
    {

        private readonly OrderDetailService _orderDetailService = new OrderDetailService();
        private readonly ProductService _productService = new ProductService();
        private int _orderId;

        public OrderDetailsPage(int orderId)
        {
            InitializeComponent();
            _orderId = orderId;
            LoadOrderDetails();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnHomeClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new OrdersPage());
        }

        private async void LoadOrderDetails()
        {
            var allOrderDetails = await _orderDetailService.GetAllOrderDetailsAsync();
            var orderDetails = allOrderDetails.Where(d => d.OrderId == _orderId).ToList();

            // Fetch product details and add to each order detail
            foreach (var detail in orderDetails)
            {
                var product = await _productService.GetProductByIdAsync(detail.ProductId);
                if (product != null)
                {
                    detail.Product = product; // Assuming Product is a property in OrderDetail
                }
            }

            OrderDetailsListView.ItemsSource = orderDetails;
        }

    }
}