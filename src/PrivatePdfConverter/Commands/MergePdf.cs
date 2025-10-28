using iText.Kernel.Pdf;
using PrivatePdfConverter.Services;

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
        var fileWithPath = PdfOperationHelper.PrepareDirectoryOutputPath(path, output, out var outputFileName);
        
        using var mergedDocument = new PdfDocument(new PdfWriter(fileWithPath));
        foreach (var pdf in pdfs)
        {
            pdf.CopyPagesTo(1, pdf.GetNumberOfPages(), mergedDocument);
            pdf.Close();
        }

        PdfOperationHelper.LogPdfCreation(outputFileName, fileWithPath);
    }
}
