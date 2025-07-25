using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TechHive.Admin;
using TechHive.Pages;

namespace TechHive.Sidebars
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminSidebarPage : FlyoutPage
    {
        public AdminSidebarPage()
        {
            InitializeComponent();
        }

        private void OnDashboardClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new AdminDashboardPage());
            IsPresented = false;
        }

        private void OnManageProductsClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new ManageProductsPage());
            IsPresented = false;
        }

        private void OnViewOrdersClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new ViewAllOrdersPage());
            IsPresented = false;
        }

        private void OnViewMessagesClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new ViewMessagesPage());
            IsPresented = false;
        }

        private void OnLogoutClicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new SignInPage());
        }
    }
}
