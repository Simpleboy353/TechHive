using System;
using TechHive.Base;
using TechHive.Database;
using TechHive.Seller;
using TechHive.Sidebars;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TechHive.Pages
{
    public partial class SignInPage : BaseContentPage
    {
        public SignInPage()
        {
            InitializeComponent();
        }

        private async void OnSignInClicked(object sender, System.EventArgs e)
        {
            var email = emailEntry.Text?.Trim().ToLower();
            var password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            // Admin Logic
            if (email == "admin@gmail.com" && password == "admin123")
            {
                Application.Current.MainPage = new AdminSidebarPage();
                return;
            }

            var user = await DatabaseHelper.GetUserByEmailAsync(email);
            if (user != null && user.Password == password)
            {
                await DisplayAlert("Sucess", "Login Successfull", "OK");
                Application.Current.MainPage = new UserSidebarPage();
                return;
            }

            await DisplayAlert("Error", "Invalid email or password.", "OK");
        }

        private async void OnSignUpTapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        private async void OnSellerSignInTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SellerSignInPage());
        }

    }
}
