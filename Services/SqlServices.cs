using SQLite;
using MindNestApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNestApp.Services
{
    public class SQLiteService
    {
        private readonly SQLiteAsyncConnection _db;

        public SQLiteService(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);

            _db.CreateTableAsync<User>().Wait();
            _db.CreateTableAsync<JournalEntry>().Wait();
            _db.CreateTableAsync<UserCustomTag>().Wait(); // Added table for custom tags
        }

        // ---------------- USERS ----------------

        public Task<User?> GetUserByEmailAsync(string email)
            => _db.Table<User>()
                  .Where(u => u.Email == email)
                  .FirstOrDefaultAsync();

        public Task<int> AddUserAsync(User user)
            => _db.InsertAsync(user);

        // ---------------- JOURNAL ENTRIES ----------------

        // CREATE
        public Task<int> AddEntryAsync(JournalEntry entry)
            => _db.InsertAsync(entry);

        // READ (ALL FOR USER)
        public Task<List<JournalEntry>> GetEntriesAsync(string email)
            => _db.Table<JournalEntry>()
                  .Where(j => j.UserEmail == email)
                  .OrderByDescending(j => j.CreatedAtTicks)
                  .ToListAsync();

        // READ (BY DATE - one entry per day)
        public Task<JournalEntry?> GetEntryByDateAsync(string email, long dayTicks)
            => _db.Table<JournalEntry>()
                  .Where(j => j.UserEmail == email && j.CreatedAtTicks == dayTicks)
                  .FirstOrDefaultAsync();

        // UPDATE
        public Task<int> UpdateEntryAsync(JournalEntry entry)
        {
            entry.UpdatedAtTicks = DateTime.UtcNow.Ticks;
            return _db.UpdateAsync(entry);
        }

        // DELETE
        public Task<int> DeleteEntryAsync(JournalEntry entry)
            => _db.DeleteAsync(entry);

        // READ BY ID
        public Task<JournalEntry?> GetEntryByIdAsync(int id)
            => _db.Table<JournalEntry>()
                  .Where(e => e.Id == id)
                  .FirstOrDefaultAsync();

        // ---------------- USER CUSTOM TAGS ----------------

        // Get all tags for a specific user
        public async Task<List<string>> GetUserTagsAsync(string userEmail)
        {
            var tags = await _db.Table<UserCustomTag>()
                                .Where(t => t.UserEmail == userEmail)
                                .OrderBy(t => t.Tag)
                                .ToListAsync();
            return tags.Select(t => t.Tag).ToList();
        }

        // Add a new tag for the user (returns the number of rows inserted)
        public Task<int> AddUserTagAsync(string userEmail, string tag)
        {
            var userTag = new UserCustomTag
            {
                UserEmail = userEmail,
                Tag = tag
            };
            return _db.InsertAsync(userTag);
        }

        // Optional: Delete a tag if needed
        public Task<int> DeleteUserTagAsync(UserCustomTag tag)
            => _db.DeleteAsync(tag);
        public Task<int> UpdateUserAsync(User user)
        {
            return _db.UpdateAsync(user);
        }

    }
    
}
