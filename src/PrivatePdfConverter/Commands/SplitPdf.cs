using iText.Kernel.Pdf;
using PrivatePdfConverter.Services;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class SplitPdf
{
    public static void SplitPdfByPages(string path, string pages, string? output)
    {
        if (!File.Exists(path))
        {
            Log.Logger.Error("PDF file '{Path}' does not exist", path);
            return;
        }

        if (!path.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            Log.Logger.Error("File '{Path}' is not a PDF file", path);
            return;
        }

        Log.Logger.Information("Read PDF file: {FileName}, Full path: '{Path}'", Path.GetFileName(path), path);

        using var sourcePdf = new PdfDocument(new PdfReader(path));
        var totalPages = sourcePdf.GetNumberOfPages();
        
        Log.Logger.Information("PDF has {TotalPages} pages", totalPages);

        var pageRanges = ParsePageRanges(pages, totalPages);
        if (!pageRanges.Any())
        {
            Log.Logger.Error("No valid page ranges found in '{Pages}'", pages);
            return;
        }

        var outputBaseName = output ?? "output";
        var outputDirectory = Path.GetDirectoryName(path) ?? ".";

        foreach (var range in pageRanges)
        {
            CreateSplitPdf(sourcePdf, range, outputDirectory, outputBaseName);
        }
    }

    private static List<PageRange> ParsePageRanges(string pagesInput, int totalPages)
    {
        var ranges = new List<PageRange>();

        if (string.IsNullOrWhiteSpace(pagesInput))
        {
            Log.Logger.Error("Pages parameter cannot be empty");
            return ranges;
        }

        // Handle "all" special case
        if (pagesInput.Trim().Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            for (int i = 1; i <= totalPages; i++)
            {
                ranges.Add(new PageRange(i, i, $"{i}"));
            }
            return ranges;
        }

        var parts = pagesInput.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            var trimmedPart = part.Trim();
            
            if (trimmedPart.Contains('-'))
            {
                // Range format like "1-5"
                var rangeParts = trimmedPart.Split('-', 2);
                if (rangeParts.Length == 2 && 
                    int.TryParse(rangeParts[0].Trim(), out var start) &&
                    int.TryParse(rangeParts[1].Trim(), out var end))
                {
                    if (start <= end && start >= 1 && end <= totalPages)
                    {
                        ranges.Add(new PageRange(start, end, $"{start}-{end}"));
                    }
                    else
                    {
                        Log.Logger.Warning("Invalid page range '{Range}'. Pages must be between 1 and {TotalPages}", trimmedPart, totalPages);
                    }
                }
                else
                {
                    Log.Logger.Warning("Invalid page range format: '{Range}'. Use format like '1-5'", trimmedPart);
                }
            }
            else
            {
                // Single page format like "3"
                if (int.TryParse(trimmedPart, out var pageNumber))
                {
                    if (pageNumber >= 1 && pageNumber <= totalPages)
                    {
                        ranges.Add(new PageRange(pageNumber, pageNumber, pageNumber.ToString()));
                    }
                    else
                    {
                        Log.Logger.Warning("Invalid page number '{PageNumber}'. Pages must be between 1 and {TotalPages}", pageNumber, totalPages);
                    }
                }
                else
                {
                    Log.Logger.Warning("Invalid page number format: '{Part}'. Use integer values", trimmedPart);
                }
            }
        }

        return ranges;
    }

    private static void CreateSplitPdf(PdfDocument sourcePdf, PageRange range, string outputDirectory, string outputBaseName)
    {
        var outputFileName = $"{outputBaseName}_{range.DisplayName}.pdf";
        var outputPath = Path.Combine(outputDirectory, outputFileName);

        try
        {
            using var targetPdf = new PdfDocument(new PdfWriter(outputPath));
            sourcePdf.CopyPagesTo(range.Start, range.End, targetPdf);

            Log.Logger.Information("Created PDF '{OutputFileName}' with pages {Start}-{End} at '{OutputPath}'", 
                outputFileName, range.Start, range.End, outputPath);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Failed to create PDF '{OutputFileName}' for pages {Start}-{End}", 
                outputFileName, range.Start, range.End);
        }
    }

    private record PageRange(int Start, int End, string DisplayName);
}