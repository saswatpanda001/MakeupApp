using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MakeupShopApp.Models;
using MakeupShopApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MakeupShopApp.Views.Users;

namespace MakeupShopApp.Views.ManageProduct
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductListPage : ContentPage
    {
        private readonly ProductService _productService = new ProductService();
        private List<Product> _allProducts;  // Stores all products for filtering
        public List<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }

        public ProductListPage()
        {
            InitializeComponent();
            LoadProducts();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserDashboard());
        }

        private async void LoadProducts()
        {
            try
            {
                _allProducts = await _productService.GetAllProductsAsync();
                Products = new List<Product>(_allProducts);
                ProductsCollectionView.ItemsSource = Products;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue.ToLower();

            // Filter products based on search text
            Products = string.IsNullOrWhiteSpace(searchText)
                ? new List<Product>(_allProducts)
                : _allProducts.Where(p => p.ProductName.ToLower().Contains(searchText)).ToList();

            ProductsCollectionView.ItemsSource = Products;
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
