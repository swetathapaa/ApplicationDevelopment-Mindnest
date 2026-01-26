using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Storage;
using MindNestApp.Models;

namespace MindNestApp.Services
{
    public class PdfExportService
    {
        public string ExportJournalsToPdf(string userEmail, List<JournalEntry> entries)
        {
#if MACCATALYST
            return ExportAsTextFallback(userEmail, entries);
#else
            return ExportAsPdf(userEmail, entries);
#endif
        }

#if MACCATALYST
        private string ExportAsTextFallback(string userEmail, List<JournalEntry> entries)
        {
            var filePath = Path.Combine(
                FileSystem.AppDataDirectory,
                $"MindNest_Journal_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
            );

            using var writer = new StreamWriter(filePath);

            writer.WriteLine("MindNest – Journal Export");
            writer.WriteLine($"User: {userEmail}");
            writer.WriteLine($"Generated on: {DateTime.Now}");
            writer.WriteLine(new string('-', 50));

            foreach (var entry in entries)
            {
                writer.WriteLine($"Date: {new DateTime(entry.CreatedAtTicks):yyyy-MM-dd HH:mm}");
                writer.WriteLine($"Category: {entry.Category}");
                writer.WriteLine($"Mood: {entry.PrimaryMood}");

                if (!string.IsNullOrWhiteSpace(entry.Tags))
                    writer.WriteLine($"Tags: {entry.Tags}");

                writer.WriteLine();
                writer.WriteLine(entry.Content);
                writer.WriteLine(new string('-', 50));
            }

            return filePath;
        }
#endif

#if !MACCATALYST
        private string ExportAsPdf(string userEmail, List<JournalEntry> entries)
        {
            // You can re-enable QuestPDF here later
            // (for Windows builds only)

            throw new NotImplementedException(
                "PDF export is disabled on this platform."
            );
        }
#endif
    }
}