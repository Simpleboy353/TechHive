using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TechHive.Base;
using TechHive.Admin;
using TechHive.Database;
using System;

namespace TechHive.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminDashboardPage : BaseContentPage
    {
        public AdminDashboardPage()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        private async void LoadDashboardData()
        {
            var users = await DatabaseHelper.GetAllUsersAsync();
            var products = await DatabaseHelper.GetAllProductsAsync();
            var orders = await DatabaseHelper.GetAllOrdersAsync();

            userCountLabel.Text = users?.Count.ToString() ?? "0";
            productCountLabel.Text = products?.Count.ToString() ?? "0";
            orderCountLabel.Text = orders?.Count.ToString() ?? "0";
        }

        private async void OnManageUsersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageUsersPage());
        }

        private async void OnManageProductsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageProductsPage());
        }

        private async void OnViewAllOrdersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewAllOrdersPage());
        }
        private async void OnViewMessagesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewMessagesPage());
        }
    }
}
