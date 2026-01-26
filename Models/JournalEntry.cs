using SQLite;
using System;

namespace MindNestApp.Models
{
    public class JournalEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UserEmail { get; set; } = string.Empty;

        // Content in Markdown/rich-text
        public string Content { get; set; } = string.Empty;

        // Timestamps
        public long CreatedAtTicks { get; set; }
        public long UpdatedAtTicks { get; set; }

        // Moods
        public string PrimaryMood { get; set; } = string.Empty;
        public string SecondaryMoods { get; set; } = string.Empty; // comma-separated max 2
        public string MoodCategory { get; set; } = string.Empty; // Positive, Neutral, Negative

        // Category & Tags
        public string Category { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty; // comma-separated or custom tags
    }
}