using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TechHive.Base;
using TechHive.Database;
using TechHive.Models;

namespace TechHive.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : BaseContentPage
    {
        private List<CartItem> CartItems = new List<CartItem>();

        public CartPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadCartItems();
        }

        private async void LoadCartItems()
        {
            int userId = Preferences.Get("UserId", 0);
            CartItems = await DatabaseHelper.GetCartItemsAsync(userId);
            CartItemsCollectionView.ItemsSource = CartItems;

            double total = CartItems.Sum(c => c.Price * c.Quantity);
            double tax = 0.13 * total;
            double finalAmount = total + tax;
            TotalPriceLabel.Text = $"Total: ${total:F2}";
            TaxLabel.Text = $"Tax: ${tax:F2}";
            FinalPriceLabel.Text = $"Final Amount: ${finalAmount:F2}";
        }

        private async void IncreaseQuantity_Clicked(object sender, EventArgs e)
        {
            if ((sender as Button)?.CommandParameter is CartItem item)
            {
                var product = new Product
                {
                    Id = item.ProductId,
                    Name = item.Name,
                    Price = item.Price,
                    ImagePath = item.Image
                };

                int newQuantity = item.Quantity + 1;
                await DatabaseHelper.AddOrUpdateCartItemAsync(item.UserId, product, newQuantity);
                LoadCartItems(); // Always reload from DB
            }
        }

        private async void DecreaseQuantity_Clicked(object sender, EventArgs e)
        {
            if ((sender as Button)?.CommandParameter is CartItem item && item.Quantity > 1)
            {
                var product = new Product
                {
                    Id = item.ProductId,
                    Name = item.Name,
                    Price = item.Price,
                    ImagePath = item.Image
                };

                int newQuantity = item.Quantity - 1;
                await DatabaseHelper.AddOrUpdateCartItemAsync(item.UserId, product, newQuantity);
                LoadCartItems(); // Always reload from DB to avoid incorrect bindings
            }
        }

        private async void RemoveItem_Clicked(object sender, EventArgs e)
        {
            if ((sender as Button)?.CommandParameter is CartItem item)
            {
                await DatabaseHelper.RemoveCartItemAsync(item);
                LoadCartItems();
            }
        }

        private async void ContinueShopping_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage());
        }

        private async void ProceedToPayment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaymentPage());
        }
    }
}
