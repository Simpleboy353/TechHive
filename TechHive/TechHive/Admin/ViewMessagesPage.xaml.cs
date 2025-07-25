using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechHive.Base;
using TechHive.Database;
using TechHive.Models;

namespace TechHive.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewMessagesPage : BaseContentPage
    {
        public ViewMessagesPage()
        {
            InitializeComponent();
            LoadMessages();
        }

        private async void LoadMessages()
        {
            List<Message> messages = await DatabaseHelper.GetAllContactMessagesAsync();
            MessagesCollectionView.ItemsSource = messages;
        }
    }
}