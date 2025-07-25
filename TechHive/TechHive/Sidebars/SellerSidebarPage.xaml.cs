using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TechHive.Seller;
using TechHive.Pages;

namespace TechHive.Sidebars
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SellerSidebarPage : FlyoutPage
    {
        public SellerSidebarPage()
        {
            InitializeComponent();
        }

        private void OnDashboardClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new SellerDashboardPage());
            IsPresented = false;
        }

        private void OnAddProductClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new AddProductPage());
            IsPresented = false;
        }

        private void OnOrdersClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new ViewOrdersPage());
            IsPresented = false;
        }

        private void OnLogoutClicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new SignInPage());
        }
    }
}
