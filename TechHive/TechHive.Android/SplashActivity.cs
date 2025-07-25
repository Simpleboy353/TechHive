using Android.App;
using Android.Content;
using Android.OS;

namespace TechHive.Droid
{
    [Activity(Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Start the actual main activity (your Xamarin app)
            StartActivity(typeof(MainActivity));
        }
    }
}
