using Xamarin.Forms;

namespace TechHive.Base
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
            NavigationPage.SetHasBackButton(this, false);
            if (Device.RuntimePlatform == Device.Android)
            {
                Padding = new Thickness(0, 30, 0, 0); // Top padding for Android status bar
            }
        }
    }
}