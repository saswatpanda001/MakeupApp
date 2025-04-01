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
using MakeupShopApp.Views.Users;


namespace MakeupShopApp.Views.ManageProduct
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductListPage : ContentPage
    {


        private readonly ProductService _productService = new ProductService();
        public List<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }

        public ProductListPage()
        {
            InitializeComponent();
            LoadProducts();
            NavigationPage.SetHasBackButton(this, false);
        }
        private async void OnHomeClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new UserDashboard());
        }


        private async void LoadProducts()
        {
            try
            {
                Products = await _productService.GetAllProductsAsync();
                ProductsCollectionView.ItemsSource = Products;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnProductSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Product selectedProduct)
            {
                await Navigation.PushAsync(new ProductDetailPage(selectedProduct));
                ((CollectionView)sender).SelectedItem = null; // Clear selection
            }
        }

    }
}