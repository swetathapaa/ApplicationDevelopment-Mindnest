using System;
using System.Collections.Generic;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using MindNestApp.Models;
using Microsoft.Maui.Storage;

namespace MindNestApp.Services
{
    public class PdfExportService
    {
        public string ExportJournalsToPdf(string userEmail, List<JournalEntry> entries)
        {
            // REQUIRED: set license ONCE before using QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;

            var filePath = Path.Combine(
                FileSystem.AppDataDirectory,
                $"MindNest_Journal_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            );

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    page.Content().Column(column =>
                    {
                        column.Spacing(10);

                        // Title
                        column.Item().Text("MindNest – Journal Export")
                            .FontSize(20)
                            .Bold();

                        // User info
                        column.Item().Text($"User: {userEmail}");
                        column.Item().Text($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm}");

                        column.Item().LineHorizontal(1);

                        // Entries
                        foreach (var entry in entries)
                        {
                            column.Item().Text(entry.DateTicks.ToString("yyyy-MM-dd HH:mm"))
                                .Bold();

                            column.Item().Text(entry.Content);

                            column.Item().PaddingBottom(10);
                        }
                    });
                });
            }).GeneratePdf(filePath);

            return filePath;
        }
    }
}