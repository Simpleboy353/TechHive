using SQLite;

namespace TechHive.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImagePath { get; set; }
        public double Price { get; set; }

        public int SellerId { get; set; }
    }
}
