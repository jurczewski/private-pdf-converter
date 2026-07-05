using iText.Kernel.Pdf;
using PrivatePdfConverter.Services;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class SplitPdf
{
    /// <summary>
    /// Split a PDF file into multiple PDFs based on page ranges or individual pages.
    /// </summary>
    /// <param name="path">Path to the source PDF file.</param>
    /// <param name="pages">Page specification: "all", ranges like "1-5,10-15", or individual pages like "1,3,5".</param>
    /// <param name="output">Optional output file name prefix. A trailing ".pdf" is handled automatically.</param>
    public static void Run(string path, string pages, string? output = null)
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

        using var sourcePdf = new PdfDocument(new PdfReader(path));
        var totalPages = sourcePdf.GetNumberOfPages();
        Log.Logger.Information("Read PDF '{Path}' with {TotalPages} pages", path, totalPages);

        var ranges = ParsePageRanges(pages, totalPages);
        if (ranges is null)
        {
            return;
        }

        var outputDir = Path.GetDirectoryName(path) ?? string.Empty;
        var outputPrefix = string.IsNullOrEmpty(output)
            ? Path.GetFileNameWithoutExtension(path) + "_pages"
            : output.PrepareOutputBaseName(path);

        foreach (var (start, end, label) in ranges)
        {
            var outputFileName = $"{outputPrefix}_{label}".PrepareOutputFileName(path);
            var outputPath = Path.Combine(outputDir, outputFileName);

            using var destPdf = new PdfDocument(new PdfWriter(outputPath));
            sourcePdf.CopyPagesTo(start, end, destPdf);

            Log.Logger.Information("Created '{OutputFileName}' (pages {Start}-{End})", outputFileName, start, end);
        }
    }

    internal static List<(int Start, int End, string Label)>? ParsePageRanges(string pages, int totalPages)
    {
        if (string.IsNullOrWhiteSpace(pages))
        {
            Log.Logger.Error("Page specification cannot be empty");
            return null;
        }

        if (pages.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return Enumerable.Range(1, totalPages)
                .Select(p => (p, p, p.ToString()))
                .ToList();
        }

        var result = new List<(int Start, int End, string Label)>();
        var parts = pages.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var part in parts)
        {
            if (part.Contains('-'))
            {
                var rangeParts = part.Split('-', 2);
                if (!int.TryParse(rangeParts[0], out var start) || !int.TryParse(rangeParts[1], out var end))
                {
                    Log.Logger.Error("Invalid page range '{Part}'. Expected format: start-end (e.g. 1-5)", part);
                    return null;
                }

                if (start < 1 || end < 1)
                {
                    Log.Logger.Error("Page numbers must be greater than 0 in range '{Part}'", part);
                    return null;
                }

                if (start > end)
                {
                    Log.Logger.Error("Start page {Start} must not be greater than end page {End} in range '{Part}'", start, end, part);
                    return null;
                }

                if (end > totalPages)
                {
                    Log.Logger.Error("Page {End} exceeds total page count {TotalPages}", end, totalPages);
                    return null;
                }

                result.Add((start, end, $"{start}-{end}"));
            }
            else
            {
                if (!int.TryParse(part, out var page))
                {
                    Log.Logger.Error("Invalid page number '{Part}'", part);
                    return null;
                }

                if (page < 1)
                {
                    Log.Logger.Error("Page number must be greater than 0, got {Page}", page);
                    return null;
                }

                if (page > totalPages)
                {
                    Log.Logger.Error("Page {Page} exceeds total page count {TotalPages}", page, totalPages);
                    return null;
                }

                result.Add((page, page, page.ToString()));
            }
        }

        return result;
    }
}
