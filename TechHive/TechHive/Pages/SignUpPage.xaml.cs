using System;
using TechHive.Database;
using TechHive.Models;
using Xamarin.Forms;
using TechHive.Base;

namespace TechHive.Pages
{
    public partial class SignUpPage : BaseContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private async void OnUserSignUpClicked(object sender, EventArgs e)
        {
            string fullName = firstNameEntry.Text?.Trim();
            string lastName = lastNameEntry.Text?.Trim();
            string email = emailEntry.Text?.Trim().ToLower();
            string password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "All fields are required.", "OK");
                return;
            }

            // Check if user already exists
            var existingUser = await DatabaseHelper.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                await DisplayAlert("Error", "An account with this email already exists.", "OK");
                return;
            }

            // Register new user
            var newUser = new AppUser
            {
                FirstName = fullName,
                LastName = lastName,
                Email = email,
                Password = password
            };

            await DatabaseHelper.AddUserAsync(newUser);

            await DisplayAlert("Success", "Registration complete. Please sign in.", "OK");
            await Navigation.PopAsync(); // Back to SignInPage
        }

        private async void OnSellerSignUpTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SellerSignUpPage());
        }

    }
}
