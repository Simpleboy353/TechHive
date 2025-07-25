using SQLite;

namespace TechHive.Models
{
    public class AppSeller
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string BusinessName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}
