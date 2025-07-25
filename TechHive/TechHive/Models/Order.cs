using SQLite;
using System.Collections.Generic;

namespace TechHive.Models
{
    public class Order
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }
        public string ProductIds { get; set; } // comma-separated product IDs
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDate { get; set; }

        [Ignore]
        public List<Product> Products { get; set; }  // Used for showing product details

        [Ignore]
        public bool CanCancel { get; set; }          // Used to show/hide Cancel button
    }
}
