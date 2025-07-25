using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using TechHive.Models;
using TechHive.Database;
using TechHive.Base;
using System.IO;

namespace TechHive.Seller
{
    public partial class EditProductPage : BaseContentPage
    {
        private Product _product;

        public EditProductPage(Product product)
        {
            InitializeComponent();
            _product = product;

            nameEntry.Text = product.Name;
            categoryPicker.SelectedItem = product.Category;
            priceEntry.Text = product.Price.ToString();
            descEditor.Text = product.Description;
            if (!string.IsNullOrEmpty(product.ImagePath))
                productImage.Source = product.ImagePath;
        }

        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            _product.Name = nameEntry.Text;
            _product.Category = categoryPicker.SelectedItem?.ToString();
            _product.Description = descEditor.Text;

            if (double.TryParse(priceEntry.Text, out double price))
                _product.Price = price;

            await DatabaseHelper.UpdateProductAsync(_product);
            await DisplayAlert("Success", "Product updated successfully!", "OK");
            await Navigation.PopAsync();
        }

        private async void OnChangeImageClicked(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync();

            if (result != null)
            {
                var newPath = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
                var stream = await result.OpenReadAsync();
                var newStream = File.OpenWrite(newPath);
                await stream.CopyToAsync(newStream);
                stream.Dispose();
                newStream.Dispose();


                _product.ImagePath = newPath;
                productImage.Source = newPath;
            }
        }
    }
}
