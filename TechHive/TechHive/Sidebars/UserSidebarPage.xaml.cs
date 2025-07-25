using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TechHive.Pages;

namespace TechHive.Sidebars
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserSidebarPage : FlyoutPage
    {
        public UserSidebarPage()
        {
            InitializeComponent();
        }

        private void OnHomeClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new HomePage());
            IsPresented = false;
        }

        private void OnAboutClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new AboutUsPage());
            IsPresented = false;
        }

        private void OnCartClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new CartPage());
            IsPresented = false;
        }

        private void OnOrdersClicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new MyOrdersPage());
            IsPresented = false;
        }

        private void OnLogoutClicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new SignInPage());
        }
    }
}
