using System;
using System.Collections.Generic;
using System.Linq;
using TechHive.Database;
using TechHive.Models;
using Xamarin.Forms;
using TechHive.Base;
using Xamarin.Forms.Xaml;

namespace TechHive.Pages
{
    public partial class HomePage : BaseContentPage
    {
        private List<Product> allProducts;
        private List<Product> filteredProducts;
        private string currentCategory = "All";

        public HomePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            allProducts = await DatabaseHelper.GetAllProductsAsync();
            FilterProducts();
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterProducts();
        }

        private void OnCategoryClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
                currentCategory = button.ClassId ?? "All";

            FilterProducts();
        }

        private void FilterProducts()
        {
            string searchText = searchBar.Text?.ToLower() ?? "";

            filteredProducts = allProducts
                .Where(p =>
                    (currentCategory == "All" || p.Category?.ToLower() == currentCategory.ToLower()) &&
                    (p.Name?.ToLower().Contains(searchText) == true || p.Category?.ToLower().Contains(searchText) == true))
                .ToList();

            productsCollectionView.ItemsSource = filteredProducts;
        }

        private async void OnProductSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Product selectedProduct)
            {
                await Navigation.PushAsync(new DetailsPage(selectedProduct));
                productsCollectionView.SelectedItem = null;
            }
        }

        private async void OnCartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartPage());
        }
    }
}
