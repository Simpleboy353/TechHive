using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TechHive.Base;
using TechHive.Database;
using TechHive.Models;

namespace TechHive.Seller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewOrdersPage : BaseContentPage
    {
        private int sellerId = AppSession.LoggedInSellerId;

        public ViewOrdersPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadOrders();
        }

        private async Task LoadOrders()
        {
            var myProducts = await DatabaseHelper.GetProductsBySellerIdAsync(sellerId);
            var myProductIds = myProducts.Select(p => p.Id.ToString()).ToList();
            var allOrders = await DatabaseHelper.GetAllOrdersAsync();
            var users = await DatabaseHelper.GetAllUsersAsync();

            var viewOrders = new List<OrderViewModel>();

            foreach (var order in allOrders)
            {
                var orderedProductIds = order.ProductIds?.Split(',') ?? Array.Empty<string>();

                var matched = orderedProductIds
                    .Where(pid => myProductIds.Contains(pid))
                    .GroupBy(pid => pid)
                    .ToDictionary(g => int.Parse(g.Key), g => g.Count());

                if (matched.Any())
                {
                    var buyer = users.FirstOrDefault(u => u.Id == order.UserId);
                    var productDetails = new List<OrderProductViewModel>();

                    foreach (var kv in matched)
                    {
                        var product = myProducts.FirstOrDefault(p => p.Id == kv.Key);
                        if (product != null)
                        {
                            productDetails.Add(new OrderProductViewModel
                            {
                                Name = product.Name,
                                Image = product.ImagePath,
                                Price = product.Price,
                                Quantity = kv.Value
                            });
                        }
                    }

                    viewOrders.Add(new OrderViewModel
                    {
                        OrderId = order.Id,
                        BuyerName = buyer?.FirstName + " " + buyer?.LastName ?? "Unknown",
                        OrderDate = order.OrderDate,
                        Status = order.Status,
                        Products = productDetails
                    });
                }
            }

            OrdersCollectionView.ItemsSource = viewOrders;
        }

        private async void OnMarkShippedClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is OrderViewModel vm)
            {
                await DatabaseHelper.UpdateOrderStatusAsync(vm.OrderId, "Shipped");
                await LoadOrders();
            }
        }

        private async void OnMarkDeliveredClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is OrderViewModel vm)
            {
                await DatabaseHelper.UpdateOrderStatusAsync(vm.OrderId, "Delivered");
                await LoadOrders();
            }
        }
    }

    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string BuyerName { get; set; }
        public string OrderDate { get; set; }
        public string Status { get; set; }
        public List<OrderProductViewModel> Products { get; set; }
    }

    public class OrderProductViewModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
