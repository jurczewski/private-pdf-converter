using iText.Kernel.Pdf;
using PrivatePdfConverter.Services;
using Serilog;

namespace PrivatePdfConverter.Commands;

public static class MergePdf
{
    public static void ConvertDirectoryToOnePdf(string path, string? output)
    {
        var filesPaths = path.LoadFilePathsFromDirectory("*.pdf");
        var pdfs = filesPaths.Select(x => new PdfDocument(new PdfReader(x)));
        SaveAsPdf(path, pdfs, output);
    }

    private static void SaveAsPdf(string path, IEnumerable<PdfDocument> pdfs, string? output)
    {
        var outputFileName = output.PrepareOutputFileName();

        var fileWithPath = path.AddFileToPath(outputFileName);
        using var mergedDocument = new PdfDocument(new PdfWriter(fileWithPath));
        foreach (var pdf in pdfs)
        {
            pdf.CopyPagesTo(1, pdf.GetNumberOfPages(), mergedDocument);
            pdf.Close();
        }

        Log.Logger.Information("PDF '{OutputFileName}' created at '{Path}'", outputFileName, fileWithPath);
    }
}
