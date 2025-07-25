using SQLite;
using System.IO;
using System.Threading.Tasks;
using TechHive.Models;

namespace TechHive.Database
{
    public class AppDatabase
    {
        private static SQLiteAsyncConnection _database;

        public static async Task InitializeAsync(string dbPath)
        {
            if (_database == null)
            {
                _database = new SQLiteAsyncConnection(dbPath);
                await _database.CreateTableAsync<AppUser>();
                await _database.CreateTableAsync<AppSeller>();
                await _database.CreateTableAsync<Product>();
                await _database.CreateTableAsync<Order>();
                await _database.CreateTableAsync <CartItem>();
                await _database.CreateTableAsync<Message>();
            }
        }

        public static SQLiteAsyncConnection Database => _database;
    }
}
