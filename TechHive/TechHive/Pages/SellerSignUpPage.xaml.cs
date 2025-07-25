using System;
using System.Runtime.CompilerServices;
using TechHive.Models;
using TechHive.Database;
using Xamarin.Forms;
using TechHive.Base;

namespace TechHive.Pages
{
    public partial class SellerSignUpPage : BaseContentPage
    {
        public SellerSignUpPage()
        {
            InitializeComponent();
        }

        private async void OnSellerSignUpClicked(object sender, EventArgs e)
        {
            var name = ownerNameEntry.Text?.Trim();
            var email = emailEntry.Text?.Trim();
            var password = passwordEntry.Text;
            var businessName = businessNameEntry.Text?.Trim();
            var location = locationEntry.Text?.Trim();
            var description = descriptionEditor.Text?.Trim();

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(businessName) ||
                string.IsNullOrWhiteSpace(location) ||
                string.IsNullOrWhiteSpace(description))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            var existingSeller = await DatabaseHelper.GetSellerByEmailAsync(email);
            if (existingSeller != null)
            {
                await DisplayAlert("Error", "An account with this email already exists.", "OK");
                return;
            }

            var newSeller = new AppSeller
            {
                Name = name,
                Email = email,
                Password = password,
                BusinessName = businessName,
                Location = location,
                Description = description
            };

            await DatabaseHelper.AddSellerAsync(newSeller);
            await DisplayAlert("Success", "Seller registered successfully!", "OK");

            // Redirect to SignInPage
            await Navigation.PushAsync(new SignInPage());
        }

        private async void OnGoToSignInTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SellerSignInPage());
        }
    }
}
