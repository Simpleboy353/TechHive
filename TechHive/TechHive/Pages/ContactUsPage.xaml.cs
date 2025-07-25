using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using TechHive.Database;

namespace TechHive.Pages
{
    public partial class ContactUsPage : ContentPage
    {
        public ContactUsPage()
        {
            InitializeComponent();
        }

        private async void OnSendClicked(object sender, EventArgs e)
        {
            string name = NameEntry.Text?.Trim();
            string email = EmailEntry.Text?.Trim();
            string message = MessageEditor.Text?.Trim();
            int userId = AppSession.LoggedInUserId; // You should store this after login

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            await DatabaseHelper.AddContactMessageAsync(userId, name, email, message);

            await DisplayAlert("Thank You!", "Your message has been sent.", "OK");

            // Optional: Clear inputs
            NameEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            MessageEditor.Text = string.Empty;
        }
    }
}
