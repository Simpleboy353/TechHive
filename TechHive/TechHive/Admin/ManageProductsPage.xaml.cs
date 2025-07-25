using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TechHive.Models;
using TechHive.Database;
using TechHive.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechHive.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageProductsPage : BaseContentPage
    {
        private List<Product> products;

        public ManageProductsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            products = await DatabaseHelper.GetAllProductsAsync();
            productsCollectionView.ItemsSource = products;
        }

        private async void OnDeleteClicked(object sender, System.EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Product product)
            {
                bool confirm = await DisplayAlert("Delete", $"Delete '{product.Name}'?", "Yes", "No");
                if (confirm)
                {
                    await AppDatabase.Database.DeleteAsync(product);
                    await LoadProducts();
                }
            }
        }

        private async void OnEditClicked(object sender, System.EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Product product)
            {
                string newName = await DisplayPromptAsync("Edit Product", "New Name:", initialValue: product.Name);
                string newCategory = await DisplayPromptAsync("Edit Product", "New Category:", initialValue: product.Category);
                string newDescription = await DisplayPromptAsync("Edit Product", "New Description:", initialValue: product.Description);
                string priceInput = await DisplayPromptAsync("Edit Product", "New Price:", initialValue: product.Price.ToString());

                if (double.TryParse(priceInput, out double newPrice) && !string.IsNullOrWhiteSpace(newName))
                {
                    product.Name = newName;
                    product.Category = newCategory;
                    product.Description = newDescription;
                    product.Price = newPrice;

                    await AppDatabase.Database.UpdateAsync(product);
                    await LoadProducts();
                }
                else
                {
                    await DisplayAlert("Invalid Input", "Price must be a number.", "OK");
                }
            }
        }
    }
}
