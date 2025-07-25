using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TechHive.Models;
using TechHive.Database;
using TechHive.Base;
using System;

namespace TechHive.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : BaseContentPage
    {
        private Product selectedProduct;
        private int userId = AppSession.LoggedInUserId;

        public DetailsPage(Product product)
        {
            InitializeComponent();
            selectedProduct = product;

            nameLabel.Text = selectedProduct.Name;
            priceLabel.Text = $"${selectedProduct.Price:F2}";
            descLabel.Text = selectedProduct.Description;

            if (!string.IsNullOrEmpty(selectedProduct.ImagePath))
                productImage.Source = ImageSource.FromFile(selectedProduct.ImagePath);
            else
                productImage.Source = "placeholder.png"; // fallback image
        }

        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            await DatabaseHelper.AddOrUpdateCartItemAsync(userId, selectedProduct, 1);
            await DisplayAlert("Added", $"{selectedProduct.Name} added to cart.", "OK");
        }
    }
}
