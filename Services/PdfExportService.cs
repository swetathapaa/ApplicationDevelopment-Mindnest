using iTextSharp.text;
using iTextSharp.text.pdf;
using MindNestApp.Models;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.IO;

namespace MindNestApp.Services
{
    public class PdfExportService
    {
        public string ExportJournalsToPdf(string userEmail, List<JournalEntry> entries)
        {
            if (entries == null || entries.Count == 0)
                throw new ArgumentException("No journal entries to export.");

            // File path
            var fileName = $"MindNest_Journals_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            // Create document
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var document = new Document(PageSize.A4, 30, 30, 30, 30);
            var writer = PdfWriter.GetInstance(document, fs);

            document.Open();

            // Fonts
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            // Header
            document.Add(new Paragraph("MindNest Journal Export", titleFont));
            document.Add(new Paragraph($"User: {userEmail}", normalFont));
            document.Add(new Paragraph($"Generated on: {DateTime.Now:dddd, dd MMMM yyyy}", normalFont));
            document.Add(new Paragraph("\n"));

            // Entries
            foreach (var entry in entries)
            {
                var entryTitle = string.IsNullOrWhiteSpace(entry.Category) ? "Untitled" : entry.Category;
                document.Add(new Paragraph($"Category: {entryTitle}", titleFont));
                document.Add(new Paragraph($"Date: {entry.CreatedAt:dddd, dd MMMM yyyy}", normalFont));

                if (!string.IsNullOrWhiteSpace(entry.Tags))
                    document.Add(new Paragraph($"Tags: {entry.Tags}", normalFont));

                if (!string.IsNullOrWhiteSpace(entry.Content))
                    document.Add(new Paragraph(entry.Content, normalFont));

                document.Add(new Paragraph("\n---------------------------------\n"));
            }

            document.Close();
            writer.Close();

            return filePath;
        }
    }
}
