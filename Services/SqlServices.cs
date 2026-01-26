using SQLite;
using MindNestApp.Models;
using System.Collections.Generic;
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
        }

        // ---------------- USERS ----------------

        public Task<User> GetUserByEmailAsync(string email)
            => _db.Table<User>()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

        public Task<int> AddUserAsync(User user)
            => _db.InsertAsync(user);

        // ---------------- JOURNAL ENTRIES ----------------

        // CREATE
        public Task<int> AddEntryAsync(JournalEntry entry)
            => _db.InsertAsync(entry);

        // READ
        public Task<List<JournalEntry>> GetEntriesAsync(string email)
            => _db.Table<JournalEntry>()
                .Where(j => j.UserEmail == email)
                .OrderByDescending(j => j.DateTicks)
                .ToListAsync();


        // UPDATE 
        public Task<int> UpdateEntryAsync(JournalEntry entry)
            => _db.UpdateAsync(entry);

        // DELETE 
        public Task<int> DeleteEntryAsync(JournalEntry entry)
            => _db.DeleteAsync(entry);

        // READ BY ID 
        public Task<JournalEntry> GetEntryByIdAsync(int id)
            => _db.Table<JournalEntry>()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
    }
}