using SQLite;   // 🔑 REQUIRED
using System;

namespace MindNestApp.Models
{
    public class JournalEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UserEmail { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        // SQLite-safe date storage
        public long DateTicks { get; set; }
    }
}