using Android.App;
using AndroidContentPM = Android.Content.PM;
using AndroidGraphics = Android.Graphics.Color;
using Android.OS;
using Android.Runtime;
using Android.Views;
using System;

namespace TechHive.Droid
{
    [Activity(Label = "TechHive", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = AndroidContentPM.ConfigChanges.ScreenSize | AndroidContentPM.ConfigChanges.Orientation | AndroidContentPM.ConfigChanges.UiMode | AndroidContentPM.ConfigChanges.ScreenLayout | AndroidContentPM.ConfigChanges.SmallestScreenSize )]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Plugin.Media.CrossMedia.Current.Initialize();
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R) // API 30+
            {
                Window.SetDecorFitsSystemWindows(false);
                Window.InsetsController?.Hide(WindowInsets.Type.StatusBars());
            }
            else if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.SetStatusBarColor(AndroidGraphics.Transparent);
            }

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] AndroidContentPM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}