﻿using SQLite;

namespace TechHive.Models
{
    public class AppUser
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Role { get; set; } // User, Admin, or Seller
    }
}
