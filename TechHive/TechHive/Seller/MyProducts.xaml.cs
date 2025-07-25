using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TechHive.Models;
using TechHive.Database;
using TechHive.Base;
using System.Threading.Tasks;

namespace TechHive.Seller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyProductsPage : BaseContentPage
    {

        private int _loggedInSellerId = AppSession.LoggedInSellerId;

        public MyProductsPage()
        {
            InitializeComponent();
            LoadSellerProducts();
        }

        private async void LoadSellerProducts()
        {
            List<Product> products = await DatabaseHelper.GetProductsBySellerIdAsync(_loggedInSellerId);
            productsCollectionView.ItemsSource = products;
        }

        private async void OnDeleteClicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var product = button?.CommandParameter as Product;

            if (product == null) return;

            bool confirm = await DisplayAlert("Confirm Delete", $"Delete \"{product.Name}\"?", "Yes", "No");
            if (confirm)
            {
                await DatabaseHelper.DeleteProductAsync(product);
                await DisplayAlert("Deleted", "Product has been deleted.", "OK");
                LoadSellerProducts(); // refresh the list
            }
        }

        private async void OnEditClicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var product = button?.CommandParameter as Product;

            if (product != null)
            {
                // Navigate to a new EditProductPage (you’ll need to create this)
                await Navigation.PushAsync(new EditProductPage(product));
            }
        }
    }
}
