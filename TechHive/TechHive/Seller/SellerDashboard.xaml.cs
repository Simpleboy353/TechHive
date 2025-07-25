using System;
using TechHive.Database;
using TechHive.Base;
using TechHive.Seller;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TechHive.Seller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SellerDashboardPage : BaseContentPage
    {
        public SellerDashboardPage()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        private async void LoadDashboardData()
        {
            int sellerId = AppSession.LoggedInSellerId;

            var products = await DatabaseHelper.GetProductsBySellerIdAsync(sellerId);
            var orders = await DatabaseHelper.GetOrdersBySellerIdAsync(sellerId);

            productCountLabel.Text = products?.Count.ToString() ?? "0";
            orderCountLabel.Text = orders?.Count.ToString() ?? "0";
        }

        private async void OnManageProductsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyProductsPage());
        }

        private async void OnViewOrdersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewOrdersPage());
        }
    }
}
