using System;
using System.IO;
using TechHive.Database;
using TechHive.Pages;
using TechHive.Services;
using Xamarin.Forms;

namespace TechHive
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.Get<IDatabaseCopier>()?.StartPeriodicCopy();

            MessagingCenter.Subscribe<Page>(this, "ApplyPadding", (page) =>
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    page.Padding = new Thickness(0, 30, 0, 0); // top padding for status bar
                }
            });

            // Set up the SignIn page first
            MainPage = new NavigationPage(new SignInPage())
            {
                BarBackgroundColor = Color.White,
                BarTextColor = Color.Black,
            };

            InitializeDatabase();
        }
        private async void InitializeDatabase()
        {
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "TechHive.db3"
            );

            await AppDatabase.InitializeAsync(dbPath);
        }
    }
}

