using iText.Kernel.Pdf;
using PrivatePdfConverter.Services;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class MergePdf
{
    /// <summary>
    /// Merge all PDF files in the specified path into a single PDF file.
    /// </summary>
    /// <param name="path">Path to the directory containing PDF files.</param>
    /// <param name="output">Optional output file name (without extension).</param>
    public static void ConvertDirectoryToOnePdf(string path, string? output)
    {
        var filesPaths = path.LoadFilePathsFromDirectory("*.pdf");
        var pdfs = filesPaths.Select(x => new PdfDocument(new PdfReader(x)));
        SaveAsPdf(path, pdfs, output);
    }

    private static void SaveAsPdf(string path, IEnumerable<PdfDocument> pdfs, string? output)
    {
        var outputFileName = output.PrepareOutputFileName(path);

        var fileWithPath = Path.Combine(path, outputFileName);
        using var mergedDocument = new PdfDocument(new PdfWriter(fileWithPath));
        foreach (var pdf in pdfs)
        {
            pdf.CopyPagesTo(1, pdf.GetNumberOfPages(), mergedDocument);
            pdf.Close();
        }

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, fileWithPath);
    }
}
