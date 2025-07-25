using SQLite;
using System;

public class Message
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int SenderId { get; set; }   // From Preferences
    public string Name { get; set; }    // Optional (if you want full name)
    public string Email { get; set; }   // Optional
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}