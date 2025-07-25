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
    public partial class ViewAllOrdersPage : BaseContentPage
    {
        private List<Order> orders;

        public ViewAllOrdersPage()
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
            orders = await AppDatabase.Database.Table<Order>().ToListAsync();
            ordersCollectionView.ItemsSource = orders;
        }

        private async void OnEditClicked(object sender, System.EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Order order)
            {
                string newStatus = await DisplayPromptAsync("Edit Order", "New Status:", initialValue: order.Status);
                string newPayment = await DisplayPromptAsync("Edit Order", "Payment Method:", initialValue: order.PaymentMethod);

                if (!string.IsNullOrWhiteSpace(newStatus) && !string.IsNullOrWhiteSpace(newPayment))
                {
                    order.Status = newStatus;
                    order.PaymentMethod = newPayment;

                    await AppDatabase.Database.UpdateAsync(order);
                    await LoadOrders();
                }
                else
                {
                    await DisplayAlert("Invalid Input", "Fields cannot be empty.", "OK");
                }
            }
        }

        private async void OnDeleteClicked(object sender, System.EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Order order)
            {
                bool confirm = await DisplayAlert("Delete Order", $"Delete Order ID {order.Id}?", "Yes", "No");
                if (confirm)
                {
                    await AppDatabase.Database.DeleteAsync(order);
                    await LoadOrders();
                }
            }
        }
    }
}
