using iTextSharp.text;
using iTextSharp.text.pdf;
using MindNestApp.Models;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.IO;

// Aliases to avoid conflicts with MAUI
using PdfFont = iTextSharp.text.Font;
using PdfElement = iTextSharp.text.Element;

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

            // Fonts with proper namespace
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, new BaseColor(0, 0, 0));
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, new BaseColor(64, 64, 64));
            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, new BaseColor(0, 0, 0));
            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, new BaseColor(0, 0, 0));
            var italicFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 12, new BaseColor(0, 0, 0));
            var boldItalicFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLDOBLIQUE, 12, new BaseColor(0, 0, 0));
            var smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, new BaseColor(128, 128, 128));

            // Header
            document.Add(new Paragraph("MindNest Journal Export", titleFont));
            document.Add(new Paragraph($"User: {userEmail}", normalFont));
            document.Add(new Paragraph($"Generated on: {DateTime.Now:dddd, dd MMMM yyyy HH:mm}", normalFont));
            document.Add(new Paragraph($"Total Entries: {entries.Count}", normalFont));
            document.Add(new Paragraph("\n"));

            // Add separator line
            var line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.5f, 100f, new BaseColor(128, 128, 128), PdfElement.ALIGN_CENTER, -1)));
            document.Add(line);
            document.Add(new Paragraph("\n"));

            // Entries
            foreach (var entry in entries)
            {
                var entryTitle = string.IsNullOrWhiteSpace(entry.Category) ? "Untitled" : entry.Category;
                
                // Category
                document.Add(new Paragraph($"Category: {entryTitle}", headerFont));
                
                // Date
                document.Add(new Paragraph($"Date: {entry.CreatedAt:dddd, dd MMMM yyyy}", smallFont));

                // Mood
                if (!string.IsNullOrWhiteSpace(entry.PrimaryMood))
                {
                    var moodText = $"Mood: {entry.PrimaryMood}";
                    if (!string.IsNullOrWhiteSpace(entry.SecondaryMoods))
                    {
                        moodText += $", {entry.SecondaryMoods}";
                    }
                    document.Add(new Paragraph(moodText, smallFont));
                }

                // Tags
                if (!string.IsNullOrWhiteSpace(entry.Tags))
                    document.Add(new Paragraph($"Tags: {entry.Tags}", smallFont));

                document.Add(new Paragraph("\n"));

                // Content with HTML formatting
                if (!string.IsNullOrWhiteSpace(entry.Content))
                {
                    AddFormattedContent(document, entry.Content, normalFont, boldFont, italicFont, boldItalicFont);
                }

                document.Add(new Paragraph("\n"));
                
                // Separator
                document.Add(line);
                document.Add(new Paragraph("\n"));
            }

            document.Close();
            writer.Close();

            return filePath;
        }

        private void AddFormattedContent(Document document, string htmlContent, PdfFont normalFont, PdfFont boldFont, PdfFont italicFont, PdfFont boldItalicFont)
        {
            // Parse HTML and apply formatting
            var paragraph = new Paragraph();
            paragraph.Leading = 16f; // Line spacing

            // Replace common HTML tags with iText formatting
            var parts = ParseHtmlContent(htmlContent);

            foreach (var part in parts)
            {
                Chunk chunk;
                
                if (part.IsBold && part.IsItalic)
                {
                    chunk = new Chunk(part.Text, boldItalicFont);
                }
                else if (part.IsBold)
                {
                    chunk = new Chunk(part.Text, boldFont);
                }
                else if (part.IsItalic)
                {
                    chunk = new Chunk(part.Text, italicFont);
                }
                else
                {
                    chunk = new Chunk(part.Text, normalFont);
                }

                paragraph.Add(chunk);
            }

            document.Add(paragraph);
        }

        private List<TextPart> ParseHtmlContent(string html)
        {
            var parts = new List<TextPart>();
            
            // Remove line breaks and replace with spaces for better PDF rendering
            html = html.Replace("<br>", "\n").Replace("<br/>", "\n").Replace("<br />", "\n");
            html = html.Replace("<p>", "\n").Replace("</p>", "\n");
            html = html.Replace("<div>", "\n").Replace("</div>", "\n");
            
            // Track current position
            int currentIndex = 0;
            bool isBold = false;
            bool isItalic = false;
            var currentText = "";

            while (currentIndex < html.Length)
            {
                // Check for opening bold tag
                if (html.Substring(currentIndex).StartsWith("<b>") || html.Substring(currentIndex).StartsWith("<strong>"))
                {
                    if (currentText.Length > 0)
                    {
                        parts.Add(new TextPart { Text = currentText, IsBold = isBold, IsItalic = isItalic });
                        currentText = "";
                    }
                    isBold = true;
                    currentIndex += html.Substring(currentIndex).StartsWith("<b>") ? 3 : 8;
                    continue;
                }

                // Check for closing bold tag
                if (html.Substring(currentIndex).StartsWith("</b>") || html.Substring(currentIndex).StartsWith("</strong>"))
                {
                    if (currentText.Length > 0)
                    {
                        parts.Add(new TextPart { Text = currentText, IsBold = isBold, IsItalic = isItalic });
                        currentText = "";
                    }
                    isBold = false;
                    currentIndex += html.Substring(currentIndex).StartsWith("</b>") ? 4 : 9;
                    continue;
                }

                // Check for opening italic tag
                if (html.Substring(currentIndex).StartsWith("<i>") || html.Substring(currentIndex).StartsWith("<em>"))
                {
                    if (currentText.Length > 0)
                    {
                        parts.Add(new TextPart { Text = currentText, IsBold = isBold, IsItalic = isItalic });
                        currentText = "";
                    }
                    isItalic = true;
                    currentIndex += html.Substring(currentIndex).StartsWith("<i>") ? 3 : 4;
                    continue;
                }

                // Check for closing italic tag
                if (html.Substring(currentIndex).StartsWith("</i>") || html.Substring(currentIndex).StartsWith("</em>"))
                {
                    if (currentText.Length > 0)
                    {
                        parts.Add(new TextPart { Text = currentText, IsBold = isBold, IsItalic = isItalic });
                        currentText = "";
                    }
                    isItalic = false;
                    currentIndex += html.Substring(currentIndex).StartsWith("</i>") ? 4 : 5;
                    continue;
                }

                // Add regular character
                currentText += html[currentIndex];
                currentIndex++;
            }

            // Add remaining text
            if (currentText.Length > 0)
            {
                parts.Add(new TextPart { Text = currentText, IsBold = isBold, IsItalic = isItalic });
            }

            return parts;
        }

        private class TextPart
        {
            public string Text { get; set; } = "";
            public bool IsBold { get; set; }
            public bool IsItalic { get; set; }
        }
    }
}