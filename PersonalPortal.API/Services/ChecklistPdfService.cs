using PersonalPortal.Core.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PersonalPortal.API.Services;

public interface IChecklistPdfService
{
    byte[] GeneratePdf(Checklist checklist);
}

public class ChecklistPdfService : IChecklistPdfService
{
    public byte[] GeneratePdf(Checklist checklist)
    {
        // Configure QuestPDF license for community use
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                page.Header().Element(ComposeHeader);
                page.Content().Element(content => ComposeContent(content, checklist));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private void ComposeHeader(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().PaddingBottom(10).BorderBottom(2).BorderColor(Colors.Grey.Darken2)
                .Text("Personal Portal Checklist")
                .FontSize(20)
                .Bold()
                .FontColor(Colors.Blue.Darken2);
        });
    }

    private void ComposeContent(IContainer container, Checklist checklist)
    {
        container.PaddingVertical(20).Column(column =>
        {
            // Checklist Header Information
            column.Item().PaddingBottom(10).Column(info =>
            {
                info.Item().Row(row =>
                {
                    row.RelativeItem().Text(text =>
                    {
                        text.Span("Name: ").Bold();
                        text.Span(checklist.Name);
                    });
                    
                    row.RelativeItem().AlignRight().Text(text =>
                    {
                        text.Span("Type: ").Bold();
                        text.Span(checklist.Type);
                    });
                });
                
                info.Item().PaddingTop(5).Text(text =>
                {
                    text.Span("Created: ").Bold();
                    text.Span(checklist.Created.ToLocalTime().ToString("g"));
                    text.Span("  |  ");
                    text.Span("Updated: ").Bold();
                    text.Span(checklist.Updated.ToLocalTime().ToString("g"));
                });
            });

            column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);

            // Group items by ItemGroup
            var groupedItems = checklist.Items
                .GroupBy(i => string.IsNullOrWhiteSpace(i.ItemGroup) ? "General" : i.ItemGroup)
                .OrderBy(g => g.Key);

            foreach (var group in groupedItems)
            {
                column.Item().PaddingTop(15).Column(groupColumn =>
                {
                    // Group Header
                    groupColumn.Item().PaddingBottom(8)
                        .Text(group.Key)
                        .FontSize(14)
                        .Bold()
                        .FontColor(Colors.Blue.Darken1);

                    // Items in 3 columns
                    var itemsList = group.ToList();
                    var itemsPerColumn = (int)Math.Ceiling(itemsList.Count / 3.0);

                    groupColumn.Item().Row(row =>
                    {
                        for (int col = 0; col < 3; col++)
                        {
                            var columnItems = itemsList
                                .Skip(col * itemsPerColumn)
                                .Take(itemsPerColumn)
                                .ToList();

                            if (columnItems.Any())
                            {
                                row.RelativeItem().PaddingRight(col < 2 ? 10 : 0).Column(itemColumn =>
                                {
                                    foreach (var item in columnItems)
                                    {
                                        itemColumn.Item().PaddingBottom(6).Row(itemRow =>
                                        {
                                            // Checkbox
                                            itemRow.ConstantItem(15).Border(1).BorderColor(Colors.Grey.Darken2)
                                                .Height(12).Width(12);

                                            // Item name and description
                                            itemRow.RelativeItem().PaddingLeft(5).Column(textColumn =>
                                            {
                                                textColumn.Item().Text(item.ItemName).Bold().FontSize(10);
                                                if (!string.IsNullOrWhiteSpace(item.Description))
                                                {
                                                    textColumn.Item().Text(item.Description)
                                                        .FontSize(8)
                                                        .FontColor(Colors.Grey.Darken1)
                                                        .Italic();
                                                }
                                            });
                                        });
                                    }
                                });
                            }
                            else
                            {
                                row.RelativeItem(); // Empty column for layout
                            }
                        }
                    });
                });
            }

            // Add spacing if no items
            if (!checklist.Items.Any())
            {
                column.Item().PaddingTop(20)
                    .Text("No items in this checklist.")
                    .FontSize(12)
                    .Italic()
                    .FontColor(Colors.Grey.Medium);
            }
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter().AlignBottom().Column(column =>
        {
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5).Text(text =>
            {
                text.Span("Generated: ");
                text.Span(DateTime.Now.ToString("g"));
                text.Span("  |  Page ");
                text.CurrentPageNumber();
                text.Span(" of ");
                text.TotalPages();
            });
        });
    }
}
