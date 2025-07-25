using System;
using TechHive.Base;
using Xamarin.Forms;

namespace TechHive.Pages
{
    public partial class AboutUsPage : BaseContentPage
    {
        public AboutUsPage()
        {
            InitializeComponent();
        }

        private async void OnContactUsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactUsPage());
        }
    }
}
