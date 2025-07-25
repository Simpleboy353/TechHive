using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TechHive.Database;
using TechHive.Models;
using TechHive.Base;
using TechHive.Sidebars;

namespace TechHive.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SellerSignInPage : BaseContentPage
    {
        public SellerSignInPage()
        {
            InitializeComponent();
        }

        private async void OnSellerSignInClicked(object sender, EventArgs e)
        {
            var email = emailEntry.Text?.Trim().ToLower();
            var password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            var seller = await DatabaseHelper.GetSellerByEmailAsync(email);

            if (seller != null && seller.Password == password)
            {
                AppSession.LoggedInSellerId = seller.Id;
                Application.Current.MainPage = new SellerSidebarPage();
            }
            else
            {
                await DisplayAlert("Login Failed", "Invalid seller email or password.", "OK");
            }
        }

        private async void OnSellerSignUpTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SellerSignUpPage());
        }
    }
}
