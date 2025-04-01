using MakeupShopApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MakeupShopApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}