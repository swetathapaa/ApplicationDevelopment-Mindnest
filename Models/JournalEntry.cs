using SQLite;
using System;

namespace MindNestApp.Models
{
    public class JournalEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UserEmail { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        // Store ticks
        public long CreatedAtTicks { get; set; }
        public long UpdatedAtTicks { get; set; }

        // Convert to local DateTime when reading
        public DateTime CreatedAt => CreatedAtTicks > 0 ? new DateTime(CreatedAtTicks, DateTimeKind.Utc).ToLocalTime() : DateTime.MinValue;
        public DateTime UpdatedAt => UpdatedAtTicks > 0 ? new DateTime(UpdatedAtTicks, DateTimeKind.Utc).ToLocalTime() : DateTime.MinValue;

        // Moods
        public string PrimaryMood { get; set; } = string.Empty;
        public string SecondaryMoods { get; set; } = string.Empty;
        public string MoodCategory { get; set; } = string.Empty;

        // Category & Tags
        public string Category { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
    }
}