using SQLite;

namespace MindNestApp.Models
{
    public class UserCustomTag
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UserEmail { get; set; } = string.Empty;

        public string Tag { get; set; } = string.Empty;
    }
}