using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechHive.Base;
using TechHive.Database;
using TechHive.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TechHive.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : BaseContentPage
    {
        private List<CartItem> cartItems;

        public PaymentPage()
        {
            InitializeComponent();

            // Populate month and year pickers
            for (int month = 1; month <= 12; month++)
                expiryMonthPicker.Items.Add(month.ToString("D2"));

            int currentYear = DateTime.Now.Year;
            for (int year = currentYear; year <= currentYear + 10; year++)
                expiryYearPicker.Items.Add(year.ToString());

            LoadCartItems();
        }

        private async void LoadCartItems()
        {
            int userId = Preferences.Get("UserId", 0);
            cartItems = await DatabaseHelper.GetCartItemsAsync(userId);

            if (cartItems == null || cartItems.Count == 0)
            {
                await DisplayAlert("Empty Cart", "Your cart is empty. Redirecting...", "OK");
                await Navigation.PopAsync();
                return;
            }

            cartListView.ItemsSource = cartItems;

            double total = cartItems.Sum(item => item.Price * item.Quantity);
            double finalAmount = total + 0.13 * total;
            totalLabel.Text = $"Total: ${finalAmount:F2}";
        }

        private void CardNumberEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > 16)
                cardNumberEntry.Text = e.NewTextValue.Substring(0, 16);
        }

        private void CvvEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > 3)
                cvvEntry.Text = e.NewTextValue.Substring(0, 3);
        }
        private async void PayNow_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cardNameEntry.Text) ||
                string.IsNullOrWhiteSpace(cardNumberEntry.Text) ||
                cardNumberEntry.Text.Length != 16 ||
                expiryMonthPicker.SelectedIndex == -1 ||
                expiryYearPicker.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(cvvEntry.Text) ||
                cvvEntry.Text.Length != 3)
            {
                await DisplayAlert("Error", "Please fill all payment details correctly.", "OK");
                return;
            }

            int userId = Preferences.Get("UserId", 0);
            if (cartItems == null || cartItems.Count == 0)
            {
                await DisplayAlert("Error", "Cart is empty.", "OK");
                return;
            }

            var productIds = string.Join(",", cartItems.Select(item => item.ProductId));
            double total = cartItems.Sum(item => item.Price * item.Quantity);
            double tax = total * 0.13;
            double finalAmount = total + tax;
            totalLabel.Text = $"{finalAmount}";

            var newOrder = new Order
            {
                UserId = userId,
                ProductIds = productIds,
                Status = "Pending",
                PaymentMethod = "Credit Card",
                OrderDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            await DatabaseHelper.AddOrderAsync(newOrder);
            await DatabaseHelper.ClearCartAsync(userId);
            cartListView.ItemsSource = null;
            totalLabel.Text = "Total: $0.00";

            await DisplayAlert("Success", $"Payment of ${finalAmount:F2} received. Order placed!", "OK");
            await Navigation.PopToRootAsync();
        }
    }
}
