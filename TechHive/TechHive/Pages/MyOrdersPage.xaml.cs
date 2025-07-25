using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using TechHive.Base;
using TechHive.Models;
using TechHive.Database;

namespace TechHive.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyOrdersPage : BaseContentPage
    {
        public MyOrdersPage()
        {
            InitializeComponent();
            LoadOrders();
        }

        private async void LoadOrders()
        {
            int userId = Preferences.Get("UserId", 0);
            var allOrders = await DatabaseHelper.GetOrdersByUserIdAsync(userId);
            var allProducts = await DatabaseHelper.GetAllProductsAsync();

            var orderViews = new List<OrderViewModel>();

            foreach (var order in allOrders)
            {
                var productIds = order.ProductIds?.Split(',') ?? Array.Empty<string>();
                var grouped = productIds
                    .Where(id => int.TryParse(id, out _))
                    .Select(id => int.Parse(id))
                    .GroupBy(pid => pid)
                    .ToDictionary(g => g.Key, g => g.Count());

                var productList = new List<OrderProductViewModel>();

                foreach (var kv in grouped)
                {
                    var product = allProducts.FirstOrDefault(p => p.Id == kv.Key);
                    if (product != null)
                    {
                        productList.Add(new OrderProductViewModel
                        {
                            Name = product.Name,
                            Price = product.Price,
                            Image = product.ImagePath,
                            Quantity = kv.Value
                        });
                    }
                }

                orderViews.Add(new OrderViewModel
                {
                    OrderId = order.Id,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    Products = productList
                });
            }

            OrdersCollectionView.ItemsSource = orderViews;
        }

        private async void CancelOrder_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var viewModel = button?.CommandParameter as OrderViewModel;

            if (viewModel == null || viewModel.Status == "Cancelled" || viewModel.Status == "Delivered" || viewModel.Status == "Shipped")
                return;

            bool confirm = await DisplayAlert("Cancel Order", "Are you sure you want to cancel this order?", "Yes", "No");
            if (!confirm) return;

            // Update in database using the actual OrderId
            await DatabaseHelper.UpdateOrderStatusAsync(viewModel.OrderId, "Cancelled");

            await DisplayAlert("Cancelled", "Order has been cancelled.", "OK");

            // Reload with updated status
            LoadOrders();
        }


    }

    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
        public string Status { get; set; }
        public List<OrderProductViewModel> Products { get; set; }
    }

    public class OrderProductViewModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
    }
}
