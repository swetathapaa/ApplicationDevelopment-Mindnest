using SQLite;

namespace MindNestApp.Models
{
    [SQLite.Table("Users")]
    public class User
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }

        [SQLite.Unique, SQLite.NotNull]
        public string Email { get; set; }

        [SQLite.NotNull]
        public string Password { get; set; }

        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        
        public bool IsDarkMode { get; set; } = false;
    }
}