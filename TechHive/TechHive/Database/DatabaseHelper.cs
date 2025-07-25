using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TechHive.Models;

namespace TechHive.Database
{
    public static class DatabaseHelper
    {
        // --- USER METHODS ---

        public static Task<int> AddUserAsync(AppUser user)
        {
            return AppDatabase.Database.InsertAsync(user);
        }

        public static Task<AppUser> GetUserByEmailAsync(string email)
        {
            return AppDatabase.Database.Table<AppUser>()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }

        public static Task<List<AppUser>> GetAllUsersAsync()
        {
            return AppDatabase.Database.Table<AppUser>().ToListAsync();
        }

        public static Task<int> DeleteUserAsync(AppUser user)
        {
            return AppDatabase.Database.DeleteAsync(user);
        }


        // --- SELLER METHODS ---

        public static Task<int> AddSellerAsync(AppSeller seller)
        {
            return AppDatabase.Database.InsertAsync(seller);
        }

        public static Task<AppSeller> GetSellerByEmailAsync(string email)
        {
            return AppDatabase.Database.Table<AppSeller>()
                .Where(s => s.Email == email)
                .FirstOrDefaultAsync();
        }

        public static Task<List<AppSeller>> GetAllSellersAsync()
        {
            return AppDatabase.Database.Table< AppSeller>().ToListAsync();
        }

        public static Task<int> DeleteSellerAsync(AppSeller seller)
        {
            return AppDatabase.Database.DeleteAsync(seller);
        }

        public static Task<AppSeller> GetSellerByCredentialsAsync(string email, string password)
        {
            return AppDatabase.Database.Table<AppSeller>()
                .Where(s => s.Email == email && s.Password == password)
                .FirstOrDefaultAsync();
        }


        // --- PRODUCT METHODS ---

        public static Task<int> AddProductAsync(Product product)
        {
            return AppDatabase.Database.InsertAsync(product);
        }

        public static Task<List<Product>> GetAllProductsAsync()
        {
            return AppDatabase.Database.Table<Product>().ToListAsync();
        }

        public static Task<int> DeleteProductAsync(Product product)
        {
            return AppDatabase.Database.DeleteAsync(product);
        }

        public static Task<List<Product>> GetProductsBySellerIdAsync(int sellerId)
        {
            return AppDatabase.Database.Table<Product>()
                .Where(p => p.SellerId == sellerId)
                .ToListAsync();
        }

        public static Task<Product> GetProductByIdAsync(int id)
        {
            return AppDatabase.Database.Table<Product>()
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public static Task<int> UpdateProductAsync(Product product)
        {
            return AppDatabase.Database.UpdateAsync(product);
        }

        // --- CART METHODS ---
        public static Task<int> AddCartItemAsync(CartItem cartItem)
        {
            return AppDatabase.Database.InsertAsync(cartItem);
        }

        public static async Task AddOrUpdateCartItemAsync(int userId, Product product, int quantity)
        {
            var existingItem = await AppDatabase.Database.Table<CartItem>()
                .Where(c => c.UserId == userId && c.ProductId == product.Id)
                .FirstOrDefaultAsync();

            if (existingItem != null)
            {
                existingItem.Quantity = quantity;
                existingItem.Name = product.Name;
                existingItem.Price = product.Price;
                existingItem.Image = product.ImagePath;

                await AppDatabase.Database.UpdateAsync(existingItem);
            }
            else
            {
                await AddCartItemAsync(new CartItem
                {
                    UserId = userId,
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Image = product.ImagePath,
                    Quantity = quantity
                });
            }
        }

        // (Original method name retained; update existing cart item)
        public static Task<int> UpdateCartItemAsync(CartItem item)
        {
            return AppDatabase.Database.UpdateAsync(item);
        }

        // (Original method name retained; remove cart item)
        public static Task<int> RemoveCartItemAsync(CartItem item)
        {
            return AppDatabase.Database.DeleteAsync(item);
        }

        // (Original method retained; gets cart items for a user)
        public static Task<List<CartItem>> GetCartItemsAsync(int userId)
        {
            return AppDatabase.Database.Table<CartItem>()
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        // (New method; clears entire cart after payment)
        public static async Task ClearCartAsync(int userId)
        {
            var items = await GetCartItemsAsync(userId);
            foreach (var item in items)
            {
                await RemoveCartItemAsync(item);
            }
        }

        // --- ORDER METHODS ---
        public static Task<int> AddOrderAsync(Order order)
        {
            return AppDatabase.Database.InsertAsync(order);
        }

        public static Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return AppDatabase.Database.Table<Order>()
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public static async Task<List<Order>> GetOrdersBySellerIdAsync(int sellerId)
        {
            var db = AppDatabase.Database;

            // Get all products listed by this seller
            var sellerProducts = await db.Table<Product>()
                                         .Where(p => p.SellerId == sellerId)
                                         .ToListAsync();

            if (sellerProducts == null || !sellerProducts.Any())
                return new List<Order>();

            // Get product IDs as strings to match against Order.ProductIds (comma-separated)
            var sellerProductIds = sellerProducts.Select(p => p.Id.ToString()).ToList();

            // Get all orders and filter those that include any seller's product
            var allOrders = await db.Table<Order>().ToListAsync();

            var sellerOrders = allOrders.Where(order =>
                order.ProductIds.Split(',')
                                .Any(pid => sellerProductIds.Contains(pid.Trim()))
            ).ToList();

            return sellerOrders;
        }

        public static Task<List<Order>> GetAllOrdersAsync()
        {
            return AppDatabase.Database.Table<Order>().ToListAsync();
        }

        public static Task<int> UpdateOrderAsync(Order order)
        {
            return AppDatabase.Database.UpdateAsync(order);
        }

        public static async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await AppDatabase.Database.Table<Order>().Where(o => o.Id == orderId).FirstOrDefaultAsync();

            if (order != null)
            {
                order.Status = newStatus;
                await AppDatabase.Database.UpdateAsync(order);
            }
        }


        public static Task<int> DeleteOrderAsync(Order order)
        {
            return AppDatabase.Database.DeleteAsync(order);
        }

        // --- CONTACT METHODS ---

        public static async Task AddContactMessageAsync(int userId, string name, string email, string messageContent)
        {
            var message = new Message
            {
                SenderId = userId,
                Name = name,
                Email = email,
                Content = messageContent,
                CreatedAt = DateTime.Now
            };

            await AppDatabase.Database.InsertAsync(message);
        }

        public static async Task<List<Message>> GetAllContactMessagesAsync()
        {
            return await AppDatabase.Database.Table<Message>()
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

    }
}
