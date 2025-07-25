using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TechHive.Models;

namespace TechHive.Database
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection _database;

        public static async Task InitializeAsync()
        {
            if (_database != null)
                return;

            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TechHive.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<AppUser>();
        }

        public static async Task<int> AddUserAsync(AppUser user)
        {
            await InitializeAsync();
            return await _database.InsertAsync(user);
        }

        public static async Task<AppUser> GetUserAsync(string email, string password)
        {
            await InitializeAsync();
            return await _database.Table<AppUser>().FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public static async Task<List<AppUser>> GetAllUsersAsync()
        {
            await InitializeAsync();
            return await _database.Table<AppUser>().ToListAsync();
        }

        public static async Task<int> DeleteUserAsync(AppUser user)
        {
            await InitializeAsync();
            return await _database.DeleteAsync(user);
        }

        public static async Task<int> UpdateUserAsync(AppUser user)
        {
            await InitializeAsync();
            return await _database.UpdateAsync(user);
        }
    }
}
