using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using Xamarin.Forms;
using TechHive.Models;
using TechHive.Base;
using TechHive.Database;

namespace TechHive.Seller
{
    public partial class AddProductPage : BaseContentPage
    {
        private string selectedImagePath;
        int currentSellerId = AppSession.LoggedInSellerId;

        public AddProductPage()
        {
            InitializeComponent();
        }

        private async void OnSelectImageClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "Photo picking not supported.", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium
            });

            if (file != null)
            {
                selectedImagePath = file.Path;
                productImage.Source = ImageSource.FromFile(selectedImagePath);
            }
        }

        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameEntry.Text) ||
                string.IsNullOrWhiteSpace(priceEntry.Text) ||
                string.IsNullOrWhiteSpace(descEditor.Text) ||
                categoryPicker.SelectedIndex == -1 ||
                string.IsNullOrEmpty(selectedImagePath))
            {
                await DisplayAlert("Error", "Please fill in all fields and select an image.", "OK");
                return;
            }

            if (!double.TryParse(priceEntry.Text, out double price))
            {
                await DisplayAlert("Error", "Invalid price format.", "OK");
                return;
            }

            var newProduct = new Product
            {
                Name = nameEntry.Text,
                Category = categoryPicker.SelectedItem.ToString(),
                Price = price,
                Description = descEditor.Text,
                ImagePath = selectedImagePath,
                SellerId = currentSellerId // Replace this with actual seller ID from session later
            };

            await DatabaseHelper.AddProductAsync(newProduct);
            await DisplayAlert("Success", "Product added successfully!", "OK");

            // Clear fields
            nameEntry.Text = "";
            priceEntry.Text = "";
            descEditor.Text = "";
            categoryPicker.SelectedIndex = -1;
            productImage.Source = null;
            selectedImagePath = null;
        }
    }
}
