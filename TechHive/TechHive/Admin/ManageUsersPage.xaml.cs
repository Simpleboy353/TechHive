using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TechHive.Models;
using TechHive.Database;
using TechHive.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TechHive.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageUsersPage : BaseContentPage
    {
        private List<UnifiedUserViewModel> allUsers;

        public ManageUsersPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            var users = await DatabaseHelper.GetAllUsersAsync();
            var sellers = await DatabaseHelper.GetAllSellersAsync();

            allUsers = new List<UnifiedUserViewModel>();

            allUsers.AddRange(users.Select(u => new UnifiedUserViewModel
            {
                Id = u.Id,
                DisplayName = $"{u.FirstName} {u.LastName}",
                Email = u.Email,
                Role = "User"
            }));

            allUsers.AddRange(sellers.Select(s => new UnifiedUserViewModel
            {
                Id = s.Id,
                DisplayName = $"{s.Name} ({s.BusinessName})",
                Email = s.Email,
                Role = "Seller"
            }));

            usersCollectionView.ItemsSource = allUsers;
        }

        private async void OnDeleteClicked(object sender, System.EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is UnifiedUserViewModel user)
            {
                bool confirm = await DisplayAlert("Delete", $"Delete {user.Role} '{user.DisplayName}'?", "Yes", "No");
                if (confirm)
                {
                    if (user.Role == "User")
                    {
                        var entity = await AppDatabase.Database.FindAsync<AppUser>(user.Id);
                        if (entity != null)
                            await AppDatabase.Database.DeleteAsync(entity);
                    }
                    else if (user.Role == "Seller")
                    {
                        var entity = await AppDatabase.Database.FindAsync<AppSeller>(user.Id);
                        if (entity != null)
                            await AppDatabase.Database.DeleteAsync(entity);
                    }

                    await LoadUsers();
                }
            }
        }

        private async void OnEditClicked(object sender, System.EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is UnifiedUserViewModel user)
            {
                string newEmail = await DisplayPromptAsync("Edit", "Email:", initialValue: user.Email);
                if (string.IsNullOrWhiteSpace(newEmail))
                {
                    await DisplayAlert("Invalid", "Email cannot be empty.", "OK");
                    return;
                }

                if (user.Role == "User")
                {
                    var u = await AppDatabase.Database.FindAsync<AppUser>(user.Id);
                    if (u != null)
                    {
                        string newFirst = await DisplayPromptAsync("Edit", "First Name:", initialValue: u.FirstName);
                        string newLast = await DisplayPromptAsync("Edit", "Last Name:", initialValue: u.LastName);

                        if (!string.IsNullOrWhiteSpace(newFirst) && !string.IsNullOrWhiteSpace(newLast))
                        {
                            u.FirstName = newFirst;
                            u.LastName = newLast;
                            u.Email = newEmail;
                            await AppDatabase.Database.UpdateAsync(u);
                        }
                    }
                }
                else if (user.Role == "Seller")
                {
                    var s = await AppDatabase.Database.FindAsync<AppSeller>(user.Id);
                    if (s != null)
                    {
                        string newName = await DisplayPromptAsync("Edit", "Name:", initialValue: s.Name);
                        string newBiz = await DisplayPromptAsync("Edit", "Business Name:", initialValue: s.BusinessName);

                        if (!string.IsNullOrWhiteSpace(newName) && !string.IsNullOrWhiteSpace(newBiz))
                        {
                            s.Name = newName;
                            s.BusinessName = newBiz;
                            s.Email = newEmail;
                            await AppDatabase.Database.UpdateAsync(s);
                        }
                    }
                }

                await LoadUsers();
            }
        }
    }

    public class UnifiedUserViewModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
